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
using CRM.Operation.Enums;
using CRM.Server.Attributes;

namespace CRM.Server.Controllers
{
    public class DocumentController : ODataController
    {
        CRMDbContext _context;
        IMapper _mapper;
        public DocumentController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }

        [EnableQuery]
        [WebApiAuthenticate]
        [ODataRoute("document")]
        public IEnumerable<DocumentContract> Get()
        {
            return _mapper.ProjectTo<DocumentContract>(_context.Documents.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
        }


        [EnableQuery]
        [WebApiAuthenticate]
        [ODataRoute("document({inn})")]
        public IEnumerable<DocumentContract> GetDocument(int inn)
        {
            return _mapper.ProjectTo<DocumentContract>(_context.Documents.
                Where(x => x.Inn == inn && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.DocumentType != (int)DocumentTypeEnum.Queue));
        }
    }
}
