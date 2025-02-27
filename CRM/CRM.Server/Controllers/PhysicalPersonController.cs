using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using AutoMapper;
using Zircon.Core.Authorization;
using CRM.Data.Database;
using CRM.Operation.Models;
using CRM.Server.Attributes;

namespace CRM.Server.Controllers
{
    public class PhysicalPersonController : ODataController
    {
        CRMDbContext _context;
        IMapper _mapper;

        public PhysicalPersonController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }

        [EnableQuery]
        [ODataRoute("physicalperson")]
        [WebApiAuthenticate]
        public IEnumerable<ContactListItemModel> Get()
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

            var result = _mapper.ProjectTo<ContactListItemModel>(
                   from
                   c in _context.Clients
                   join
                    p in _context.PhysicalPeople
                    on c.Inn equals p.Inn
                   join
                   u in _context.UserClientComps.Select(x => new { Inn = x.Inn, ULogonName = x.ULogonName}).Distinct()
                   on p.Inn equals u.Inn
                   where (p.ValidFrom <= DateTime.Now && p.ValidTo >= DateTime.Now)
                   && (c.ValidFrom <= DateTime.Now && c.ValidTo >= DateTime.Now)
                   && ((u.ULogonName == CommonUserContextManager.CurrentUser.UserName || u.ULogonName == condition))
                   && c.IsHidden == false
                   select p);

            //var result1 = result
            //      .GroupBy(x => x.INN)
            //      .Select(g => g.First())
            //      .ToList();

            return result;
        }
        
        [EnableQuery]
        [ODataRoute("physicalperson({inn})")]
        [WebApiAuthenticate]
        public ContactListItemModel GetPhysicalPerson(int inn)
        {
             return _mapper.ProjectTo<ContactListItemModel>(
                from p in _context.PhysicalPeople
                where (p.ValidFrom <= DateTime.Now && p.ValidTo >= DateTime.Now)
                && p.Inn == inn
                select p
                ).FirstOrDefault();
        }

        [EnableQuery]
        [ODataRoute("birthdaypersons")]
        [WebApiAuthenticate]
        public IEnumerable<ContactListItemModel> GetBirthdayPersons()
        {
            string condition = string.Empty;

            var perm = CommonUserContextManager.CurrentUser.Permissions.FirstOrDefault(x => x.Name == "CRMRole");

            if (bool.Parse(perm.Parameters.FirstOrDefault(x => x.Key == "Full").Value))
            {
                condition = "";
            }
          
            return _mapper.ProjectTo<ContactListItemModel>(
                _context.BirthdayClientsView
                    .Where(c => 
                        (c.ULogonName == CommonUserContextManager
                        .CurrentUser.UserName || c.ULogonName == condition)
                        && c.DaysLeft > 0 && c.DaysLeft < 10
                        &&(c.ValidFrom <= DateTime.Now && c.ValidTo >= DateTime.Now)));
        }
    }
}