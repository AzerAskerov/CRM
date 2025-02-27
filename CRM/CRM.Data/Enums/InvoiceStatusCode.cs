
namespace CRM.Data.Enums
{
   
        public enum InvoiceStatusCode : int
        {
            None = 0,

            PreCreate = 5,
           
            Create = 10,

            Sent = 20,

            PartiallyPaid = 30,

            PartiallyAdoptedOn7S = 35,

            Late = 40,

            PrepareRemainder = 50,
            Reminder = 60,
            HandedOver = 65,

            Paid = 70,
            AdoptedOn7S = 75,
            Concerted = 80,

            PaidAndCanceled = 90,

            Closed = 95,

            Canceled = 99,

            PreparedForPayOut = 100,

            ApprovedForPayOut = 110,

            
            CreatedPaymentTask = 120,
            
            PreliminaryPaid = 130
        }
    
}
