using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

using CRM.Data.Database;
using CRM.Operation;
using CRM.Operation.Models.RequestModels;
using CRM.Server.Attributes;
using CRM.Operation.Models;

namespace CRM.Server.Controllers
{
    [Route("api/policy")]
    [ApiController]
    public class PolicyController : BaseController
    {
        CRMDbContext _context;
        public PolicyController(CRMDbContext db)
        {
            _context = db;
        }

        [HttpPost("products")]
        [WebApiAuthenticate]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public ActionResult GetPolicies(ClientContract model)
        {
            GenerateProductListOperation<ClientContract> operation = new GenerateProductListOperation<ClientContract>(_context);
            operation.Execute(model) ;
            return OperationResult(operation.Result, operation.Model);
        }

        [HttpPost("curatorproducts")]
        [WebApiAuthenticate]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public ActionResult GetCuratorPolicies(CuratorProductRequest model)
        {
            GetCuratorProductsOperation operation = new GetCuratorProductsOperation(_context);
            operation.Execute(model);
            return OperationResult(operation.Result, operation.Model);
        }

        [HttpPost("GetClientPolicy")]
        [WebApiAuthenticate]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public ActionResult GetClientPolicy(GetClientPolicyModel model)
        {
            GenerateProductListOperation<GetClientPolicyModel> operation = new GenerateProductListOperation<GetClientPolicyModel>(_context);
            operation.Execute(model);
            return OperationResult(operation.Result, operation.Model?.FirstOrDefault(x => x.PolicyNumber == model.PolicyNumber));
        }

        [HttpPost("OpenPolicy")]
        [WebApiAuthenticate]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public ActionResult OpenPolicy(ProductBaseModel model)
        {
            OpenPolicyByPolicyNumberOperation operation = new OpenPolicyByPolicyNumberOperation(_context);
            operation.Execute(model.PolicyNumber);
            return OperationResult(operation.Result, operation.Model);
        }
    }
}
