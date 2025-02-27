using System;

namespace CRM.Operation.Models.Login
{
    public class UserSummaryShort
    {
        public string Login { get; set; }
        public string FullName { get; set; }
        public string UserEmail { get; set; }
        public Guid UserGuid { get; set; }
        public string UserPhone { get; set; }

        public string FormatedUserName { get { return string.Format("{0} {1}", Login, FullName); } }
        public DateTime TillValidDate { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Login, FullName);
        }
    }
}