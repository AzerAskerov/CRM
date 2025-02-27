using System.Linq;
using AutoMapper;
using CRM.Data.Database;
using CRM.Operation.Models;
using CRM.Server.Attributes;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.EntityFrameworkCore;

namespace CRM.Server.Controllers
{
    public class DealODataController : ODataController
    {
        private readonly CRMDbContext _context;
        private readonly IMapper _mapper;
    
        public DealODataController(CRMDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    
        [EnableQuery]
        [ODataRoute("deal")]
        [WebApiAuthenticate]
        public IQueryable<DealListItem> Get()
        {
            return _mapper.ProjectTo<DealListItem>
            (from
                    deal in _context.Deal
                select deal).AsNoTracking();
        }
    }
}