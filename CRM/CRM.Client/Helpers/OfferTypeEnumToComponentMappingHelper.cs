using System;
using System.Collections.Generic;
using CRM.Client.Shared.Components.OfferFormComponents;
using CRM.Data.Enums;

namespace CRM.Client.Helpers
{
    /// <summary>
    /// Mapping dictionary for <see cref="OfferTypeEnum"/> to offer form component map.
    /// </summary>
    public static class OfferTypeEnumToComponentMappingHelper
    {
        public static readonly Dictionary<OfferTypeEnum,Type> FormComponentMap = new Dictionary<OfferTypeEnum,Type>()
        {
            {OfferTypeEnum.VehicleInsurance, (typeof(VehicleInsuranceOfferComponent))},
            {OfferTypeEnum.OtherTypes, (typeof(OtherTypeOfferComponent))},
            {OfferTypeEnum.PropertyInsurance, (typeof(PropertyInsuranceOfferComponent))},
            {OfferTypeEnum.PersonalAccidentInsurance, (typeof(PersonalAccidentOfferComponent))},
            {OfferTypeEnum.LifeInsurance, (typeof(LifeInsuranceOfferComponent))},
            {OfferTypeEnum.VoluntaryHealthInsurance, (typeof(VoluntaryHealthOfferComponent))},
            {OfferTypeEnum.CarAndEquipmentInsurance, (typeof(CarAndEquipmentInsuranceOfferComponent))},
            {OfferTypeEnum.LiabilityInsurance, (typeof(LiabilityInsuranceOfferComponent))},
            {OfferTypeEnum.CargoInsurance, (typeof(CargoInsuranceOfferComponent))},
            {OfferTypeEnum.BenchInsurance, (typeof(BenchInsuranceOfferComponent))},
            {OfferTypeEnum.HiringLiabilityInsurance, (typeof(HiringLiabilityInsuranceOfferComponent))},
            {OfferTypeEnum.ProductLiabilityInsurance, (typeof(ProductLiabilityInsuranceOfferComponent))},
            {OfferTypeEnum.PropertyLiabilityInsurance, (typeof(PropertyLiabilityInsuranceOfferComponent))},
            {OfferTypeEnum.ElectronicsInsurance, (typeof(ElectronicsInsuranceOfferComponent))},
        };
    }
}