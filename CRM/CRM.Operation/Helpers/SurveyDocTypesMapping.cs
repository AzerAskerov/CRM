using System.Collections.Generic;
using System.Linq;
using CRM.Data.Enums;
using Zircon.Core.Extensions;

namespace CRM.Operation.Helpers
{
    public static class SurveyDocTypesMapping
    {
        public static readonly Dictionary<OfferTypeEnum, List<string>> SurveyDocTypes = new Dictionary<OfferTypeEnum, List<string>>
            {
                {
                    OfferTypeEnum.VehicleInsurance, new List<string>()
                    {
                        "Survey.VehicleOther", "Survey.VehicleBack", "Survey.VehicleTop", "Survey.VehicleInner", "Survey.VehicleInner2", "Survey.VehicleInner3",
                        "Survey.VehicleInner4", "Survey.VehicleHoodArea", "Survey.VehicleFront", "Survey.VehicleFront2", "Survey.VehicleFront3",
                        "Survey.VehicleRight", "Survey.VehicleRight2", "Survey.VehicleRight3", "Survey.VehicleRight4", "Survey.VehicleLeft",
                        "Survey.VehicleLeft2", "Survey.VehicleLeft3", "Survey.VehicleLeft4", "Survey.VehicleLeft5", "Survey.VehiclePassport"
                    }.Select(x=>x.Translate()).ToList()
                },
                {
                    OfferTypeEnum.PropertyInsurance, new List<string>
                    {
                       "Property.0" ,"Property.1", "Property.2", "Property.3", "Property.4",
                        "Property.5", "Property.6", "Property.7", "Property.8",
                    }.Select(x=>x.Translate()).ToList()
                },
            };
    }
}