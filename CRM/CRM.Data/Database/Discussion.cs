using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CRM.Data.Database
{
    public partial class Discussion
    {
        public int Id { get; set; }
        public Guid DealGuid { get; set; }
        public Guid SenderGuid { get; set; }
        public Guid? ReceiverGuid { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
        public virtual Deal Deal { get; set; }
    }
}
