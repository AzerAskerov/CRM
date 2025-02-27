using System;
using System.Threading.Tasks;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.RequestModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace CRM.Client.Shared.Components.OfferFormComponents
{
    public partial class LiabilityInsuranceOfferComponent
    {
        [Parameter]
        public LiabilityInsuranceOfferModel OfferModel { get; set; }
        [Parameter]
        public DealModel DealModel { get; set; }



        public EditContext EditContext { get; set; }
        [Parameter]
        public EventCallback<EditContext> ModelSet { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; }

        protected override async Task OnInitializedAsync()
        {
            OfferModel ??= new LiabilityInsuranceOfferModel();
            EditContext = new EditContext(OfferModel);
            await ModelSet.InvokeAsync(EditContext);
            await base.OnInitializedAsync();
        }

        private void BeneficiarySelected(ClientContract clientContract)
        {
            OfferModel.Beneficiary = clientContract;
        }
    }
}