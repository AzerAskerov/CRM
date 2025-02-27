using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using CRM.Operation.Models;
using CRM.Operation.Models.Login;
using System;
using Zircon.Core.Config;
using CRM.Operation.Localization;
using CRM.Client.Shared;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class ContactView
    {
        [Parameter]
        public RoleModel RoleModelParam { get; set; }
        [Parameter]
        public string inn { get; set; }
        [Parameter]
        public ContactListItemModel ClientContractModel { get; set; }

        
        private RoleModel RoleModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            RoleModel = await State.IsOwnClient(ClientContractModel);
            var user = (await AuthenticationStateTask).User;
            if (user.IsInRole("CRMRole"))
            {
                var d = user.FindFirst(x => x.Type == ClaimTypes.Role && x.Value == "CRMRole");


                RoleModel.Basic = bool.Parse(d.Properties["Basic"]);
                RoleModel.Full = bool.Parse(d.Properties["Full"]);
            }
        }
    }
}
