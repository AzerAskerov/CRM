using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;

using Blazored.SessionStorage;
using Blazored.LocalStorage;
using Blazored.Modal;

using Ababil.Components.Core;

using CRM.Client.States;
using CRM.Client.Shared;


namespace CRM.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddScoped<AppState>();
            builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            builder.Services.AddSingleton<IComponentService, ComponentService>();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddBlazoredModal();
            builder.Services.AddApiAuthorization();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredSessionStorage();
            builder.Services.AddMessageBox();
            builder.Services.AddToggle();
            builder.Services.AddAntDesign();
            builder.RootComponents.Add<App>("app");
                       

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
