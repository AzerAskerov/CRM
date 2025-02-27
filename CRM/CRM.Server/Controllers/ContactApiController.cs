using AutoMapper;
using CRM.Data.Database;
using CRM.Server.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Operation.Models.RequestModels;
using AntDesign;
using CRM.Operation;
using CRM.Operation.MobileOperations;
using CRM.Operation.Models.MobileModels;

namespace CRM.Server.Controllers
{
    [Route("api/ContactApi")]
    [ApiController]
    public class ContactApiController : BaseController
    {
        private readonly IMapper _mapper;
        public ContactApiController(IMapper mapper, CRMDbContext dbContext)
        {
            _mapper = mapper;
            DbContext = dbContext;
        }

        [WebApiAuthenticate]
        [HttpGet("GetAuthTokenBy")]
        public async Task<List<string>> GetContact(int inn, string finCode)
        {
            var query = from ci in DbContext.ContactInfos
                        join ccip in DbContext.ClientContactInfoComps on ci.Id equals ccip.ContactInfoId
                        join p in (from p in DbContext.PhysicalPeople
                                   where (inn == 0 || p.Inn == inn)
                                         && (string.IsNullOrEmpty(finCode) || p.Pin == finCode)
                                         && (p.ValidFrom <= DateTime.Now && p.ValidTo >= DateTime.Now)
                                   orderby p.ValidFrom descending
                                   select p).Take(1)
                            on ccip.Inn equals p.Inn
                        where ci.Type == 6 && (ccip.ValidFrom <= DateTime.Now && ccip.ValidTo >= DateTime.Now)
                        select ci.Value;

            return await query.Distinct().ToListAsync();
        }
        [WebApiAuthenticate]
        [HttpGet("GetAllMobileUsers")]
        public ActionResult GetAllMobileUsers(GetMobileClientsSearchModel searchModel)
        {
            GetMobileClientOperation operation = new GetMobileClientOperation(DbContext);
            operation.Execute(searchModel);
            return Ok(operation.Model);
        }

        [WebApiAuthenticate]
        [HttpPost("AddPushNotification")]
        public ActionResult AddPushNotificationModel(PushNotificationDto model)
        {
            AddPushNotificationOperation operation = new AddPushNotificationOperation(DbContext);
            operation.Execute(model);
            return Ok(operation.Result);
        }
    }
}