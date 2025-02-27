using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Data.Database
{
    public partial class DealHistory
    {
        public int Id { get; set; }
        public Guid UserGuid { get; set; }
        public Guid DealGuid { get; set; }
        public string ChangedValue { get; set; }
        public short ChangeType { get; set; }
        public DateTime RegisteredDate { get; set; }
    }
}
