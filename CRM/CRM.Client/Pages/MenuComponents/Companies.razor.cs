using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CRM.Client.Models;
using CRM.Client.Shared;
using CRM.Client.Shared.Components;
using CRM.Client.States;
using CRM.Operation.Localization;
using CRM.Operation.Models;
using CRM.Operation.Models.Login;
using Microsoft.AspNetCore.Components;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class Companies
    {
        [Parameter] public string INN { get; set; }
        CompanySearchModel CompanySearchModel { get; set; } = new CompanySearchModel();
        List<CompanyListItemModel> companies = Enumerable.Empty<CompanyListItemModel>().ToList();

        List<Button<CompanyListItemModel>> actions = new List<Button<CompanyListItemModel>>();
        public MetaData MetaData { get; set; } = new MetaData() { PageSize = 10, CurrentPage = 1, Skip = 0 };
        protected override async Task OnParametersSetAsync()
        {
            using (var task = LoadingManager.Loading("companies", "Clients.Loading".Translate(), ""))
            {
                companies = await State.Companies("", MetaData);

                Button<CompanyListItemModel> button = new Button<CompanyListItemModel>()
                {
                    ActionName = OpenCompanyCard,
                    Icon = "",
                    Name = "",
                    Text = ""
                };

                actions.Add(button);


                if (!string.IsNullOrEmpty(INN))
                {
                    var company = new CompanyListItemModel
                    {
                        INN = Convert.ToInt32(INN)
                    };
                    RoleModel roleModel = await State.IsOwnClient(company);
                    if (!roleModel.IsOwnClient)
                    {
                        MessageBoxService.ShowError(LocalizationExtension.Translate("MessageBox.CompanyNotFound"));
                        return;
                        
                    }
                    OpenCompanyCard(company);
                }

            }
        }
       
        protected async void OpenCompanyCard(CompanyListItemModel c)
        {
            c.ClientType = Operation.Models.RequestModels.ClientType.Company;
            ComponentParameters param = new ComponentParameters();
            param.Add(nameof(CompanyView.ClientContractModel), c);
            Compo.Show<CompanyView>("Company", param);
        }

        private async Task SearchAsync(CompanySearchModel model, MetaData metaData)
        {
            //string expand = "";
            string filter = "";

            if (!string.IsNullOrEmpty(model.FullName))
                filter += $"and contains(fullname, '{model.FullName}')";

            if (!string.IsNullOrEmpty(model.TinNumber))
                filter += $" and tinnumber eq '{model.TinNumber}'";

            if (!string.IsNullOrEmpty(model.RegNumber))
                filter += $" and regnumber eq '{model.RegNumber}'";

            if (!string.IsNullOrEmpty(model.DocumentNumber))
            {
                //expand = $"&$expand=documents";
                filter += $"and documents/any(t:t/documentnumber eq '{model.DocumentNumber}')";
            }

            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                //expand += expand.Contains("expand") ? ",contactsinfo" : $"&$expand=contactsinfo";
                filter += $"and contactsinfo/any(t:t/value eq '{model.PhoneNumber}' and t/type eq 1)";
            }

            string query = $"{filter}";/*{expand}*/

            //if (string.IsNullOrEmpty(query)) return;

            using (var task = LoadingManager.Loading("companies", "Clients.Loading".Translate(), ""))
            {
                companies = await State.Companies(query, MetaData);
            }

            this.StateHasChanged();
        }

        private async Task SelectedPage(int page)
        {
            MetaData.Skip = (page - 1) * MetaData.PageSize;
            using (var task = LoadingManager.Loading("companies", "Clients.Loading".Translate(), ""))
            {
                await SearchAsync(CompanySearchModel, MetaData);
            }
        }
    }
}
