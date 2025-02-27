using System.Collections.Generic;

namespace CRM.Operation.Models
{
    public class FileManagementGenerateTokenRequest
    {
        public string ObjectType { get; set; }

        public string ObjectIdentificationNumber { get; set; }

        public IList<string> DocumentType { get; set; }

        public int ViewMode { get; set; }
    }
}