using CRM.Client.Shared.Components;
using CRM.Client.States;
using CRM.Operation.Models.Login;
using CRM.Operation.Models.RequestModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Operation.Localization;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class ParticipatedInsuredPersonPolicies
    {
        [CascadingParameter(Name = "ClientContractModel")]
        private ClientContract ClientContractModel { get; set; }

        [CascadingParameter(Name = "RoleModel")]
        private RoleModel RoleModel { get; set; }

        private List<ParticipatedInsuredPersonPoliciesViewModel> policies = Enumerable.Empty<ParticipatedInsuredPersonPoliciesViewModel>().ToList();
        private MetaData MetaData { get; set; } = new MetaData() { PageSize = 10, CurrentPage = 1, Skip = 0 };
        private List<ParticipatedInsuredPersonPoliciesViewModel> Model = new List<ParticipatedInsuredPersonPoliciesViewModel>();

        private List<ParticipatedInsuredPersonPoliciesViewModel> InforceProductList = new List<ParticipatedInsuredPersonPoliciesViewModel>();
        private List<ParticipatedInsuredPersonPoliciesViewModel> InOtherStatusProductList = new List<ParticipatedInsuredPersonPoliciesViewModel>();

        protected override async Task OnParametersSetAsync()
        {
            if (!ClientContractModel.INN.HasValue) return;

            using (var task = LoadingManager.Loading("policies", "Clients.Loading".Translate(), ""))
            {
                var result = await State.ParticipatedInsuredPersonPolicies(ClientContractModel);
                if (result != null && result.Success && result.Model != null && result.Model.Count > 0)
                {
                    Model = result?.Model.OrderBy(x => x.StartDate).ToList();
                    policies = Model.OrderByDescending(x => x.StartDate).Skip(MetaData.Skip).Take(MetaData.PageSize).ToList();

                    if (Model != null && Model.Count > 0)
                    {
                        InforceProductList.Clear(); InOtherStatusProductList.Clear();

                        var grouped = result.Model.GroupBy(x => new { x.PolicyType, x.LocalStatus }).Select(x => x.FirstOrDefault());
                        InforceProductList.AddRange(grouped.Where(x => x.LocalStatus == Data.Enums.PolicyStatusLocal.InForce));

                        InOtherStatusProductList.AddRange(grouped.Where(x => x.LocalStatus != Data.Enums.PolicyStatusLocal.InForce).GroupBy(x => new { x.PolicyType }).Select(x => x.FirstOrDefault()));
                    }

                }
            }
        }

        private void SelectedPage(int page)
        {
            MetaData.Skip = (page - 1) * MetaData.PageSize;
            policies = Model.OrderByDescending(x => x.StartDate).Skip(MetaData.Skip).Take(MetaData.PageSize).ToList();
        }
    }
}
