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
using CRM.Client.States;
using CRM.Data.Enums;
using CRM.Operation.Helpers;
using CRM.Operation.JsonConverters;
using CRM.Operation.Localization;
using CRM.Operation.Models;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.Login;
using CRM.Operation.Models.RequestModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Zircon.Core.Extensions;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class DealCardForUnderwriter
    {
        [Parameter]
        public DealModel Model { get; set; } 

        [CascadingParameter]
        public ComponentInstance BaseCompo { get; set; }

        public EditContext EditContext { get; set; }

        public bool IsFormReadOnly ()
        {
            return   Model.DealStatus.In(DealStatus.Rejected, DealStatus.Linked) || Model.UnderwriterUser == null;
        }
        public string FileUploadIFrameUrl { get; set; }
        public string SurveyFilesIFrameUrl { get; set; }

        public List<UserSummaryShort> Underwriters { get; set; } = new List<UserSummaryShort>();

        public UserSummaryShort CurrentUser { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("window.dealStyles");
        }

        private static Dictionary<OfferTypeEnum, Type> FormComponentMap =>
            OfferTypeEnumToComponentMappingHelper.FormComponentMap;
        protected override async Task OnInitializedAsync()
        {
            InitializeModel();
        
            await InitializeUsers();
        
            await InitializeFileUploadIFrame();

            await InitializeSurveyFileIFrame();

            await base.OnInitializedAsync();

        }

        private void InitializeModel()
        {
            Model.Survey ??= new SurveyModel
            {
                DealGuid = Model.DealGuid,
                SumInsuredAmount = Model.SumInsured,
                OfferType = (OfferTypeEnum)Model.SelectedOfferType!,
                Status = 5
            };
            
            EditContext = new EditContext(Model);

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

            Underwriters = (await State.GetUsersWithPermissionParameterValue(new GetUsersWithPermissionParameterValueContract
            {
                PermissionName = "CRMRole",
                ParameterName = "CRMDealUserType",
                ParameterValue = "Underwriter"
            }));

            StateHasChanged();
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

            FileUploadIFrameUrl = responseModel.Link;
        }

        private async Task InitializeSurveyFileIFrame()
        {
            var generateTokenRequest = new FileManagementGenerateTokenRequest
            {
                ObjectType = "DealSurvey",
                ViewMode = (int) FileManagementViewMode.Media,
                ObjectIdentificationNumber = Model.DealNumber,
                DocumentType = (OfferTypeEnum)Model.SelectedOfferType switch
                {
                    OfferTypeEnum.VehicleInsurance => new List<string>()
                        {
                            "Survey.VehicleOther",
                            "Survey.VehicleBack",
                            "Survey.VehicleTop",
                            "Survey.VehicleInner",
                            "Survey.VehicleInner2",
                            "Survey.VehicleInner3",
                            "Survey.VehicleInner4",
                            "Survey.VehicleHoodArea",
                            "Survey.VehicleFront",
                            "Survey.VehicleFront2",
                            "Survey.VehicleFront3",
                            "Survey.VehicleRight",
                            "Survey.VehicleRight2",
                            "Survey.VehicleRight3",
                            "Survey.VehicleRight4",
                            "Survey.VehicleLeft",
                            "Survey.VehicleLeft2",
                            "Survey.VehicleLeft3",
                            "Survey.VehicleLeft4",
                            "Survey.VehicleLeft5",
                            "Survey.VehiclePassport"
                        }.Select(LocalizationExtension.Translate)
                        .ToList(),
                    OfferTypeEnum.PropertyInsurance => new List<string>()
                        {
                            "Property.1",
                            "Property.2",
                            "Property.3",
                            "Property.4",
                            "Property.5",
                            "Property.6",
                            "Property.7",
                            "Property.8",
                        }.Select(LocalizationExtension.Translate)
                        .ToList(),
                    _ => new List<string>()
                }
            };

            var response = await HttpClient.PostAsJsonAsync("api/FileManagement/GetIFrameUrl", generateTokenRequest);
            var responseModel =
                JsonConvert.DeserializeObject<FileManagementGetIFrameLinkResponse>(
                    await response.Content.ReadAsStringAsync());

            SurveyFilesIFrameUrl = responseModel.Link;
        }

        public async void Print()
        {
            var isFormValid = EditContext.Validate();
            var isOfferFormValid = EditContext.Validate();

            if (!isFormValid || !isOfferFormValid)
                return;

            await JSRuntime.InvokeVoidAsync("generatePDF");
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
            //sending existing form model to child component.
            builder.AddAttribute(2,"OfferModel", Model.FormComponentModel);
            builder.AddAttribute(2,"DealModel", Model);
            builder.AddAttribute(3,"ReadOnly", true);
            
            builder.CloseComponent();
        };

        #region is active states

        private bool IsSubmitOfferButtonActive()
        {
            return (Model.UnderwriterUser != null) &&
                   !(Model.Offers?.FirstOrDefault(x => x.IsAgreed != null && x.IsAgreed.Value)?.OfferPeriodTypeOid == 1 &&
                     Model.DealStatus == DealStatus.Linked);
        }

        private bool IsRejectButtonActive()
        {
            return (Model.UnderwriterUser != null) &&
                   !(Model.Offers?.FirstOrDefault(x => x.IsAgreed != null && x.IsAgreed.Value)?.OfferPeriodTypeOid == 1 &&
                     Model.DealStatus == DealStatus.Linked);
        }

        
        private bool IsDealPolicyLinkActive()
        {
            return Model.DealStatus.In(DealStatus.Agreed, DealStatus.Linked);
        }
        
        #endregion
        
        private async void UnderWriterSelected(ChangeEventArgs obj)
        {
            Model.UnderwriterUser = Underwriters.FirstOrDefault(x => x.Login.Equals(obj.Value.ToString()));
            
            var response = await HttpClient.PostAsJsonAsync("api/Deal/SetUnderwriterForDeal", Model, new JsonSerializerOptions()
            {
                Converters = { new DealOfferFormJsonConverter() },
                WriteIndented = true
            });

            SetState(await response.Content.ReadFromJsonAsync<OperationResult>());
           
            StateHasChanged();
        }
        
        private async void SubmitOffers()
        {
            if (!EditContext.Validate())
                return;

            if (Model.Offers != null && !Model.Offers.Any())
            {
                MessageBoxService.ShowWarning(LocalizationExtension.Translate("MessageBox.NoOfferToSubmit"));
                return;
            }

            if (Model.Offers!.All(x => x.IsReadOnly))
            {
                MessageBoxService.ShowWarning(LocalizationExtension.Translate("MessageBox.NoOfferToSubmit"));
                return;
            }

            SubmitButtonLoading = true;
            
            var response = await HttpClient.PostAsJsonAsync("api/Deal/SubmitOffers", Model, new JsonSerializerOptions()
            {
                Converters = { new DealOfferFormJsonConverter() },
                WriteIndented = true
            });

            SubmitButtonLoading = false;
            
            if (!response.IsSuccessStatusCode) return;
            
            MessageBoxService.ShowSuccess(LocalizationExtension.Translate("MessageBox.SubmitOfferSuccess"));
            
            Model.DealStatus = DealStatus.Offered;
            Model.ResponsiblePersonType = DealResponsiblePersonTypeEnum.Seller;
            Model.Offers!.ForEach(x=>x.IsReadOnly = true);

            StateHasChanged();
        }

        private async void RejectDeal()
        {
            RejectButtonLoading = true;
            
            var response = await HttpClient.PostAsJsonAsync($"api/Deal/ChangeDealStatus",new ChangeDealStatusRequest
            {
                DealGuid = Model.DealGuid,
                DealStatus = DealStatus.Rejected
            });

            SetState(await response.Content.ReadFromJsonAsync<OperationResult>());

            RejectButtonLoading = false;
            
            if (!response.IsSuccessStatusCode) return;
            
            MessageBoxService.ShowSuccess(LocalizationExtension.Translate("MessageBox.RejectDealSuccess"));

            Model.DealStatus = DealStatus.Rejected;
            Model.Offers = new List<OfferViewModel>();
            StateHasChanged();
        }
        
        private async void SubmitDiscussion(MouseEventArgs args)
        {
            if (string.IsNullOrEmpty(Model.DealDiscussionModel.NewDiscussionContent))
                return;

            DiscussionSubmitLoading = true;
            
            var discussion = new DiscussionViewModel()
            {
                Content = Model.DealDiscussionModel.NewDiscussionContent,
                DateTime = DateTime.Now,
                DealGuid = Model.DealGuid,
                Receiver = Model.CreatedByUser,
                IsRead = false,
                Sender = CurrentUser
            };

            var response = await HttpClient.PostAsJsonAsync("api/Deal/SubmitDiscussion", discussion, new JsonSerializerOptions()
            {
                Converters = { new DealOfferFormJsonConverter() },
                WriteIndented = true
            });

            SetState(await response.Content.ReadFromJsonAsync<OperationResult>());

            DiscussionSubmitLoading = false;
            
            //Adding new discussion to table view
            Model.DealDiscussionModel.DiscussionHistory.Add(discussion);
            Model.DealDiscussionModel.DiscussionHistory = Model.DealDiscussionModel.DiscussionHistory.OrderByDescending(x => x.DateTime).ToList();
            Model.DealDiscussionModel.NewDiscussionContent = string.Empty;

            Model.ResponsiblePersonType = DealResponsiblePersonTypeEnum.Seller;
            
            StateHasChanged();
        }

        public bool DiscussionSubmitLoading { get; set; }
        public bool SubmitButtonLoading { get; set; }
        public bool RejectButtonLoading { get; set; }
        public bool SubmitSurveyButtonLoading { get; set; }

        private async void SubmitSurvey()
        {
            if (!EditContext.Validate())
                return;

            SubmitSurveyButtonLoading = true;
            
            var response = await HttpClient.PostAsJsonAsync("api/Deal/SubmitSurvey", Model, new JsonSerializerOptions()
            {
                Converters = { new DealOfferFormJsonConverter() },
                WriteIndented = true
            });

            SubmitSurveyButtonLoading = false;
            
            var responseModel = await response.Content.ReadFromJsonAsync<OperationResult<DealModel>>();
            
            if (!response.IsSuccessStatusCode) return;
            
            
            SetState(responseModel);

            Model = responseModel.Model;

            await InitializeSurveyFileIFrame();
            
            StateHasChanged();
        }

        public void StateChanged()
        {
            StateHasChanged();
        }

        private bool IsSurveyFilesSectionActive()
        {
            return Model.Survey.Status != (int)SurveyStatusEnum.NotSetYet;
        }
    }
}