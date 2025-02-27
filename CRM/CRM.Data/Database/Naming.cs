using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class Naming
    {
        public int Id { get; set; }
        public string DefaultValue { get; set; }
        public int Type { get; set; }
        public string ValueEn { get; set; }
        public string ValueRu { get; set; }
    }
}
