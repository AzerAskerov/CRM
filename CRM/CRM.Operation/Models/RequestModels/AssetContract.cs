using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.RequestModels
{
    [DataContract]
    public class AssetContract : IModel
    {
        [Key]
        [IgnoreDataMember]
        public int Id { get; set; }

        [DataMember(Name = "assetType")]
        public int AssetType { get; set; }

        [DataMember(Name = "insuredObjectGuid")]
        public Guid InsuredObjectGuid { get; set; }


        [DataMember(Name = "assetInfo")]
        public string AssetInfo { get; set; }

        [DataMember(Name = "assetDescription")]
        public string AssetDescription { get; set; }
   
    }
}
