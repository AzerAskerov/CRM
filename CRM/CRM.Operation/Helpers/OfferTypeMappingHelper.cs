using System;
using System.Collections.Generic;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Models.DealOfferModels;

namespace CRM.Operation.Helpers
{
    public static class OfferTypeMappingHelper
    {
        public static Dictionary<OfferTypeEnum, Type> OfferEnumToDbType = new Dictionary<OfferTypeEnum, Type>()
        {
            {OfferTypeEnum.VehicleInsurance, typeof(VehicleInsurance)},
            {OfferTypeEnum.OtherTypes, typeof(OtherTypeOffer)},
            {OfferTypeEnum.PropertyInsurance, typeof(PropertyInsurance)},
            {OfferTypeEnum.PersonalAccidentInsurance, typeof(PersonalAccident)},
            {OfferTypeEnum.LifeInsurance, typeof(LifeInsurance)},
            {OfferTypeEnum.VoluntaryHealthInsurance, typeof(VoluntaryHealthInsurance)},
            {OfferTypeEnum.CarAndEquipmentInsurance, typeof(CarAndEquipmentInsurance)},
            {OfferTypeEnum.LiabilityInsurance, typeof(LiabilityInsurance)},
            {OfferTypeEnum.CargoInsurance, typeof(CargoInsurance)},
            {OfferTypeEnum.ElectronicsInsurance, typeof(ElectronicsInsuranceOfferModel)},
            {OfferTypeEnum.ProductLiabilityInsurance, typeof(ProductLiabilityInsuranceOfferModel)},
            {OfferTypeEnum.PropertyLiabilityInsurance, typeof(PropertyLiabilityInsuranceOfferModel)},
            {OfferTypeEnum.HiringLiabilityInsurance, typeof(HiringLiabilityInsuranceOfferModel)},
            {OfferTypeEnum.BenchInsurance, typeof(BenchInsuranceOfferModel)},
        };
        
        public static Dictionary<OfferTypeEnum, Type> OfferEnumToModelType = new Dictionary<OfferTypeEnum, Type>()
        {
            {OfferTypeEnum.VehicleInsurance, typeof(VehicleInsuranceOfferModel)},
            {OfferTypeEnum.OtherTypes, typeof(OtherTypeOfferModel)},
            {OfferTypeEnum.PropertyInsurance, typeof(PropertyInsuranceOfferModel)},
            {OfferTypeEnum.PersonalAccidentInsurance, typeof(PersonalAccidentOfferModel)},
            {OfferTypeEnum.LifeInsurance, typeof(LifeInsuranceOfferModel)},
            {OfferTypeEnum.VoluntaryHealthInsurance, typeof(VoluntaryHealthInsuranceOfferModel)},
            {OfferTypeEnum.CarAndEquipmentInsurance, typeof(CarAndEquipmentInsuranceOfferModel)},
            {OfferTypeEnum.LiabilityInsurance, typeof(LiabilityInsuranceOfferModel)},
            {OfferTypeEnum.CargoInsurance, typeof(CargoInsuranceOfferModel)},
            {OfferTypeEnum.ElectronicsInsurance, typeof(ElectronicsInsuranceOfferModel)},
            {OfferTypeEnum.ProductLiabilityInsurance, typeof(ProductLiabilityInsuranceOfferModel)},
            {OfferTypeEnum.PropertyLiabilityInsurance, typeof(PropertyLiabilityInsuranceOfferModel)},
            {OfferTypeEnum.HiringLiabilityInsurance, typeof(HiringLiabilityInsuranceOfferModel)},
            {OfferTypeEnum.BenchInsurance, typeof(BenchInsuranceOfferModel)},
        };
    }
}