using CRM.Data.Enums;
using System;

namespace CRM.Operation.Models.RequestModels
{
    public class ParticipatedInsuredPersonPoliciesViewModel
    {
        [Attributes.DisplayNameLocalized("ParticipatedInsuredPersonPolicies.PolicyNumber")]
        public string PolicyNumber { get; set; }

        [Attributes.DisplayNameLocalized("ParticipatedInsuredPersonPolicies.Status")]
        public string Status { get; set; }

        [Attributes.DisplayNameLocalized("ParticipatedInsuredPersonPolicies.PolicyType")]
        public string PolicyType { get; set; }
        public int Lob { get; set; }

        [Attributes.DisplayNameLocalized("ParticipatedInsuredPersonPolicies.StartDate")]
        public DateTime StartDate { get; set; }

        [Attributes.DisplayNameLocalized("ParticipatedInsuredPersonPolicies.EndDate")]
        public DateTime EndDate { get; set; }
        public PolicyStatusLocal LocalStatus { get; set; }

    }
}
