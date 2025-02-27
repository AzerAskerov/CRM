using CRM.Operation.Attributes;
using CRM.Operation.SourceLoaders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Zircon.Core;
using Zircon.Core.Attributes;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.RequestModels
{

    [DataContract]
    public class ClientContract : IModel
    {
        public ClientContract()
        {
            ContactsInfo = new List<ContactInfoContract>();
            Documents = new List<DocumentContract>();
            Relations = new List<RelationContract>();
            ClientComments = new List<CommentContract>();
            Tags = new List<TagContract>();
            Address = new AddressContract();
            Assets = new List<AssetContract>();
            LeadObjects = new List<LeadObjectModel>();
        }

        public ClientContract(ClientContract clientContract)
        {
            INN = clientContract.INN;
            ClientType = clientContract.ClientType;
            CompanyName = clientContract.CompanyName;
            FirstName = clientContract.FirstName;
            FullName = clientContract.FullName;
            LastName = clientContract.LastName;
            FatherName = clientContract.FatherName;
            Gender = clientContract.Gender;
            PinNumber = clientContract.PinNumber;
            BirthDate = clientContract.BirthDate;
            PositionCustom = clientContract.PositionCustom;
            Position = clientContract.Position;
            MonthlyIncome = clientContract.MonthlyIncome;
            Tags = clientContract.Tags;
            Tag = clientContract.Tag;
            ClientComments = clientContract.ClientComments;
            Address = clientContract.Address;
            ContactsInfo = clientContract.ContactsInfo;
            Documents = clientContract.Documents;
            Relations = clientContract.Relations;
            ImageBase64 = clientContract.ImageBase64;
            LeadObjects = clientContract.LeadObjects;
        }
        [DataMember(Name = "INN")]
        [Key]
        public int? INN { get; set; }

        [DataMember(Name = "clienttype")]
        public ClientType ClientType { get; set; }

        [DataMember(Name = "companyname")]
        [Attributes.DisplayNameLocalized("ContactListItemModel.CompanyName")]
        public string CompanyName { get; set; }

        [DataMember(Name = "firstname")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastname")]
        public string LastName { get; set; }

        [DataMember(Name = "fathername")]
        public string FatherName { get; set; }

        [DataMember(Name = "fullname")]
        public string FullName { get; set; }

        [DataMember(Name = "gender")]
        public int? Gender { get; set; }

        [DataMember(Name = "imageBase64")]
        public string ImageBase64 { get; set; }

        [DataMember(Name = "pinnumber")]
        [Attributes.DisplayNameLocalized("ContactListItemModel.PinNumber")]
        public string PinNumber { get; set; }

        [DataMember(Name = "birthdate")]
        [Attributes.DisplayNameLocalized("ContactListItemModel.BirthDate")]
        public DateTime? BirthDate { get; set; }

        [DataMember(Name = "positioncustom")]
        public string PositionCustom { get; set; }

        [DataMember(Name = "position")]
        [CustomSource(typeof(PositionSourceLoader))]
        public int? Position { get; set; }

        [DataMember(Name = "monthlyincome")]
        public decimal? MonthlyIncome { get; set; }

        [DataMember(Name = "tags")]
        [ValidateComplexType]
        public List<TagContract> Tags { get; set; }

        [CustomSource(typeof(TagSourceLoader))]
        public int Tag { get; set; }

        [DataMember(Name = "clientcomments")]
        [ValidateComplexType]
        public List<CommentContract> ClientComments { get; set; }

        [DataMember(Name = "assets")]
        [ValidateComplexType]
        public List<AssetContract> Assets { get; set; }

        [DataMember(Name = "address")]
        [ValidateComplexType]
        public AddressContract Address { get; set; }

        [DataMember(Name = "leadObjects")]
        [ValidateComplexType]
        public List<LeadObjectModel> LeadObjects { get; set; }

        [DataMember(Name = "additionalInfo")]
        public string AdditionalInfo { get; set; }

        [DataMember(Name = "contactsinfo")]
        [ValidateComplexType]
        public List<ContactInfoContract> ContactsInfo { get; set; }

        [DataMember(Name = "documents")]
        [ValidateComplexType]
        public List<DocumentContract> Documents { get; set; }

        [DataMember(Name = "relations")]
        [ValidateComplexType]
        public virtual IEnumerable<RelationContract> Relations { get; set; }

        [DataMember(Name = "operationType")]
        public string OperationType { get; set; }
        [DataMember(Name = "operationDescription")]
        public string OperationDescription { get; set; }

        [DataMember(Name = "tinnumber")]
        public string TinNumber { get; set; }

        [DataMember(Name = "idnumber")]

        public string IdNumber { get; set; }

        public string FullConcatName
        {
            get
            {
                switch (ClientType)
                {
                    case ClientType.Pyhsical:
                        return $"{LastName} {FirstName}";

                    case ClientType.Company:
                        return CompanyName;
                    default:
                        return "";

                }
            }
        }

    }

    public enum ClientType
    {
        Pyhsical = 1,
        Company = 2,
        Undefined = 0
    }
}
