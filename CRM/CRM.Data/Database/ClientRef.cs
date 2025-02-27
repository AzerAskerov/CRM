using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class ClientRef
    {
        public ClientRef()
        {
            Addresses = new HashSet<Address>();
            Appointments = new HashSet<Appointment>();
            ClientContactInfoComps = new HashSet<ClientContactInfoComp>();
            ClientRelationCompClient1Navigations = new HashSet<ClientRelationComp>();
            ClientRelationCompClient2Navigations = new HashSet<ClientRelationComp>();
            Clients = new HashSet<Client>();
            CommentComps = new HashSet<CommentComp>();
            Companies = new HashSet<Company>();
            Documents = new HashSet<Document>();
            PhysicalPeople = new HashSet<PhysicalPerson>();
            UserClientComps = new HashSet<UserClientComp>();
            TagComps = new HashSet<TagComp>();
            VehicleInsurance = new HashSet<VehicleInsurance>();
            OtherTypeOffer = new HashSet<OtherTypeOffer>();
            PropertyInsurance = new HashSet<PropertyInsurance>();
            CarAndEquipmentInsurance = new HashSet<CarAndEquipmentInsurance>();
            LiabilityInsurance = new HashSet<LiabilityInsurance>();
            CargoInsurance = new HashSet<CargoInsurance>();
            Assets = new HashSet<Asset>();
            //      Tenders = new HashSet<Tender>();
            ClientHistories = new HashSet<ClientHistory>();
            LeadObjects = new HashSet<LeadObject>();
        }

        public int Inn { get; set; }
        public virtual ICollection<ClientHistory> ClientHistories { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<ClientContactInfoComp> ClientContactInfoComps { get; set; }
        public virtual ICollection<ClientRelationComp> ClientRelationCompClient1Navigations { get; set; }
        public virtual ICollection<ClientRelationComp> ClientRelationCompClient2Navigations { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<CommentComp> CommentComps { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<PhysicalPerson> PhysicalPeople { get; set; }
        public virtual ICollection<UserClientComp> UserClientComps { get; set; }
        public virtual ICollection<TagComp> TagComps { get; set; }
        public virtual ICollection<Deal> Deal { get; set; }
        public virtual ICollection<VehicleInsurance> VehicleInsurance { get; set; }
        public virtual ICollection<OtherTypeOffer> OtherTypeOffer { get; set; }
        public virtual ICollection<PropertyInsurance> PropertyInsurance { get; set; }
        public virtual ICollection<CarAndEquipmentInsurance> CarAndEquipmentInsurance { get; set; }
        public virtual ICollection<LiabilityInsurance> LiabilityInsurance { get; set; }
        public virtual ICollection<CargoInsurance> CargoInsurance { get; set; }



        public virtual ICollection<ElectronicsInsuranceOffer> ElectronicsInsuranceOffers { get; set; }
        public virtual ICollection<HiringLiability> HiringLiabilityOffers { get; set; }
        public virtual ICollection<ProductLiabilityOffer> ProductLiabilityOffers { get; set; }
        public virtual ICollection<PropertyLiability> PropertyLiabilityOffers { get; set; }
        public virtual ICollection<BenchOffer> BenchOffers { get; set; }

        public virtual ICollection<LeadObject> LeadObjects { get; set; }


        // public virtual ICollection<Tender> Tenders { get; set; }
    }
}
