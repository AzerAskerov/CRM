using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class TagComp
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int Inn { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public virtual Tag Tag { get; set; }

        public virtual ClientRef ClientRef { get; set; }
    }
}
