using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using CRM.Client.Cache;
using CRM.Client.Models;
using CRM.Client.Pages.MenuComponents;
using CRM.Client.States;
using CRM.Data.Enums;
using CRM.Operation.Localization;
using CRM.Operation.Models;
using CRM.Operation.Models.DealOfferModels;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Zircon.Core.Extensions;

namespace CRM.Client.Shared.Components
{
    public partial class OfferComponent
    {
        [Parameter]
        public DealModel DealModel { get; set; }
        
        [Parameter]
        public bool ReadOnly { get; set; }

        protected override void OnInitialized()
        {
            //Initialize offers list if null.
            DealModel.Offers ??= new List<OfferViewModel>();
            DealModel.Offers.ForEach(x=>
            {
                x.IsReadOnly = true;
                InitializeOffersAttachmentFileManagementToken(x).ConfigureAwait(true); // initializing file management token for readonly offers.
            });
            base.OnInitialized();
        }

        private bool? IsUnderwriterUser()
        {
            var authState = ServerAuthenticationStateTask as ServerAuthenticationStateProvider;
            return authState?.IsUnderwriterUser();
        }

          private bool? IsOperuUser()
        {
            var authState = ServerAuthenticationStateTask as ServerAuthenticationStateProvider;
            return authState?.IsOperuUser();
        }


        
        private async Task AddOfferItem()
        {
            var offerModel = new OfferViewModel{ DealGuid = DealModel.DealGuid, CreatedDate = DateTime.Now };
            
            DealModel.Offers.Add(offerModel);

            RegenerateOfferNumbers();

            await InitializeFileUploadIFrameLinkForOffer(offerModel);
            await InitializeOffersAttachmentFileManagementToken(offerModel);
        }

        private void RemoveOfferItem(OfferViewModel offerItem)
        {
            if (!DealModel.Offers.Contains(offerItem))
                return;
            
            DealModel.Offers.Remove(offerItem);
            
            RegenerateOfferNumbers();
        }

        private void RegenerateOfferNumbers()
        {
            int counter = 1;
            foreach (var dealModelOffer in DealModel.Offers)
            {
                dealModelOffer.OfferNumber = $"{DealModel.DealNumber}/{counter++}";
            }
        }
        
        private async Task InitializeFileUploadIFrameLinkForOffer(OfferViewModel offerViewModel)
        {
            FileManagementGenerateTokenRequest requestModel = new FileManagementGenerateTokenRequest
            {
                DocumentType = new List<string>{"Offer"}, // Offer
                ObjectType = "DEALS",
                ObjectIdentificationNumber = offerViewModel.OfferNumber, // Offer number
                ViewMode = (int) FileManagementViewMode.Complex
            };
            var response = await HttpClient.PostAsJsonAsync("api/FileManagement/GetIFrameUrl", requestModel);
            var responseModel =
                JsonConvert.DeserializeObject<FileManagementGetIFrameLinkResponse>(
                    await response.Content.ReadAsStringAsync());

            offerViewModel.FileUploadIFrameUrl = responseModel.Link;
            StateHasChanged();
        }
        
        private async Task InitializeOffersAttachmentFileManagementToken(OfferViewModel offerViewModel)
        {
            FileManagementGenerateTokenRequest requestModel = new FileManagementGenerateTokenRequest
            {
                DocumentType = new List<string>{"Offer"}, // Offer
                ObjectType = "DEALS",
                ObjectIdentificationNumber = offerViewModel.OfferNumber, // Offer number
                ViewMode = (int) FileManagementViewMode.FileList
            };
            var response = await HttpClient.PostAsJsonAsync("api/FileManagement/GenerateToken", requestModel);
            var responseModel =
                JsonConvert.DeserializeObject<FileManagementGenerateTokenResponse>(
                    await response.Content.ReadAsStringAsync());

            offerViewModel.FileManagementToken = responseModel.Token;
            StateHasChanged();
        }

        private string _selectedOfferNumber;
        private string SelectedOfferNumber
        {
            get
            {
                return _selectedOfferNumber;
            }
            set
            {
                _selectedOfferNumber = value;
            
                foreach (var dealModelOffer in DealModel.Offers)
                {
                    dealModelOffer.IsAgreed = dealModelOffer.OfferNumber.Equals(value);
                }
            
                StateHasChanged();
            }
        }
        private async void SubmitSelectedOffer() 
        {
            if (string.IsNullOrEmpty(_selectedOfferNumber))
                return;
            
            var offerViewModel = DealModel.Offers.First(x => x.OfferNumber.Equals(_selectedOfferNumber));
            
            var response = await HttpClient.GetAsync($"api/Deal/SubmitAgreedOffer?offerNumber={offerViewModel.OfferNumber}");

            SetState(await response.Content.ReadFromJsonAsync<OperationResult>());
            
            if (!response.IsSuccessStatusCode) return;
            
            MessageBoxService.ShowSuccess(LocalizationExtension.Translate("MessageBox.SubmitSelectedOffer"));
            
            DealModel.DealStatus = DealStatus.Agreed;
            DealModel.ResponsiblePersonType = DealResponsiblePersonTypeEnum.Underwriter;

           
            StateHasChanged();
        }

        private bool IsAddOfferBtnActive()
        {
            return IsOperuUser() == false &&(DealModel.UnderwriterUser != null && !ReadOnly) &&
                   !(DealModel.Offers?.FirstOrDefault(x => x.IsAgreed != null && x.IsAgreed.Value)?.OfferPeriodTypeOid == 1 &&
                     DealModel.DealStatus == DealStatus.Linked);
        }

        private bool IsSubmitSelectedOfferBtnActive()
        {
            return IsOperuUser() == false &&  DealModel.Offers.Any(x => x.IsAgreed == true) && DealModel.DealStatus == DealStatus.Offered && IsUnderwriterUser() == false;
        }
    }
}