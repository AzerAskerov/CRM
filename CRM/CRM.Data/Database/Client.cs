using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class Client
    {
        public int Id { get; set; }
        public int Inn { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int ClientType { get; set; }

        public bool IsHidden { get; set; }

        public virtual ClientRef ClientRef { get; set; }
    }
}
