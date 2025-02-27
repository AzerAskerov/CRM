using System;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;

using CRM.Data.Database;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Models;
using CRM.Server.Attributes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CRM.Operation.Enums;

namespace CRM.Server.Controllers
{
    public class ClientContractController : ODataController
    {
        CRMDbContext _context;
        IMapper _mapper;
        public ClientContractController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        [WebApiAuthenticate]
        [ODataRoute("GetClientContractByContactInfo(contactsinfoValue={contactsinfoValue})")]
        public IEnumerable<ClientContract> GetClientContractByContactInfo([FromODataUri] string contactsinfoValue)
        {
            if (string.IsNullOrEmpty(contactsinfoValue))
                return new List<ClientContract>();

            var contactsinfo = _context.ContactInfos.FirstOrDefault(x => x.Value.Equals(contactsinfoValue));
            if (contactsinfo == null)
                return new List<ClientContract>();

            var clientContactInfoComps = _context.ClientContactInfoComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.ContactInfoId.Equals(contactsinfo.Id))?.Select(c => c.Inn);
            var clientContracts = new List<ClientContract>();

            foreach (var inn in clientContactInfoComps)
            {
                #region physical/legal
                var physicalPersons = _context.PhysicalPeople.FirstOrDefault(x => x.Inn == inn && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);
                var legalPersons = _context.Companies.FirstOrDefault(x => x.Inn == inn && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);
                var docs = _context.Documents.FirstOrDefault(x => x.Inn == inn && x.DocumentType == (int)DocumentTypeEnum.Tin && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);
                #endregion

                #region tags
                var tagConstracts = new List<TagContract>();
                var tagCompositions = _context.TagComps.Where(x => x.Inn == inn && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now)?.Select(c => c.TagId);
                var tags = _context.Tags.Where(x => tagCompositions.Any(z => z == x.Id));
                tags.ToList().ForEach(x => tagConstracts.Add(new TagContract { Id = x.Id, Name = x.Name }));
                #endregion

                clientContracts.Add(new ClientContract
                {
                    INN = inn,
                    CompanyName = legalPersons?.CompanyName,
                    FirstName = physicalPersons?.FirstName,
                    LastName = physicalPersons?.LastName,
                    FatherName = physicalPersons?.FatherName,
                    FullName = physicalPersons?.FullName,
                    PinNumber = physicalPersons?.Pin,
                    TinNumber = docs?.DocumentNumber,
                    Tags = tagConstracts
                });
            }

            return clientContracts;
        }

        [EnableQuery]
        [ODataRoute("clientcontract({inn})")]
        [WebApiAuthenticate]
        public IEnumerable<ClientContract> GetContract(int inn)
        {
            var df = _mapper.ProjectTo<ClientContract>(
                from cr in _context.ClientRefs
                where cr.Inn == inn
                select cr
                ).ToList();
            return df;
        }

        [HttpGet]
        [EnableQuery]
        [WebApiAuthenticate]
        [ODataRoute("GetClientContractByDocNumber(DocNumber={DocNumber})")]
        public IEnumerable<ClientContract> GetContractByDocNumber([FromODataUri] string DocNumber)
        {
            var document = _context.Documents.FirstOrDefault(x => x.DocumentNumber.Equals(DocNumber) &&
                                                                  x.ValidFrom <= DateTime.Now &&
                                                                  x.ValidTo >= DateTime.Now);
            if (document is null)
                return new List<ClientContract>();

            return _mapper.ProjectTo<ClientContract>(
               from cr in _context.ClientRefs
               where cr.Inn == document.Inn

               select cr
               ).ToList();
        }

        [HttpGet]
        [EnableQuery]
        [WebApiAuthenticate]
        [ODataRoute("GetCompanyContractByDocNumber(DocNumber={DocNumber})")]
        public IEnumerable<ClientContract> GetCompanyContractByDocNumber([FromODataUri] string DocNumber)
        {
            var document = _context.Documents.FirstOrDefault(x => x.DocumentNumber.Equals(DocNumber) &&
                                                                  x.DocumentType == (int)DocumentTypeEnum.Tin &&
                                                                  x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);
            if (document is null)
                return new List<ClientContract>();

            return _mapper.ProjectTo<ClientContract>(
               from cr in _context.ClientRefs
               where cr.Inn == document.Inn
               select cr
               ).ToList();
        }
    }
}
