using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using CRM.Operation.Models;
using CRM.Operation.Models.Login;


namespace CRM.Client.Pages.MenuComponents
{
    public partial class CompanyView
    {
        [Parameter]
        public CompanyListItemModel ClientContractModel { get; set; }


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
