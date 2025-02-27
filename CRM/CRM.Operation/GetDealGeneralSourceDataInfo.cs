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
    public class GetDealGeneralSourceDataInfo : BusinessOperation<DealOfferDropdownDataModel>
    {
        private CRMDbContext _db;
        public DealOfferDropdownDataModel Data { get; set; }

        public GetDealGeneralSourceDataInfo(CRMDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }
        protected override void DoExecute()
        {
            Data = new DealOfferDropdownDataModel();

            Data.Payment = _db.Codes.Where(x => x.CodeOid == Parameters.PaymentId 
                                            && x.TypeOid == (int)CodeTypeEnum.PAYMENT_TYPE 
                                            && DateTime.Now > x.ValidFrom 
                                            && DateTime.Now < x.ValidTo).FirstOrDefault()?.Value;
           
            
            Data.AreaType = _db.Codes.Where(x => x.CodeOid == Parameters.AreaTypeId 
                                                && x.TypeOid == (int)CodeTypeEnum.INSURANCE_AREA 
                                                && DateTime.Now > x.ValidFrom 
                                                && DateTime.Now < x.ValidTo).FirstOrDefault()?.Value;
           
            
            Data.Currency = _db.Codes.Where(x => x.CodeOid == Parameters.CurrencyId 
                                            && x.TypeOid == (int)CodeTypeEnum.CURRENCY 
                                            && DateTime.Now > x.ValidFrom 
                                            && DateTime.Now < x.ValidTo).FirstOrDefault()?.Value;
            
            Data.Lang = _db.Codes.Where(x => x.CodeOid == Parameters.LangId 
                                                && x.TypeOid == (int)CodeTypeEnum.OFFER_LANGUAGE 
                                                && DateTime.Now > x.ValidFrom 
                                                && DateTime.Now < x.ValidTo).FirstOrDefault()?.Value;
            
            
            Data.PolicyPeriod = _db.Codes.Where(x => x.CodeOid == Parameters.PolicyPeriodId 
                                                && x.TypeOid == (int)CodeTypeEnum.OFFER_POLICY_PERIOD 
                                                && DateTime.Now > x.ValidFrom 
                                                && DateTime.Now < x.ValidTo).FirstOrDefault()?.Value;
            
            Data.InsuranceType = _db.Codes.Where(x => x.CodeOid == Parameters.InsuranceTypeOid 
                                                && x.TypeOid == (int)CodeTypeEnum.INSURANCE_TYPE 
                                                && DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo).FirstOrDefault()?.Value;
           
            Data.DeductibleAmount = _db.Codes.FirstOrDefault(x => x.CodeOid == Convert.ToInt32(Parameters.DeductibleAmountOid)
                                                                                                            && x.TypeOid == (int)CodeTypeEnum.OFFER_DEDUCTIBLE_AMOUNTS
                                                                                                            && DateTime.Now > x.ValidFrom
                                                                                                            && DateTime.Now < x.ValidTo)?.Value;

        }
    }
}
