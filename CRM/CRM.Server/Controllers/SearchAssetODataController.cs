using AutoMapper;
using CRM.Data.Database;
using CRM.Operation.Models;
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
    public class SearchAssetODataController : ODataController
    {
        CRMDbContext _context;
        IMapper _mapper;
        public SearchAssetODataController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }
        [HttpGet]
        [EnableQuery]
        [ODataRoute("findClientByAsset(AssetInfo={AssetInfo})")]
        [WebApiAuthenticate]
        public IEnumerable<ClientContract> FindClientByAsset([FromODataUri] string AssetInfo)
        {

            var asset = _context.Assets.FirstOrDefault(x => x.AssetInfo.Equals(AssetInfo) && x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now);
            if (asset is null)
                return new List<ClientContract>();

            return _mapper.ProjectTo<ClientContract>(
               from cr in _context.ClientRefs
               where cr.Inn == asset.Inn
               select cr
               ).ToList();
        }
    }
}
