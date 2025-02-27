using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AutoMapper;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Helpers;
using CRM.Operation.Models;
using CRM.Operation.Models.DealOfferModels;
using Microsoft.EntityFrameworkCore;
using Zircon.Core.Extensions;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class LinkPolicyToDealOperation : BusinessOperation<DealPolicyLinkViewModel>
    {
        private readonly CRMDbContext _crmDbContext;
        private readonly IMapper _mapper;
        private Deal _deal;
        private Offer _offer;
        
        public LinkPolicyToDealOperation(DbContext db, IMapper mapper) : base(db)
        {
            _crmDbContext = (CRMDbContext) db;
            _mapper = mapper;
        }

        protected override void ValidateParameters()
        {
            base.ValidateParameters();
            
            _deal = _crmDbContext.Deal.SingleOrDefault(x=>x.DealGuid == Parameters.DealGuid);
            _offer = _deal?.Offer.FirstOrDefault(x => x.Id == Parameters.OfferId);
            
            if (_deal is null)
            {
                Result.AddError($"Deal not found with id : {Parameters.DealGuid}");
                return;
            }
            
            if (_offer is null || _offer.IsAgreed != true)
            {
                Result.AddError("Error occurred on linking policy to deal");
                return;
            }

            if (Parameters.PolicyModel is null)
            {
                Result.AddError("Policy needed");
                return;
            }
            
            //Policy status check
            if (!Parameters.PolicyModel.PolicyStatusLocal.In(PolicyStatusLocal.Issued, PolicyStatusLocal.PreIssued,
                PolicyStatusLocal.InForce))
            {
                Result.AddError("LinkPolicyToDealOperation.PolicyIsNotinIssuedStatus".Translate());
                return;
            }
            
            //** We removed validation for now...**
            
            //Expiration validation check..
            // if (offer.OfferPeriodTypeOid.Equals(1)) //Onetime
            // {
            //     int expiredInDays = Convert.ToInt32(_crmDbContext.Codes
            //         .FirstOrDefault(x => x.TypeOid == (int)CodeTypeEnum.ONE_TIME_OFFER_EXPIRATION_DAY_PERIOD)
            //         ?.Value);
            //
            //     if ((DateTime.UtcNow - Parameters.PolicyModel.StartDate.Date).Days > expiredInDays)
            //     {
            //         Result.AddError("LinkPolicyToDealOperation.PolicyStartDateValidation".Translate());
            //         return;
            //     }
            // }
            // else //Periodic
            // {
            //     if (Parameters.PolicyModel.StartDate.Date < offer.StartDate ||
            //         Parameters.PolicyModel.StartDate.Date > offer.ExpireDate)
            //     {
            //         Result.AddError("LinkPolicyToDealOperation.PolicyStartDateValidation".Translate());
            //         return;
            //     }
            // }
        }

        protected override void DoExecute()
        {

            var dealPolicyLink = _mapper.Map<DealPolicyLink>(Parameters);

            _crmDbContext.DealPolicyLink.Add(dealPolicyLink);

            _deal.DealStatus = (int) DealStatus.Linked;

            _deal.ResponsiblePersonType = (int) DealResponsiblePersonTypeEnum.NotAssigned;
            _deal.CurrentUserGuid = Parameters.CurrentUserGuid;
            _deal.CurrentUserName = Parameters.CurrentUserName;
            _crmDbContext.SaveChanges();
        }

        protected override void DoFinalize()
        {
            base.DoFinalize();

            //Creating shortcut for policy files on IMS side if everything went well..
            if (Result.Success)
            {
                CreateShortcutForFilesOperation shortcutForFilesOperation = new CreateShortcutForFilesOperation();
                if ((OfferTypeEnum)_deal.DealType == OfferTypeEnum.VehicleInsurance)
                {
                    foreach (var item in _deal.VehicleInsurance.Vehicles)
                    {
                        shortcutForFilesOperation.Execute(new CreateShortcutForFilesRequest()
                        {
                            DocumentType = SurveyDocTypesMapping.SurveyDocTypes.ContainsKey((OfferTypeEnum)_deal.DealType!)
                           ? SurveyDocTypesMapping.SurveyDocTypes[(OfferTypeEnum)_deal.DealType!]
                           : new List<string>(),
                            ObjectType = "DealSurvey",
                            ObjectIdentificationNumber = _deal.DealNumber, // Deal number
                            ShortcutFolderName = GenerateShortcutFolderNameForVehicle(item.VehicleRegNumber)
                        });
                    }
                }
                else
                {
                    shortcutForFilesOperation.Execute(new CreateShortcutForFilesRequest()
                        {
                            DocumentType = SurveyDocTypesMapping.SurveyDocTypes.ContainsKey((OfferTypeEnum)_deal.DealType!)
                           ? SurveyDocTypesMapping.SurveyDocTypes[(OfferTypeEnum)_deal.DealType!]
                           : new List<string>(),
                            ObjectType = "DealSurvey",
                            ObjectIdentificationNumber = _deal.DealNumber, // Deal number
                            ShortcutFolderName = GenerateShortcutFolderName()
                        });
                }
            }
        }

        private string GenerateShortcutFolderName()
        {
            var dealOfferType = (OfferTypeEnum)_deal.DealType;

            return Parameters.PolicyModel.Product switch
            {
                LobCode.CascoCombi => 
                Parameters.PolicyNumber,
                
                _ => Parameters.PolicyNumber
            };
        }


        private string GenerateShortcutFolderNameForVehicle(string regNumber)
        {
            return Parameters.PolicyModel.Product switch
            {
                LobCode.CascoCombi =>  $"{Parameters.PolicyNumber}_{regNumber}",

                _ => Parameters.PolicyNumber
            };
        }
    }
}