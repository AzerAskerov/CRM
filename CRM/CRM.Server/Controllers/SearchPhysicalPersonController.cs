using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;

using AutoMapper;

using CRM.Data.Database;
using CRM.Operation.Models;
using CRM.Server.Attributes;

namespace CRM.Server.Controllers
{
    public class SearchPhysicalPersonController : ODataController
    {
        CRMDbContext _context;
        IMapper _mapper;
        public SearchPhysicalPersonController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }

        [EnableQuery]
        [ODataRoute("searchphysicalperson")]
        [WebApiAuthenticate]
        public IEnumerable<ContactListItemModel> Get()
        {

            return _mapper.ProjectTo<ContactListItemModel>(
    _context.Clients
        .Join(
            _context.PhysicalPeople,
            c => c.Inn,
            p => p.Inn,
            (c, p) => new { Client = c, PhysicalPerson = p })
        .Where(cp => cp.PhysicalPerson.ValidFrom <= DateTime.Now
                    && cp.PhysicalPerson.ValidTo >= DateTime.Now
                    && !cp.Client.IsHidden)
        .Select(cp => cp.PhysicalPerson)
);
        }
    }
}
