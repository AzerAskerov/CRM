using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using CRM.Operation;
using CRM.Data.Database;
using CRM.Operation.Models;
using System;
using System.Text.Json;
using CRM.Operation.JsonConverters;
using CRM.Operation.Models.DealOfferModels;
using Newtonsoft.Json;
using Zircon.Core.OperationModel;
using JsonSerializer = System.Text.Json.JsonSerializer;
using CRM.Server.Attributes;

namespace CRM.Server.Controllers
{
    [ApiController]
    [Route("api/sourceloader")]
    public class SourceLoaderController : BaseController
    {
        private readonly IConfiguration _configuration;
        CRMDbContext _context;
        public SourceLoaderController(CRMDbContext db, IConfiguration configuration)
        {
            _context = db;
            _configuration = configuration;
        }


        [HttpPost("load")]
        [WebApiAuthenticate]
        public ActionResult Load(SourceLoaderModel sourceLoaderModel)
        {
            try
            {
                sourceLoaderModel.Parent = (IModel)JsonConvert.DeserializeObject(sourceLoaderModel.ParentAsJson, Type.GetType(sourceLoaderModel.ParentPropertyNamespace)!);
            }
            catch (Exception e)
            {
                // ignored
            }

            SourceLoaderOperation operation = new SourceLoaderOperation(_context);
            operation.Execute(sourceLoaderModel);

            return OperationResult(operation.Result, operation.Model);
        }
    }
}
