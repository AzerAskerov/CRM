using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using CRM.Client.Cache;
using CRM.Client.Models;
using CRM.Client.Shared;
using CRM.Client.Shared.Components;
using CRM.Client.States;
using CRM.Data.Enums;
using CRM.Operation.Localization;
using CRM.Operation.Models;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.Login;
using CRM.Operation.Models.RequestModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class Deal
    {
        [Parameter]
        public string DealGuid { get; set; }
        DealSearchModel DealSearchModel { get; set; } = new DealSearchModel();

        List<DealListItem> deals = Enumerable.Empty<DealListItem>().ToList();

        List<Button<DealListItem>> actions = new List<Button<DealListItem>>();

        [Parameter]
        public string DealNumber { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        public PermissionParameterModel PermissionParameters { get; set; }
        public DealScopeModel UserGuids { get; set; }

        private UserModel CurrentUser { get; set; }
        private bool? IsUnderwriterUser()
        {
            var authState = ServerAuthenticationStateTask as ServerAuthenticationStateProvider;
            return authState?.IsUnderwriterUser();
        }
        protected override async Task OnInitializedAsync()
        {
            CurrentUser = await State.HasUserLoggedIn();

        }

        private bool? IsOperuUser()
        {
            var authState = ServerAuthenticationStateTask as ServerAuthenticationStateProvider;
            return authState?.IsOperuUser();
        }

        public MetaData MetaData { get; set; } = new MetaData() { PageSize = 200, CurrentPage = 1, Skip = 0 };
        protected override async Task OnParametersSetAsync()
        {



            if (!CurrentUser.IsAuthenticated)
                return;

            if (!string.IsNullOrEmpty(DealNumber))
            {
                var deal = await State.GetDealByDealNumber(DealNumber);

                OpenDealCard(deal);
                return;
            }
            if (IsOperuUser() == false)
            {
                using (var task = LoadingManager.Loading("deals", "Clients.Loading".Translate(), ""))
                {
                    PermissionParameters = await State.GetUsersWithPermission();

                    string filter = string.Empty;


                    if (IsUnderwriterUser() == false)
                    {

                        Enum.TryParse(PermissionParameters.Parameters,
                       out UserAccessScope resultScope);
                        UserGuids = await State.GetScopeUserGuids(new GetScopeUsersRequestModel { Scope = resultScope });
                        if (UserGuids != null)
                        {
                            var guidStr = CombineUserGuids(UserGuids.Guids);

                            if (!string.IsNullOrEmpty(guidStr))
                            {
                                filter = $" and CreatedByUserGuid by {CurrentUser.Guid}";

                            }
                            else
                            {
                                MessageBoxService.ShowError(LocalizationExtension.Translate("MessageBox.NoUserGuidFound"));
                                return;
                            }
                            filter = $" and CreatedByUserGuid in ({CombineUserGuids(UserGuids.Guids)})";
                        }

                    }

                    var result = await State.Deals(filter, MetaData);

                    if (result.IsSuccessStatusCode)
                    {
                        var responseContent = await result.Content.ReadAsStringAsync();
                        var model = JsonConvert.DeserializeObject<ODataModel<List<DealListItem>>>(responseContent);
                        MetaData.TotalPages = model.Count;
                        deals = model.Value;

                        if (deals != null && deals.Count > 0)
                        {
                            for (int i = 0; i < deals.Count; i++)
                            {
                                if (deals[i]?.DealSubject?.Length > 30)
                                {
                                    deals[i].DealSubject = deals[i]?.DealSubject?.Substring(0, 30) + "..."; ; // Display the entire string if it's 30 characters or less.
                                }

                            }
                        }

                    }
                    else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        SetState(new OperationResult { Name = System.Net.HttpStatusCode.NotFound.ToString(), Issues = new List<Issue> { new Issue { Code = result.StatusCode.ToString(), Severity = IssueSeverity.Error, Message = new Message { Text = "Request is too long" } } } });
                        return;
                    }

                    foreach (var dealListItem in deals)
                    {
                        dealListItem.HasUnreadMessage =
                            (dealListItem.ResponsiblePersonType == DealResponsiblePersonTypeEnum.Seller &&
                                IsUnderwriterUser() == false)
                            || (dealListItem.ResponsiblePersonType == DealResponsiblePersonTypeEnum.Underwriter &&
                                IsUnderwriterUser() == true);
                    }

                    deals = deals.OrderByDescending(x => x.HasUnreadMessage).ThenByDescending(x => x.CreatedTimeStamp)
                        .ToList();

                    Button<DealListItem> button = new Button<DealListItem>()
                    {
                        ActionName = OpenDealCard,
                        Icon = "",
                        Name = "",
                        Text = ""
                    };

                    actions.Add(button);

                    if (!string.IsNullOrEmpty(DealGuid))
                    {
                        Guid guid;
                        try
                        {
                            guid = new Guid(DealGuid);
                        }
                        catch (Exception)
                        {
                            MessageBoxService.ShowError(LocalizationExtension.Translate("MessageBox.WrongDealGuid"));
                            return;
                        }
                        var deal = await State.GetDealByDealGuid(new Guid(DealGuid));

                        if (deal == null)
                        {
                            MessageBoxService.ShowError(LocalizationExtension.Translate("MessageBox.DealNotFound"));
                            return;
                        }
                        OpenDealCard(deal);

                    }

                }
            }
           
        }

        public static string CombineUserGuids(List<UserGuids> userGuids)
        {
            StringBuilder combinedString = new StringBuilder();

            if (userGuids != null && userGuids.Count > 0)
            {
                foreach (var guid in userGuids)
                {
                    combinedString.Append($"'{guid.UserGuid.ToString()}',");
                }

                if (combinedString.Length > 0)
                {
                    combinedString.Length--; // Remove the trailing comma
                }
            }

            return combinedString.ToString();
        }

        protected async void OpenDealCard<T>(T passedDealParameter)
        {
            using (var task = LoadingManager.Loading("deals", "Clients.Loading".Translate(), ""))
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



                if (IsUnderwriterUser() == true)
                {
                    MarkUserMessagesAsRead(dealModel.DealGuid, CurrentUser.Guid!.Value, dealModel.DealNumber);
                    param.Add(nameof(DealCardForUnderwriter.Model), dealModel);
                    _componentService.Show<DealCardForUnderwriter>("Deal", param);
                }
                else if (IsUnderwriterUser() == false)
                {
                    CurrentUser = await State.HasUserLoggedIn();

                    if (CurrentUser == null || CurrentUser.Guid.Value == null)
                    {
                        await JS.InvokeVoidAsync("alert", $"User sessiyası bitib. Yenidən login olun. CurrentUser.Guid = {CurrentUser.Guid.Value} və dealModel.CreatedByUser.UserGuid = {dealModel.CreatedByUser.UserGuid}");
                        return;
                    }
                 
                    MarkUserMessagesAsRead(dealModel.DealGuid, CurrentUser.Guid!.Value, dealModel.DealNumber);
                    param.Add(nameof(DealCardForSeller.Model), dealModel);
                    _componentService.Show<DealCardForSeller>("Deal", param);
                }
                 else if (IsOperuUser() == true)
                {
                    MarkUserMessagesAsRead(dealModel.DealGuid, CurrentUser.Guid!.Value, dealModel.DealNumber);
                    param.Add(nameof(DealCardForOperu.Model), dealModel);
                    _componentService.Show<DealCardForOperu>("Deal", param);
                }


                else throw new NotSupportedException();
            }
        }

        protected async void MarkUserMessagesAsRead(Guid dealGuid, Guid userGuid, string dealNumber)
        {
            try
            {
                var requestModel = new MarkUserDealMessagesAsReadModel
                {
                    DealGuid = dealGuid,
                    UserGuid = userGuid
                };
                var result = await HttpClient.PostJsonAsync<MarkUserDealMessagesAsReadModel, OperationResult>("api/Deal/MarkUserMessagesAsRead", requestModel);

            }
            catch (Exception e)
            {

                Console.WriteLine(e?.Message);
                Console.WriteLine(e?.InnerException?.Message);
            }
        }


        private async Task SearchAsync(DealSearchModel model, MetaData metaData)
        {

            PermissionParameters = await State.GetUsersWithPermission();

            string filter = string.Empty;
            if (IsUnderwriterUser() == false)
            {

                Enum.TryParse(PermissionParameters.Parameters,
                    out UserAccessScope resultScope);
                UserGuids = await State.GetScopeUserGuids(new GetScopeUsersRequestModel { Scope = resultScope });
                filter = $" and CreatedByUserGuid in ({CombineUserGuids(UserGuids.Guids)})";
            }

            if (model.ClientInn.HasValue)
                filter += $"and ClientInn eq {model.ClientInn}";

            

            if (!string.IsNullOrEmpty(model.FullName))
                filter += $"and ClientInnNavigation/PhysicalPeople/any(t: contains(t/FullName, '{model.FullName}'))";

            if (!string.IsNullOrEmpty(model.DiscussionText))
                filter += $"and discussion/any(t: contains(t/Content, '{model.DiscussionText}'))";

            if (!string.IsNullOrEmpty(model.MediatorUserName))
                filter += $"and contains(CreatedByUserFullName, '{model.MediatorUserName}')";

            if (!string.IsNullOrEmpty(model.UnderwriterUserName))
                filter += $"and contains(UnderwriterUserFullName, '{model.UnderwriterUserName}')";

            if (!string.IsNullOrEmpty(model.PinNumber))
                filter += $" and ClientInnNavigation/PhysicalPeople/any(t:t/Pin eq '{model.PinNumber}')";

            if (!string.IsNullOrEmpty(model.DealNumber))
                filter += $" and dealnumber eq '{model.DealNumber}'";

            if (!string.IsNullOrEmpty(model.DocumentNumber))
            {
                //expand = $"&$expand=documents";
                filter += $" and ClientInnNavigation/documents/any(t:t/documentnumber eq '{model.DocumentNumber}')";
            }

            if ((int)model.DealStatus > -1)
            {
                //expand = $"&$expand=documents";
                filter += $" and DealStatus eq '{model.DealStatus}'";
            }

            if ((int)model.DealType > -1)
            {
                //expand = $"&$expand=documents";
                filter += $" and DealType eq '{model.DealType}'";
              
            }

            if (!string.IsNullOrEmpty(model.DealSubject))
            {
                filter += $"and contains(DealSubject, '{model.DealSubject}')";
                Console.WriteLine(filter);
            }


            string query = $"{filter}";

            using (var task = LoadingManager.Loading("deals", "Clients.Loading".Translate(), ""))
            {

                var result  = await State.Deals(query, MetaData);
                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    var model_ = JsonConvert.DeserializeObject<ODataModel<List<DealListItem>>>(responseContent);
                    MetaData.TotalPages = model_.Count;
                    deals = model_.Value;

                    if (deals != null && deals.Count > 0)
                    {
                        for (int i = 0; i < deals.Count; i++)
                        {
                            if (deals[i]?.DealSubject?.Length > 30)
                            {
                                deals[i].DealSubject = deals[i]?.DealSubject?.Substring(0, 30) + "..."; ; // Display the entire string if it's 30 characters or less.
                            }

                        }
                    }
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    SetState(new OperationResult { Name = System.Net.HttpStatusCode.NotFound.ToString(), Issues = new List<Issue> { new Issue { Code = result.StatusCode.ToString(), Severity = IssueSeverity.Error, Message = new Message { Text = "Request is too long" } } } });
                    return;
                }
                //================================

                foreach (var dealListItem in deals)
                {
                    dealListItem.HasUnreadMessage =
                        (dealListItem.ResponsiblePersonType == DealResponsiblePersonTypeEnum.Seller &&
                            IsUnderwriterUser() == false)
                        || (dealListItem.ResponsiblePersonType == DealResponsiblePersonTypeEnum.Underwriter &&
                            IsUnderwriterUser() == true);
                }

                deals = deals.OrderByDescending(x => x.HasUnreadMessage).ThenByDescending(x => x.CreatedTimeStamp)
                    .ToList();

                //================================


            }

            this.StateHasChanged();
        }

        private async Task SelectedPage(int page)
        {
            MetaData.Skip = (page - 1) * MetaData.PageSize;
            using (var task = LoadingManager.Loading("deals", "Clients.Loading".Translate(), ""))
            {
                await SearchAsync(DealSearchModel, MetaData);
            }
        }
    }
}