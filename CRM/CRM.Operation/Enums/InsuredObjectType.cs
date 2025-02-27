using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Enums
{
    public enum InsuredObjectType
    {
        /// <summary>
        /// General insured person from the ClientRegister for the various products. No product-specific data. Value <value>10</value>
        /// </summary>
        InsuredPerson = 10,

        /// <summary>
        /// Dummy object
        /// </summary>
        Dummy = 99,

        /// <summary>
        /// Kasko Vehicle
        /// </summary>
        KaskoVehicle = 101,

        /// <summary>
        /// Kasko vehicle equipment
        /// </summary>
        VehicleEquipment = 104,

        /// <summary>
        /// Kasko vehicle dirvers
        /// </summary>
        Drivers = 109,

        /// <summary>
        /// Kasko vehicle fleet <value>110</value>
        /// </summary>
        Fleet = 110,

        /// <summary>
        /// Personal Accidents insured group
        /// </summary>
        PAPersonGroup = 120,

        /// <summary>
        /// Personal Accidents insured person from a group
        /// </summary>
        PAInsuredPerson = 121,

        /// <summary>
        /// Personal Accidents standard coverage object
        /// </summary>
        PAGroupCoverage = 122,

        /// <summary>
        /// Personal Accidents standard risks object (used?)
        /// </summary>
        PARisks = 123,

        /// <summary>
        /// Type is used for Travel product and it relates to PersonalInsurance.PersonsGroup
        /// </summary>
        TravelPersonGroup = 124,

        /// <summary>
        /// Type is used for Travel product and it relates to PersonalInsurance.InsuredPersons
        /// </summary>
        TravelInsuredPerson = 125,

        /// <summary>
        /// Vehicle used in travel
        /// </summary>
        TravelVehicle = 128,

        /// <summary>
        /// Property used in travel
        /// </summary>
        TravelProperty = 135,

        /// <summary>
        /// Type is used for CMTPL product and it relates to MOD.Vehicle table
        /// </summary>
        MtplDetails = 130,

        /// <summary>
        /// Type is used for BankCasco product and it relates to MOD.Vehicle table
        /// </summary>
        BankCascoDetails = 140,

        /// <summary>
        /// Type is used for Retail Kasko product and it relates to MOD.Vehicle table
        /// </summary>
        RetailKaskoVehicle = 150,

        /// <summary>
        /// 
        /// </summary>
        RetailKaskoCoverageKasko = 151,

        /// <summary>
        /// Retail Kasko vehicle equipment. Table MOD.VehicleEquipment
        /// </summary>
        RetailKaskoVehicleEquipment = 157,

        /// <summary>
        /// Type is used for Property product and it relates to Property.Property
        /// </summary>
        Property = 201,

        /// <summary>
        /// Type is used for Passenger product and it relates to MOD.Vehicle table
        /// </summary>
        Passenger = 203,

        /// <summary>
        /// This is used for vehicle product and relates to MOD.Vehicle
        /// </summary>
        MiniRetailKaskoVehicle = 160,

        MiniRetailKaskoCoverageKasko = 161,

        /// <summary>
        /// This is used for life product and relates to Policies.InsuredObject
        /// </summary>
        Covid19 = 165,

        Covid19Coverage = 166,

        /// <summary>
        /// Type is used for life product and it relates to Policies.InsuredObject
        /// </summary>
        VoluntaryHealth = 168,

        VoluntaryHealthCoverageBase = 169,

        VoluntaryHealthRisks = 170,

        /// <summary>
        /// for RentACar product and relates to Mod.Vehicle
        /// </summary>
        RentACarVehicle = 210,

        /// <summary>
        /// for RentACar product and relates to vehicle equipment
        /// </summary>
        //RentACarVehicleEquipment=169,

        /// <summary>
        /// for RentACar product to 
        /// </summary>
        RentACarCoverageKasko = 211,

        VoluntaryPropertyLiability = 212,

        /// <summary>
        /// CMR
        /// </summary>
        CMRVehicle = 217,

        /// <summary>
        /// CMR
        /// </summary>
        CMRCoverageKasko = 218,


        CMRRisksKasko = 219,

        RisksBelongings = 220,

        BelongingsCoverage = 221,

        RiskElectricDevices = 222,

        ElectricDevicesCoverage = 223,

        RiskWorkBench = 224,

        WorkBenchCoverage = 225,

        RiskCessationCommercialActivity = 226,

        CessationCommercialActivityCoverage = 227,

        RiskFireAndOtherDisasters = 228,

        FireAndOtherDisastersCoverage = 229,

        RiskDuringTransportingValuables = 230,

        DuringTransportingValuablesCoverage = 231,

        RiskPersonalLiabilityForUseOfProperty = 232,

        PersonalLiabilityForUseOfPropertyCoverage = 233,

        RiskThirdPartyPersonalLiability = 234,

        ThirdPartyPersonalLiabilityCoverage = 235,

        /// <summary>
        /// for PropertyUltra  product to property child object
        /// </summary>
        PropertyUltraPropertyChild = 236,

        PropertyUltraRiskProperty = 237,

        PropertyUltraPropertyCoverage = 238,

        /// <summary>
        /// CMR cargo object
        /// </summary>
        CargoObject = 240,

        /// <summary>
        /// CMR cargo object
        /// </summary>
        CargoLuggageCoverage = 241,

        /// <summary>
        /// Property belongings<value>111</value>
        /// </summary>
        Belongings = 111,

    }
}
