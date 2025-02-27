

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zircon.Core.HttpContextHelper;
using Zircon.Core.OperationModel;
using CRM.Data.Database;

namespace CRM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected ISession Session => ContextHelper.Current.Session;
        protected CRMDbContext DbContext { get; set; }
        
        protected ActionResult OperationResult(OperationResult result = null, object output = null)
        {
            if (result == null)
                return Ok();

            if (result.Success)
                if (output == null)
                    return Ok(result);
                else
                    return Ok(output);
            else
                return Ok(result);

        }

        protected ActionResult OperationResult<T>(OperationResult result, T output)
        {
            OperationResult<T> resp = new OperationResult<T>(output);
            resp.Merge(result);

            //if (result == null)
                return Ok(resp);

            //if (result.Success)
            //    return Ok(StatusCodes.Status202Accepted, resp);
            //else
            //    return Ok(StatusCodes.Status500InternalServerError, resp);
        }
    }
}