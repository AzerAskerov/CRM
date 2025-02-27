
using CRM.Operation.Attributes;
using CRM.Operation.Enums;
using CRM.Operation.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Zircon.Core.Extensions;

namespace CRM.Operation.Models
{
    [DataContract]
    public class ContactListItemModel : ClientContract
    {

        [DataMember(Name = "positionname")]
        [DisplayNameLocalized("ContactListItemModel.PositionName")]
        public string PositionName { get; set; }

        [DataMember(Name = "gendertype")]
        [DisplayNameLocalized("ContactListItemModel.GenderType")]
        public string GenderType{ get; set; }

        [DataMember(Name = "age")]
        [DisplayNameLocalized("ContactListItemModel.Age")]
        public int Age { get; set; }
        
        [DataMember(Name = "days_left")]
        [DisplayNameLocalized("ContactListItemModel.DaysLeft")]
        public int DaysLeft { get; set; }
    }
}
