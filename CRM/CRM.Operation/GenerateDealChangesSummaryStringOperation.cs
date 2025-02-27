using System.Linq;
using System.Text;
using CRM.Data.Database;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using Microsoft.EntityFrameworkCore;
using Zircon.Core.Extensions;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class GenerateDealChangesSummaryStringOperation : BusinessOperation
    {
        private readonly CRMDbContext _dbContext;
        public string SummaryString;
        public GenerateDealChangesSummaryStringOperation(CRMDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        protected override void DoExecute()
        {
            StringBuilder summary = new StringBuilder();
            
            foreach (var changedProperty in _dbContext.ChangeTracker.Entries().SelectMany(x => x.Properties)
                .Where(x => x.IsModified))
            {
                string propertyNameLocalized = string.Empty;
                string originalValue = changedProperty.OriginalValue?.ToString();
                string newValue = changedProperty.CurrentValue.ToString();
                
                switch (changedProperty.Metadata.Name)
                {
                    case nameof(Deal.CreatedByUserFullName):
                        propertyNameLocalized = "CreatedByUser.FullName".Translate();
                        break;
                    case nameof(Deal.UnderwriterUserFullName):
                        propertyNameLocalized = "UnderwriterUser.FullName".Translate();
                        break;
                    case nameof(Deal.SumInsured):
                        propertyNameLocalized = "OfferModel.SumInsured".Translate();
                        break;
                    case nameof(Deal.ClientInn):
                        propertyNameLocalized = "CardComponent.InsuredPerson".Translate();
                        var clientType = (ClientType)_dbContext.Clients.FirstOrDefault(x => x.Inn.Equals(int.Parse(originalValue))).ClientType;
                        originalValue = clientType == ClientType.Pyhsical
                            ? _dbContext.PhysicalPeople.FirstOrDefault(x => x.Inn.Equals(int.Parse(originalValue))).FullName
                            : _dbContext.Companies.FirstOrDefault(x => x.Inn.Equals(int.Parse(originalValue))).CompanyName;
                        
                        clientType = (ClientType)_dbContext.Clients.FirstOrDefault(x => x.Inn.Equals(int.Parse(newValue))).ClientType;
                        newValue = clientType == ClientType.Pyhsical
                            ? _dbContext.PhysicalPeople.FirstOrDefault(x => x.Inn.Equals(int.Parse(newValue))).FullName
                            : _dbContext.Companies.FirstOrDefault(x => x.Inn.Equals(int.Parse(newValue))).CompanyName;
                        break;
                    case nameof(Deal.DealLanguageOid):
                        var offerLanguageSourceLoader = new OfferLanguageSourceLoader();
                        propertyNameLocalized = "DropDown.OfferLanguageOid".Translate();
                        originalValue = offerLanguageSourceLoader.GetItem(originalValue, _dbContext).Value;
                        newValue = offerLanguageSourceLoader.GetItem(newValue, _dbContext).Value;
                        break;
                    case nameof(Deal.VehicleInsurance.BeneficiaryClientInn):
                        propertyNameLocalized = "OfferModel.Beneficiary".Translate();
                        clientType = (ClientType)_dbContext.Clients.FirstOrDefault(x => x.Inn.Equals(originalValue)).ClientType;
                        var oldNameFromDb = clientType == ClientType.Pyhsical
                            ? _dbContext.PhysicalPeople.FirstOrDefault(x => x.Inn.Equals(originalValue))?.FullName
                            : _dbContext.Companies.FirstOrDefault(x => x.Inn.Equals(originalValue))?.CompanyName;
                        originalValue = oldNameFromDb;
                        
                        clientType = (ClientType)_dbContext.Clients.FirstOrDefault(x => x.Inn.Equals(newValue)).ClientType;
                        var newValueFromDb = clientType == ClientType.Pyhsical
                            ? _dbContext.PhysicalPeople.FirstOrDefault(x => x.Inn.Equals(newValue))?.FullName
                            : _dbContext.Companies.FirstOrDefault(x => x.Inn.Equals(newValue))?.CompanyName;
                        newValue = newValueFromDb;
                        break;
                    case nameof(Deal.VehicleInsurance.PolicyPeriodOid):
                        var offerPolicyPeriodSourceLoader = new OfferPolicyPeriodSourceLoader();
                        propertyNameLocalized = "OfferModel.PolicyPeriodOid".Translate();
                        originalValue = offerPolicyPeriodSourceLoader.GetItem(originalValue, _dbContext).Value;
                        newValue = offerPolicyPeriodSourceLoader.GetItem(newValue, _dbContext).Value;
                        break;
                    case nameof(Deal.VehicleInsurance.Commission):
                        propertyNameLocalized = "OfferModel.Commission".Translate();
                        break;
                    case nameof(Deal.VehicleInsurance.DeductibleAmountOid):
                        var deductibleAmountSourceLoader = new DeductibleAmountSourceLoader();
                        propertyNameLocalized = "DropDown.DeductibleAmountOid".Translate();
                        originalValue = deductibleAmountSourceLoader.GetItem(originalValue, _dbContext).Value;
                        newValue = deductibleAmountSourceLoader.GetItem(newValue, _dbContext).Value;
                        break;
                    case nameof(Deal.VehicleInsurance.CurrencyOid):
                        var currencySourceLoader = new CurrencySourceLoader();
                        propertyNameLocalized = "OfferModel.PolicyPeriodOid".Translate();
                        originalValue = currencySourceLoader.GetItem(originalValue, _dbContext).Value;
                        newValue = currencySourceLoader.GetItem(newValue, _dbContext).Value;
                        break;
                    case nameof(Deal.VehicleInsurance.PaymentTypeOid):
                        var paymentTypeSourceLoader = new PaymentTypeSourceLoader();
                        propertyNameLocalized = "DropDown.PaymentTypeOid".Translate();
                        originalValue = paymentTypeSourceLoader.GetItem(originalValue, _dbContext).Value;
                        newValue = paymentTypeSourceLoader.GetItem(newValue, _dbContext).Value;
                        break;
                    case nameof(Deal.VehicleInsurance.InsuranceArea):
                        propertyNameLocalized = "OfferModel.InsuranceArea".Translate();
                        break;
                    case nameof(VehicleInfoForInsurance.MarketValueOfVehicle):
                        propertyNameLocalized = "OfferModel.MarketValueOfVehicle".Translate();
                        break;
                    case nameof(VehicleInfoForInsurance.ManufactureYear):
                        propertyNameLocalized = "OfferModel.ManufactureYear".Translate();
                        break;
                    case nameof(VehicleInfoForInsurance.VehBrandOid):
                        var vehicleBrandsSourceLoader = new VehicleBrandsSourceLoader();
                        propertyNameLocalized = "DropDown.VehBrandOid".Translate();
                        originalValue = vehicleBrandsSourceLoader.GetItem(originalValue, null).Value;
                        newValue = vehicleBrandsSourceLoader.GetItem(newValue, null).Value;
                        break;
                    case nameof(VehicleInfoForInsurance.VehModelOid):
                        var vehicleModelsSourceLoader = new VehicleModelsSourceLoader();
                        propertyNameLocalized = "DropDown.VehBrandOid".Translate();
                        originalValue = vehicleModelsSourceLoader.GetItem(originalValue, null).Value;
                        newValue = vehicleModelsSourceLoader.GetItem(newValue, null).Value;
                        break;
                    case nameof(VehicleInfoForInsurance.OfficialServiceRequired):
                        propertyNameLocalized = "OfferModel.OfficialServiceRequired".Translate();
                        break;
                    case nameof(VehicleInfoForInsurance.PersonalAccidentInsuranceOfDriverAndPassengerItemsOid):
                        var personalAccidentItemsSourceLoader = new PersonalAccidentItemsSourceLoader();
                        propertyNameLocalized = "DropDown.PersonalAccidentInsuranceOfDriverAndPassengerItemsOid".Translate();
                        originalValue = personalAccidentItemsSourceLoader.GetItem(originalValue, null).Value;
                        newValue = personalAccidentItemsSourceLoader.GetItem(newValue, null).Value;
                        break;
                    case nameof(VehicleInfoForInsurance.CountOfInsuredSeats):
                        propertyNameLocalized = "OfferModel.OfficialServiceRequired".Translate();
                        break;
                    case nameof(VehicleInfoForInsurance.PropertyLiabilityInsuranceItemsOid):
                        var propertyLiabilityItemsSourceLoader = new PropertyLiabilityItemsSourceLoader();
                        propertyNameLocalized = "DropDown.PropertyLiabilityInsuranceItemsOid".Translate();
                        originalValue = propertyLiabilityItemsSourceLoader.GetItem(originalValue, null).Value;
                        newValue = propertyLiabilityItemsSourceLoader.GetItem(newValue, null).Value;
                        break;
                    case nameof(VehicleInfoForInsurance.EngineCapacity):
                        propertyNameLocalized = "OfferModel.EngineCapacity".Translate();
                        break;
                    case nameof(VehicleInfoForInsurance.SelectedVehicleUsagePurposeOid):
                        var vehicleUsagePurposeSourceLoader = new VehicleUsagePurposeSourceLoader();
                        propertyNameLocalized = "DropDown.SelectedVehicleUsagePurposeOid".Translate();
                        originalValue = vehicleUsagePurposeSourceLoader.GetItem(originalValue, null).Value;
                        newValue = vehicleUsagePurposeSourceLoader.GetItem(newValue, null).Value;
                        break;
                    case nameof(Deal.VehicleInsurance.IsRenew):
                        propertyNameLocalized = "OfferModel.IsRenew".Translate();
                        originalValue = originalValue.Equals("True") ? "Seçildi" : "Seçilmədi";
                        newValue = newValue.Equals("True") ? "Seçildi" : "Seçilmədi";
                        break;
                    case nameof(Deal.OtherTypeOffer.Comment):
                        propertyNameLocalized = "OfferModel.Comment".Translate();
                        break;
                    case nameof(Deal.OtherTypeOffer.ExistingPolicyNumber):
                        propertyNameLocalized = "PolicyNumber".Translate();
                        break;
                    case nameof(Deal.PropertyInsurance.ImmovablePropertyInsuranceAmount):
                        propertyNameLocalized = "OfferModel.ImmovablePropertyInsuranceAmount".Translate();
                        break;
                    case nameof(Deal.PropertyInsurance.MovablePropertyInsuranceAmount):
                        propertyNameLocalized = "OfferModel.MovablePropertyInsuranceAmount".Translate();
                        break;
                    case nameof(Deal.PropertyInsurance.InsuranceTypeOid):
                        var insuranceTypeSourceLoader = new InsuranceTypeSourceLoader();
                        propertyNameLocalized = "DropDown.InsuranceTypeOid".Translate();
                        originalValue = insuranceTypeSourceLoader.GetItem(originalValue, null).Value;
                        newValue = insuranceTypeSourceLoader.GetItem(newValue, null).Value;
                        break;
                    case nameof(Deal.PropertyInsurance.MovablePropertyDescription):
                        propertyNameLocalized = "OfferModel.MovablePropertyDescription".Translate();
                        break;
                    case nameof(Deal.PropertyInsurance.SecurityAndFirefightingSystemInfo):
                        propertyNameLocalized = "OfferModel.SecurityAndFirefightingSystemInfo".Translate();
                        break;
                    case nameof(Deal.PropertyInsurance.LostHistoryForTheLastFiveYears):
                        propertyNameLocalized = "OfferModel.LostHistoryForTheLastThreeYears".Translate();
                        break;
                    case nameof(Deal.PropertyInsurance.PropertyTypeOid):
                        var propertyTypeSourceLoader = new PropertyTypeSourceLoader();
                        propertyNameLocalized = "DropDown.PropertyTypeOid".Translate();
                        originalValue = propertyTypeSourceLoader.GetItem(originalValue, null).Value;
                        newValue = propertyTypeSourceLoader.GetItem(newValue, null).Value;
                        break;
                    case nameof(Deal.PersonalAccident.CompanyActivityType):
                        propertyNameLocalized = "OfferModel.CompanyActivityType".Translate();
                        break;
                    case nameof(Deal.PersonalAccident.EmployeesNumber):
                        propertyNameLocalized = "OfferModel.EmployeesNumber".Translate();
                        break;
                    case nameof(Deal.VoluntaryHealthInsurance.TypeOfActivity):
                        propertyNameLocalized = "OfferModel.TypeOfActivity".Translate();
                        break;
                    case nameof(Deal.VoluntaryHealthInsurance.IsJobContainsHarmfulFactors):
                        propertyNameLocalized = "OfferModel.IsJobContainsHarmfulFactors".Translate();
                        break;
                    case nameof(Deal.VoluntaryHealthInsurance.NumberOfOfficeWorkers):
                        propertyNameLocalized = "OfferModel.IsJobContainsHarmfulFactors".Translate();
                        break;
                    case nameof(Deal.VoluntaryHealthInsurance.NumberOfProductionWorkers):
                        propertyNameLocalized = "OfferModel.NumberOfProductionWorkers".Translate();
                        break;
                    case nameof(Deal.LiabilityInsurance.BodyInjuryLimitPerPerson):
                        propertyNameLocalized = "OfferModel.BodyInjuryLimitPerPerson".Translate();
                        break;
                    case nameof(Deal.LiabilityInsurance.PropertyDamageLimitPerAccident):
                        propertyNameLocalized = "OfferModel.PropertyDamageLimitPerAccident".Translate();
                        break;
                    case nameof(Deal.LiabilityInsurance.AnnualTurnoverOfCompany):
                        propertyNameLocalized = "OfferModel.AnnualTurnoverOfCompany".Translate();
                        break;
                    case nameof(Deal.LiabilityInsurance.BodyInjuryLimitPerAccident):
                        propertyNameLocalized = "OfferModel.BodyInjuryLimitPerAccident".Translate();
                        break;
                    case nameof(Deal.LiabilityInsurance.BodyInjuryAnnualAggregateLimit):
                        propertyNameLocalized = "OfferModel.BodyInjuryAnnualAggregateLimit".Translate();
                        break;
                    case nameof(Deal.LiabilityInsurance.PropertyDamageAnnualAggregateLimit):
                        propertyNameLocalized = "OfferModel.PropertyDamageAnnualAggregateLimit".Translate();
                        break;
                    case nameof(Deal.CargoInsurance.InvoiceNumber):
                        propertyNameLocalized = "OfferModel.InvoiceNumber".Translate();
                        break;
                    case nameof(Deal.CargoInsurance.InvoiceDate):
                        propertyNameLocalized = "OfferModel.InvoiceDate".Translate();
                        break;
                    case nameof(Deal.CargoInsurance.InvoiceAmountOfCargo):
                        propertyNameLocalized = "OfferModel.InvoiceAmountOfCargo".Translate();
                        break;
                    case nameof(Deal.CargoInsurance.ShippingCosts):
                        propertyNameLocalized = "OfferModel.ShippingCosts".Translate();
                        break;
                    case nameof(Deal.CargoInsurance.StartPoint):
                        propertyNameLocalized = "OfferModel.StartPoint".Translate();
                        break;
                    case nameof(Deal.CargoInsurance.DestinationPoint):
                        propertyNameLocalized = "OfferModel.DestinationPoint".Translate();
                        break;
                    case nameof(Deal.CargoInsurance.Packaging):
                        propertyNameLocalized = "OfferModel.Packaging".Translate();
                        break;
                    case nameof(Deal.CargoInsurance.ProbableShippingDate):
                        propertyNameLocalized = "OfferModel.ProbableShippingDate".Translate();
                        break;
                    case nameof(Deal.CargoInsurance.ProbableDeliveryDate):
                        propertyNameLocalized = "OfferModel.ProbableDeliveryDate".Translate();
                        break;


                    case nameof(VehicleInfoForInsurance.RepairNeed):
                        propertyNameLocalized = "OfferModel.RepairNeed".Translate();
                        break;
                    case nameof(VehicleInsurance.LastFiveYearLossHistory):
                        propertyNameLocalized = "OfferModel.LastFiveYearLossHistory".Translate();
                        break;
                    case nameof(VehicleInsurance.InfoForInsuranceCompany):
                        propertyNameLocalized = "OfferModel.InfoForInsuranceCompany".Translate();
                        break;
                    case nameof(VehicleInsurance.UnconditionalExemptionAmount):
                        propertyNameLocalized = "OfferModel.UnconditionalExemptionAmount".Translate();
                        break;
                    case nameof(VehicleInsurance.ThirdPartiesAmount):
                        propertyNameLocalized = "OfferModel.ThirdPartiesAmount".Translate();
                        break;
                    case nameof(VehicleInsurance.PersonalAccidentAmount):
                        propertyNameLocalized = "OfferModel.PersonalAccidentAmount".Translate();
                        break;


                    case nameof(BenchOffer.AnyInjuryKnowledge):
                        propertyNameLocalized = "OfferModel.AnyInjuryKnowledge".Translate();
                        break;
                    case nameof(BenchOffer.AnyUsefulInfoForInsuranceCompany):
                        propertyNameLocalized = "OfferModel.AnyUsefulInfoForInsuranceCompany".Translate();
                        break;
                    case nameof(BenchOffer.ImmovablePropertyTypeOid):
                        propertyNameLocalized = "OfferModel.ImmovablePropertyTypeOid".Translate();
                        break;
                    case nameof(BenchOffer.ImmovablePropertyTypeOther):
                        propertyNameLocalized = "OfferModel.ImmovablePropertyTypeOther".Translate();
                        break;
                    case nameof(BenchOffer.Injuries):
                        propertyNameLocalized = "OfferModel.Injuries".Translate();
                        break;
                    case nameof(BenchOffer.InsuranceAreaType):
                        propertyNameLocalized = "OfferModel.InsuranceAreaType".Translate();
                        break;
                    case nameof(BenchOffer.InsurancePredmetAddress):
                        propertyNameLocalized = "OfferModel.InsurancePredmetAddress".Translate();
                        break;
                    case nameof(BenchOffer.InsuredBenchAmount):
                        propertyNameLocalized = "OfferModel.InsuredBenchAmount".Translate();
                        break;
                    case nameof(BenchOffer.InsuredBenchValue):
                        propertyNameLocalized = "OfferModel.InsuredBenchValue".Translate();
                        break;
                    case nameof(BenchOffer.LastThreeYearsInjuryKnowledge):
                        propertyNameLocalized = "OfferModel.LastThreeYearsInjuryKnowledge".Translate();
                        break;
                    case nameof(BenchOffer.LastThreeYearsInjuryKnowledgeYes):
                        propertyNameLocalized = "OfferModel.LastThreeYearsInjuryKnowledgeYes".Translate();
                        break;
                    case nameof(BenchOffer.ManufacturerName):
                        propertyNameLocalized = "OfferModel.ManufacturerName".Translate();
                        break;
                    case nameof(BenchOffer.MovablePropertyTypeOid):
                        propertyNameLocalized = "OfferModel.MovablePropertyTypeOid".Translate();
                        break;
                    case nameof(BenchOffer.PackagingExpenses):
                        propertyNameLocalized = "OfferModel.PackagingExpenses".Translate();
                        break;
                    case nameof(BenchOffer.Power):
                        propertyNameLocalized = "OfferModel.Power".Translate();
                        break;
                    case nameof(BenchOffer.ReplacementCost):
                        propertyNameLocalized = "OfferModel.ReplacementCost".Translate();
                        break;
                    case nameof(BenchOffer.Series):
                        propertyNameLocalized = "OfferModel.Series".Translate();
                        break;
                    case nameof(BenchOffer.SpecialConditions):
                        propertyNameLocalized = "OfferModel.SpecialConditions".Translate();
                        break;
                    case nameof(BenchOffer.SpecialConditionsYes):
                        propertyNameLocalized = "OfferModel.SpecialConditionsYes".Translate();
                        break;
                    case nameof(BenchOffer.Type):
                        propertyNameLocalized = "OfferModel.Type".Translate();
                        break;
                    case nameof(BenchOffer.Unit):
                        propertyNameLocalized = "OfferModel.Unit".Translate();
                        break;
                    case nameof(BenchOffer.Voltage):
                        propertyNameLocalized = "OfferModel.Voltage".Translate();
                        break;
                    case nameof(BenchOffer.Year):
                        propertyNameLocalized = "OfferModel.Year".Translate();
                        break;


                    case nameof(ElectronicsInsuranceOffer.CurrentPrice):
                        propertyNameLocalized = "OfferModel.CurrentPrice".Translate();
                        break;
                    case nameof(ElectronicsInsuranceOffer.CustomsExpenses):
                        propertyNameLocalized = "OfferModel.CustomsExpenses".Translate();
                        break;
                    case nameof(ElectronicsInsuranceOffer.DangerousChemicals):
                        propertyNameLocalized = "OfferModel.DangerousChemicals".Translate();
                        break;
                    case nameof(ElectronicsInsuranceOffer.DangerousChemicalsUsage):
                        propertyNameLocalized = "OfferModel.DangerousChemicalsUsage".Translate();
                        break;
                    case nameof(ElectronicsInsuranceOffer.Description):
                        propertyNameLocalized = "OfferModel.Description".Translate();
                        break;
                    case nameof(ElectronicsInsuranceOffer.DeviceAddress):
                        propertyNameLocalized = "OfferModel.DeviceAddress".Translate();
                        break;
                    case nameof(ElectronicsInsuranceOffer.DeviceIsNew):
                        propertyNameLocalized = "OfferModel.DeviceIsNew".Translate();
                        break;
                    case nameof(ElectronicsInsuranceOffer.DeviceReplacementCost):
                        propertyNameLocalized = "OfferModel.DeviceReplacementCost".Translate();
                        break;
                    case nameof(ElectronicsInsuranceOffer.ElectronicAnyInfo):
                        propertyNameLocalized = "OfferModel.ElectronicAnyInfo".Translate();
                        break;
                    case nameof(ElectronicsInsuranceOffer.EmailToNotify):
                        propertyNameLocalized = "OfferModel.EmailToNotify".Translate();
                        break;


                    case nameof(HiringLiability.AnnualGeneralLimit):
                        propertyNameLocalized = "OfferModel.AnnualGeneralLimit".Translate();
                        break;
                    case nameof(HiringLiability.AnyInfo):
                        propertyNameLocalized = "OfferModel.AnyInfo".Translate();
                        break;
                    case nameof(HiringLiability.DaysPassedInSea):
                        propertyNameLocalized = "OfferModel.DaysPassedInSea".Translate();
                        break;
                    case nameof(HiringLiability.DistanceFromSea):
                        propertyNameLocalized = "OfferModel.DistanceFromSea".Translate();
                        break;
                    case nameof(HiringLiability.GateGoodCondition):
                        propertyNameLocalized = "OfferModel.GateGoodCondition".Translate();
                        break;
                    case nameof(HiringLiability.LaborAnnualSalaryFond):
                        propertyNameLocalized = "OfferModel.LaborAnnualSalaryFond".Translate();
                        break;
                    case nameof(HiringLiability.LaborQuantity):
                        propertyNameLocalized = "OfferModel.LaborQuantity".Translate();
                        break;
                    case nameof(HiringLiability.LimitPerAccident):
                        propertyNameLocalized = "OfferModel.LimitPerAccident".Translate();
                        break;
                    case nameof(HiringLiability.LimitPerPerson):
                        propertyNameLocalized = "OfferModel.LimitPerPerson".Translate();
                        break;
                    case nameof(HiringLiability.MaxEmployeesInSea):
                        propertyNameLocalized = "OfferModel.MaxEmployeesInSea".Translate();
                        break;
                    case nameof(HiringLiability.MaxTransportedEmployees):
                        propertyNameLocalized = "OfferModel.MaxTransportedEmployees".Translate();
                        break;
                    case nameof(HiringLiability.SeaWorkerAnnualSalaryFond):
                        propertyNameLocalized = "OfferModel.SeaWorkerAnnualSalaryFond".Translate();
                        break;
                    case nameof(HiringLiability.SeaWorkerQuantity):
                        propertyNameLocalized = "OfferModel.SeaWorkerQuantity".Translate();
                        break;
                    case nameof(HiringLiability.ServantAnnualSalaryFond):
                        propertyNameLocalized = "OfferModel.ServantAnnualSalaryFond".Translate();
                        break;
                    case nameof(HiringLiability.ServantQuantity):
                        propertyNameLocalized = "OfferModel.ServantQuantity".Translate();
                        break;
                    case nameof(HiringLiability.ShowAcids):
                        propertyNameLocalized = "OfferModel.ShowAcids".Translate();
                        break;
                    case nameof(HiringLiability.TypeOfTransportation):
                        propertyNameLocalized = "OfferModel.TypeOfTransportation".Translate();
                        break;
                    case nameof(HiringLiability.WorkOnSea):
                        propertyNameLocalized = "OfferModel.WorkOnSea".Translate();
                        break;



                    case nameof(ProductLiabilityOffer.ActivityFeatures):
                        propertyNameLocalized = "OfferModel.ActivityFeatures".Translate();
                        break;
                    case nameof(ProductLiabilityOffer.AllCompanyNames):
                        propertyNameLocalized = "OfferModel.AllCompanyNames".Translate();
                        break;
                    case nameof(ProductLiabilityOffer.CurrentYearSales):
                        propertyNameLocalized = "OfferModel.CurrentYearSales".Translate();
                        break;
                    case nameof(ProductLiabilityOffer.ExplainDangerousProduct):
                        propertyNameLocalized = "OfferModel.ExplainDangerousProduct".Translate();
                        break;
                    case nameof(ProductLiabilityOffer.ExplainProducts):
                        propertyNameLocalized = "OfferModel.ExplainProducts".Translate();
                        break;
                    case nameof(ProductLiabilityOffer.IsAnyDirtAround):
                        propertyNameLocalized = "OfferModel.IsAnyDirtAround".Translate();
                        break;
                    case nameof(ProductLiabilityOffer.IsAnyProductDangerous):
                        propertyNameLocalized = "OfferModel.IsAnyProductDangerous".Translate();
                        break;
                    case nameof(ProductLiabilityOffer.IsSituationGood):
                        propertyNameLocalized = "OfferModel.IsSituationGood".Translate();
                        break;
                    case nameof(ProductLiabilityOffer.LocalCompanyFoundDate):
                        propertyNameLocalized = "OfferModel.LocalCompanyFoundDate".Translate();
                        break;
                    case nameof(ProductLiabilityOffer.LossHistory):
                        propertyNameLocalized = "OfferModel.LossHistory".Translate();
                        break;
                    case nameof(ProductLiabilityOffer.NumberOfEmployees):
                        propertyNameLocalized = "OfferModel.NumberOfEmployees".Translate();
                        break;
                    case nameof(ProductLiabilityOffer.QualityCheck):
                        propertyNameLocalized = "OfferModel.QualityCheck".Translate();
                        break;



                    case nameof(CargoInsurance.CargoDescription):
                        propertyNameLocalized = "OfferModel.CargoDescription".Translate();
                        break;
                    case nameof(CargoInsurance.CargoFeatures):
                        propertyNameLocalized = "OfferModel.CargoFeatures".Translate();
                        break;

                    case nameof(CargoInsurance.Route):
                        propertyNameLocalized = "OfferModel.Route".Translate();
                        break;
                    case nameof(CargoInsurance.StartPointCountryId):
                        propertyNameLocalized = "OfferModel.StartPointCountryId".Translate();
                        break;

                    case nameof(CargoInsurance.EndPointCountryId):
                        propertyNameLocalized = "OfferModel.EndPointCountryId".Translate();
                        break;

                    case nameof(CargoInsurance.StartPointCityId):
                        propertyNameLocalized = "OfferModel.StartPointCityId".Translate();
                        break;
                    case nameof(CargoInsurance.EndPointCityId):
                        propertyNameLocalized = "OfferModel.EndPointCityId".Translate();
                        break;

                    case nameof(CarAndEquipmentInsurance.InsuranceValue):
                        propertyNameLocalized = "OfferModel.InsuranceValue".Translate();
                        break;

                    case nameof(CarAndEquipmentInsurance.AnyInfoForInsuranceCompany):
                        propertyNameLocalized = "OfferModel.AnyInfoForInsuranceCompany".Translate();
                        break;
                }
                
                if(string.IsNullOrEmpty(propertyNameLocalized))
                    continue;

                summary.AppendLine(GenerateSummaryLine(propertyNameLocalized, originalValue, newValue));
            }

            if (summary.Length > 0) SummaryString = summary.ToString();
        }

        private string GenerateSummaryLine(string propertyNameLocalized, string originalValue, string newValue) =>
            $"{propertyNameLocalized} : `{originalValue}` -> `{newValue}`";
    }
}