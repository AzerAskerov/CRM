using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class ClientRelationComp
    {
        public int Id { get; set; }
        public int Client1 { get; set; }
        public int Client2 { get; set; }
        public int RelationId { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public virtual ClientRef ClientRef { get; set; }
        public virtual ClientRef Client2Navigation { get; set; }
    }
}
