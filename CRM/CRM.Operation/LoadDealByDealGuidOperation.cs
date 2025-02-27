using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Cache;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.Login;
using CRM.Operation.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class LoadDealByDealGuidOperation : BusinessOperation<Guid>
    {
        private CRMDbContext CrmDbContext => DbContext as CRMDbContext;
        private readonly IMapper _mapper;
        public DealModel DealModel { get; private set; }
        
        public LoadDealByDealGuidOperation(DbContext db, IMapper mapper) : base(db)
        {
            _mapper = mapper;
        }

        protected override void DoExecute()
        {
            var deal = CrmDbContext.Deal.Find(Parameters);

            if (deal is null)
            {
                Result.AddError($"Deal not found with guid : {Parameters}");
                return;
            }
            
            //Initializing new model.
            DealModel = _mapper.Map<DealModel>(deal);
            
            //Client contract.
            var clientContract = _mapper.Map<ClientContract>(deal.ClientInnNavigation);
            DealModel.Client = clientContract;
            
            //Seller user.
            DealModel.CreatedByUser = InMemoryCache.GetUser(deal.CreatedByUserGuid);
            
            //Underwriter user.
            if (deal.UnderwriterUserGuid.HasValue)
            {
                DealModel.UnderwriterUser = InMemoryCache.GetUser(deal.UnderwriterUserGuid.Value);
            }
            
            //Discussion.
            DealModel.DealDiscussionModel = new DealDiscussionModel
            {
                DiscussionHistory = deal.Discussion.OrderByDescending(x=>x.DateTime).Select(x=>
                {
                    var model = _mapper.Map<DiscussionViewModel>(x);

                    model.Sender = InMemoryCache.GetUser(x.SenderGuid);
                    
                    if(x.ReceiverGuid.HasValue)
                        model.Receiver = InMemoryCache.GetUser(x.ReceiverGuid.Value);

                    return model;
                }).ToList()
            };
            
            //Deal policy link
            foreach (var dealModelDealPolicyLink in DealModel.DealPolicyLinks)
            {
                //By User
                dealModelDealPolicyLink.ByUser = InMemoryCache.GetUser(dealModelDealPolicyLink.ByUserGuid);
                
                //Offer number
                dealModelDealPolicyLink.OfferNumber =
                    deal.Offer.FirstOrDefault(x => x.Id == dealModelDealPolicyLink.OfferId)?.OfferNumber;
            }
            DealModel.Tender = _mapper.Map<TenderModel>(CrmDbContext.Tenders.FirstOrDefault(t => t.DealGuid == DealModel.DealGuid)) ?? new TenderModel();
            //Offer forms.
            switch ((OfferTypeEnum)deal.DealType)
            {
                case OfferTypeEnum.VehicleInsurance:
                    DealModel.FormComponentModel = _mapper.Map<VehicleInsuranceOfferModel>(deal.VehicleInsurance);
                    break;
                case OfferTypeEnum.OtherTypes:
                    DealModel.FormComponentModel = _mapper.Map<OtherTypeOfferModel>(deal.OtherTypeOffer);
                    break;
                case OfferTypeEnum.PropertyInsurance: 
                    DealModel.FormComponentModel = _mapper.Map<PropertyInsuranceOfferModel>(deal.PropertyInsurance);
                    break;
                case OfferTypeEnum.PersonalAccidentInsurance:
                    DealModel.FormComponentModel = _mapper.Map<PersonalAccidentOfferModel>(deal.PersonalAccident);
                    break;
                case OfferTypeEnum.LifeInsurance:
                    DealModel.FormComponentModel = _mapper.Map<LifeInsuranceOfferModel>(deal.LifeInsurance);
                    break;
                case OfferTypeEnum.VoluntaryHealthInsurance:
                    DealModel.FormComponentModel = _mapper.Map<VoluntaryHealthInsuranceOfferModel>(deal.VoluntaryHealthInsurance);
                    break;
                case OfferTypeEnum.CarAndEquipmentInsurance:
                    DealModel.FormComponentModel = _mapper.Map<CarAndEquipmentInsuranceOfferModel>(deal.CarAndEquipmentInsurance);
                    break;
                case OfferTypeEnum.LiabilityInsurance:
                    DealModel.FormComponentModel = _mapper.Map<LiabilityInsuranceOfferModel>(deal.LiabilityInsurance);
                    break;
                case OfferTypeEnum.CargoInsurance:
                    DealModel.FormComponentModel = _mapper.Map<CargoInsuranceOfferModel>(deal.CargoInsurance);
                    break;
        
                case OfferTypeEnum.HiringLiabilityInsurance:
                    DealModel.FormComponentModel = _mapper.Map<HiringLiabilityInsuranceOfferModel>(deal.HiringLiability);
                    break;
                case OfferTypeEnum.ProductLiabilityInsurance:
                    DealModel.FormComponentModel = _mapper.Map<ProductLiabilityInsuranceOfferModel>(deal.ProductLiabilityOffer);
                    break;
                case OfferTypeEnum.PropertyLiabilityInsurance:
                    DealModel.FormComponentModel = _mapper.Map<PropertyLiabilityInsuranceOfferModel>(deal.PropertyLiability);
                    break;
                case OfferTypeEnum.BenchInsurance:
                    DealModel.FormComponentModel = _mapper.Map<BenchInsuranceOfferModel>(deal.BenchOffer);
                    break;
                case OfferTypeEnum.ElectronicsInsurance:
                    DealModel.FormComponentModel = _mapper.Map<ElectronicsInsuranceOfferModel>(deal.ElectronicsInsuranceOffer);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}