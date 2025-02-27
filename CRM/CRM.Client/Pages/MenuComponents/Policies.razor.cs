
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
using System.Security.Cryptography.X509Certificates;
using Microsoft.JSInterop;
using System;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class Policies
    {
        [CascadingParameter(Name = "ClientContractModel")]
        private ClientContract ClientContractModel { get; set; }

        [CascadingParameter(Name = "RoleModel")]
        private RoleModel RoleModel { get; set; }

        private List<ProductBaseModel> policies = Enumerable.Empty<ProductBaseModel>().ToList();
        private List<ProductBaseModel> InforceProductList = new List<ProductBaseModel>();
        private List<ProductBaseModel> InOtherStatusProductList = new List<ProductBaseModel>();
        private List<ProductBaseModel> Model;

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (!ClientContractModel.INN.HasValue) return;

            using (var task = LoadingManager.Loading("policies", "Clients.Loading".Translate(), ""))
            {
                var result = await State.GetPolicies(ClientContractModel);
                if (result.Success)
                {
                    Model = result.Model.OrderBy(x => x.CreatedDate).ToList();
                    policies = Model.OrderByDescending(x => x.CreatedDate).ToList();
                    InforceProductList.Clear(); InOtherStatusProductList.Clear();

                     var grouped = result.Model.GroupBy(x => new { x.Product, x.PolicyStatusLocal }).Select(x => x.FirstOrDefault());
                    InforceProductList.AddRange(grouped.Where(x => x.PolicyStatusLocal == Data.Enums.PolicyStatusLocal.InForce));

                   
                    InOtherStatusProductList.AddRange(grouped.Where(x => x.PolicyStatusLocal != Data.Enums.PolicyStatusLocal.InForce &&
                    !InforceProductList.Any(y => y.Product == x.Product)).ToList().GroupBy(x => x.Product).Select(x => x.FirstOrDefault()));
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

        protected async Task NavigateToPolicy(ProductBaseModel policy)
        {
            var result = await HttpClient.PostJsonAsync<ProductBaseModel, OperationResult<PolicyActionGuidModel>>("api/Policy/OpenPolicy", policy);

            if (result.Success)
            {
                var model = result.Model;
                var url = model.BaseUrl + "Common/Policy/Open/" + model.PolicyActionGuid.ToString(); // Convert ActionGuid to string if not null
                await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            }
            else
            {
                MessageBoxService.ShowError(LocalizationExtension.Translate("PolicyNotFound"));
                SetState(result);
                StateHasChanged();
            }

        }
    }
}
