using CRM.Operation.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models.MobileModels
{
    public class PushNotificationDto
    {
        public string Token { get; set; }
        public PlatformType Platform { get; set; }
        public string DeviceId { get; set; }
        public string UserId { get; set; }
    }
}
