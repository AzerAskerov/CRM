using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using CRM.Client.Shared.Components;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Models.Login;
using CRM.Client.States;
using CRM.Operation.Localization;
using Microsoft.JSInterop;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class Invoices
    {
        [CascadingParameter(Name = "ClientContractModel")]
        public ClientContract ClientContractModel { get; set; }

        [CascadingParameter(Name = "RoleModel")]
        private RoleModel RoleModel { get; set; }

        List<InvoiceModel> Model;
        List<InvoiceModel> invoices = Enumerable.Empty<InvoiceModel>().ToList();
        private decimal NotPaid = 0m;
        private decimal Paid = 0m;
        private int Count = 0;

        public MetaData MetaData { get; set; } = new MetaData() { PageSize = 10, CurrentPage = 1, Skip = 0 };
        protected override async Task OnParametersSetAsync()
        {

            if (!ClientContractModel.INN.HasValue) return;

            using (var task = LoadingManager.Loading("invoices", "Clients.Loading".Translate(), ""))
            {
                var result = await State.GetInvoices(ClientContractModel);
                if (result.Success)
                {
                    MetaData.TotalPages = result.Model.Count;
                    Count = @MetaData.TotalPages;
                    Paid = result.Model.Sum(x => x.AmountPaid);
                    NotPaid = result.Model.Where(x => (x.StatusCode == Data.Enums.InvoiceStatusCode.Create || x.StatusCode == Data.Enums.InvoiceStatusCode.PartiallyPaid)).Sum(x => x.AmountTotal) - Paid;
                    Model = result.Model;
                    invoices = result.Model.OrderByDescending(x => x.CreateTimeStamp).ToList();
                }
            }
        }

        bool loaded = false;
        protected override async Task OnInitializedAsync()
        {
            await OnParametersSetAsync();
            loaded = true;
        }
        protected override async Task OnAfterRenderAsync(bool firstrender)
        {

            if (loaded)
            {
                loaded = false;

                await JsRuntime.InvokeVoidAsync("ApplyjQueryDatatable");
            }
        }

    }
}
