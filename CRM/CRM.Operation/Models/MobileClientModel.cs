using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models
{
    public class MobileClientModel
    {
        public int IIN { get; set; }
        public string FullName { get; set; }
        public string Pin { get; set; }
        public List<MobileClientTokenModel> Tokens { get; set; }
    }
    public class MobileClientTokenModel
    {
        public string Token { get; set; }
        public string Platform { get; set; }
        public string Value { get; set; }
    }
}
