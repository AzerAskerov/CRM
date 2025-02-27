using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.Data.Database
{
    public partial class Tender
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public int? PurposeOid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid DealGuid { get; set; }

        public virtual Deal Deal { get; set; }

    }
}
