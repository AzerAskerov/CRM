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
    public partial class BenchPrintOut
    {
        [Parameter]
        public DealModel DealModel { get; set; }
        [Parameter]
        public BenchInsuranceOfferModel OfferModel { get; set; }

        protected async override Task OnParametersSetAsync()
        {


            var responseGeneral = await HttpClient.PostAsJsonAsync("api/Deal/GetDealSPecificInfo",
                new DealOfferDropdownDataModel
                {
                    AreaTypeId = OfferModel.InsuranceAreaType,
                    CurrencyId = OfferModel.CurrencyOid,
                    LangId = DealModel.OfferLanguageOid,
                    PaymentId = OfferModel.PaymentTypeOid,
                    PolicyPeriodId = OfferModel.PolicyPeriodOid
                }
                );


            var responseGeneralResult = await responseGeneral.Content.ReadFromJsonAsync<OperationResult<DealOfferDropdownDataModel>>();

            DealModel.OfferLang = responseGeneralResult.Model.Lang;
            OfferModel.PaymentType = responseGeneralResult.Model.Payment;
            OfferModel.Currency = responseGeneralResult.Model.Currency;
            OfferModel.InsuranceAreaTypeDisplayValue = responseGeneralResult.Model.AreaType;
            OfferModel.PolicyPeriod = responseGeneralResult.Model.PolicyPeriod;


            var responseProperty = await HttpClient.PostAsJsonAsync("api/Deal/GetMovableNImmovableProperty",
              new DealOfferDropdownDataModel
              {
                  MovableTypeOid = OfferModel.MovablePropertyTypeOid,
                  ImmovableTypeOid = OfferModel.ImmovablePropertyTypeOid,
              }
              );


            var responsePropertyResult = await responseProperty.Content.ReadFromJsonAsync<OperationResult<DealOfferDropdownDataModel>>();

            OfferModel.MovablePropertyType = responsePropertyResult.Model.MovableType;
            OfferModel.ImmovablePropertyType = responsePropertyResult.Model.ImmovableType;
            base.OnParametersSet();
        }

    }
}
