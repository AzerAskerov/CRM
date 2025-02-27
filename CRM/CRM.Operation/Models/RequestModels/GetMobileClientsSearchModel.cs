using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models.RequestModels
{
    public class GetMobileClientsSearchModel
    {
        public string? Pin { get; set; }
        public List<string>? Pins { get; set; }
        public string? Platform { get; set; }
    }
}
