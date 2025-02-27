using System;
using CRM.Operation.Attributes;
using CRM.Operation.SourceLoaders;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.DealOfferModels
{
    public class VoluntaryHealthInsuranceEmployeeGroupViewModel : IModel
    {
        public int Id { get; set; }
        public Guid VoluntaryHealthInsuranceGuid { get; set; }
        
        [CustomSource(typeof(VoluntaryHealthEmployeeAgeRangeSourceLoader))]
        [DisplayNameLocalized("DropDown.AgeRangeOid")]
        [RequiredLocalized]
        public int? AgeRangeOid { get; set; }
        public string AgeRange { get; set; }
        
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.Count")]
        public int? Count { get; set; }
        
        [RequiredLocalized]
        [DisplayNameLocalized("DropDown.Gender")]
        [CustomSource(typeof(SexCodeSourceLoader))]
        public int? Gender { get; set; }
        public string GenderName { get; set; }
        
        [RequiredLocalized]
        [DisplayNameLocalized(("DropDown.EmployeeType"))]
        [CustomSource(typeof(VoluntaryHealthEmployeeTypeSourceLoader))]
        public int? EmployeeType { get; set; }
        public string EmployeeTypeName { get; set; }
    }
}