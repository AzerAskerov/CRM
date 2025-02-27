using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Client.Shared.Components
{
    public partial class MainMenuLinkBase : ComponentBase,IDisposable
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Href { get; set; }



        [Parameter]
        public string ActiveClass { get; set; }

        [Parameter]
        public MainMenuLinkMatch HrefMatch { get; set; } = MainMenuLinkMatch.Exact;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter]
        public RouteData RouteData { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs e)
            => StateHasChanged();

        protected bool IsActive()
        {
            string hr = Href;
            string relUri = NavigationManager.Uri;
            string currentUrl = ""+ NavigationManager.ToBaseRelativePath(NavigationManager.Uri);

                if(currentUrl == null)
            {
                currentUrl = "";
            }
            
            if ((HrefMatch == MainMenuLinkMatch.Exact && Href == currentUrl) || (HrefMatch == MainMenuLinkMatch.StartsWith && currentUrl.StartsWith(Href)))
                return true;

            return false;
        }
    }
}
