using System;
using System.Net.Http;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using Blazored.SessionStorage;



namespace CRM.Client.Shared.Layout
{
    public partial class LoginLayout
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        [Inject]
        private ISessionStorageService SessionStorage { get; set; }

        protected override void OnInitialized()
        {
            CultureInfo culture = new CultureInfo("az");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
