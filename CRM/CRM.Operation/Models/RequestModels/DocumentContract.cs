using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Zircon.Core.OperationModel;

using CRM.Operation.Attributes;
using CRM.Operation.SourceLoaders;
using CRM.Operation.Enums;
using Zircon.Core.Attributes;

namespace CRM.Operation.Models.RequestModels
{
    public class DocumentContract : IModel
    {
        [Key]
        [IgnoreDataMember]
        public int? Id { get; set; }

        [DataMember(Name = "documentNumber")]
        [Attributes.RequiredLocalized]
        [Attributes.DisplayNameLocalized("DocumentContract.DocumentNumber")]
        public string DocumentNumber { get; set; }

        [DataMember(Name = "documentType")]
        [CustomSource(typeof(DocumentSourceLoader))]
        [Attributes.RequiredLocalized]
        [Attributes.DisplayNameLocalized("DocumentContract.DocumentType")]
        public int DocumentType { get; set; }

        [DataMember(Name = "documentexpiredate")]
        public DateTime? DocumentExpireDate { get; set; }

        [DataMember(Name = "validfrom")]
        public DateTime? ValidFrom { get; set; }
    }
}
