using CRM.Data.Database;
using CRM.Operation;
using CRM.Operation.Models.RequestModels;
using CRM.Server.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Server.Controllers
{
    [Route("api/commentcreators")]
    public class CommentsWithCreatorsController : BaseController
    {
        CRMDbContext _context;

        public CommentsWithCreatorsController(CRMDbContext dbContext)
        {
            DbContext = dbContext;
        }

        [Authorize]
        [WebApiAuthenticate]
        [HttpPost("comments")]

        public async Task<ActionResult> GetComments(CommentsWithCreatorRequestModel model)
        {
            CommentsWithCreatorsOperation op = new CommentsWithCreatorsOperation(DbContext);
            op.Execute(model);
            return OperationResult(op.Result, op.Comments);
        }

    }
}
