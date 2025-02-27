using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using CRM.Data.Database;
using CRM.Operation;
using CRM.Operation.Models.RequestModels;
using CRM.Server.Attributes;

namespace CRM.Server.Controllers
{
    [ApiController]
    [Route("api/client")]
    public class ClientController : BaseController
    {
        private readonly IConfiguration _configuration;
        CRMDbContext _context;
        public ClientController(CRMDbContext db, IConfiguration configuration)
        {
            _context = db;
            _configuration = configuration;
        }

        [HttpPost("createorupdate")]
        [WebApiAuthenticate]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult ClientCreateOrUpdate(ClientInfoContract clientInfoContract)
        {
            ClientQueuePutOperation operation = new ClientQueuePutOperation(_context);
            operation.Execute(clientInfoContract);
            return OperationResult(operation.Result);
        }

        //[HttpPost("update")]
        //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        //public ActionResult Update(ClientInfoContract clientInfoContract)
        //{
        //    ClientQueuePutOperation operation = new ClientQueuePutOperation(_context);
        //    operation.Execute(clientInfoContract);
        //    return OperationResult(operation.Result);
        //}

        [HttpPost("create")]
        [WebApiAuthenticate]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public ActionResult Create(ClientInfoContract clientInfoContract)
        {
            ClientQueuePutOperation operation = new ClientQueuePutOperation(_context);
            operation.Execute(clientInfoContract);
            return OperationResult(operation.Result);
        }
    }
}
