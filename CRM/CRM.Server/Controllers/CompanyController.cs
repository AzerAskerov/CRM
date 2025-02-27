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

namespace CRM.Server.Controllers
{
    public class CompanyController : ODataController
    {
        CRMDbContext _context;
        IMapper _mapper;
        public CompanyController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }

        [EnableQuery]
        [ODataRoute("company")]
        [WebApiAuthenticate]
        public IEnumerable<CompanyListItemModel> Get()
        {
            string condition = string.Empty;

            var perm = CommonUserContextManager.CurrentUser.Permissions.FirstOrDefault(x => x.Name == "CRMRole");

            if (bool.Parse(perm.Parameters.FirstOrDefault(x => x.Key == "Full").Value))
            {
                condition = "";
            }
            else
            {
                condition = CommonUserContextManager.CurrentUser.UserName;
            }

            return _mapper.ProjectTo<CompanyListItemModel>
                (from
                 c in _context.Clients
                 join
                   cc in _context.Companies
                  on c.Inn equals cc.Inn
                 join
                 u in _context.UserClientComps
                 on cc.Inn equals u.Inn
                 where (cc.ValidFrom <= DateTime.Now && cc.ValidTo >= DateTime.Now)
                 && (c.ValidFrom <= DateTime.Now && c.ValidTo >= DateTime.Now)
                 && (u.ULogonName == CommonUserContextManager.CurrentUser.UserName || u.ULogonName == condition)
                  && c.IsHidden == false
                 select cc).AsQueryable();
        }

        [EnableQuery]
        [ODataRoute("company({inn})")]
        [WebApiAuthenticate]
        public CompanyListItemModel GetCompany(int inn)
        {
            return _mapper.ProjectTo<CompanyListItemModel>(
                from
                c in _context.Companies
                where (c.ValidFrom <= DateTime.Now && c.ValidTo >= DateTime.Now)
                && c.Inn == inn
                select c
                ).FirstOrDefault();
        }
    }
}
