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
    public class CommentController : ODataController
    {
        CRMDbContext _context;
        IMapper _mapper;
        public CommentController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }

        [EnableQuery]
        [ODataRoute("comment")]
        [WebApiAuthenticate]
        public IEnumerable<CommentContract> Get()
        {
            return _mapper.ProjectTo<CommentContract>(_context.CommentComps);
        }


        [EnableQuery]
        [ODataRoute("comment({inn})")]
        [WebApiAuthenticate]
        public IEnumerable<CommentContract> GetComment(int inn)
        {
            return _mapper.ProjectTo<CommentContract>(_context.CommentComps.
                Where(x => x.Inn == inn && x.ContactId == null));
        }
    }
}
