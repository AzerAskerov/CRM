using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CRM.Data.Database;
using CRM.Operation;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using CRM.Server.Attributes;

namespace CRM.Server.Controllers
{
    [Route("api/invoice")]
    [ApiController]
    public class InvoiceController : BaseController
    {
        public InvoiceController(CRMDbContext db)
        {
            DbContext = db;
        }


        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost("invoices")]
        [WebApiAuthenticate]
        public ActionResult GetInvoioces(ClientContract model)
        {
            GenerateInvoiceListOperation operation = new GenerateInvoiceListOperation(DbContext);
            operation.Execute(model);
            return OperationResult(operation.Result, operation.Model);
        }

    }
}
