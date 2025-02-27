namespace CRM.Operation.Models.DealOfferModels
{
    public interface IRenewableDealOfferModel
    {
        public bool IsRenew { get; set; }
        public string ExistingPolicyNumber { get; set; }
    }
}