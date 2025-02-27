using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Attributes;
using CRM.Operation.Localization;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.Login;
using CRM.Operation.Models.RequestModels;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models
{
    public class DealListItem : IModel
    {
        [DisplayNameLocalized("DealModel.DealSubject")]
        public string DealSubject { get; set; }
        public Guid DealGuid { get; set; }

        public int? ClientInn { get; set; }
        
        [DisplayNameLocalized("DealNumber")]
        public string DealNumber { get; set; }
        public DealStatus DealStatus { get; set; }

        [DisplayNameLocalized("DealModel.DealStatus")]
        public string DealStatusAsString => DealStatus.ToString().Translate();
        [DisplayNameLocalized("DealTemplateName")]
        public string DealTemplateName => DealType.ToString().Translate();


        [DisplayNameLocalized("InsuredPerson")]
        public string ClientFullname => ClientInnNavigation.Companies?.Any() == true
                ? ClientInnNavigation.Companies.FirstOrDefault()?.CompanyName
                : ClientInnNavigation.PhysicalPeople.FirstOrDefault()?.FullName;
        public OfferTypeEnum DealType { get; set; }
        
        [DisplayNameLocalized("CreatedTimeStamp")]
        public DateTime CreatedTimeStamp { get; set; }
        public Guid CreatedByUserGuid { get; set; }
        [DisplayNameLocalized("CreatedByUser.FullName")]
        public string CreatedByUserFullName { get; set; }
        public ClientRefModel ClientInnNavigation { get; set; }
        public Guid? UnderwriterUserGuid { get; set; }
        [DisplayNameLocalized("UnderwriterUser.FullName")]
        public string UnderwriterUserFullName { get; set; }
        public UserSummaryShort UnderwriterUser { get; set; }
        public UserSummaryShort CreatedBy { get; set; }
        public DealResponsiblePersonTypeEnum ResponsiblePersonType { get; set; }
        public List<DiscussionViewModel> Discussion { get; set; }
        
        [JsonIgnore]
        public bool HasUnreadMessage { get; set; }
    }
}