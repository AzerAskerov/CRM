using CRM.Operation.Enums;
using CRM.Operation.Queue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.RequestModels
{
    [Serializable]
    [XmlRoot(ElementName = "ClientData")]
    public class ClientDataPayload : BaseQueuePayload, IModel
    {
        public ClientDataPayload()
        {
            Priority = 1;
            Type = QueueItemType.CRM;
            SubtypeOid = QueueItemSubtype.ClientCreateOrUpdate;
            SystemOid = 1;
        }

        [XmlElement("INN")]
        public int? INN { get; set; }

        [XmlElement("Source")]
        public int Source { get; set; }

        [XmlElement("LobOid")]
        public int? LobOid { get; set; }

        [XmlElement("ResponsiblePerson")]
        public string ResponsiblePerson { get; set; }

        [XmlIgnore]
        public ClientType ClientType { get; set; }

        [XmlElement("ClientType")]
        public string ClientTypeValue
        {
            get
            {
                return ClientType.ToString();
            }

            set
            {
                ClientType result = ClientType.Pyhsical;
                Enum.TryParse(value, true, out result);

                ClientType = result;
            }
        }

        

        [XmlElement("CompanyName")]
        public string CompanyName { get; set; }

        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [XmlElement("FullName")]
        public string FullName { get; set; }


        [XmlElement("LastName")]
        public string LastName { get; set; }

        [XmlElement("FatherName")]
        public string FatherName { get; set; }

        [XmlElement("PinNumber")]
        public string PinNumber { get; set; }

        [XmlElement("BirthDate", typeof(DateTime?))]
        public DateTime? BirthDate { get; set; }

        [XmlElement("Gender")]
        public int? Gender { get; set; }

        [XmlElement("PositionCustom")]
        public string PositionCustom { get; set; }

        [XmlElement("Position")]
        public int? Position { get; set; }

        [XmlElement("MonthlyIncome")]
        public decimal? MonthlyIncome { get; set; }

        [XmlElement("ClientComment")]
        public List<CommentData> ClientComments { get; set; }

        [XmlElement("Address")]
        public AddressData Address { get; set; }

        [XmlElement("LeadObjects")]
        public List<LeadObjectData> LeadObjects { get; set; }

        [XmlElement("ContactsInfo")]
        public List<ContactInfoData> ContactsInfo { get; set; }

        [XmlElement("Tags")]
        public List<TagData> Tags { get; set; }

        [XmlElement("assets")]
        public List<AssetData> Assets { get; set; }

        [XmlElement("Documents")]
        public List<DocumentData> Documents { get; set; }

        [XmlElement("Relations")]
        public List<RelationData> Relations { get; set; }

        [XmlElement("OperationType")]
        public string OperationType { get; set; }
        
        [XmlElement("OperationDescription")]
        public string OperationDescription { get; set; }

        [XmlElement("ImageBase64")]
        public string ImageBase64 { get; set; }
    }

    
    public class AddressData
    {
        [XmlElement("CountryId")]
        public int? countryId { get; set; }

        [XmlElement("RegOrCityId")]
        public int? regOrCityId { get; set; }

        [XmlElement("DistrictOrStreet")]
        public string districtOrStreet { get; set; }
        
        [XmlElement("AdditionalInfo")]
        public string additionalInfo { get; set; }

    }

    public class CommentData
    {
        [XmlElement("Text")]
        public string Text { get; set; }

        [XmlElement("Creator")]
        public string Creator { get; set; }

        [XmlElement("FullName")]
        public string FullName { get; set; }
    }

    public class CallData
    {
        [XmlElement("calltimestamp")]
        public DateTime CallTimestamp { get; set; }

        [XmlElement("responsibleperson")]
        public string ResponsibleNumber { get; set; }

        [XmlElement("direction")]
        public string Direction { get; set; }
    }

    public class AssetData
    {
        [XmlElement("assetType")]
        public int AssetType { get; set; }

        [XmlElement("insuredObjectGuid")]
        public Guid InsuredObjectGuid { get; set; }

        [XmlElement("assetInfo")]
        public string AssetInfo { get; set; }

        [XmlElement("assetDescription")]
        public string AssetDescription { get; set; }

        [XmlElement("validTo")]
        public DateTime? ValidTo { get; set; }

        [XmlElement("validFrom")]
        public DateTime? ValidFrom { get; set; }
    }



    public class ContactInfoData
    {
        [XmlElement("Type")]
        public int Type { get; set; }

        [XmlElement("Value")]
        public string Value { get; set; }

        [XmlElement("Reason")]
        public int? Reason { get; set; }

        [XmlElement("ContactComment")]
        public List<CommentData> ContactComments { get; set; }

        [XmlElement("ContactCall")]
        public List<CallData> Calls { get; set; }
    }

    public class DocumentData
    {
        [XmlElement("DocumentNumber")]
        public string DocumentNumber { get; set; }

        [XmlElement("DocumentType")]
        public int DocumentType { get; set; }

        [XmlElement("DocumentExpireDate")]
        public DateTime? DocumentExpireDate { get; set; }
    }

    public class RelationData
    {

        [XmlElement("ClientINN")]
        public int? ClientINN { get; set; }

        [XmlElement("relationType")]
        public int RelationType { get; set; }

        [XmlElement("code")]
        public string Code { get; set; }

        [XmlElement("documentType")]
        public DocumentTypeEnum DocumentType { get; set; }
    }
    public class TagData
    {
        [XmlElement("Id")]
        public int? Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; } 
    }


    public class LeadObjectData
    {
        [XmlElement("DiscountPercentage")]
        public decimal DiscountPercentage { get; set; }

        [XmlElement("CampaignName")]
        public string CampaignName { get; set; }

        [XmlElement("Uniquekey")]
        public string UniqueKey { get; set; }

        [XmlElement("UserGuid")]
        public string UserGuid { get; set; }

        [XmlElement("Payload")]
        public string Payload { get; set; }

        [XmlElement("ObjectId")]
        public string ObjectId { get; set; }

        [XmlElement("ValidTo")]
        public DateTime ValidTo { get; set; }

        [XmlElement("ValidFrom")]
        public DateTime ValidFrom { get; set; }
    }


}
