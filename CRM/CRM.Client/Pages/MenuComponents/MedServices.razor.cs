using CRM.Client.Shared.Components;
using CRM.Client.States;
using CRM.Operation.Enums;
using CRM.Operation.Localization;
using CRM.Operation.Models;
using CRM.Operation.Models.Login;
using CRM.Operation.Models.RequestModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class MedServices
    {
        [CascadingParameter(Name = "ClientContractModel")]
        public ClientContract ClientContractModel { get; set; }

        [CascadingParameter(Name = "RoleModel")]
        private RoleModel RoleModel { get; set; }

        List<MedicineModel> Model;
        List<MedicineModel> services = Enumerable.Empty<MedicineModel>().ToList();
        private int UserClinicCount = 0;
        private DateTime? LastServiceDate;

        public MetaData MetaData { get; set; } = new MetaData() { PageSize = 10, CurrentPage = 1, Skip = 0 };

        protected override async Task OnParametersSetAsync()
        {

            using (var task = LoadingManager.Loading("medservices", "Clients.Loading".Translate(), ""))
            {
                var result = await State.GetMedServices(ClientContractModel.INN.Value);
                if (result.Success)
                {
                    MetaData.TotalPages = result.Model != null ? result.Model.Count : 0;
                    UserClinicCount = @MetaData.TotalPages;
                    LastServiceDate = result.Model.Max(x => x.ServiceDate);
                    Model = result.Model;
                    services = result.Model.OrderByDescending(x => x.ServiceDate).Skip(MetaData.Skip).Take(MetaData.PageSize).ToList();
                }
            }

            
        }

        private async Task SelectedPage(int page)
        {
            MetaData.Skip = (page - 1) * MetaData.PageSize;
            using (var task = LoadingManager.Loading("medservices", "Clients.Loading".Translate(), ""))
            {
                services = Model.OrderByDescending(x => x.ServiceDate).Skip(MetaData.Skip).Take(MetaData.PageSize).ToList();
            }
        }
    }
}
