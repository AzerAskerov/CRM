using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using CRM.Operation.Localization;


namespace CRM.Operation.Localization
{
    public class Localize
    {
        public Dictionary<string, ResourceLocalization> resources;
        public LocalizationDictionary dictionary;
        private int defaultLanguageId;

        public Localize(LocalizationModel model)
        {
            dictionary = model.Dictionary;
            resources = model.Resources;
        }

        public Localize()
        {

        }

        public static async Task<Localize> Create(HttpClient client)
        {
            LocalizationModel model = await Initialize(client);
            Localize localize = new Localize(model);

            return localize;
        }

        private async static Task<LocalizationModel> Initialize(HttpClient client)
        {
            try
            {
                var response = await client.GetAsync("api/localize/local");
                
                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LocalizationModel>(result);
            }
            catch (Exception ex) { Console.WriteLine("Initialize" + ex.Message); }

            return null;
        }


        public string GetText(string resId, int languageId, string defaultText)
        {
            ResourceLocalization res = null;
            try
            {
                res = this.resources[resId];
            }
            catch (KeyNotFoundException)
            {
            }

            if (res != null)
            {
                if ((res.TranslationId != null) && (languageId > -1) &&
                    (res.TranslationId < this.dictionary.MaxVocabularySize))
                {
                    
                    
                    string txt = (res.TranslationId < this.dictionary.Vocabularies[languageId].Count)
                                     ? this.dictionary.Vocabularies[languageId].Translations[(int)res.TranslationId]
                                     : string.Empty;
                    if (txt.Length == 0)
                    {
                        return "[" + this.dictionary.Vocabularies[languageId].LanguageDisplayName + "] " + defaultText;
                    }

                    return txt;
                }
            }


            return defaultText;
        }

        public string GetText(string resId, string defaultText)
        {

            return this.GetText(resId, DefaultLanguageId, defaultText);
        }

        public int DefaultLanguageId
        {
            get
            {

                if (Thread.CurrentThread.CurrentUICulture != CultureInfo.InvariantCulture)
                {
                    for (int i = 0; i < this.dictionary.Count; i++)
                    {
                        if (dictionary.Vocabularies[i].Locale.Substring(0, 2).ToUpper(CultureInfo.InvariantCulture) == Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpper(CultureInfo.InvariantCulture))
                        {
                            return i;
                        }
                    }
                }

                return this.defaultLanguageId;
            }
        }
    }
}
