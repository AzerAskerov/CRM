using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class CallHistory
    {
        public int Id { get; set; }
        public DateTime CallTimestamp { get; set; }
        public string ResponsibleNumner { get; set; }
        public int ContactInfoId { get; set; }
        public string Direction { get; set; }

        public virtual ContactInfo ContactInfo { get; set; }
    }
}
