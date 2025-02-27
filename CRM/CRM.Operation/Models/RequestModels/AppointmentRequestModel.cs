using CRM.Data.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Zircon.Core.Authorization;
using Zircon.Core.Extensions;

namespace CRM.Operation.Models.RequestModels
{
    [DataContract]
    public class AppointmentRequestModel
    {
        public int id { get; set; }
        public string text { get; set; }
        public string Status { get; set; }
        public string StatusText { get; set; }
        public string Description { get; set; }
        public string CommentsAfterAppointent { get; set; }
        //[RequiredLocalized]
        [DataMember(Name = "start_date")]
        public string start_date { get; set; } //should be declared with underscore
        [JsonProperty("end_date")]
        //[RequiredLocalized]
        public string end_date { get; set; }//should be declared with underscore
        public string Location { get; set; }
        public byte? TypeId { get; set; }
        public int? Inn { get; set; }
        public static explicit operator AppointmentRequestModel(Appointment ev)
        {
            return new AppointmentRequestModel
            {
                id = ev.Id,
                text = ev.Title,
                Inn = ev.Inn,
                Description = ev.Description,
                Status = ev.Status.ToString(),
                StatusText = Enum.GetName(typeof(AppointmentStatus), ev.Status).Translate(),
                start_date = ev.StartDate.ToString("yyyy-MM-dd HH:mm"),
                end_date = ev.EndDate.ToString("yyyy-MM-dd HH:mm"),
                Location = ev.Location,
                TypeId = ev.TypeId,
                CommentsAfterAppointent = ev.CommentsAfterAppointent

            };
        }
        public static explicit operator Appointment(AppointmentRequestModel ev)
        {
            return new Appointment
            {
                Id = ev.id,
                Title = ev.text,
                Description = ev.Description,
                Location = ev.Location,
                Inn = Convert.ToInt32(ev.Inn),
                Status = Convert.ToByte(ev.Status),
                ULogonName = CommonUserContextManager.CurrentUser.UserName,
                StartDate = DateTime.Parse(ev.start_date,
                    System.Globalization.CultureInfo.InvariantCulture),
                EndDate = DateTime.Parse(ev.end_date,
                    System.Globalization.CultureInfo.InvariantCulture),
                TypeId = ev.TypeId,
                CommentsAfterAppointent = ev.CommentsAfterAppointent
            };
        }
    }

    public enum AppointmentStatus
    {
        Active = 0,
        Cancelled = 1
    }
}
