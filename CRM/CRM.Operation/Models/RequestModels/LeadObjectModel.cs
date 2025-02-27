using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.RequestModels
{
    public class LeadObjectModel : IModel
    {
        [Key]
        [IgnoreDataMember]
        public int Id { get; set; }

        [DataMember(Name = "uniqueKey")]
        public string UniqueKey { get; set; }

        [DataMember(Name = "discountPercentage")]
        public decimal DiscountPercentage { get; set; }

        [DataMember(Name = "campaignName")]
        public string CampaignName { get; set; }

        [DataMember(Name = "payload")]
        public string Payload { get; set; }

        [DataMember(Name = "objectId")]
        public string ObjectId { get; set; }

        [DataMember(Name = "userGuid")]
        public string UserGuid { get; set; }

        [DataMember(Name = "validTo")]
        public string ValidTo { get; set; }

        [DataMember(Name = "validFrom")]
        public string ValidFrom { get; set; }
    }
}
