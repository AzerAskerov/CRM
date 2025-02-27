using CRM.Operation.Attributes;
using CRM.Operation.Enums;
using CRM.Operation.SourceLoaders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Zircon.Core.Attributes;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.RequestModels
{
    public class RelationContract : IModel
    {
        [Key]
        [IgnoreDataMember]
        public int? Id { get; set; }

        [DataMember(Name = "inn")]
        [Attributes.DisplayNameLocalized("RelationContract.ClientINN")]
        public virtual int? ClientINN { get; set; }

        public string  ClientName { get; set; }

        [DataMember(Name = "relationType")]
        [CustomSource(typeof(RelationTypeSourceLoader))]
        [Attributes.RequiredLocalized]
        [Attributes.DisplayNameLocalized("RelationContract.RelationType")]
        public int RelationType { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "documentType")]
        public int DocumentType { get; set; }

        public DateTime ValidFrom { get; set; }

    }
}
