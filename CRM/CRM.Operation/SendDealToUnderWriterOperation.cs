using System.Linq;
using System.Text;
using AutoMapper;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Data.NumberProvider;
using CRM.Operation.Cache;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.RequestModels;
using Zircon.Core.Config;
using Zircon.Core.Extensions;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class SendDealToUnderWriterOperation : BusinessOperation<DealModel>, ISendEmailOperation
    {
        private readonly CRMDbContext _dbContext;
        private readonly IMapper _mapper;
        private Deal _deal;
        private bool isNewDeal;

        public SendDealToUnderWriterOperation(CRMDbContext db, IMapper mapper) : base(db)
        {
            _dbContext = db;
            _mapper = mapper;
        }

        protected override void Prepare()
        {
            _deal = _dbContext.Deal.Find(Parameters.DealGuid);

            AssignDealNumber();
        }

        protected override void DoExecute()
        {
            if (_deal is null)
            {
                isNewDeal = true;
                RegisterNewDeal();
            }
            
            else UpdateExistingDeal(_deal);


            Save();
        }
        private void RegisterNewDeal()
        {
            Parameters.DealStatus = DealStatus.PendingUnderwriting;
            Parameters.ResponsiblePersonType = DealResponsiblePersonTypeEnum.Underwriter;
            _deal = _mapper.Map<Deal>(Parameters);

            switch ((OfferTypeEnum)_deal.DealType)
            {
                case OfferTypeEnum.VehicleInsurance:
                    _deal.VehicleInsurance = _mapper.Map<VehicleInsurance>(Parameters.FormComponentModel);
                    _deal.VehicleInsurance.DealGuid = _deal.DealGuid;
                    break;
                case OfferTypeEnum.OtherTypes:
                    _deal.OtherTypeOffer = _mapper.Map<OtherTypeOffer>(Parameters.FormComponentModel);
                    _deal.OtherTypeOffer.DealGuid = _deal.DealGuid;
                    break;
                case OfferTypeEnum.PropertyInsurance:
                    _deal.PropertyInsurance = _mapper.Map<PropertyInsurance>(Parameters.FormComponentModel);
                    _deal.PropertyInsurance.DealGuid = _deal.DealGuid;
                    break;
                case OfferTypeEnum.PersonalAccidentInsurance:
                    _deal.PersonalAccident = _mapper.Map<PersonalAccident>(Parameters.FormComponentModel);
                    _deal.PersonalAccident.DealGuid = _deal.DealGuid;
                    break;
                case OfferTypeEnum.LifeInsurance:
                    _deal.LifeInsurance = _mapper.Map<LifeInsurance>(Parameters.FormComponentModel);
                    _deal.LifeInsurance.DealGuid = _deal.DealGuid;
                    break;
                case OfferTypeEnum.VoluntaryHealthInsurance:
                    _deal.VoluntaryHealthInsurance = _mapper.Map<VoluntaryHealthInsurance>(Parameters.FormComponentModel);
                    _deal.VoluntaryHealthInsurance.DealGuid = _deal.DealGuid;
                    break;
                case OfferTypeEnum.CarAndEquipmentInsurance:
                    _deal.CarAndEquipmentInsurance = _mapper.Map<CarAndEquipmentInsurance>(Parameters.FormComponentModel);
                    _deal.CarAndEquipmentInsurance.DealGuid = _deal.DealGuid;
                    break;
                case OfferTypeEnum.LiabilityInsurance:
                    _deal.LiabilityInsurance = _mapper.Map<LiabilityInsurance>(Parameters.FormComponentModel);
                    _deal.LiabilityInsurance.DealGuid = _deal.DealGuid;
                    break;
                case OfferTypeEnum.CargoInsurance:
                    _deal.CargoInsurance = _mapper.Map<CargoInsurance>(Parameters.FormComponentModel);
                    _deal.CargoInsurance.StartPoint = _deal.CargoInsurance.StartPointCountryId + "/" + _deal.CargoInsurance.StartPointCityId;
                    _deal.CargoInsurance.DestinationPoint = _deal.CargoInsurance.EndPointCountryId + "/" + _deal.CargoInsurance.EndPointCityId;
                    _deal.CargoInsurance.DealGuid = _deal.DealGuid;
                    break;
                case OfferTypeEnum.HiringLiabilityInsurance:
                    _deal.HiringLiability = _mapper.Map<HiringLiability>(Parameters.FormComponentModel);
                    _deal.HiringLiability.DealGuid = _deal.DealGuid;
                    break;
                case OfferTypeEnum.ProductLiabilityInsurance:
                    _deal.ProductLiabilityOffer = _mapper.Map<ProductLiabilityOffer>(Parameters.FormComponentModel);
                    _deal.ProductLiabilityOffer.DealGuid = _deal.DealGuid;
                    break;
                case OfferTypeEnum.PropertyLiabilityInsurance:
                    _deal.PropertyLiability = _mapper.Map<PropertyLiability>(Parameters.FormComponentModel);
                    _deal.PropertyLiability.DealGuid = _deal.DealGuid;
                    break;
                case OfferTypeEnum.ElectronicsInsurance:
                    _deal.ElectronicsInsuranceOffer = _mapper.Map<ElectronicsInsuranceOffer>(Parameters.FormComponentModel);
                    _deal.ElectronicsInsuranceOffer.DealGuid = _deal.DealGuid;
                    break;
                case OfferTypeEnum.BenchInsurance:
                    _deal.BenchOffer = _mapper.Map<BenchOffer>(Parameters.FormComponentModel);
                    _deal.BenchOffer.DealGuid = _deal.DealGuid;
                    break;
            }

            _dbContext.Deal.Add(_deal);
        }

        private void UpdateExistingDeal(Deal deal)
        {
            Parameters.DealStatus = DealStatus.PendingUnderwriting;
            Parameters.ResponsiblePersonType = DealResponsiblePersonTypeEnum.Underwriter;
            _mapper.Map(Parameters, deal);

            switch ((OfferTypeEnum)deal.DealType)
            {
                case OfferTypeEnum.VehicleInsurance:
                    deal.VehicleInsurance = _mapper.Map(Parameters.FormComponentModel, deal.VehicleInsurance);
                    break;
                case OfferTypeEnum.OtherTypes:
                    deal.OtherTypeOffer = _mapper.Map(Parameters.FormComponentModel, deal.OtherTypeOffer);
                    break;
                case OfferTypeEnum.PropertyInsurance:
                    deal.PropertyInsurance = _mapper.Map(Parameters.FormComponentModel, deal.PropertyInsurance);
                    break;
                case OfferTypeEnum.PersonalAccidentInsurance:
                    deal.PersonalAccident = _mapper.Map(Parameters.FormComponentModel, deal.PersonalAccident);
                    break;
                case OfferTypeEnum.LifeInsurance:
                    deal.LifeInsurance = _mapper.Map(Parameters.FormComponentModel, deal.LifeInsurance);
                    break;
                case OfferTypeEnum.VoluntaryHealthInsurance:
                    deal.VoluntaryHealthInsurance = _mapper.Map(Parameters.FormComponentModel, deal.VoluntaryHealthInsurance);
                    break;
                case OfferTypeEnum.CarAndEquipmentInsurance:
                    deal.CarAndEquipmentInsurance = _mapper.Map(Parameters.FormComponentModel, deal.CarAndEquipmentInsurance);
                    break;
                case OfferTypeEnum.LiabilityInsurance:
                    deal.LiabilityInsurance = _mapper.Map(Parameters.FormComponentModel, deal.LiabilityInsurance);
                    break;
                case OfferTypeEnum.CargoInsurance:
                    deal.CargoInsurance = _mapper.Map(Parameters.FormComponentModel, deal.CargoInsurance);
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
        }

        private void AssignDealNumber()
        {
            if (!string.IsNullOrEmpty(Parameters.DealNumber))
                return;
            
            var numberProvider = new DealNumberProvider(_dbContext);

            Parameters.DealNumber = numberProvider.GetUniqueId();
        }

        private void SendDealUpdatedEmailToUnderwriting()
        {
            var generateDealChangesSummaryStringOperation = new GenerateDealChangesSummaryStringOperation(_dbContext);
            generateDealChangesSummaryStringOperation.Execute();
            var underwriterEmail = _deal.UnderwriterUserGuid.HasValue
                ? InMemoryCache.GetUser(_deal.UnderwriterUserGuid.Value).UserEmail
                : Settings.GetString("UNDERWRITER_GROUP_MAIL");
            
            var emailModel = new EmailModel
            {
                Body = string.Format("SendDealUpdatedEmailToUnderwriting.Body".Translate(), _deal.CreatedByUserFullName,
                    _deal.UnderwriterUserFullName, generateDealChangesSummaryStringOperation.SummaryString),
                From = Settings.GetString("MAILSERVER_MAIL_FROM"),
                To = underwriterEmail,
                Subject = string.Format("SendDealUpdatedEmailToUnderwriting.Subject".Translate(), _deal.DealNumber),
                SystemOid = 8,
            };

            Result.Merge((this as ISendEmailOperation).SendEmail(emailModel));
        }

        private void SendNewDealEmailToUnderwriting()
        {
            var underwriterEmail = _deal.UnderwriterUserGuid.HasValue
                ? InMemoryCache.GetUser(_deal.UnderwriterUserGuid.Value).UserEmail
                : Settings.GetString("UNDERWRITER_GROUP_MAIL");
            
            var emailModel = new EmailModel
            {
                Body = string.Format("SendNewDealEmailToUnderwriting.Body".Translate(), _deal.CreatedByUserFullName,
                    _deal.DealNumber),
                From = Settings.GetString("MAILSERVER_MAIL_FROM"),
                To = underwriterEmail,
                Subject = string.Format("SendNewDealEmailToUnderwriting.Subject".Translate(), _deal.DealNumber),
                SystemOid = 8,
            };

            Result.Merge((this as ISendEmailOperation).SendEmail(emailModel));
        }


        private void Save()
        {
            if (_dbContext.ChangeTracker.HasChanges())
            {
                _dbContext.SaveChanges();
            }

            if (isNewDeal)
                SendNewDealEmailToUnderwriting();
            else
                SendDealUpdatedEmailToUnderwriting();
        }
    }
}