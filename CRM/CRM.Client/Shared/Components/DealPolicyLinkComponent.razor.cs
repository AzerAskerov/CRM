using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CRM.Client.Cache;
using CRM.Client.States;
using CRM.Data.Enums;
using CRM.Operation.Localization;
using CRM.Operation.Models;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.RequestModels;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Zircon.Core.Extensions;

namespace CRM.Client.Shared.Components
{
    public partial class DealPolicyLinkComponent
    {
        [Parameter]
        public DealModel DealModel { get; set; }
        public string PolicySearchResultMessage { get; set; }
        public string PolicyNumber { get; set; }

        [Parameter]
        public EventCallback CallParentStateHasChanged { get; set; }
        string cachedDeals = "CachedDeals";
        #region Commands

        private async void SearchAndLinkPolicy()
        {
            SearchAndLinkPolicyButtonLoading = true;

            var result = await State.GetClientPolicy(new GetClientPolicyModel(DealModel.Client, PolicyNumber));

            if (result is null || !result.Success)
            {
                MessageBoxService.ShowError("MessageBox.ErrorOccuredWhileSearchingPolicy");
                if(result != null)
                    SetState(result);
                return;
            }
            
            var policy = result.Model;

            if (policy is null)
            {
                MessageBoxService.ShowError(LocalizationExtension.Translate("MessageBox.PolicyNotFound"));
                SearchAndLinkPolicyButtonLoading = false;
                StateHasChanged();
                return;
            }

            var policyLinkModel = new DealPolicyLinkViewModel
            {
                DealGuid = DealModel.DealGuid,
                OfferId = AgreedOffer.Id,
                PolicyNumber = policy.PolicyNumber,
                PolicyModel = policy,
                LinkDate = DateTime.UtcNow,
                OfferNumber = AgreedOffer.OfferNumber,
                ByUser = DealModel.UnderwriterUser,
                ByUserGuid = DealModel.UnderwriterUser.UserGuid
            };
            
            var response = await HttpClient.PostAsJsonAsync("api/Deal/LinkPolicyToDeal", policyLinkModel);

            var responseResult = await response.Content.ReadFromJsonAsync<OperationResult>();

            SearchAndLinkPolicyButtonLoading = false;

            if (!response.IsSuccessStatusCode || !responseResult.Success)
            {
                StateHasChanged();
                SetState(responseResult);
                return;
            }
            
            //Success
            MessageBoxService.ShowSuccess(LocalizationExtension.Translate("MessageBox.DealPolicyLinkSuccess"));
            
            DealModel.DealStatus = DealStatus.Linked;
            
            DealModel.DealPolicyLinks.Add(policyLinkModel);
           
            StateHasChanged();
            await CallParentStateHasChanged.InvokeAsync(null);
        }

        #endregion

        private OfferViewModel AgreedOffer => DealModel.Offers.OrderByDescending(x => x.OfferNumber)
            .FirstOrDefault(x => x.IsAgreed == true);

        public bool SearchAndLinkPolicyButtonLoading { get; set; }

        private bool IsNewDealPolicyLinkActive()
        {
            return DealModel.DealPolicyLinks.All(x => DealModel.Offers.FirstOrDefault(y => y.OfferNumber.Equals(x.OfferNumber))?.OfferPeriodTypeOid == 2);
        }
    }
}