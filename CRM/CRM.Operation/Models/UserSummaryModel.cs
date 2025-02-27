using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zircon.Core.Authorization;

namespace CRM.Operation.Models
{
    public class UserSummaryModel
    {
        public string Login { get; set; }

        public string FullName { get; set; }
        public string UserEmail { get; set; }

        public DateTime TillValidDate { get; set; }

        public Guid UserGuid { get; set; }
        public bool IsAuthenticated { get; set; }

        public PermissionCollection Permissions { get; set; }

        public string[] MainHierarchy { get; set; }

        public string[] ElementHierarchy { get; set; }

        public string[] SubordinateHierarchy { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Login, FullName);
        }
    }
}
