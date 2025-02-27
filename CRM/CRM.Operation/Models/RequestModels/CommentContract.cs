using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;

namespace CRM.Operation.Models.RequestModels
{
    [DataContract]
    public class CommentContract
    {
        [IgnoreDataMember]
        [Key]
        public int? Id { get; set; }
        
        [Attributes.RequiredLocalized]
        [Attributes.DisplayNameLocalized("CommentContract.Text")]
        [DataMember(Name = "Text")]
        public string Text { get; set; }

        [DataMember(Name = "creator")]
        public string Creator { get; set; }


        [DataMember(Name = "fullname")]
        public string FullName { get; set; }

        [DataMember(Name = "create_timestamp")]
        public DateTime? CreateTimeStamp { get; set; }
    }
}
