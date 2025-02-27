using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;

using Zircon.Core.Authorization;
using Zircon.Core.Extensions;

using CRM.Data.Database;
using CRM.Operation.Attributes;
using System.Collections.Generic;

namespace CRM.Operation.Models
{
    [DataContract]
    public class AppointmentModel
    {
        [RequiredLocalized]
        public string TypeId { get; set; }
        //[RequiredLocalized]
        public int id { get; set; }
        [RequiredLocalized]
        public string text { get; set; }

        public string CommentsAfterAppointent { get; set; }
        public string Status { get; set; }
        public string StatusText { get; set; }
        public string Description { get; set; }
        //[RequiredLocalized]
        [DataMember(Name = "start_date")]
        public string start_date { get; set; } //should be declared with underscore
        [JsonProperty("end_date")]
        //[RequiredLocalized]
        public string end_date { get; set; }//should be declared with underscore
        public string Location { get; set; }
        [RequiredLocalized]
        [Attributes.DisplayNameLocalized("AppointmentModel.SelectedClient")]
        public ClientAppointmentModel SelectedClient { get; set; }



        public static explicit operator AppointmentModel(Appointment ev)
        {
            return new AppointmentModel
            {
                id = ev.Id,
                text = ev.Title,
                SelectedClient = new ClientAppointmentModel() { INN = ev.Inn },
                Description = ev.Description,
                Status = ev.Status.ToString(),
                StatusText = Enum.GetName(typeof(AppointmentStatus), ev.Status).Translate(),
                start_date = ev.StartDate.ToString("yyyy-MM-dd HH:mm"),
                end_date = ev.EndDate.ToString("yyyy-MM-dd HH:mm"),
                Location = ev.Location,
                TypeId = ev.TypeId.ToString(),
                CommentsAfterAppointent = ev.CommentsAfterAppointent
            };
        }

        public static explicit operator Appointment(AppointmentModel ev)
        {
            return new Appointment
            {
                Id = ev.id,
                Title = ev.text,
                Description = ev.Description,
                Location = ev.Location,
                Inn = ev.SelectedClient.INN.Value,
                Status = Convert.ToByte( ev.Status),
                ULogonName = CommonUserContextManager.CurrentUser.UserName,
                StartDate = DateTime.Parse(ev.start_date,
                    System.Globalization.CultureInfo.InvariantCulture),
                EndDate = DateTime.Parse(ev.end_date,
                    System.Globalization.CultureInfo.InvariantCulture),
                TypeId =Convert.ToByte(ev.TypeId),
                CommentsAfterAppointent = ev.CommentsAfterAppointent
            };
        }
    }
   
    public enum AppointmentStatus
    {
        Active  = 0,
        Cancelled = 1
    }
}
