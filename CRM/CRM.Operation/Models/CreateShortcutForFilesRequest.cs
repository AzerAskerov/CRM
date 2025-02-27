using System.Collections.Generic;

namespace CRM.Operation.Models
{
    public class CreateShortcutForFilesRequest
    {
        public string ObjectType { get; set; }

        public string ObjectIdentificationNumber { get; set; }

        public IList<string> DocumentType { get; set; }

        public string ShortcutFolderName { get; set; }
    }
}