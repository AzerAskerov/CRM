using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Zircon.Core.Authorization;
using AutoMapper;
using CRM.Data.Database;
using CRM.Operation.Models;
using CRM.Server.Attributes;
using CRM.Data.Enums;
using CRM.Operation.Models.RequestModels;

namespace CRM.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : BaseController
    {
        private readonly CRMDbContext _context;
        IMapper _mapper;


        public EventsController(CRMDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET api/events
        [HttpPost("types")]
        [WebApiAuthenticate]
        public IEnumerable<CodeViewModel> GetTypes()
        {
            List<Code> appointmentTypes = _context.Codes.Where(x => x.TypeOid ==
            (int)CodeTypeEnum.APPOINTMENT_TYPES && DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo).ToList();

            var codes =_mapper.Map<List<CodeViewModel>>(appointmentTypes).ToList();

            return codes;
        }
        
        // GET api/events
        [HttpGet]
        [WebApiAuthenticate]
        public IEnumerable<AppointmentModel> Get()
        {
            var d = _context.Appointments
               .Where(x=>x.ULogonName == CommonUserContextManager.CurrentUser.UserName)
               .ToList()
               .Select(e => (AppointmentModel)e);
            
            return d;
        }

        // GET api/events/5
        [HttpGet("{id}")]
        [WebApiAuthenticate]
        public AppointmentModel Get(int id)
        {
            var d = (AppointmentModel)_context
                .Appointments
                .Find(id);
            return d;
        }


        // POST api/events
        [HttpPost]
        [WebApiAuthenticate]
        public ObjectResult Post([FromForm] AppointmentRequestModel apiEvent)
        {
            var newEvent = (Appointment)apiEvent;
            _context.Appointments.Add(newEvent);
            _context.SaveChanges();

            return Ok(new
            {
                tid = newEvent.Id,
                action = "inserted"
            });
        }


        // PUT api/events/5
        [HttpPut("{id}")]
        [WebApiAuthenticate]
        public ObjectResult Put(int id, [FromForm] AppointmentRequestModel appModel)
        {
            var updatedEvent = (Appointment)appModel;
            var dbEvent = _context.Appointments.Find(id);
            dbEvent.Title = updatedEvent.Title;
            dbEvent.StartDate = updatedEvent.StartDate;
            dbEvent.EndDate = updatedEvent.EndDate;
            dbEvent.Inn = updatedEvent.Inn;
            dbEvent.Description = updatedEvent.Description;
            dbEvent.Location = updatedEvent.Location;
            dbEvent.Status = updatedEvent.Status;
            dbEvent.TypeId = updatedEvent.TypeId;
            dbEvent.CommentsAfterAppointent = updatedEvent.CommentsAfterAppointent;
            _context.SaveChanges();

            return Ok(new
            {
                action = "updated"
            });
        }


        // DELETE api/events/5
        [HttpDelete("{id}")]
        [WebApiAuthenticate]
        public ObjectResult DeleteEvent(int id)
        {
            var e = _context.Appointments.Find(id);
            if (e != null)
            {
                _context.Appointments.Remove(e);
                _context.SaveChanges();
            }

            return Ok(new
            {
                action = "deleted"
            });
        }

    }
}
