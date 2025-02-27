using AutoMapper;
using CRM.Data.Database;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using CRM.Server.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Server.Controllers
{
    public class ClientRelationApiController : BaseController
    {
        private readonly IMapper _mapper;
        public ClientRelationApiController(IMapper mapper, CRMDbContext dbContext)
        {
            _mapper = mapper;
            DbContext = dbContext;
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public ActionResult GetRelationsByVoen(ClientContract client)
        {
            var people = new List<ClientRelationModel>();

            var document = DbContext.Documents.
                FirstOrDefault(x => x.DocumentNumber == client.PinNumber && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);

            if (document == null)
                return OperationResult(new Zircon.Core.OperationModel.OperationResult(), people);

            var relations = DbContext.ClientRelationComps.Where(x => x.Client1 == document.Inn && x.RelationId == 6).ToList();

            relations.ForEach(rel =>
            {
                var pp = DbContext.PhysicalPeople.FirstOrDefault(p => p.Inn == rel.Client2 &&
                p.ValidFrom <= DateTime.Now && p.ValidTo >= DateTime.Now);

                var tagComp = DbContext.TagComps.FirstOrDefault(x => x.Inn == rel.Client2 && x.TagId == 26);

                people.Add(new ClientRelationModel
                {
                    FirstName = pp.FirstName,
                    LastName = pp.LastName,
                    FatherName = pp.FatherName,
                    FullName = pp.FullName,
                    RelationalINN = rel.Client2,
                    PinNumber = pp.Pin,
                    Tags = tagComp == null ? null : new List<TagContract>() { new TagContract { Id = tagComp.TagId, Name = tagComp.Tag.Name } }
                });
            });


            return OperationResult(new Zircon.Core.OperationModel.OperationResult(), people);
        }


        [WebApiAuthenticate]
        [HttpGet("getcurators")]
        public ActionResult GetCurators()
        {

            var curators = from relation in DbContext.ClientRelationComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.RelationId == 6)

                           join physicalPerson in DbContext.PhysicalPeople.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now) on
                           relation.Client2 equals physicalPerson.Inn
                           select new ClientRelationModel
                           {
                               INN = relation.Client1,
                               RelationalINN = relation.Client2,
                               FirstName = physicalPerson.FirstName,
                               LastName = physicalPerson.LastName,
                               FullName = physicalPerson.FullName,
                               FatherName = physicalPerson.FatherName
                           };

            return OperationResult(new Zircon.Core.OperationModel.OperationResult(), curators.ToList());
        }


        [WebApiAuthenticate]
        [HttpGet("getcurator")]
        public ActionResult GetCurator(string pinNumber)
        {

            var curators = from relation in DbContext.ClientRelationComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.RelationId == 6)

                           join physicalPerson in DbContext.PhysicalPeople.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.Pin == pinNumber) on
                           relation.Client2 equals physicalPerson.Inn
                           select new ClientRelationModel
                           {
                               INN = relation.Client1,
                               RelationalINN = relation.Client2,
                               FullName = physicalPerson.FullName,
                               FirstName = physicalPerson.FirstName,
                               LastName = physicalPerson.LastName,
                               FatherName = physicalPerson.FatherName,
                               PinNumber = physicalPerson.Pin
                           };

            return OperationResult(new Zircon.Core.OperationModel.OperationResult(), curators.FirstOrDefault());
        }
    }
}
