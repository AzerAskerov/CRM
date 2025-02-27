using System;
using System.Threading.Tasks;

using CRM.Client.States;
using CRM.Operation.Models.Login;
using CRM.Operation.Localization;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Components;

namespace CRM.Client.Pages
{
    public partial class LoginBase : BaseComponent
    {
        protected LoginModel LoginModel = new LoginModel();




        protected async Task LoginAsync(LoginModel loginModel)
        {
            Console.WriteLine("LoginBase.LoginAsync");
            var result = await ((ServerAuthenticationStateProvider)ServerAuthenticationStateTask).Login(loginModel);
            SetState(result);
            if (result.Success)
            {
                var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
                var queries = QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnUrl", out var returnUrl);
                NavManager.NavigateTo(returnUrl.ToString());
                return;
            }

            NavManager.NavigateTo("login");
        }

        protected override async Task OnParametersSetAsync()
        {
            if (ResourceManager.Localized == null)
            {
                //Console.WriteLine(ResourceManager.Localized +  " Getting Localizationn");
                await ResourceManager.GetLocalize(HttpClient, SessionStorage);

            }
        }
    }
}
