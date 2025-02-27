using System.Runtime.Serialization;
using CRM.Operation.Attributes;
using CRM.Operation.Models.RequestModels;


namespace CRM.Operation.Models
{
    public class CompanyListItemModel : ClientContract
    {
        [DataMember(Name = "tinnumber")]
        [DisplayNameLocalized("CompanyListItemModel.TinNumber")]
        public string TinNumber { get; set; }
    }
}
