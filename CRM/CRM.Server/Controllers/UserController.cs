using System;
using CRM.Operation;
using CRM.Operation.Models.RequestModels;
using CRM.Server.Attributes;
using Microsoft.AspNetCore.Mvc;
using Zircon.Core.Authorization;

namespace CRM.Server.Controllers
{
    public class UserController : BaseController
    {
        [WebApiAuthenticate]
        public IActionResult GetUserInfoByUserGuid(Guid userGuid)
        {
            var op = new GetUserInfoByUserGuidFromWebIms();
            op.Execute(userGuid);
            return new JsonResult(op.UserSummaryShortModel);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetUsersWithPermission(PermissionContract contract)
        {
            var op = new GetUsersWithPermissionFromWebImsOperation();
            op.Execute(contract);
            return new JsonResult(op.Users);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetUsersWithPermissionParameterValue(GetUsersWithPermissionParameterValueContract contract)
        {
            var op = new GetUsersWithPermissionParameterValueFromWebImsOperation();
            op.Execute(contract);
            return new JsonResult(op.Users);
        }
    }
}