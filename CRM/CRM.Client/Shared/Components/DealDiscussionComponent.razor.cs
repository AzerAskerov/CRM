using System;
using CRM.Operation.Models.DealOfferModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CRM.Client.Shared.Components
{
    public partial class DealDiscussionComponent
    {
        [Parameter]
        public DealDiscussionModel DealDiscussionModel { get; set; }
        
        [Parameter]
        public bool ReadOnly { get; set; }
        
        [Parameter]
        public bool SubmitDiscussionButtonLoading { get; set; }

        [Parameter]
        public Action<MouseEventArgs> DiscussionSendClickEvent { get; set; }
    }
}