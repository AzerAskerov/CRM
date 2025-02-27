using System.Collections.Generic;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CRM.Data.Database;
using CRM.Operation;
using CRM.Server.Attributes;

namespace CRM.Server.Controllers
{
    [Route("api/medicine")]
    [ApiController]
   
    public class MedicineController : BaseController
    {
        public MedicineController(CRMDbContext db)
        {
            DbContext = db;
        }


        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost("medical")]
        [WebApiAuthenticate]
        public ActionResult GetMedicicalServices([FromBody] int inn)
        {
            GenerateMedicineListOperation operation = new GenerateMedicineListOperation(DbContext);
            operation.Execute(inn);
            return OperationResult(operation.Result, operation.Model);
        }

    }
}
