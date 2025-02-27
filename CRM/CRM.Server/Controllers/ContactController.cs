using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;

using AutoMapper;

using CRM.Data.Database;
using CRM.Operation.Models.RequestModels;
using CRM.Server.Attributes;

namespace CRM.Server.Controllers
{
    public class ContactController : ODataController
    {
        CRMDbContext _context;
        IMapper _mapper;
        public ContactController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }

        [EnableQuery]
        [WebApiAuthenticate]
        [ODataRoute("contact")]
        public IEnumerable<ContactInfoContract> Get()
        {
            return _mapper.ProjectTo<ContactInfoContract>(_context.ClientContactInfoComps.
                Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
        }


        [EnableQuery]
        [WebApiAuthenticate]
        [ODataRoute("contact({inn})")]
        public IEnumerable<ContactInfoContract> GetContact(int inn)
        {
            return _mapper.ProjectTo<ContactInfoContract>(_context.ClientContactInfoComps.
                Where(x => x.Inn == inn && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now)); 
        }
    }
}
