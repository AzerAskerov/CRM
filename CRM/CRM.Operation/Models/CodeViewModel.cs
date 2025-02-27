using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models
{
    public class CodeViewModel
    {
        public int Id { get; set; }
        public int TypeOid { get; set; }
        public int? CodeOid { get; set; }
        public string Value { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
