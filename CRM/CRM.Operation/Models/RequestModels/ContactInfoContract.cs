using CRM.Operation.Attributes;
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
    [DataContract]
    public class ContactInfoContract : IModel
    {
        public ContactInfoContract()
        {
            ContactComments = new List<CommentContract>();
            Calls = new List<CallContract>();
        }

        [Key]
        [IgnoreDataMember]
        public int? Id { get; set; }

        [DataMember(Name = "type")]
        [CustomSource(typeof(ContactTypeSourceLoader))]
        public int Type { get; set; }

        [DataMember(Name = "value")]
        [Attributes.RequiredLocalized]
        [Attributes.DisplayNameLocalized("ContactInfoContract.Value")]
        public string Value { get; set; }

        [DataMember(Name = "reason")]
        public int? Reason { get; set; }

        [DataMember(Name = "point")]
        public int? Point { get; set; }

        [DataMember(Name = "contactComments")]
        public List<CommentContract> ContactComments { get; set; }

        [DataMember(Name = "calls")]
        public List<CallContract> Calls { get; set; }
    }
}
