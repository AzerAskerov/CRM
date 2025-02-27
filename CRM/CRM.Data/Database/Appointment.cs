using System;
using System.Collections.Generic;



namespace CRM.Data.Database
{
    public partial class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CommentsAfterAppointent { get; set; }
        public byte Status { get; set; }
        public int Inn { get; set; }
        public string Location { get; set; }
        public string ULogonName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte? TypeId { get; set; }
        public virtual ClientRef InnNavigation { get; set; }
    }
}
