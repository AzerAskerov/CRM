using Castle.Core.Internal;
using CRM.Data.Database;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class GetMobileClientOperation : BusinessOperation<GetMobileClientsSearchModel>
    {
        CRMDbContext _context;
        public List<MobileClientModel> Model;

        public GetMobileClientOperation(CRMDbContext context) : base(context)
        {
            _context = context;
        }

        protected override void DoExecute()
        {
            Model = new List<MobileClientModel>();

            IQueryable<PhysicalPerson> userQuery = _context.PhysicalPeople.AsQueryable();
            IQueryable<PushNotificationModel> notificationQuery = _context.PushNotificationModels.AsQueryable();

            if (Parameters.Pins?.Count > 0)
            {
                var upperCasePins = Parameters.Pins?.Select(x => x.ToUpper()).ToList();
                userQuery = userQuery.Where(x => x.Pin != null && upperCasePins.Any(y => y == x.Pin.ToUpper())).AsQueryable();
            }
            else if (!Parameters.Pin.IsNullOrEmpty())
            {
                userQuery = userQuery.Where(x => Parameters.Pin.ToUpper() == x.Pin.ToUpper()).AsQueryable();
            }
            if (!Parameters.Platform.IsNullOrEmpty())
            {
                notificationQuery = Parameters.Platform.ToLower() switch
                {
                    "android" => notificationQuery.Where(x => x.PlatformType == 1).AsQueryable(),
                    "ios" => notificationQuery.Where(x => x.PlatformType == 2).AsQueryable(),
                    _ => notificationQuery.AsQueryable(),
                };
            }

            var resultQuery = userQuery.Join(notificationQuery, pp => pp.Pin, pn => pn.Pin, (pp, pn) => new
            {
                Pin = pp.Pin,
                FullName = pp.FullName,
                INN = pp.Inn,
                Token = pn.Token,
                Platform = pn.PlatformType == 1 ? "Android" : pn.PlatformType == 2 ? "iOS" : "All",
            }).AsQueryable();

            var dbResult = resultQuery.ToList();

            Model = dbResult.GroupBy(x => x.INN).Select(group => new MobileClientModel
            {
                IIN = group.Key,
                Pin = group.FirstOrDefault().Pin,
                FullName = group.FirstOrDefault().FullName,
                Tokens = group.Select(result => new MobileClientTokenModel
                {
                    Token = result.Token,
                    Platform = result.Platform,
                    Value = $"{result.Token}/{result.Platform}"
                }).ToList()
            }).ToList();
        }
    }
}
