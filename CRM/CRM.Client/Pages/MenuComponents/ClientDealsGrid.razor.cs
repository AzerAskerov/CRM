
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
using CRM.Operation.Models.DealOfferModels;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class ClientDealsGrid
    {
        [CascadingParameter(Name = "ClientContractModel")]
        private ClientContract ClientContractModel { get; set; }

        [CascadingParameter(Name = "RoleModel")]
        private RoleModel RoleModel { get; set; }

        List<DealModel> Model;
        List<DealModel> deals = Enumerable.Empty<DealModel>().ToList();
        public MetaData MetaData { get; set; } = new MetaData() {PageSize = 200, CurrentPage = 1, Skip = 0};

        protected override async Task OnParametersSetAsync()
        {
            using (var task = LoadingManager.Loading("deals", "Clients.Loading".Translate(), ""))
            {
                var result = await State.GetDeals(ClientContractModel.INN.Value);
                if (result.Success)
                {
                    MetaData.TotalPages = result.Model != null ? result.Model.Count : 0;
                    Model = result.Model;
                    deals = result.Model.OrderByDescending(x => x.CreatedTimeStamp).Skip(MetaData.Skip)
                        .Take(MetaData.PageSize).ToList();
                }
            }
        }

        private async Task SelectedPage(int page)
        {
            MetaData.Skip = (page - 1) * MetaData.PageSize;
            using (var task = LoadingManager.Loading("deals", "Clients.Loading".Translate(), ""))
            {
                deals = Model.OrderByDescending(x => x.CreatedTimeStamp).Skip(MetaData.Skip).Take(MetaData.PageSize)
                    .ToList();
            }
        }
    }
}
