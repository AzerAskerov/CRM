using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;

using AutoMapper;

using CRM.Data.Database;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Models;
using CRM.Server.Attributes;

namespace CRM.Server.Controllers
{
    public class RelationController : ODataController
    {
        CRMDbContext _context;
        IMapper _mapper;
        public RelationController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }

        [EnableQuery]
        [ODataRoute("relation")]
        [WebApiAuthenticate]
        public IEnumerable<RelationContract> Get()
        {
            return _mapper.ProjectTo<RelationContract>(_context.ClientRelationComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
        }


        
    }
}
