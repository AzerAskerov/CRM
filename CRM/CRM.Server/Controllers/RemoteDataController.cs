using CRM.Data.Database;
using CRM.Operation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Server.Controllers
{
    [Route("api/remotedata")]
    [ApiController]
    public class RemoteDataController : BaseController
    {
        public RemoteDataController(CRMDbContext db)
        {
            DbContext = db;
        }

        [HttpPost("countries")]
        public ActionResult Countries()
        {
            GetRemoteCountriesOperation operation = new GetRemoteCountriesOperation(DbContext);
            operation.Execute();

            return OperationResult(operation.Result, operation.Data);
        }


        [HttpPost("cities")]
        public ActionResult Cities([FromBody] string city)
        {
            GetRemoteCitiesOperation operation = new GetRemoteCitiesOperation(DbContext);
            operation.Execute(city);

            return OperationResult(operation.Result, operation.Cities);
        }
    }
}
