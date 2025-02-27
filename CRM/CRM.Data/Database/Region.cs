using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class Region
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public int Seq { get; set; }
        public DateTime ValidTill { get; set; }
    }
}
