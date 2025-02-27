using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNet.OData;

using AutoMapper;

using Zircon.Core.Authorization;


using CRM.Data.Database;
using CRM.Operation.Models;
using Microsoft.OData.Edm;
using CRM.Operation.Models.RequestModels;
using Microsoft.AspNet.OData.Routing;
using CRM.Server.Attributes;

namespace CRM.Server.Controllers
{
    public class TagController : ODataController
    {
        CRMDbContext _context;
        IMapper _mapper;
        public TagController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }

        [EnableQuery]
        [ODataRoute("tag")]
        [WebApiAuthenticate]
        public IEnumerable<TagContract> Get()
        {
            return _mapper.ProjectTo<TagContract>(_context.TagComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
        }


        [EnableQuery]
        [ODataRoute("tag({inn})")]
        [WebApiAuthenticate]
        public IEnumerable<TagContract> GetTag(int inn)
        {
            return _mapper.ProjectTo<TagContract>(_context.TagComps.
                Where(x => x.Inn == inn && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
        }
    }
}
