﻿using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.RequestModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Client.Shared.Components.OfferFormComponents
{
    public partial class BenchInsuranceOfferComponent
    {
        [Parameter]
        public BenchInsuranceOfferModel OfferModel { get; set; }
        [Parameter]
        public DealModel DealModel { get; set; }   
        


        public EditContext EditContext { get; set; }
        [Parameter]
        public EventCallback<EditContext> ModelSet { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; }

        protected override async Task OnInitializedAsync()
        {
            OfferModel ??= new BenchInsuranceOfferModel();
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
