using AutoMapper;
using CRM.Data.Database;
using CRM.Operation;
using CRM.Operation.Models.RequestModels;
using CRM.Server.Attributes;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Server.Controllers
{
    [Route("api/participatedperson")]
    public class ParticipatedInsuredPersonController : BaseController
    {
        CRMDbContext _context;
        IMapper _mapper;

        public ParticipatedInsuredPersonController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }

        [HttpPost("policies")]
        [WebApiAuthenticate]
        public IActionResult GetParticipatedInsuredPersonPolicies(ClientContract model)
        {
            var y = _context.PhysicalPeople.FirstOrDefault(x => x.Inn == model.INN.Value
              && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);
            GetParticipatedPersonPoliciesOperation operation = new GetParticipatedPersonPoliciesOperation(DbContext);
            operation.Execute(y.Pin);
            return OperationResult(operation.Result, operation.Model);
        }

    }
}
