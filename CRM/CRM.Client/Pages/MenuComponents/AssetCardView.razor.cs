using CRM.Client.States;
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
    public partial class AssetCardView
    {
        [CascadingParameter]
        public CRM.Client.Shared.ComponentInstance BaseCompo { get; set; }

        [CascadingParameter(Name = "ClientContractModel")]
        public ClientContract ClientContractModel { get; set; }

        [CascadingParameter(Name = "RoleModel")]
        private RoleModel RoleModel { get; set; }
        protected List<AssetViewModel> Assets { get; set; }= Enumerable.Empty<AssetViewModel>().ToList();

        protected override async Task OnInitializedAsync()
        {
            if (!ClientContractModel.INN.HasValue) return;
            using (var task = LoadingManager.Loading("assets", "Clients.Loading".Translate(), ""))
            {
                Assets = await State.GetAssets(ClientContractModel);

            }


        }

    }
}
