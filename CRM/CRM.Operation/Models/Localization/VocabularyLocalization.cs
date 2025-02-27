using System.Collections.Generic;

namespace CRM.Operation.Localization
{
    public class VocabularyLocalization
    {
        public List<string> Translations { get; set; }
        public string LanguageName{ get; set; }
        public string LanguageDisplayName{ get; set; }
        public string Locale{ get; set; }
        public int Count
        {
            get
            {
                return this.Translations.Count;
            }
        }
        public int LanguageId{ get; set; }
    }
}

