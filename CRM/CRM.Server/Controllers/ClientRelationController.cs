using System;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;

using CRM.Data.Database;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Models;
using CRM.Server.Attributes;

namespace CRM.Server.Controllers
{

    public class ClientRelationController : ODataController
    {
        CRMDbContext _context;
        IMapper _mapper;
        public ClientRelationController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }

        [EnableQuery]
        [ODataRoute("clientrelation")]
        [WebApiAuthenticate]
        public IEnumerable<ClientRelationModel> Get()
        {
            var df = _mapper.ProjectTo<ClientRelationModel>(_context.ClientRelationComps.
                Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
            return df;
        }

        [EnableQuery]
        [WebApiAuthenticate]
        [ODataRoute("clientrelation({inn})")]
        public IEnumerable<ClientRelationModel> GetRelation(int inn)
        {
           return _mapper.ProjectTo<ClientRelationModel>(_context.ClientRelationComps.
                Where(x => x.Client1 == inn && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
        }
        
    }
}
