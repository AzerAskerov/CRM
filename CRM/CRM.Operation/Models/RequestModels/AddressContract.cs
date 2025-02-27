using CRM.Operation.Attributes;
using CRM.Operation.Enums;
using CRM.Operation.SourceLoaders;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Zircon.Core.Attributes;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.RequestModels
{
    public class AddressContract : IModel
    {
        [DataMember(Name = "id")]
        [IgnoreDataMember]
        public int? Id { get; set; }

        [CustomSource(typeof(CountrySourceLoader))]
       
        [DataMember(Name = "countryId")]
        public int? CountryId { get; set; }

        [CustomSource(typeof(RegionSourceLoader))]
    
        [DataMember(Name = "regOrCityId")]
        public int? RegOrCityId { get; set; }

        [DataMember(Name = "districtOrStreet")]
       
        public string DistrictOrStreet { get; set; }

        [DataMember(Name = "additionalInfo")]
        public string AdditionalInfo { get; set; }
    }
}
