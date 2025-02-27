using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;

using AutoMapper;

using CRM.Data.Database;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Enums;
using CRM.Server.Attributes;

namespace CRM.Server.Controllers
{
    public class AddressController : ODataController
    {
        CRMDbContext _context;
        IMapper _mapper;
        public AddressController(CRMDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }

        [EnableQuery]
        [ODataRoute("address")]
        [WebApiAuthenticate]
        public IEnumerable<AddressContract> Get()
        {
            return _mapper.ProjectTo<AddressContract>(_context.Addresses.
                Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
        }


        [EnableQuery]
        [ODataRoute("address({inn})")]
        [WebApiAuthenticate]
        public AddressContract GetAddress(int inn)
        {
            var result = _mapper.ProjectTo<AddressContract>(_context.Addresses.
                Where(x => x.Inn == inn && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now)).OrderByDescending(a => a.Id).FirstOrDefault();

            if (result != null)
            {
                if (result.CountryId != (int)CountryCodeContainer.DefaultCountryCode1 && 
                    result.CountryId != (int)CountryCodeContainer.DefaultCountryCode2)
                {
                    result.RegOrCityId = 0;

                    //result.DistrictOrStreet = string.Empty;
                    //result.AdditionalInfo = string.Empty;
                }
            }

            return result;
        }
    }
}
