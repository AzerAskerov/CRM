using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Client.Shared
{
    public partial class SlidePanelTemplate
    {
        [Inject] private IComponentService ComponentService { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IJSRuntime Js { get; set; }

        private readonly Collection<ComponentReference> Modals = new Collection<ComponentReference>();


        protected override void OnInitialized()
        {
            ((ComponentService)ComponentService).OnModalInstanceAdded += Update;
           // ((ComponentService)ComponentService).OnModalCloseRequested += CloseInstance;
            NavigationManager.LocationChanged += CancelModals;

        }


        private async void CancelModals(object sender, LocationChangedEventArgs e)
        {
            Modals.Clear();
            await InvokeAsync(StateHasChanged);
        }

        private async void Update(ComponentReference modalReference)
        {
            Modals.Add(modalReference);
            await InvokeAsync(StateHasChanged);
            await Js.InvokeVoidAsync("OpenAddNew");
        }



        internal void CloseInstance(ComponentReference modal)
        {
            DismissInstance(modal.Id);
        }

        internal async  Task  CloseInstance(Guid Id)
        {
            DismissInstance(Id);
            await Js.InvokeVoidAsync("CloseAddNew");
        }


        internal void DismissInstance(Guid Id)
        {
            var reference = Modals.SingleOrDefault(x => x.Id == Id);

            if (reference != null)
            {
                //reference.Dismiss(result);
                Modals.Remove(reference);
                StateHasChanged();
            }
            
        }
    }
}
