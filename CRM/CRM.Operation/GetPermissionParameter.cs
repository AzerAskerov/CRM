using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CRM.Data.Enums;
using CRM.Operation.Helpers;
using Microsoft.EntityFrameworkCore;
using Zircon.Core.Authorization;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class GetPermissionParameter : BusinessOperation
    {
        public string PermissionParameters { get; set; }
        public GetPermissionParameter(DbContext db = null) : base(db)
        {
        }

        protected override void DoExecute()
        {

            PermissionParameters = CommonUserContextManager.CurrentUser.Permissions
                                                .FirstOrDefault(p => p.Name == Permissions.CRMRole.ToString())?.Parameters?.FirstOrDefault(x => x.Key == "Scope").Value;

        }
    }
    public class PermissionRequestModel
    {
        public string userGuid { get; set; }
        public string permissionName { get; set; }
        public int? lob { get; set; }
    }

}