using AutoMapper;
using CRM.Data.Database;
using CRM.Operation.Enums;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using CRM.Server.Attributes;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Server.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/assets")]
    public class AssetsController : BaseController
    {
        CRMDbContext _context;
        IMapper _mapper;
        public AssetsController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }


        [HttpPost("clientassets")]
        [WebApiAuthenticate]
        public IEnumerable<AssetViewModel> Get(ClientContract model)
        {
            var dbOperation = _mapper.ProjectTo<AssetViewModel>(
                from
                ad in _context.AssetDetails
                join a in _context.Assets on ad.Id equals a.AssetDetailId
                where (a.ValidFrom <= DateTime.Now && a.ValidTo >= DateTime.Now)
                 && a.Inn == model.INN
                select new AssetViewModel {
                    AssetType = (InsuredObjectType) ad.AssetType,
                    AssetDescription = ad.AssetDescription, AssetInfo = ad.AssetInfo}
                ).ToList().Distinct();


            
            return dbOperation;
       
        
        }



        [EnableQuery]
        [ODataRoute("asset({inn})")]
        [WebApiAuthenticate]
        public AssetContract GetAsset(int inn)
        {
            return _mapper.ProjectTo<AssetContract>(
                from
                a in _context.Assets
                where (a.ValidFrom <= DateTime.Now && a.ValidTo >= DateTime.Now)
                && a.Inn == inn
                select a
                ).FirstOrDefault();
        }
    }
}
