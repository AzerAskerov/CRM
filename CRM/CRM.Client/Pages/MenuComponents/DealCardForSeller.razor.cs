using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using CRM.Client.Cache;
using CRM.Client.Helpers;
using CRM.Client.Models;
using CRM.Client.Shared;
using CRM.Client.Shared.Components;
using CRM.Client.States;
using CRM.Data.Enums;
using CRM.Operation.JsonConverters;
using CRM.Operation.Localization;
using CRM.Operation.Models;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.Login;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Zircon.Core;
using Zircon.Core.Extensions;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class DealCardForSeller
    {
        [Parameter]
        public DealModel Model { get; set; }
        
        private EditContext EditContext { get; set; }
        private EditContext OfferFormEditContext { get; set; }

        [CascadingParameter]
        public ComponentInstance BaseCompo { get; set; }

        [Inject]
        public IComponentService _componentService { get; set; }
        public string FileUploadIFrameUrl { get; set; }
        public UserSummaryShort CurrentUser { get; set; }
        private static Dictionary<OfferTypeEnum, Type> FormComponentMap =>
            OfferTypeEnumToComponentMappingHelper.FormComponentMap;

        private bool ReadOnly => Model.ResponsiblePersonType != DealResponsiblePersonTypeEnum.Seller;
        public DealGeneralInfoComponent dealGeneralInfoComponent { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("window.dealStyles");
        }

        /// <summary>
        /// Renders offer form component dynamically based on selected form type.
        /// </summary>
        /// <returns></returns>
        RenderFragment CreateOfferFormComponent() => builder =>
        {
            if (!Model.SelectedOfferType.HasValue)
                return;
            
            OfferTypeEnum selectedOfferType = (OfferTypeEnum)Model.SelectedOfferType.Value;

            if (!FormComponentMap.ContainsKey(selectedOfferType))
                return;

            builder.OpenComponent(0, FormComponentMap[selectedOfferType]);
            var callback = EventCallback.Factory.Create<EditContext>(this, (context =>
            {
                Model.FormComponentModel = context.Model as IDealOfferFormModel;
                OfferFormEditContext = context;
            }));
            builder.AddAttribute(1,"ModelSet", callback);
            //sending existing form model to child component.
            
            builder.AddAttribute(2,"OfferModel", Model.FormComponentModel);
            builder.AddAttribute(3,"DealModel", Model);
          
            if (ReadOnly)
            {
                builder.AddAttribute(3,"ReadOnly", true);
            }
            builder.CloseComponent();
        };
        
        #region Initializers

        protected override async Task OnParametersSetAsync()
        {
            await InitializeUsers();

            InitializeDealModel();

            EditContext = new EditContext(Model);

            await InitializeFileUploadIFrame();

            await base.OnInitializedAsync();

            await base.OnParametersSetAsync();

        }

        private async Task InitializeUsers()
        {
            var currentUser = await State.HasUserLoggedIn();
            CurrentUser = new UserSummaryShort
            {
                Login = currentUser.UserName,
                FullName = currentUser.FullName,
                UserGuid = currentUser.Guid.Value
            };
        }

        /// <summary>
        /// Generates iFrame url for file uploading and assigns to 'FileUploadIFrameUrl' property.
        /// </summary>
        /// <returns></returns>
        private async Task InitializeFileUploadIFrame()
        {
            FileManagementGenerateTokenRequest requestModel = new FileManagementGenerateTokenRequest
            {
                DocumentType = new List<string>{"Other"}, // Other
                ObjectType = "DEALS",
                ObjectIdentificationNumber = Model.DealGuid.ToString(), // Deal number
                ViewMode = (int) FileManagementViewMode.Complex
            };
            var response = await HttpClient.PostAsJsonAsync("api/FileManagement/GetIFrameUrl", requestModel);
            var responseModel =
                JsonConvert.DeserializeObject<FileManagementGetIFrameLinkResponse>(
                    await response.Content.ReadAsStringAsync());

            FileUploadIFrameUrl = responseModel?.Link;
        }

        private void InitializeDealModel()
        {
            Model ??= new DealModel
            {
                CreatedByUser = CurrentUser,
                CreatedTimeStamp = DateTime.Now,
                ResponsiblePersonType = DealResponsiblePersonTypeEnum.Seller,
                Tender = new TenderModel()
            };
        }
        
        #endregion

        private void ClientSelected(ClientContract clientContract)
        {
            Model.Client = clientContract;
        }
        
        private void OfferFormTypeSelected()
        {
            Model.FormComponentModel = null;
            StateHasChanged();
        }

        private void OfferSelected(ChangeEventArgs obj)
        {
            var selectedOfferNumber = obj.Value.ToString();
            var offerViewModel = Model.Offers.First(x => x.OfferNumber.Equals(selectedOfferNumber));
            offerViewModel!.IsAgreed = true;
        }

        #region Button clicks

        private async void SaveAsDraftBtnClick(MouseEventArgs args)
        {
            var isFormValid = EditContext.Validate();
            Model.Tender.DealGuid = Model.DealGuid;//yeni
            var isOfferFormValid = OfferFormEditContext.Validate();
            
            if (!isFormValid || !isOfferFormValid)
                return;

            SaveAsDraftButtonLoading = true;
            var response = await HttpClient.PostAsJsonAsync("api/Deal/SaveDealAsDraft", Model, new JsonSerializerOptions()
            {
                Converters = { new DealOfferFormJsonConverter() },
                WriteIndented = true
            });

            var responseResult = await response.Content.ReadFromJsonAsync<OperationResult<DealModel>>();
            
            SetState(responseResult);

            SaveAsDraftButtonLoading = false;

            if (!response.IsSuccessStatusCode)
            {
                MessageBoxService.ShowError(LocalizationExtension.Translate("MessageBox.SaveDealError"));
                return;
            }

            Model = responseResult.Model;

            MessageBoxService.ShowSuccess(LocalizationExtension.Translate("MessageBox.SaveDealSuccess"));
            
            StateHasChanged();

           
        }

        public void OnLangChange(ChangeEventArgs args)
        {
            var SelectedValue = args.Value.ToString();
        }
        public bool PrintShow { get; set; }

        public async void Print() {
            var isFormValid = EditContext.Validate();
            var isOfferFormValid = OfferFormEditContext.Validate();

            if (!isFormValid || !isOfferFormValid)
                return;

            PrintShow = true;
            await JSRuntime.InvokeVoidAsync("generatePDF");
        }

        private async void SendToUnderwriterBtnClick(MouseEventArgs args)
        {
            var isFormValid = EditContext.Validate();
            var isOfferFormValid = OfferFormEditContext.Validate();
            Model.Tender.DealGuid = Model.DealGuid;//yeni
            if (!isFormValid || !isOfferFormValid)
                return;

            SendToUnderwriterButtonLoading = true;
            var response = await HttpClient.PostAsJsonAsync("api/Deal/SendDealToUnderWriter", Model, new JsonSerializerOptions()
            {
                Converters = { new DealOfferFormJsonConverter() },
                WriteIndented = true
            });

            if (!response.IsSuccessStatusCode)
            {
                MessageBoxService.ShowError(LocalizationExtension.Translate("MessageBox.SendToUnderwriterError"));
                return;
            }

            var responseResult = await response.Content.ReadFromJsonAsync<OperationResult<DealModel>>();
           
            SetState(responseResult);

            SendToUnderwriterButtonLoading = false;

            Model = responseResult.Model;

            MessageBoxService.ShowSuccess(LocalizationExtension.Translate("MessageBox.SendToUnderwriterSuccess"));
            StateHasChanged();

            
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
        protected async void OpenDealCard<T>(T passedDealParameter)
        {
            using (var task = LoadingManager.Loading("deals", LocalizationExtension.Translate("Clients.Loading")))
            {
                DealModel dealModel = null;
                ComponentParameters param = new ComponentParameters();

                if (passedDealParameter is DealListItem dealListItem)
                {
                    dealModel = await State.GetDealByDealGuid(dealListItem.DealGuid);
                }
                else if (passedDealParameter is DealModel model)
                {
                    dealModel = model;
                }

                if (dealModel is null)
                    throw new ArgumentNullException();

                param.Add(nameof(DealCardForSeller.Model), dealModel);
                _componentService.Show<DealCardForSeller>("Deal", param);
            }
        }

        private async Task RenewBtnClick(MouseEventArgs args)
        {
            await BaseCompo.Close();
            var newDealGuid = Guid.NewGuid();

            var response = await HttpClient.PostAsJsonAsync("api/FileManagement/CopyPathData", new CopyPathModel
            {
                sourceGuid = Model.DealGuid,
                sourceObjType = "DEALS",
                destinationObjType = "DEALS",
                destinationGuid = newDealGuid,
            });
            var responseModel =
                JsonConvert.DeserializeObject<FileManagementGetIFrameLinkResponse>(
                    await response.Content.ReadAsStringAsync());

            FileUploadIFrameUrl = responseModel?.Link;

            OpenDealCard(Model);
            dealGeneralInfoComponent.Refresh(CurrentUser, newDealGuid);

        }


        private async void SubmitDiscussion(MouseEventArgs args)
        {
            if (string.IsNullOrEmpty(Model.DealDiscussionModel.NewDiscussionContent))
                return;

            SubmitDiscussionButtonLoading = true;
            
            var discussion = new DiscussionViewModel()
            {
                Content = Model.DealDiscussionModel.NewDiscussionContent,
                DateTime = DateTime.Now,
                DealGuid = Model.DealGuid,
                Sender = CurrentUser,
                Receiver = Model.UnderwriterUser,
                IsRead = false
            };

            var response = await HttpClient.PostAsJsonAsync("api/Deal/SubmitDiscussion", discussion, new JsonSerializerOptions()
            {
                Converters = { new DealOfferFormJsonConverter() },
                WriteIndented = true
            });

            SetState(await response.Content.ReadFromJsonAsync<OperationResult>());

            //Adding new discussion to table view
            Model.DealDiscussionModel.DiscussionHistory.Add(discussion);
            Model.DealDiscussionModel.DiscussionHistory = Model.DealDiscussionModel.DiscussionHistory.OrderBy(x => x.DateTime).ToList();
            Model.DealDiscussionModel.NewDiscussionContent = string.Empty;

            SubmitDiscussionButtonLoading = false;

            Model.ResponsiblePersonType = DealResponsiblePersonTypeEnum.Underwriter;
            
            StateHasChanged();
        }

        #endregion

        #region States

        private bool IsSaveBtnActive()
        {
            return Model.DealStatus.In(DealStatus.Draft, DealStatus.New) || (Model.ResponsiblePersonType == DealResponsiblePersonTypeEnum.Seller && !Model.DealStatus.In(DealStatus.Offered));
        }

        private bool IsSendToUnderwriterBtnActive()
        {
            return (Model.DealStatus.In(DealStatus.Draft, DealStatus.New) || Model.ResponsiblePersonType == DealResponsiblePersonTypeEnum.Seller) && Model.DealStatus != DealStatus.Offered;
        }

        private bool IsFormActive()
        {
            return Model.DealStatus.In(DealStatus.Draft, DealStatus.New);
        }

        private bool _saveAsDraftButtonLoading;
        public bool SaveAsDraftButtonLoading
        {
            get => _saveAsDraftButtonLoading;
            set
            {
                _saveAsDraftButtonLoading = value;
                AnyButtonClicked = value;
            }
        }

        private bool _sendToUnderwriterButtonLoading;
        public bool SendToUnderwriterButtonLoading
        {
            get => _sendToUnderwriterButtonLoading;
            set
            {
                _sendToUnderwriterButtonLoading = value;
                AnyButtonClicked = value;
            }
        }

        private bool _submitDiscussionButtonLoading;
        public bool SubmitDiscussionButtonLoading
        {
            get => _submitDiscussionButtonLoading;
            set
            {
                _submitDiscussionButtonLoading = value;
                AnyButtonClicked = value;
            }
        }

        public bool AnyButtonClicked { get; set; }

        #endregion
    }
}