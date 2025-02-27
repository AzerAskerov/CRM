using System.Collections.Generic;
using System.IO;
using System.Linq;
using CRM.Operation.Models.Terror;
using Microsoft.Extensions.Configuration;

namespace CRM.Operation.Helpers
{
   
    public static class AppSettings
    {

        #region Methods

        public static string GetSettingValue(string MainKey, string SubKey)
        {
            return Configuration.GetSection(MainKey).GetChildren().FirstOrDefault(x=>x.Key == SubKey)?.Value;
        }

        #endregion

        #region Properties

        public static IConfigurationRoot _configuration;
        public static IConfigurationRoot Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    IConfigurationBuilder builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                    _configuration = builder.Build();
                }
                return _configuration;
            }
        }
        public static TerrorConfig TerrorConfiguration
        {
            get
            {
                return Configuration.Get<Root>().terrorConfig;
            }
        }

        public static List<FieldsConfig> FieldConfiguration
        {
            get
            {
                return TerrorConfiguration.fieldsConfig;
            }
        }
        #endregion

    }
}