using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNet.OData;

using AutoMapper;

using Zircon.Core.Authorization;

using CRM.Data.Database;
using CRM.Operation.Models;
using Microsoft.AspNet.OData.Routing;
using CRM.Server.Attributes;
using Microsoft.EntityFrameworkCore;
using CRM.Operation.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Server.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompanyApiController : BaseController
    {
        CRMDbContext _context;
        IMapper _mapper;
        public CompanyApiController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }


        [WebApiAuthenticate]
        [HttpGet("getall")]
        public ActionResult GetAll()
        {
            var companies = from
              company in _context.Companies.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now)
                            join doc in _context.Documents.Where(x => x.DocumentType == 2 && x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now)
                           on company.Inn equals doc.Inn
                            select new CompanyListItemModel { CompanyName = company.CompanyName, TinNumber = doc.DocumentNumber, INN = company.Inn };



            return OperationResult(new Zircon.Core.OperationModel.OperationResult(), companies.ToList());
        }

     
    }
}
