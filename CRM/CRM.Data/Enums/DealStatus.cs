namespace CRM.Data.Enums
{
    /// <summary>
    /// Represents statuses of deal.
    /// </summary>
    public enum DealStatus : short
    {
        Select = -1,
        New = 0,
        Draft = 1, 
        SurveySent = 2, 
        Offered = 3, 
        Rejected = 4, 
        Agreed = 5, 
        Linked = 6, 
        PendingUnderwriting = 7
    }
}