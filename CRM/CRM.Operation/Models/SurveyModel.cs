using System;
using System.Text.Json.Serialization;
using CRM.Data.Enums;
using CRM.Operation.Attributes;
using CRM.Operation.Localization;
using CRM.Operation.SourceLoaders;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models
{
    public class SurveyModel : IModel
    {
        public Guid DealGuid { get; set; }
        
        [CustomSource(typeof(SurveyerTypeSourceLoader))]
        [DisplayNameLocalized("OfferModel.SurveyerType")]
        public int? SurveyerType { get; set; }

        public string SurveyerLogonName { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string SurveyLink { get; set; }

        [JsonIgnore]
        public string StatusAsString
        {
            get
            {
                return Status switch
                {
                    0 => (SurveyStatusEnum.NotSetYet).ToString().Translate(),
                    _ => ((SurveyStatusEnum) Status).ToString().Translate()
                };
            }
            set
            {
                if (Enum.IsDefined(typeof(SurveyStatusEnum), value))
                {
                    Status = (int) Enum.Parse<SurveyStatusEnum>(value);
                }
            }
        }
        
        public decimal? SumInsuredAmount { get; set; }
        
        public OfferTypeEnum OfferType { get; set; }

        public SurveyModel()
        {
            Status = (int) SurveyStatusEnum.NotSetYet;
        }
    }
}