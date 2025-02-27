using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;

using Ababil.Components.Core.Services;
using Blazored.Modal.Services;
using Blazored.SessionStorage;

using CRM.Operation.Localization;
using CRM.Client.States;
using Blazored.LocalStorage;

namespace CRM.Client
{
    public class BaseComponent : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected NavigationManager NavManager { get; set; }

        [Inject]
        protected AppState State { get; set; }

        [Inject]
        protected IJSRuntime JS { get; set; }

        [Inject]
        protected IMessageBoxService MessageBoxService { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        protected AuthenticationStateProvider ServerAuthenticationStateTask { get; set; }

        [Inject]
        protected IModalService Modal { get; set; }

        [Inject]
        protected ISessionStorageService SessionStorage { get; set; }

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        public void Render(RenderTreeBuilder builder)
        {
            BuildRenderTree(builder);
        }

        public void SetState(OperationResult result)
        {
            try
            {
                OperationResultBoxBase.SetResult(MessageBoxService, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SetState: " + ex.ToString());
            }

        }

        public void SetState<T>(OperationResult<T> result)
        {

            Console.WriteLine("MessageBoxBase");
            try
            {
                Console.WriteLine("SetState<T>" + MessageBoxService != null);
                OperationResultBoxBase.SetResult(MessageBoxService, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SetState: " + ex.ToString());
            }
        }

        protected async override Task OnInitializedAsync()
        {
            if (ResourceManager.Localized == null)
                await ResourceManager.GetLocalize(HttpClient, SessionStorage);
        }

    }
}
