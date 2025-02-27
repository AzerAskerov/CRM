using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class Source
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ParamsJson { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
