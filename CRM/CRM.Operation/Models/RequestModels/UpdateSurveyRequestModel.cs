using CRM.Operation.Attributes;
using System;


namespace CRM.Operation.Models.RequestModels
{
    public class UpdateSurveyRequestModel
    {
        [RequiredLocalized]
        public string DealNumber { get; set; }
        public string SurveyerLogonName { get; set; }
        public int? Status { get; set; }
        public string Description { get; set; }

        public Guid CurrentUserGuid { get; set; }
        public string CurrentUserName { get; set; }

    }
}