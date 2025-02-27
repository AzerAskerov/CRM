using Microsoft.AspNetCore.Mvc;

using CRM.Operation.Localization;

using CRM.Data.Database;
using CRM.Operation;
using CRM.Server.Attributes;

namespace CRM.Server.Controllers
{
    [Route("api/localize")]
    [ApiController]
    public class LocalizeController : BaseController
    {
        public LocalizeController(CRMDbContext db)
        {
            DbContext = db;
        }

        [HttpGet("local")]
        public ActionResult<LocalizationModel> GetLocalization()
        {
            GetLocalizationOperation operation = new GetLocalizationOperation(DbContext);
            operation.Execute();

            return operation.Model;
        }
    }
}