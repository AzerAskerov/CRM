using Blazored.Modal;
using Blazored.Modal.Services;
using CRM.Client.States;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Client.Pages
{
    public class LogoutBase : BaseComponent
    {
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }

        protected void Close()
        {
            Task.Run(async () => await ((ServerAuthenticationStateProvider)ServerAuthenticationStateTask).Logout());
            BlazoredModal.Close(ModalResult.Ok(true));
        }
        protected void Cancel() => BlazoredModal.Cancel();
    }
}
