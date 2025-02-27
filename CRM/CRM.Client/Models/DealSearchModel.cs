using CRM.Data.Enums;

namespace CRM.Client.Models
{
    public class DealSearchModel : ContactSearchModel
    {
        public string DealSubject { get; set; }
        public string DealNumber { get; set; }
        public string MediatorUserName { get; set; }
        public string UnderwriterUserName { get; set; }
        public string DiscussionText { get; set; }
        public DealStatus DealStatus { get; set; } = DealStatus.Select;
        public OfferTypeEnum DealType { get; set; } = OfferTypeEnum.Select;
        public int? ClientInn { get; set; }
    }
}