

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CRM.Data.Database
{
    public partial class ClassifierCounter
    {
        public int CounterOid { get; set; }
        public int NextValue { get; set; }
        public string Usage { get; set; }
        public string Prefix { get; set; }
    }
}
