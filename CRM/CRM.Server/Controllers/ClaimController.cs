using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

using CRM.Data.Database;
using CRM.Operation;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using CRM.Server.Attributes;

namespace CRM.Server.Controllers
{
    [Route("api/claim")]
    [ApiController]

    public class ClaimController : BaseController
    {
        CRMDbContext _context;
        public ClaimController(CRMDbContext db)
        {
            _context = db;
        }

        [HttpPost("claims")]
        [WebApiAuthenticate]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public ActionResult GetClaims(ClientContract model)
        {
            GenerateClaimListOperation operation = new GenerateClaimListOperation(_context);
            operation.Execute(model) ;
            return OperationResult(operation.Result, operation.Model);
        }


    }
}
