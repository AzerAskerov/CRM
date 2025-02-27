using System.Collections.Generic;

namespace CRM.Operation.Localization
{
    public class LocalizationDictionary
    {
        public int MaxVocabularySize { get; set; }
        public int Count { get; set; }
        public List<VocabularyLocalization> Vocabularies { get; set; }
    }
}
