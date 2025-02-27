using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models
{
    public class CopyPathModel
    {
        public Guid sourceGuid { get; set; }
        public string sourceObjType { get; set; }
        public string sourceDocType { get; set; }
        public Guid destinationGuid { get; set; }

        public string destinationObjType { get; set; }
        public string destinationDocType { get; set; }
    }
}
