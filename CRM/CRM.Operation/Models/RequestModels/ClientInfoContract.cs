using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CRM.Operation.Models.RequestModels
{
    [DataContract]
    public class ClientInfoContract
    {
        public ClientInfoContract()
        {
            Clients = new List<ClientContract>();
        }

        [DataMember(Name = "source")]
        public int Source { get; set; }

        [DataMember(Name = "responsiblePerson")]
        public string ResponsiblePerson { get; set; }


        [DataMember(Name = "clients")]
        public List<ClientContract> Clients { get; set; }

        [DataMember(Name = "lobOid")]
        public int? LobOid { get; set; }
    }

  
}
