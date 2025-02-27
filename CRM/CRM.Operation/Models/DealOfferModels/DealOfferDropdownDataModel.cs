using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models.DealOfferModels
{
    public class DealOfferDropdownDataModel
    {
        public int? PaymentId { get; set; }
        public string Payment { get; set; }


        public int? CurrencyId { get; set; }
        public string Currency { get; set; }


        public int? LangId { get; set; }
        public string Lang { get; set; }

        public int? AreaTypeId { get; set; }
        public string AreaType { get; set; }

        public int? PolicyPeriodId { get; set; }
        public string PolicyPeriod { get; set; }


        public int? ImmovableTypeOid { get; set; }
        public string ImmovableType { get; set; }
        public int? MovableTypeOid { get; set; }
        public string MovableType { get; set; }

        public string PersonalAccidentInsuranceOfDriverAndPassengerItemsOid { get; set; }
        public string PersonalAccidentInsuranceOfDriverAndPassengerItems { get; set; }
        public string PropertyLiabilityInsuranceItemsOid { get; set; }
        public string PropertyLiabilityInsuranceItems { get; set; }
        public string SelectedVehicleUsagePurposeOid { get; set; }
        public string SelectedVehicleUsagePurpose { get; set; }
        public int? DeductibleAmountOid { get; set; }
        public string DeductibleAmount { get; set; }

        public string PackagingName { get; set; }
        public int? PackagingId { get; set; }
        public string TransportationName { get; set; }
        public int? TransportationId { get; set; }


        public int? AgeRangeOid { get; set; }
        public string AgeRange { get; set; }
        public int? Gender { get; set; }
        public string GenderName { get; set; }
        public int? EmployeeType { get; set; }
        public string EmployeeTypeName { get; set; }

        public int? InsuranceTypeOid { get; set; }
        public string InsuranceType { get; set; }
    }
}
