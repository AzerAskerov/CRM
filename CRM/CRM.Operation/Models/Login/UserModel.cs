using System;
using System.Runtime.Serialization;
using Zircon.Core.Authorization;

namespace CRM.Operation.Models.Login
{ 
    [DataContract]
    public class UserModel
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string FatherName { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public bool IsAuthenticated { get; set; }
        [DataMember]
        public Guid? Guid { get; set; }
        [DataMember]
        public DateTime? BirthDate { get; set; }
        [DataMember]
        public bool IsNewClient { get; set; }
        [DataMember]
        public string Token { get; set; }
        [DataMember]
        public PermissionCollection Permissions{get;set;}
    }
}
