using AutoMapper;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Data.NumberProvider;
using CRM.Operation.Models.DealOfferModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class SaveDealAsDraftOperation : BusinessOperation<DealModel>
    {
        private readonly CRMDbContext _dbContext;
        private readonly IMapper _mapper;
        
        public SaveDealAsDraftOperation(CRMDbContext db, IMapper mapper) : base(db)
        {
            _dbContext = db;
            _mapper = mapper;
        }

        protected override void Prepare()
        {
            AssignDealNumber();
        }

        protected override void DoExecute()
        {
            var deal = _dbContext.Deal.Find(Parameters.DealGuid);
            
            if (deal is null) RegisterNewDeal();
            
            else UpdateExistingDeal(deal);
        }

        private void RegisterNewDeal()
        {
            Parameters.DealStatus = DealStatus.Draft;
            Parameters.ResponsiblePersonType = DealResponsiblePersonTypeEnum.Seller;
            var deal = _mapper.Map<Deal>(Parameters);

            switch ((OfferTypeEnum)deal.DealType)
            {
                case OfferTypeEnum.VehicleInsurance:
                    deal.VehicleInsurance = _mapper.Map<VehicleInsurance>(Parameters.FormComponentModel);
                    deal.VehicleInsurance.DealGuid = deal.DealGuid;
                    break;
                case OfferTypeEnum.OtherTypes:
                    deal.OtherTypeOffer = _mapper.Map<OtherTypeOffer>(Parameters.FormComponentModel);
                    deal.OtherTypeOffer.DealGuid = deal.DealGuid;
                    break;
                case OfferTypeEnum.PropertyInsurance:
                    deal.PropertyInsurance = _mapper.Map<PropertyInsurance>(Parameters.FormComponentModel);
                    deal.PropertyInsurance.DealGuid = deal.DealGuid;
                    break;
                case OfferTypeEnum.PersonalAccidentInsurance:
                    deal.PersonalAccident = _mapper.Map<PersonalAccident>(Parameters.FormComponentModel);
                    deal.PersonalAccident.DealGuid = deal.DealGuid;
                    break;
                case OfferTypeEnum.LifeInsurance:
                    deal.LifeInsurance = _mapper.Map<LifeInsurance>(Parameters.FormComponentModel);
                    deal.LifeInsurance.DealGuid = deal.DealGuid;
                    break;
                case OfferTypeEnum.VoluntaryHealthInsurance:
                    deal.VoluntaryHealthInsurance = _mapper.Map<VoluntaryHealthInsurance>(Parameters.FormComponentModel);
                    deal.VoluntaryHealthInsurance.DealGuid = deal.DealGuid;
                    break;
                case OfferTypeEnum.CarAndEquipmentInsurance:
                    deal.CarAndEquipmentInsurance = _mapper.Map<CarAndEquipmentInsurance>(Parameters.FormComponentModel);
                    deal.CarAndEquipmentInsurance.DealGuid = deal.DealGuid;
                    break;
                case OfferTypeEnum.LiabilityInsurance:
                    deal.LiabilityInsurance = _mapper.Map<LiabilityInsurance>(Parameters.FormComponentModel);
                    deal.LiabilityInsurance.DealGuid = deal.DealGuid;
                    break;
                case OfferTypeEnum.CargoInsurance:
                   
                    deal.CargoInsurance = _mapper.Map<CargoInsurance>(Parameters.FormComponentModel);
                    deal.CargoInsurance.StartPoint = deal.CargoInsurance.StartPointCountryId + "/" + deal.CargoInsurance.StartPointCityId;
                    deal.CargoInsurance.DestinationPoint = deal.CargoInsurance.EndPointCountryId + "/" + deal.CargoInsurance.EndPointCityId;
                   // deal.CargoInsurance.Route = deal.CargoInsurance.RouteCountryId + "/" + deal.CargoInsurance.RouteCityId;
                    deal.CargoInsurance.DealGuid = deal.DealGuid;
                    break;
                case OfferTypeEnum.HiringLiabilityInsurance:
                    deal.HiringLiability = _mapper.Map<HiringLiability>(Parameters.FormComponentModel);
                    deal.HiringLiability.DealGuid = deal.DealGuid;
                    break;
                case OfferTypeEnum.ProductLiabilityInsurance:
                    deal.ProductLiabilityOffer = _mapper.Map<ProductLiabilityOffer>(Parameters.FormComponentModel);
                    deal.ProductLiabilityOffer.DealGuid = deal.DealGuid;
                    break;
                case OfferTypeEnum.PropertyLiabilityInsurance:
                    deal.PropertyLiability = _mapper.Map<PropertyLiability>(Parameters.FormComponentModel);
                    deal.PropertyLiability.DealGuid = deal.DealGuid;
                    break;
                case OfferTypeEnum.ElectronicsInsurance:
                    deal.ElectronicsInsuranceOffer = _mapper.Map<ElectronicsInsuranceOffer>(Parameters.FormComponentModel);
                    deal.ElectronicsInsuranceOffer.DealGuid = deal.DealGuid;
                    break;
                case OfferTypeEnum.BenchInsurance:
                    deal.BenchOffer = _mapper.Map<BenchOffer>(Parameters.FormComponentModel);
                    deal.BenchOffer.DealGuid = deal.DealGuid;
                    break;
            }

            _dbContext.Deal.Add(deal);
            _dbContext.SaveChanges();
        }

        private void UpdateExistingDeal(Deal deal)
        {
            Parameters.DealStatus = DealStatus.Draft;
            Parameters.ResponsiblePersonType =DealResponsiblePersonTypeEnum.Seller;
            _mapper.Map(Parameters,deal);

            switch ((OfferTypeEnum)deal.DealType)
            {
                case OfferTypeEnum.VehicleInsurance:
                    deal.VehicleInsurance = _mapper.Map(Parameters.FormComponentModel,deal.VehicleInsurance);
                    break;
                case OfferTypeEnum.OtherTypes:
                    deal.OtherTypeOffer = _mapper.Map(Parameters.FormComponentModel,deal.OtherTypeOffer);
                    break;
                case OfferTypeEnum.PropertyInsurance:
                    deal.PropertyInsurance = _mapper.Map(Parameters.FormComponentModel,deal.PropertyInsurance);
                    break;
                case OfferTypeEnum.PersonalAccidentInsurance:
                    deal.PersonalAccident = _mapper.Map(Parameters.FormComponentModel,deal.PersonalAccident);
                    break;
                case OfferTypeEnum.LifeInsurance:
                    deal.LifeInsurance = _mapper.Map(Parameters.FormComponentModel,deal.LifeInsurance);
                    break;
                case OfferTypeEnum.VoluntaryHealthInsurance:
                    deal.VoluntaryHealthInsurance = _mapper.Map(Parameters.FormComponentModel,deal.VoluntaryHealthInsurance);
                    break;
                case OfferTypeEnum.CarAndEquipmentInsurance:
                    deal.CarAndEquipmentInsurance = _mapper.Map(Parameters.FormComponentModel,deal.CarAndEquipmentInsurance);
                    break;
                case OfferTypeEnum.LiabilityInsurance:
                    deal.LiabilityInsurance = _mapper.Map(Parameters.FormComponentModel,deal.LiabilityInsurance);
                    break;
                case OfferTypeEnum.CargoInsurance:
                    deal.CargoInsurance = _mapper.Map(Parameters.FormComponentModel,deal.CargoInsurance);
                    deal.CargoInsurance.StartPoint = deal.CargoInsurance.StartPointCountryId + "/" + deal.CargoInsurance.StartPointCityId;
                    deal.CargoInsurance.DestinationPoint = deal.CargoInsurance.EndPointCountryId + "/" + deal.CargoInsurance.EndPointCityId;
                   // deal.CargoInsurance.Route = deal.CargoInsurance.RouteCountryId + "/" + deal.CargoInsurance.RouteCityId;
                    break;
                case OfferTypeEnum.HiringLiabilityInsurance:
                    deal.HiringLiability = _mapper.Map(Parameters.FormComponentModel, deal.HiringLiability);
                    break;
                case OfferTypeEnum.ProductLiabilityInsurance:
                    deal.ProductLiabilityOffer = _mapper.Map(Parameters.FormComponentModel, deal.ProductLiabilityOffer);
                    break;
                case OfferTypeEnum.PropertyLiabilityInsurance:
                    deal.PropertyLiability = _mapper.Map(Parameters.FormComponentModel, deal.PropertyLiability);
                    break;
                case OfferTypeEnum.ElectronicsInsurance:
                    deal.ElectronicsInsuranceOffer = _mapper.Map(Parameters.FormComponentModel, deal.ElectronicsInsuranceOffer);
                    break;
                case OfferTypeEnum.BenchInsurance:
                    deal.BenchOffer = _mapper.Map(Parameters.FormComponentModel, deal.BenchOffer);
                    break;
            }

            _dbContext.SaveChanges();
        }
        
        private void AssignDealNumber()
        {
            if (!string.IsNullOrEmpty(Parameters.DealNumber))
                return;
            
            var numberProvider = new DealNumberProvider(_dbContext);

            Parameters.DealNumber = numberProvider.GetUniqueId();
        }
    }
}