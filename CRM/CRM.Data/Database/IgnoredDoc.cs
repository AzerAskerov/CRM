using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Data.Database
{
    public partial class IgnoredDoc
    {
        public int Id { get; set; }
        public string IgnoredDocNumber { get; set; }
        public int? IgnoredDocType { get; set; }
    }
}
