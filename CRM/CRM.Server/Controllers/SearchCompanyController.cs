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
    public class SearchCompanyController : ODataController
    {
        CRMDbContext _context;
        IMapper _mapper;
        public SearchCompanyController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }

        [EnableQuery]
        [ODataRoute("searchcompany")]
        [WebApiAuthenticate]
        public IEnumerable<CompanyListItemModel> Get()
        {
            return _mapper.ProjectTo<CompanyListItemModel>
                  (from
                   c in _context.Clients
                   join
                     cc in _context.Companies
                    on c.Inn equals cc.Inn
                   where (cc.ValidFrom <= DateTime.Now && cc.ValidTo >= DateTime.Now)
                   && (c.ValidFrom <= DateTime.Now && c.ValidTo >= DateTime.Now)
                    && c.IsHidden == false
                   select cc);
        }
    }
}
