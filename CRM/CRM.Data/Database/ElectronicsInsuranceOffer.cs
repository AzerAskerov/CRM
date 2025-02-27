using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Data.Database
{
   public partial class ElectronicsInsuranceOffer
    {
        public Guid DealGuid { get; set; }
        public string EmailToNotify { get; set; }
        public string Comment { get; set; }
        public bool DangerousChemicalsUsage { get; set; }
        public int? BeneficiaryClientInn { get; set; }
        public int PolicyPeriodOid { get; set; }
        public int CurrencyOid { get; set; }
        public int PaymentTypeOid { get; set; }
        public int InsuranceAreaType { get; set; }


        public string InsuranceArea { get; set; }


        public bool IsRenew { get; set; }


        public string ExistingPolicyNumber { get; set; }



        public string CompanyActivityType { get; set; }


        public string InsurancePredmetAddress { get; set; }

        public string Unit { get; set; }


        public string Description { get; set; }


        public int Year { get; set; }


        public decimal DeviceReplacementCost { get; set; }

        public string ManufacturerName { get; set; }

        public string Series { get; set; }


        public string Type { get; set; }


        public string Voltage { get; set; }


        public string Power { get; set; }

        public decimal CustomsExpenses { get; set; }


        public decimal PackagingExpenses { get; set; }


        public decimal CurrentPrice { get; set; }


        public string ElectronicAnyInfo { get; set; }

        public string LastFiveYearsInjuryKnowledge { get; set; }


        public int? ImmovablePropertyTypeOid { get; set; }


        public int? MovablePropertyTypeOid { get; set; }


        public string ImmovablePropertyTypeOther { get; set; }


        public string MovablePropertyTypeOther { get; set; }


        public string SecurityAndFirefightingSystemInfo { get; set; }

        public bool SpecialConditions { get; set; }


        public string SpecialConditionsYes { get; set; }


        public string DeviceAddress { get; set; }

        public bool DeviceIsNew { get; set; }

        public bool TakeCare { get; set; }


        public bool Training { get; set; }

        public string AnyInfo { get; set; }



        public string DangerousChemicals { get; set; }
        public virtual ClientRef BeneficiaryClientInnNavigation { get; set; }
        public virtual Deal Deal { get; set; }

    }
}
