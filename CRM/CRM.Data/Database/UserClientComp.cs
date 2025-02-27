using System;



namespace CRM.Data.Database
{
    public partial class UserClientComp
    {
        public int Id { get; set; }
        public int? Inn { get; set; }
        public string ULogonName { get; set; }
        public int? LobOid { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; } 

        public virtual ClientRef ClientRef { get; set; }
    }
}
