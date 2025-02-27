using CRM.Operation.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CRM.Operation.Models
{
    [DataContract]
    public class ClientRelationModel : ClientContract
    {
        [DataMember(Name = "relationTypeId")]
        public int RelationTypeId { get; set; }

        [DataMember(Name = "RelationalINN")]
        public int RelationalINN { get; set; }
    }
}
