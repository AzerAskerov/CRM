using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models.RemoteApi
{
    public class CountryData
    {
        public string Name { get; set; }
        public string Iso2 { get; set; }
        public double Long { get; set; }
        public double Lat { get; set; }
    }

    public class Country
    {
        public bool Error { get; set; }
        public string Msg { get; set; }
        public List<CountryData> Data { get; set; }
    }
}
