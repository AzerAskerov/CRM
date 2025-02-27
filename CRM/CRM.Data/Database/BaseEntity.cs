using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Data.Database
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public int Inn { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
