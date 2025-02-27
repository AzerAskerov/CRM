using CRM.Client.States;
using CRM.Operation.Models;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.SourceLoaders;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CRM.Client.Shared.Components.PrintOut.Offers
{
    public partial class PropertyLiabilityPrintOut
    {
      
        [Parameter]
        public DealModel DealModel { get; set; }
        [Parameter]
        public PropertyLiabilityInsuranceOfferModel OfferModel { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected async override Task OnParametersSetAsync()
        {


            var response = await HttpClient.PostAsJsonAsync("api/Deal/GetDealSPecificInfo",
                new DealOfferDropdownDataModel
                {
                    AreaTypeId = OfferModel.InsuranceAreaType != null ? OfferModel.InsuranceAreaType : 0,
                    CurrencyId = OfferModel.CurrencyOid,
                    LangId = DealModel.OfferLanguageOid,
                    PaymentId = OfferModel.PaymentTypeOid,
                    PolicyPeriodId = OfferModel.PolicyPeriodOid
                }
                );


            var responseResult = await response.Content.ReadFromJsonAsync<OperationResult<DealOfferDropdownDataModel>>();

            DealModel.OfferLang = responseResult.Model.Lang;
            OfferModel.PaymentType = responseResult.Model.Payment;
            OfferModel.Currency = responseResult.Model.Currency;
            OfferModel.InsuranceAreaTypeDisplayValue = responseResult.Model.AreaType;
            OfferModel.PolicyPeriod = responseResult.Model.PolicyPeriod;
            base.OnParametersSet();
        }
    }
}
