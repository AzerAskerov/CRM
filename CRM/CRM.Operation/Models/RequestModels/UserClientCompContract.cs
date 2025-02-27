using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models.RequestModels
{
    public class UserClientCompContract
    {
        public int Id { get; set; }
        public int? Inn { get; set; }
        public string ULogonName { get; set; }
        public int? LobOid { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
