using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class Document
    {
        public int Id { get; set; }
        public string DocumentNumber { get; set; }
        public int DocumentType { get; set; }
        public int Inn { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime? DocumentExpireDate { get; set; }

        public virtual ClientRef ClientRef { get; set; }
    }
}
