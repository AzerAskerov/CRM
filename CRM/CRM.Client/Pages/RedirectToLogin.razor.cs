using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace CRM.Client.Pages
{
    public partial class RedirectToLoginBase : ComponentBase
    {
        [Inject]
        protected NavigationManager NavManager { get; set; }

        protected override void OnInitialized()
        {
           var returnUrl = NavManager.ToBaseRelativePath(NavManager.Uri);
           Console.WriteLine("LoginRedirectBase.OnInitializedAsync");
            if (string.IsNullOrWhiteSpace(returnUrl))
                NavManager.NavigateTo("login", false);
            else
                NavManager.NavigateTo($"login?returnUrl={returnUrl}", false);
        }

    }
}
