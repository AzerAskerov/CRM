using System.Collections.Generic;

using Zircon.Core.OperationModel;
using Zircon.Core.Localization;
using Config = Zircon.Core.Config;
using Localize = Zircon.Core.Localization;

using CRM.Data.Database;
using CRM.Operation.Localization;


namespace CRM.Operation
{
    public class GetLocalizationOperation : BusinessOperation
    {
        public LocalizationModel Model = new LocalizationModel();
      
        public GetLocalizationOperation(CRMDbContext db) : base(db)
        {
        }

        protected override void DoExecute()
        {
            Localize.Localize localized = new Localize.Localize(Config.Settings.GetInt32("UIApplicationID"),
                Config.Settings.GetInt32("CompanyID"));
                  
            Model.Resources = new Dictionary<string, ResourceLocalization>();

            foreach (var i in localized.Resources)
                Model.Resources.Add(i.Key, new ResourceLocalization { TranslationId = i.Value.TranslationId });

            Model.Dictionary = new LocalizationDictionary();
            Model.Dictionary.Vocabularies = new List<VocabularyLocalization>();
            Model.Dictionary.Count = localized.Dictionary.Count;
            Model.Dictionary.MaxVocabularySize = localized.Dictionary.MaxVocabularySize;
            //Model.HashString = HashString;

            for (int i = 0; i < Model.Dictionary.Count; i++)
            {
                VocabularyLocalization vocabulary = new VocabularyLocalization();

                vocabulary.LanguageDisplayName = localized.Dictionary[i].LanguageDisplayName;
                vocabulary.LanguageId = localized.Dictionary[i].LanguageId;
                vocabulary.LanguageName = localized.Dictionary[i].LanguageName;
                vocabulary.Locale = localized.Dictionary[i].Locale;
                vocabulary.Translations = new List<string>();

                foreach (var t in localized.Dictionary[i].Translations)
                    vocabulary.Translations.Add((string)t);

                Model.Dictionary.Vocabularies.Add(vocabulary);
            }
        }
    }
}
