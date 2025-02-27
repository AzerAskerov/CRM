using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Client.Cache;
using CRM.Client.States;
using CRM.Operation.Localization;
using CRM.Operation.Models;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.RequestModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace CRM.Client.Shared.Components.OfferFormComponents
{
    public partial class CargoInsuranceOfferComponent
    {
        public List<string> Countries { get; set; }
        public List<string> Cities { get; set; }

        public List<string> StartPointCountries = new List<string>();
        public List<string> StartPointCities = new List<string>();

        public List<string> EndPointCountries = new List<string>();
        public List<string> EndPointCities = new List<string>();


        public List<string> RouteCountries = new List<string>();
        public List<string> RouteCities = new List<string>();

        string countriesCacheKey = "countries", cityCacheKey = "cities";
        [Parameter]
        public CargoInsuranceOfferModel OfferModel { get; set; }

        [Parameter]
        public DealModel DealModel { get; set; }



        public EditContext EditContext { get; set; }
        [Parameter]
        public EventCallback<EditContext> ModelSet { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; }


        public List<string> ExtractCountries(RemoteCountryModel deserializedResponse)
        {
            List<string> Countries = new List<string>() { "CountryNotSelected".Translate() };
            if (deserializedResponse != null)
            {
                foreach (var country in deserializedResponse.Data)
                {
                    if (country != null)
                    {
                        Countries.Add(country.Country);
                    }
                }
            }

            return Countries;
        }

        protected override async Task OnInitializedAsync()
        {
            OfferModel ??= new CargoInsuranceOfferModel();
            EditContext = new EditContext(OfferModel);
            await ModelSet.InvokeAsync(EditContext);


            var cacheCountries =  await DataCaching<List<string>>.GetCachedData(countriesCacheKey, SessionStorage);
            if (cacheCountries == null)
            {
                using (var task = LoadingManager.Loading("ClaimReport", "ClaimReport.Loading".Translate(), ""))
                {

                    var countriesResult = await State.GetRemoteCountriesAsync(); // get countries


                    if (!countriesResult.Success)
                    {
                        SetState(countriesResult);
                        return;
                    }
                    Countries = ExtractCountries(countriesResult.Model);
                    StartPointCountries = ExtractCountries(countriesResult.Model);
                    EndPointCountries = ExtractCountries(countriesResult.Model);
                    RouteCountries = ExtractCountries(countriesResult.Model);
                    await DataCaching<List<string>>.Cache(ExtractCountries(countriesResult.Model), countriesCacheKey, SessionStorage);
                }
            }
            else
            {
                Countries = cacheCountries;
                StartPointCountries = cacheCountries;
                EndPointCountries = cacheCountries;
                RouteCountries = cacheCountries;
            }

            if (!string.IsNullOrEmpty(OfferModel.StartPointCityId))
            {
               await OnStartPointValueChanged(OfferModel.StartPointCountryId);
            }

            if (!string.IsNullOrEmpty(OfferModel.EndPointCityId))
            {
                await OnEndPointValueChanged(OfferModel.EndPointCountryId);
            }

            if (!string.IsNullOrEmpty(OfferModel.RouteCityId))
            {
                await OnRouteValueChanged(OfferModel.RouteCountryId);
            }

            await base.OnInitializedAsync();
        }

        private void BeneficiarySelected(ClientContract clientContract)
        {
            OfferModel.Beneficiary = clientContract;
        }

        public async Task OnStartPointValueChanged(string value)
        {
            var cityCached = await DataCaching<List<string>>.GetCachedData($"{value}_{cityCacheKey}", SessionStorage);

            if (cityCached == null)
            {
                using (var task = LoadingManager.Loading("ClaimReport", "ClaimReport.Loading".Translate(), ""))
                {
                    var citiesResult = await State.GetRemoteCitiesAsync(value);

                    if (!citiesResult.Success)
                    {
                        SetState(citiesResult);
                        return;
                    }

                    StartPointCities = citiesResult.Model;
                    await DataCaching<List<string>>.Cache(citiesResult.Model, $"{value}_{cityCacheKey}", SessionStorage);

                }

            }
            else
            {
                StartPointCities = cityCached;
            }

            OfferModel.StartPointCountryId = value;

            StateHasChanged();
        }
        public async Task OnEndPointValueChanged(string value)
        {
            var cityCached = await DataCaching<List<string>>.GetCachedData($"{value}_{cityCacheKey}", SessionStorage);

            if (cityCached == null)
            {
                using (var task = LoadingManager.Loading("ClaimReport", "ClaimReport.Loading".Translate(), ""))
                {
                    var citiesResult = await State.GetRemoteCitiesAsync(value);

                    if (!citiesResult.Success)
                    {
                        SetState(citiesResult);
                        return;
                    }

                    EndPointCities = citiesResult.Model;
                    await DataCaching<List<string>>.Cache(citiesResult.Model, $"{value}_{cityCacheKey}", SessionStorage);

                }

            }
            else
            {
                EndPointCities = cityCached;
            }

            OfferModel.EndPointCountryId = value;
        }


        public async Task OnRouteValueChanged(string value)
        {
            var cityCached = await DataCaching<List<string>>.GetCachedData($"{value}_{cityCacheKey}", SessionStorage);

            if (cityCached == null)
            {
                using (var task = LoadingManager.Loading("ClaimReport", "ClaimReport.Loading".Translate(), ""))
                {
                    var citiesResult = await State.GetRemoteCitiesAsync(value);

                    if (!citiesResult.Success)
                    {
                        SetState(citiesResult);
                        return;
                    }

                    RouteCities = citiesResult.Model;
                    Cities = citiesResult.Model;
                    await DataCaching<List<string>>.Cache(citiesResult.Model, $"{value}_{cityCacheKey}", SessionStorage);

                }

            }
            else
            {
                RouteCities = cityCached;
                Cities = cityCached;
            }

            OfferModel.RouteCountryId = value;
        }


    }
}