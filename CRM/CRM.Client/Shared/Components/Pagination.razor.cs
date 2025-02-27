using CRM.Operation.Localization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Client.Shared.Components
{
    public partial class Pagination
    {
        [Parameter]
        public MetaData MetaData { get; set; }
        [Parameter]
        public int Spread { get; set; }
        [Parameter]
        public EventCallback<int> SelectedPage { get; set; }

        private List<PagingLink> _links;

        
        protected override void OnParametersSet()
        {
            CreatePaginationLinks();
        }
        private void CreatePaginationLinks()
        {
            _links = new List<PagingLink>();
            _links.Add(new PagingLink(MetaData.CurrentPage - 1, MetaData.HasPrevious, "Pagination.Previous".Translate()));
            int count = (int)Math.Ceiling(MetaData.TotalPages / (double)MetaData.PageSize);
            for (int i = 1; i <= count; i++)
            {
                if (i >= MetaData.CurrentPage - Spread && i <= MetaData.CurrentPage + Spread)
                {
                    _links.Add(new PagingLink(i, true, i.ToString()) { Active = MetaData.CurrentPage == i });
                }
            }
            _links.Add(new PagingLink(MetaData.CurrentPage + 1, MetaData.HasNext, "Pagination.Next".Translate()));
        }
        private async Task OnSelectedPage(PagingLink link)
        {
            if (link.Page == MetaData.CurrentPage || !link.Enabled)
                return;
            MetaData.CurrentPage = link.Page;
            await SelectedPage.InvokeAsync(link.Page);
        }
    }
}
