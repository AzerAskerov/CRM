using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zircon.Core.Authorization;

namespace CRM.Operation.Models
{
    public class AuthorizeContractModel
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public PermissionContract[] Permissions { get; set; }
    }
}
