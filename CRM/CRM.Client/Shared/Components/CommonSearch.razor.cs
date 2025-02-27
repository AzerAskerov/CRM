using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using CRM.Client.Pages.MenuComponents;
using CRM.Client.Helpers;

namespace CRM.Client.Shared.Components
{
    public partial class CommonSearch
    {
        [Parameter]
        public EventCallback<int?> ReleatedClientChanged { get; set; }
        [Parameter]
        public EventCallback<RelationContract> CheckForDuplicateChanged { get; set; }

        [Parameter]
        public int? ReleatedClient { get; set; }
        [Parameter]
        public string Place { get; set; }

        private ClientContract selectedClient;
        public string SearchCriteria { get; set; }
        public Dictionary<string, object> additionalProperties = new Dictionary<string, object>();
        public ClientContract SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                if (value != null)
                {
                    ReleatedClient = value.INN;
                    ReleatedClientChanged.InvokeAsync(ReleatedClient);
                    CheckForDuplicateChanged.InvokeAsync(selectedClient.Relations.FirstOrDefault());
                    this.StateHasChanged();
                }
            }
        }

        protected override Task OnInitializedAsync()
        {
            SearchCriteria = "pin";
            additionalProperties.Add("style", Place == "ContactCard" ? "height:54px" : "height:32px");
            return base.OnInitializedAsync();
        }

        public async Task<IEnumerable<ClientContract>> SearchClient(string searchText)
        {
            List<ClientContract> clients = new List<ClientContract>();

            string query = SearchCriteria switch
            {
                "name" => QueryBuildingHelper.BuildNameSearchQuery(searchText),
                "companyname" => QueryBuildingHelper.BuildCompanyNameSearchQuery(searchText),
                "pin" => QueryBuildingHelper.BuildPinSearchQuery(searchText),
                "tin" => QueryBuildingHelper.BuildTinSearchQuery(searchText),
                "number" => QueryBuildingHelper.BuildNumberSearchQuery(searchText),
                "document" => QueryBuildingHelper.BuildDocumentSearchQuery(searchText),
                "asset" => QueryBuildingHelper.BuildAssetSearchQuery(searchText),
                _ => ""
            };

            clients = await State.GetClientInfo<List<ClientContract>>(query);



            return await Task.FromResult(clients);
        }

        private async void OpenClientCard(ClientContract selectClient)
        {
            ComponentParameters param = new ComponentParameters();
            if (selectClient.ClientType == ClientType.Company)
            {
                CompanyListItemModel model = new CompanyListItemModel() { INN = selectClient.INN, ClientType = ClientType.Company };
                param.Add(nameof(CompanyView.ClientContractModel), model);
                Comp.Show<CompanyView>("Company", param);
            }
            else
            {
                ContactListItemModel model = new ContactListItemModel() { INN = selectClient.INN, ClientType = ClientType.Pyhsical };
                param.Add(nameof(ContactView.ClientContractModel), model);
                Comp.Show<ContactView>("Contact", param);
            }
        }
    }
}
