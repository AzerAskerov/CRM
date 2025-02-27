using System;
using System.Collections.Generic;
using CRM.Data.Enums;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CRM.Data.Database
{
    public partial class Deal
    {
        public Deal()
        {
            DealPolicyLink = new HashSet<DealPolicyLink>();
            Discussion = new HashSet<Discussion>();
            Offer = new HashSet<Offer>();
        }

        public string CurrentUserName { get; set; }
        public string GeneralNote { get; set; }
        public string DealSubject { get; set; }
        public Guid CurrentUserGuid { get; set; }
        public Guid DealGuid { get; set; }
        public Guid CreatedByUserGuid { get; set; }
        public string CreatedByUserFullName { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public int ClientInn { get; set; }
        public string DealNumber { get; set; }
        public Guid? UnderwriterUserGuid { get; set; }
        public string UnderwriterUserFullName { get; set; }
        public int DealType { get; set; }
        public int DealLanguageOid { get; set; }
        public int DealStatus { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string SurveyContactInfo { get; set; }
        public decimal? SumInsured { get; set; }
        public int ResponsiblePersonType { get; set; }

        public virtual ClientRef ClientInnNavigation { get; set; }
        public virtual VehicleInsurance VehicleInsurance { get; set; }
        public virtual OtherTypeOffer OtherTypeOffer { get; set; }
        public virtual PropertyInsurance PropertyInsurance { get; set; }
        public virtual PersonalAccident PersonalAccident { get; set; }
        public virtual LifeInsurance LifeInsurance { get; set; }
        public virtual VoluntaryHealthInsurance VoluntaryHealthInsurance { get; set; }
        public virtual CarAndEquipmentInsurance CarAndEquipmentInsurance { get; set; }
        public virtual LiabilityInsurance LiabilityInsurance { get; set; }
        public virtual CargoInsurance CargoInsurance { get; set; }
        public virtual PropertyLiability PropertyLiability { get; set; }
        public virtual BenchOffer BenchOffer { get; set; }
        public virtual ElectronicsInsuranceOffer ElectronicsInsuranceOffer { get; set; }
        public virtual ProductLiabilityOffer ProductLiabilityOffer { get; set; }
        public virtual HiringLiability HiringLiability { get; set; }
        public virtual ICollection<DealPolicyLink> DealPolicyLink { get; set; }
        public virtual ICollection<Offer> Offer { get; set; }
        public virtual ICollection<Discussion> Discussion { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual Tender Tender { get; set; }
    }
}
