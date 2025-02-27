using System.Collections.Generic;

namespace CRM.Operation.Localization
{
    public  class LocalizationModel
    {
        public LocalizationDictionary Dictionary { get; set; }
        public Dictionary<string, ResourceLocalization> Resources {get;set;}

    }
}
