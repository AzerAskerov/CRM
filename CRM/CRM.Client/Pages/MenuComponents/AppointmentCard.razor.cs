using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.JSInterop;

using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using Microsoft.AspNetCore.Components;
using CRM.Client.States;
using Zircon.Core.Extensions;
using CRM.Data.Database;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class AppointmentCard
    {
        public bool ShowDefaultOption { get; set; } = true;
        public List<CodeViewModel> TypeList { get; set; } = new List<CodeViewModel>();

        private AppointmentModel Model = new AppointmentModel() { Status = "0" };

        public Dictionary<string, object> additionalProperties = new Dictionary<string, object>();
        public EditContext AppContext { get; set; }

        public bool Expired { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            TypeList = await State.AppointmentTypes();
            AppContext = new EditContext(Model);
            await base.OnInitializedAsync();

        }

        private string SearchCriteria { get; set; } = "pin";

        private async Task Close()
        {
            await JS.InvokeAsync<object>("appointments.closeform", null);
        }

        private async Task Save(AppointmentModel model)
        {
            await JS.InvokeAsync<object>("appointments.saveform", model);
        }
        private async Task Delete()
        {
            await JS.InvokeAsync<object>("appointments.deleteEvent", null);
        }

        private void OnSearchCriteriaChanged(ChangeEventArgs e)
        {
            SearchCriteria = e.Value.ToString();
        }

        public async void OnInnChange(ChangeEventArgs e)
        {
            string inn = e.Value.ToString();
            Model.SelectedClient = null;

            if (!string.IsNullOrEmpty(inn))
            {
                Model.SelectedClient = await LoadSelectedClient(Convert.ToInt32(inn));
                Model.SelectedClient.INN = Convert.ToInt32(inn);
                AppContext.NotifyFieldChanged(AppContext.Field("SelectedClient"));
            }
            this.StateHasChanged();
        }

        public void OnTitleChange(ChangeEventArgs e)
        {
            Model.text = e.Value.ToString();
        }


        public async Task<IEnumerable<ClientAppointmentModel>> SearchClient(string searchText)
        {
            List<ClientAppointmentModel> clients = new List<ClientAppointmentModel>();
            
            string query = "";

            if (SearchCriteria == "name")
                query = $"api/searchphysicalperson?$select=inn,pinnumber,firstname,lastname,fathername&$filter= contains(fullname, '{searchText}')&$expand=tags($select = name, id)";
            else if (SearchCriteria == "companyname")
                query = $"api/searchcompany?$select=inn,companyname&$filter= contains(companyname, '{searchText}')&$expand=tags($select = name, id)";
            else if (SearchCriteria == "pin")
                query = $"api/searchphysicalperson?$select=inn,pinnumber,firstname,lastname,fathername&$filter=pinnumber eq '{searchText}'&$expand=tags($select = name, id)";
            else if (SearchCriteria == "tin")
                if(searchText.EndsWith("1"))
                    query = $"api/searchcompany?$select=inn,companyname,firstname,lastname,fathername,clienttype&$filter=tolower(tinnumber) eq '{searchText.ToLower()}'&$expand=tags($select = name, id)";
                else
                    query = $"api/searchphysicalperson?$select=inn,pinnumber,firstname,lastname,fathername&$filter=tolower(tinnumber) eq '{searchText.ToLower()}'&$expand=tags($select=name,id)";
            else if (SearchCriteria == "number")
                query = $"api/clientcontract?$select= firstname,lastname,fathername,companynameINN,clienttype,&$expand=tags($select=name,id)&$filter= contactsinfo/any(t:t/value eq '{searchText}' )";
            else if (SearchCriteria == "document")
                query = $"api/clientcontract?$select= firstname,lastname,fathername,companyname,INN,clienttype&$filter=documents/any(t:t/documentNumber eq '{searchText}')&$expand=tags($select=name,id)";

            clients = await State.GetClientInfo<List<ClientAppointmentModel>>(query);

            return clients;
        }

        private int? ConvertPerson(ClientAppointmentModel client)
        {
            return client?.INN;
        }

        private async Task<ClientAppointmentModel> LoadSelectedClient(int? inn)
        {
            string query = $"api/clientcontract({inn})?$select= firstname,lastname,fathername,companyname,INN";
            ClientAppointmentModel client = await State.GetClientInfo<ClientAppointmentModel>(query);
            return client;
        }
    }
}
