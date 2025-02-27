using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CRM.Client.States;
using CRM.Operation.Models;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using Zircon.Core;

namespace CRM.Client.Shared.Components.OfferFormComponents
{
    public partial class VehicleInsuranceOfferComponent
    {
       
        [Parameter]
        public VehicleInsuranceOfferModel OfferModel { get; set; }
        [Parameter]
        public DealModel DealModel { get; set; }

        
        public void AddVehicleItem()
        {
            OfferModel.Vehicles.Add(new InsuredVehicleModel());
        }

        protected void RemoveVehicleItem(InsuredVehicleModel item)
        {
            OfferModel.Vehicles.Remove(item);
        }
        protected InsuredVehicleModel vehicleModel;

        public EditContext EditContext { get; set; }
        [Parameter]
        public EventCallback<EditContext> ModelSet { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; }


        protected override async Task OnInitializedAsync()
        {
            OfferModel ??= new VehicleInsuranceOfferModel();
            var result = await State.GetVehicles(OfferModel.DealGuid);

            OfferModel.Vehicles = (result?.Model != null && result.Model.Count > 0) ? result.Model : new System.Collections.Generic.List<InsuredVehicleModel>() { new InsuredVehicleModel() };


            EditContext = new EditContext(OfferModel);

        
            var stateVehBrand = await State.Sourceloader(new SourceLoaderModel()
            {
                ParentPropertyNamespace = typeof(VehicleInsuranceOfferModel).AssemblyQualifiedName,
                PropertyName = nameof(OfferModel.VehBrandOid),
                ParentAsJson = JsonConvert.SerializeObject(OfferModel)
            });

            var statePAInsuranceOfDriverAndPassengerItemsOid = await State.Sourceloader(new SourceLoaderModel()
            {
                ParentPropertyNamespace = typeof(VehicleInsuranceOfferModel).AssemblyQualifiedName,
                PropertyName = nameof(OfferModel.PersonalAccidentInsuranceOfDriverAndPassengerItemsOid),
                ParentAsJson = JsonConvert.SerializeObject(OfferModel)
            });

            var statePropertyLiabilityInsuranceItems = await State.Sourceloader(new SourceLoaderModel()
            {
                ParentPropertyNamespace = typeof(VehicleInsuranceOfferModel).AssemblyQualifiedName,
                PropertyName = nameof(OfferModel.PropertyLiabilityInsuranceItemsOid),
                ParentAsJson = JsonConvert.SerializeObject(OfferModel)
            });

            var stateSelectedVehicleUsagePurposes = await State.Sourceloader(new SourceLoaderModel()
            {
                ParentPropertyNamespace = typeof(VehicleInsuranceOfferModel).AssemblyQualifiedName,
                PropertyName = nameof(OfferModel.SelectedVehicleUsagePurposeOid),
                ParentAsJson = JsonConvert.SerializeObject(OfferModel)
            });

           
            VehBrands = stateVehBrand.Model;
            PersonalAccidentInsuranceOfDriverAndPassengerItems = statePAInsuranceOfDriverAndPassengerItemsOid.Model;
            SelectedVehicleUsagePurposes = stateSelectedVehicleUsagePurposes.Model;
            PropertyLiabilityInsuranceItems = statePropertyLiabilityInsuranceItems.Model;

            foreach (var item in OfferModel.Vehicles)
            {
                if (!string.IsNullOrEmpty(item.VehModelOid))
                {
                    await OnBrandValueChanged(item.VehBrandOid, item);
                }
            }
            await ModelSet.InvokeAsync(EditContext);
            await base.OnInitializedAsync();
        }

        public List<SourceItem> VehBrands { get; set; }
        public List<SourceItem> VehModels { get; set; }
        public List<SourceItem> PersonalAccidentInsuranceOfDriverAndPassengerItems { get; set; }
        public List<SourceItem> PropertyLiabilityInsuranceItems { get; set; }
        public List<SourceItem> SelectedVehicleUsagePurposes { get; set; }

        private DropDownComponent<int?> _modelDropDownComponent;

        private void BeneficiarySelected(ClientContract clientContract)
        {
            OfferModel.Beneficiary = clientContract;
        }

        private async Task BrandSelected()
        {
            await _modelDropDownComponent.LoadItems();
        }


        public async Task OnBrandValueChanged(string value, InsuredVehicleModel model)
        {
            model.VehBrandOid = value;

            var stateVehModel = await State.Sourceloader(new SourceLoaderModel()
            {
                ParentPropertyNamespace = typeof(VehicleInsuranceOfferModel).AssemblyQualifiedName,
                PropertyName = nameof(model.VehModelOid),
                ParentAsJson = JsonConvert.SerializeObject(model)
            });
           model.VehModels = stateVehModel.Model;
          
        }
    }
}