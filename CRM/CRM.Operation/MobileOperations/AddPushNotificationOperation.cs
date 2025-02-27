using CRM.Data.Database;
using CRM.Operation.Models.MobileModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation.MobileOperations
{
    public class AddPushNotificationOperation : BusinessOperation<PushNotificationDto>
    {
        private CRMDbContext Context => (CRMDbContext)DbContext;
        public AddPushNotificationOperation(DbContext db) : base(db)
        {
        }

        protected override void DoExecute()
        {

            var existDevice = Context.PushNotificationModels.FirstOrDefault(x => x.PlatformType == (int)Parameters.Platform &&
                                                                                 x.DeviceId == Parameters.DeviceId);
            if(existDevice == null)
            {
                PushNotificationModel model = new PushNotificationModel
                {
                    DeviceId = Parameters.DeviceId,
                    Pin = Parameters.UserId.ToUpper(),
                    PlatformType = (int)Parameters.Platform,
                    Token = Parameters.Token,
                };
                Context.PushNotificationModels.Add(model);
            }
            else
            {
                existDevice.Pin = Parameters.UserId.ToUpper();
                existDevice.Token = Parameters.Token;
                Context.PushNotificationModels.Update(existDevice);
            }

            Context.SaveChanges();

        }
    }
}
