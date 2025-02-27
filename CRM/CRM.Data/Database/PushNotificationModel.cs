using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Data.Database
{
    public class PushNotificationModel
    {
        public int Id { get; set; }
        public string Pin { get; set; }
        public int PlatformType { get; set; }
        public string DeviceId { get; set; }
        public string Token { get; set; }
    }
}
