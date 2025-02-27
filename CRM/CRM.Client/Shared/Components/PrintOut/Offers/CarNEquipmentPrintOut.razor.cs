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
using Zircon.Core;

namespace CRM.Client.Shared.Components.PrintOut.Offers
{
    public partial class CarNEquipmentPrintOut
    {
        [Parameter]
        public List<InsuredVehicleModel> Vehicles { get; set; }
        [Parameter]
        public List<SourceItem> VehModels { get; set; }
        [Parameter]
        public List<SourceItem> VehBrands { get; set; }

        [Parameter]
        public DealModel DealModel { get; set; }
        [Parameter]
        public CarAndEquipmentInsuranceOfferModel OfferModel { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected async override Task OnParametersSetAsync()
        {


            if (DealModel != null && OfferModel != null)
            {
                var responseGeneral = await HttpClient.PostAsJsonAsync("api/Deal/GetDealSPecificInfo",
               new DealOfferDropdownDataModel
               {
                   CurrencyId = OfferModel.CurrencyOid,
                   LangId = DealModel.OfferLanguageOid,
                   PaymentId = OfferModel.PaymentTypeOid,
                   PolicyPeriodId = OfferModel.PolicyPeriodOid,
                   DeductibleAmountOid = OfferModel.DeductibleAmountOid
               }
               );


                var responseGeneralResult = await responseGeneral.Content.ReadFromJsonAsync<OperationResult<DealOfferDropdownDataModel>>();

                DealModel.OfferLang = responseGeneralResult.Model.Lang;
                OfferModel.PaymentType = responseGeneralResult.Model.Payment;
                OfferModel.Currency = responseGeneralResult.Model.Currency;
                OfferModel.InsuranceAreaTypeDisplayValue = responseGeneralResult.Model.AreaType;
                OfferModel.PolicyPeriod = responseGeneralResult.Model.PolicyPeriod;
                OfferModel.DeductibleAmount = responseGeneralResult.Model.DeductibleAmount;

                if (Vehicles != null && Vehicles.Count > 0)
                {
                    foreach (var vehicle in Vehicles)
                    {
                        var response = await HttpClient.PostAsJsonAsync("api/Deal/GetDealVehicleInfo",
                          new DealOfferDropdownDataModel
                          {
                              PersonalAccidentInsuranceOfDriverAndPassengerItemsOid = vehicle.PersonalAccidentInsuranceOfDriverAndPassengerItemsOid?.ToString(),
                              PropertyLiabilityInsuranceItemsOid = vehicle.PropertyLiabilityInsuranceItemsOid?.ToString(),
                              SelectedVehicleUsagePurposeOid = vehicle.SelectedVehicleUsagePurposeOid?.ToString(),
                          }
                          );

                        if (response != null)
                        {
                            var responseResult = await response?.Content?.ReadFromJsonAsync<OperationResult<DealOfferDropdownDataModel>>();

                            vehicle.SelectedVehicleUsagePurpose = responseResult?.Model?.SelectedVehicleUsagePurpose;
                        }

                        vehicle.VehBrand = VehBrands?.Where(x => x.Key.ToString().Equals(vehicle?.VehBrandOid))?.FirstOrDefault()?.Value;
                        vehicle.VehModel = vehicle.VehModels?.Where(x => x.Key.ToString().Equals(vehicle?.VehModelOid))?.FirstOrDefault()?.Value;
                    }
                }


            }



                base.OnParametersSet();
        }
    }
}
