using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models.RequestModels
{
    public class PermissionRequestModel
    {
        public string userGuid { get; set; }
        public string permissionName { get; set; }
        public int? lob { get; set; }
    }

    public class PermissionParameterRequestModel : PermissionRequestModel
    {
        public string ParameterName { get; set; }

    }
}
