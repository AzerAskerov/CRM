using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CRM.Data.Database
{
    public partial class VoluntaryHealthInsuranceEmployeeGroup
    {
        public int Id { get; set; }
        public Guid VoluntaryHealthInsuranceGuid { get; set; }
        public int AgeRangeOid { get; set; }
        public int Count { get; set; }
        public int Gender { get; set; }
        public int EmployeeType { get; set; }

        public virtual VoluntaryHealthInsurance VoluntaryHealthInsurance { get; set; }
    }
}
