using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CRM.Data.Enums;
using CRM.Operation.Attributes;
using CRM.Operation.Models.Login;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using Microsoft.AspNetCore.Components;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.DealOfferModels
{
    public class DealModel : IModel
    {
        private ClientContract _client;

        public DealModel()
        {
            DealGuid = Guid.NewGuid();
            DealDiscussionModel = new DealDiscussionModel();
            DealStatus = DealStatus.New;
            Tender = new TenderModel();
        }
        public string GeneralNote { get; set; }
        public Guid CurrentUserGuid { get; set; }
        public string CurrentUserName { get; set; }

        [RequiredLocalized]
        [DisplayNameLocalized("DealModel.DealSubject")]
        [MaxLength(500)]
        public string DealSubject { get; set; }
        public Guid DealGuid { get; set; }
        [DisplayNameLocalized("DealNumber")]
        public string DealNumber { get; set; }

        [DisplayNameLocalized("CreatedTimeStamp")]
        public DateTime CreatedTimeStamp { get; set; }
        public UserSummaryShort CreatedByUser { get; set; }
        public UserSummaryShort UnderwriterUser { get; set; }


        public delegate void DealModelChangedEventHandler(
    DealResponsiblePersonTypeEnum newResponsiblePersonType,
    DealStatus newDealStatus,
    UserSummaryShort UnderwriterUser

);

        public event DealModelChangedEventHandler OnModelChanged;
        int _responsiblePersonType;
        public DealResponsiblePersonTypeEnum ResponsiblePersonType
        {
            get => (DealResponsiblePersonTypeEnum)_responsiblePersonType;
            set
            {
                if (_responsiblePersonType != (int)value)
                {
                    _responsiblePersonType = (int)value;
                    OnModelChanged?.Invoke(
                (DealResponsiblePersonTypeEnum)_responsiblePersonType,
                (DealStatus)_dealStatus,
                UnderwriterUser
            );
                }
            }
        }

        [RequiredLocalized]
        [CascadingParameter(Name = "ClientContractModel")]
        [DisplayNameLocalized("Client")]
        public ClientContract Client
        {
            get => _client;
            set
            {
                _client = value;

                if (_client != null && FormComponentModel != null)
                {
                    FormComponentModel.DealClientType = _client.ClientType;
                }
            }
        }

        [RegexLocalized(@"^((?!994)\d{9,12})|(994\d{9})|(\d{7})$", "ClientSearchExtendedModel.PhoneFormatInvalid")]
        public string ClientPhoneNumber { get; set; }

        [RegexLocalized(@"^((?!994)\d{9,12})|(994\d{9})|(\d{7})$", "ClientSearchExtendedModel.PhoneFormatInvalid")]
        public string SurveyContactInfo { get; set; }

        [CustomSource(typeof(OfferTypeSourceLoader))]
        [RequiredLocalized]
        public int? SelectedOfferType { get; set; }

        [RequiredLocalized]
        [ValidateComplexType]
        public IDealOfferFormModel FormComponentModel { get; set; }
        
        [RequiredLocalized]
        [CustomSource(typeof(OfferLanguageSourceLoader))]
        [DisplayNameLocalized("OfferModel.OfferLanguage")]
        public int? OfferLanguageOid { get; set; }

        public string OfferLang { get; set; }
        private int _dealStatus;
     
        public DealStatus DealStatus
        {
            get => (DealStatus)_dealStatus;
            set
            {
                if (_dealStatus != (int)value)
                {
                    _dealStatus = (int)value;
                    OnModelChanged?.Invoke(
                (DealResponsiblePersonTypeEnum)_responsiblePersonType,
                (DealStatus)_dealStatus,
                UnderwriterUser
            );
                }
            }
        }
        public decimal? SumInsured { get; set; }
        
        public DealDiscussionModel DealDiscussionModel { get; set; }

        public SurveyModel Survey { get; set; }
        
        [ValidateComplexType]
        public List<OfferViewModel> Offers { get; set; }

        [ValidateComplexType]
        public TenderModel Tender { get; set; }

        public List<DealPolicyLinkViewModel> DealPolicyLinks { get; set; }
    }
}