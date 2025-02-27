
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CRM.Client.Shared.Layout
{
    public class MainLayoutBase : LayoutComponentBase
    {
        //[CascadingParameter]
        //Task<AuthenticationState> AuthenticationStateTask { get; set; }

        //[Inject]
        //protected NavigationManager NavManager { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("loadscripts");
        }

    }
}
