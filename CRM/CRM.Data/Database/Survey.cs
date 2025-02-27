using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CRM.Data.Database
{
    public partial class Survey
    {
        public Guid DealGuid { get; set; }
        public int SurveyerType { get; set; }
        public string SurveyerLogonName { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string SurveyLink { get; set; }

        public virtual Deal Deal { get; set; }
    }
}
