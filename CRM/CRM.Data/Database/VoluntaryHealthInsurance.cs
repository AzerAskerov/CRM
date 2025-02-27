using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CRM.Data.Database
{
    public partial class VoluntaryHealthInsurance
    {
        public VoluntaryHealthInsurance()
        {
            VoluntaryHealthInsuranceEmployeeGroup = new HashSet<VoluntaryHealthInsuranceEmployeeGroup>();
        }

        public Guid DealGuid { get; set; }
        public int PolicyPeriodOid { get; set; }
        public int CurrencyOid { get; set; }
        public int PaymentTypeOid { get; set; }
        public string EmailToNotify { get; set; }
        public decimal Commission { get; set; }
        public string TypeOfActivity { get; set; }
        public string InsuranceArea { get; set; }
        public string IsJobContainsHarmfulFactors { get; set; }
        public int? NumberOfOfficeWorkers { get; set; }
        public int? NumberOfProductionWorkers { get; set; }
        public bool IsRenew { get; set; }
        public string ExistingPolicyNumber { get; set; }

        public virtual Deal Deal { get; set; }
        public virtual ICollection<VoluntaryHealthInsuranceEmployeeGroup> VoluntaryHealthInsuranceEmployeeGroup { get; set; }
    }
}
