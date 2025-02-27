using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models
{
    public class CountryData
    {
        public string Country { get; set; }
        public string Iso2 { get; set; }
        public string Iso3 { get; set; }
        public List<string> Cities { get; set; }
    }

    public class RemoteCountryModel
    {
        public bool Error { get; set; }
        public string Msg { get; set; }
        public List<CountryData> Data { get; set; }
    }
}
