using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CRM.Client.Shared;
using CRM.Client.Shared.Components;
using CRM.Client.States;
using CRM.Operation.Localization;
using CRM.Operation.Models;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class Index
    {
        List<ContactListItemModel> BirthdayClients = new List<ContactListItemModel>();

        public MetaData MetaData { get; set; } = new MetaData() { PageSize = 10, CurrentPage = 1, Skip = 0 };

        List<Button<ContactListItemModel>> actions = new List<Button<ContactListItemModel>>();

        private async Task GetBirthdayClients(MetaData metaData)
        {
            using (LoadingManager.Loading("clients", "Clients.Loading".Translate(), ""))
            {
                BirthdayClients = await State.BirthdayPersons(MetaData);
                
                foreach (var client in BirthdayClients)
                {
                    if (client.FullName.IsNullOrEmpty())
                    {
                        client.FullName = client.FullName;
                    }

                    client.Age = DateTime.Today.Year - client.BirthDate.Value.Year;
                }

                Button<ContactListItemModel> button = new Button<ContactListItemModel>()
                {
                    ActionName = OpenContractCard,
                    Icon = "",
                    Name = "",
                    Text = ""
                };
            
                actions.Add(button);
                
                StateHasChanged();
            }
        }

        protected async void OpenContractCard(ContactListItemModel c)
        {
            c.ClientType = Operation.Models.RequestModels.ClientType.Pyhsical;
            ComponentParameters param = new ComponentParameters();
            param.Add(nameof(ContactView.ClientContractModel), c);
            Comp.Show<ContactView>("Contact", param);
        }
        
        private async Task SelectedPage(int page)
        {
            MetaData.Skip = (page - 1) * MetaData.PageSize;

            using (LoadingManager.Loading("clients", "Clients.Loading".Translate(), ""))
            {
                await GetBirthdayClients(MetaData);
            }
        }
    }
}