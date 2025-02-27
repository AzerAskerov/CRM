

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using CRM.Operation.Models.RequestModels;
using CRM.Operation.Models.Login;
using CRM.Operation.Models;
using CRM.Client.Shared.Components;
using CRM.Client.States;
using CRM.Operation.Localization;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class Claims
    {
        [CascadingParameter(Name = "ClientContractModel")]
        public ClientContract ClientContractModel { get; set; }

        [CascadingParameter(Name = "RoleModel")]
        private RoleModel RoleModel { get; set; }

        ClaimModel Model = new ClaimModel() { NonClaimDayCount = 0, AccidentCount = 0};
        List<ClaimListItemModel> claims = Enumerable.Empty<ClaimListItemModel>().ToList();
        private int Count = 0;

        public MetaData MetaData { get; set; } = new MetaData() { PageSize = 10, CurrentPage = 1, Skip = 0 };
        protected override async Task OnParametersSetAsync()
        {
            if (!ClientContractModel.INN.HasValue) return;

            using (var task = LoadingManager.Loading("claims", "Clients.Loading".Translate(), ""))
            {
                var result = await State.GetClaims(ClientContractModel);
                if (result.Success)
                {
                    MetaData.TotalPages = result.Model.AccidentCount.HasValue ? result.Model.AccidentCount.Value : 0;
                    Count = @MetaData.TotalPages;

                    Model = result.Model;
                    claims = result.Model.Claims.OrderByDescending(x => x.AccidentDate).Skip(MetaData.Skip).Take(MetaData.PageSize).ToList();
                }
            }
        }

        private async Task SelectedPage(int page)
        {
            MetaData.Skip = (page - 1) * MetaData.PageSize;

            using (var task = LoadingManager.Loading("claims", "Clients.Loading".Translate(), ""))
            {
                claims = Model.Claims.OrderByDescending(x => x.AccidentDate).Skip(MetaData.Skip).Take(MetaData.PageSize).ToList();
            }
        }
    }
}
