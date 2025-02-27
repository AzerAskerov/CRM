using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models.RemoteApi
{
    public class City
    {
        public bool Error { get; set; }
        public string Msg { get; set; }
        public List<string> Data { get; set; }
    }
}
