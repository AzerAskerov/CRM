using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class Relation
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Reverse { get; set; }
        public int? TagId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
