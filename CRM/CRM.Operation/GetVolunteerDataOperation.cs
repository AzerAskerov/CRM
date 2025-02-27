using AutoMapper;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Models.DealOfferModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class GetVolunteerDataOperation : BusinessOperation<DealOfferDropdownDataModel>
    {
        private CRMDbContext _db;
        public IMapper _mapper;
        public DealOfferDropdownDataModel Data { get; set; }

        public GetVolunteerDataOperation(CRMDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _db = dbContext;
            _mapper = mapper;
        }
        protected override void DoExecute()
        {
            Data = new DealOfferDropdownDataModel();
            Data.AgeRange = _db.Codes?.FirstOrDefault(x => x.CodeOid == Convert.ToInt32(Parameters.AgeRangeOid) &&
                                                                                               x.TypeOid == (int)CodeTypeEnum.VOLUNTARY_HEALTH_EMPLOYEE_AGE_RANGES &&
                                                                                               DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo)?.Value;


            Data.EmployeeTypeName = _db.Codes?.FirstOrDefault(x => x.CodeOid == Convert.ToInt32(Parameters.EmployeeType) &&
                                                                                             x.TypeOid == (int)CodeTypeEnum.VOLUNTARY_HEALTH_EMPLOYEE_TYPE &&
                                                                                             DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo)?.Value;



            Data.GenderName = _db.Codes?.FirstOrDefault(x => x.CodeOid == Convert.ToInt32(Parameters.Gender) &&
                                                                                             x.TypeOid == (int)CodeTypeEnum.SEX_CODE &&
                                                                                             DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo)?.Value;

        }
    }
}
