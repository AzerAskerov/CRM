
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using CRM.Operation.Localization;
using CRM.Client.Models;
using CRM.Client.Shared.Components;
using CRM.Operation.Models;
using CRM.Client.Shared;
using CRM.Client.States;
using System;
using Microsoft.AspNetCore.Components;
using CRM.Operation.Models.Login;


namespace CRM.Client.Pages.MenuComponents
{
    public partial class Contacts
    {
        [Parameter] public string INN { get; set; }
        ContactSearchModel ContactSearchModel { get; set; } = new ContactSearchModel();

        List<ContactListItemModel> clients = Enumerable.Empty<ContactListItemModel>().ToList();

        public MetaData MetaData { get; set; } = new MetaData() { PageSize = 10, CurrentPage = 1,   Skip = 0 };

        List<Button<ContactListItemModel>> actions = new List<Button<ContactListItemModel>>();

        protected override async Task OnParametersSetAsync()
        {
            using (var task = LoadingManager.Loading("clients", "Clients.Loading".Translate(), ""))
            {
             clients = await State.PhysicalPerson(" ", MetaData);

            Button<ContactListItemModel> button = new Button<ContactListItemModel>()
            {
                ActionName = OpenContractCard,
                Icon = "",
                Name = "",
                Text = ""
            };
            
            actions.Add(button);

                if (!string.IsNullOrEmpty(INN))
                {
                    var contact = new ContactListItemModel
                    {
                        INN = Convert.ToInt32(INN)
                    };
                    RoleModel roleModel = await State.IsOwnClient(contact);
                    if (!roleModel.IsOwnClient)
                    {
                        MessageBoxService.ShowError(LocalizationExtension.Translate("MessageBox.ContactNotFound"));
                        return;

                    }
                    OpenContractCard(contact);
                }
            }

            
        }



        protected async void OpenContractCard(ContactListItemModel c)
        {
            c.ClientType = Operation.Models.RequestModels.ClientType.Pyhsical;
            ComponentParameters param = new ComponentParameters();
            param.Add(nameof(ContactView.ClientContractModel), c);
            Comp.Show<ContactView>("Contact", param);
        }

        private async Task SearchContactAsync(ContactSearchModel model)
        {
            MetaData.CurrentPage = 1;
            MetaData.PageSize = 10;
            MetaData.Skip = 0;

             await GetContacts(model, MetaData);
        }

        private async Task GetContacts(ContactSearchModel model, MetaData metaData)
        {
            #region Filter
            //string expand = "";
            string filter = "";
            if (!string.IsNullOrEmpty(model.DocumentNumber))
            {
                using (var task = LoadingManager.Loading("clients", "Clients.Loading".Translate(), ""))
                {
                    var client = await State.PhysicalPersonByDocumentNumber(model.DocumentNumber);
                    clients = client;
                }

                StateHasChanged();
                return;
            }

            if (!string.IsNullOrEmpty(model.FullName))
                filter += $"and contains(fullname, '{model.FullName}')";

            if (!string.IsNullOrEmpty(model.PinNumber))
                filter += $" and pinnumber eq '{model.PinNumber}'";

            if (!string.IsNullOrEmpty(model.RegNumber))
                filter += $" and regnumber eq '{model.RegNumber}'";

            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                //expand += expand.Contains("expand") ? ",contactsinfo" : $"&$expand=contactsinfo";
                filter += $"and contactsinfo/any(t:t/value eq '{model.PhoneNumber}' and t/type eq 1)";
            }

            string query = $"{filter}";/*{expand}*/

            //if (string.IsNullOrEmpty(query)) return;
            #endregion
           
            
     

            using (var task = LoadingManager.Loading("clients", "Clients.Loading".Translate(), ""))
            {
                clients = await State.PhysicalPerson(query, MetaData);
            }

            this.StateHasChanged();
        }

        private async Task SelectedPage(int page)
        {
            MetaData.Skip = (page - 1) * MetaData.PageSize;

            using (var task = LoadingManager.Loading("clients", "Clients.Loading".Translate(), ""))
            {
                await GetContacts(ContactSearchModel, MetaData);
            }
        }
    }
}
