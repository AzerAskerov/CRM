using System;
using Zircon.Core.Localization;
using CRM.Data.Enums;
using CRM.Data.Models;
using CRM.Operation.Attributes;


namespace CRM.Operation.Models
{
    public class ProductBaseModel
    {
        public string CuratorName { get; set; }
        public string CuratorSurname { get; set; }
        public Guid? PolicyActionGuid { get; set; }
        public Guid? ClientGuid { get; set; }
        [DisplayNameLocalized("ProductBaseModel.ClientFullName")]
        public string ClientFullName { get; set; }
        [DisplayNameLocalized("ProductBaseModel.PolicyNumber")]
        public string PolicyNumber { get; set; }
        [DisplayNameLocalized("ProductBaseModel.Status")]
        public string Status { get; set; }
        [DisplayNameLocalized("ProductBaseModel.StartDate")]
        public DateTime StartDate { get; set; }
        [DisplayNameLocalized("ProductBaseModel.EndDate")]
        public DateTime EndDate { get; set; }
       
        public LobCode Product { get; set; }
        [DisplayNameLocalized("ProductBaseModel.CreatedDate")]
        public DateTime CreatedDate { get; set; }
        public PolicyStatusLocal PolicyStatusLocal { get; set; }
        public InvoiceStatusCode InvoiceStatusCode { get; set; }
        [DisplayNameLocalized("ProductBaseModel.ProductName")]
        public string ProductName { get; set; }
        [DisplayNameLocalized("ProductBaseModel.ArchiveStatus")]
        public string ArchiveStatus { get; set; }
        [DisplayNameLocalized("ProductBaseModel.AddendumCount")]
        public int AddendumCount { get; set; }

        public ProductBaseModel()
        {

        }

        public ProductBaseModel(Policy source)
        {
            CuratorName = source.CuratorName;
            CuratorSurname = source.CuratorSurname;
            ClientGuid = source.ClientGuid;
            ClientFullName = source.ClientFullName;
            PolicyNumber = source.PolicyNumber;
            StartDate = source.StartDate;
            EndDate = source.EndDate;
            Product = (LobCode)source.Product;
            CreatedDate = source.CreatedDate;
            PolicyActionGuid = source.PolicyActionGuid;
            Status = GetPolicyStatus(source.PolicyStatus, source.PolicyActionStatus,
                source.Type, StartDate, EndDate, null);
            InvoiceStatusCode = source.InvStatusCode;
            ProductName = source.LobName;
            PolicyStatusLocal = GetPolicyStatusLocal(source.PolicyStatus, source.PolicyActionStatus,
                source.Type, StartDate, EndDate, null);
            ArchiveStatus = GetArchiveStatus(source.ArchiveStatus);
            AddendumCount = source.AddendumCount;
        }

        private string GetPolicyStatus(
           PolicyStatus policyStatus,
            PolicyActionStatus policyActionStatus,
            PolicyAction policyAction,
            DateTime actionStartDate,
            DateTime actionEndDate,
            DateTime? discontinueDate)
        {
            return GetPolicyStatusString(
                policyStatus,
                policyActionStatus,
                policyAction,
                actionStartDate,
                actionEndDate,
                discontinueDate,
                DateTime.Now);
        }

        private static string GetPolicyStatusString(
            PolicyStatus policyStatus,
            PolicyActionStatus policyActionStatus,
            PolicyAction policyAction,
            DateTime actionStartDate,
            DateTime actionEndDate,
            DateTime? discontinueDate,
            DateTime currentDate)
        {
            string statusString = string.Empty;

            switch (policyAction)
            {
                case PolicyAction.PolicyIssuance:
                case PolicyAction.Renewal:
                case PolicyAction.DuplicateIssuance:
                    {
                        switch (policyStatus)
                        {
                            case PolicyStatus.Draft:
                                {
                                    if (policyAction == PolicyAction.Renewal)
                                    {
                                        statusString = DBResources.GetText("EA0008261", "Atjaunojuma sagatave");
                                        //extendedLabel = DBResources.GetText("EA0008260", "Atjaunojuma sagatave ({0}) (Pol. {1})");
                                    }
                                    else
                                    {
                                        statusString = DBResources.GetText("EA0008259", "Polises sagatave");
                                        //extendedLabel = DBResources.GetText("EA0008258", "Polises sagatave ({0})");
                                    }
                                    break;
                                }
                            case PolicyStatus.Issued:
                                {
                                    if (policyActionStatus == PolicyActionStatus.PreIssued)
                                    {
                                        statusString = DBResources.GetText("EA0022442", "Pre-issued");
                                    }
                                    else if (actionStartDate > currentDate)
                                    {
                                        statusString = DBResources.GetText("EA0008189", "Polise izdota");
                                    }
                                    else
                                        if (discontinueDate.HasValue)
                                    {
                                        statusString = "Polise izbeigta";
                                    }
                                    else
                                            if (actionEndDate < currentDate)
                                    {
                                        statusString = DBResources.GetText("EA0008188", "Polise beigusies");
                                    }
                                    else
                                    {
                                        statusString = DBResources.GetText("EA0008187", "Polise spēkā");
                                    }
                                    break;
                                }
                            case PolicyStatus.Annulled:
                                {
                                    if (policyActionStatus == PolicyActionStatus.Rejected)
                                        statusString = DBResources.GetText("EA0022443", "Wasn't issued");
                                    else
                                        statusString = DBResources.GetText("EA0008186", "Polise anulēta");

                                    break;
                                }
                            case PolicyStatus.Terminated:
                                {
                                    statusString = DBResources.GetText("EA0008185", "Polise pārtraukta");
                                    break;
                                }
                        }
                        break;
                    }
                case PolicyAction.AppendixIssuance:
                    {
                        switch (policyActionStatus)
                        {
                            case PolicyActionStatus.Draft:
                                {
                                    statusString = DBResources.GetText("EA0008377", "Pielikuma sagatave");
                                    //string extendedLabel = DBResources.GetText("EA0008376", "Pielikuma sagatave ({0}) (Pol. {1})");
                                    break;
                                }
                            case PolicyActionStatus.PreIssued:
                                {
                                    statusString = DBResources.GetText("EA0022442", "Pre-issued");
                                    break;
                                }
                            case PolicyActionStatus.Issued:
                                {
                                    if (actionStartDate > currentDate)
                                    {
                                        statusString = DBResources.GetText("EA0008184", "Pielikums izdots");
                                    }
                                    else
                                        if (discontinueDate.HasValue)
                                    {
                                        statusString = "Pielikums izbeigts";
                                    }
                                    else
                                            if (actionEndDate < currentDate)
                                    {
                                        statusString = DBResources.GetText("EA0008183", "Pielikums beidzies");
                                    }
                                    else
                                    {
                                        statusString = DBResources.GetText("EA0008182", "Pielikums spēkā");
                                    }
                                    break;
                                }
                            case PolicyActionStatus.Annuled:
                                {
                                    statusString = DBResources.GetText("EA0008181", "Pielikums anulēts");
                                    break;
                                }
                            case PolicyActionStatus.Terminated:
                                {
                                    statusString = DBResources.GetText("EA0008180", "Pielikums pārtraukts");
                                    break;
                                }
                        }
                        break;
                    }
                case PolicyAction.Annulment:
                    {
                        statusString = DBResources.GetText("EA0008179", "Anulēšanas pielikums");
                        break;
                    }
                case PolicyAction.Termination:
                case PolicyAction.AppendixTermination:
                case PolicyAction.CaseTermination:
                    {
                        statusString = DBResources.GetText("EA0008178", "Pārtraukšanas pielikums");
                        break;
                    }
            }
            return statusString;
        }


        private PolicyStatusLocal GetPolicyStatusLocal(
           PolicyStatus policyStatus,
           PolicyActionStatus policyActionStatus,
           PolicyAction policyAction,
           DateTime actionStartDate,
           DateTime actionEndDate,
           DateTime? discontinueDate)
        {
            return GetPolicyStatus(
                policyStatus,
                policyActionStatus,
                policyAction,
                actionStartDate,
                actionEndDate,
                discontinueDate,
               DateTime.Now,
                null,
                null);
        }

        private PolicyStatusLocal GetPolicyStatus(
           PolicyStatus policyStatus,
           PolicyActionStatus policyActionStatus,
           PolicyAction policyAction,
           DateTime actionStartDate,
           DateTime actionEndDate,
           DateTime? discontinueDate,
           DateTime currentDate,
           string draftNumber,
           string policyNumber)
        {
            PolicyStatusLocal lStatus = PolicyStatusLocal.Draft;

            switch (policyAction)
            {
                case PolicyAction.PolicyIssuance:
                case PolicyAction.Renewal:
                case PolicyAction.DuplicateIssuance:
                    {
                        switch (policyStatus)
                        {
                            case PolicyStatus.Issued:
                                {
                                    if (policyActionStatus == PolicyActionStatus.PreIssued)
                                    {
                                        lStatus = PolicyStatusLocal.PreIssued;
                                    }
                                    else if (actionStartDate > currentDate)
                                    {
                                        lStatus = PolicyStatusLocal.Issued;
                                    }

                                    else
                                            if (actionEndDate < currentDate)
                                    {
                                        lStatus = PolicyStatusLocal.Expired;
                                    }
                                    else
                                    {
                                        lStatus = PolicyStatusLocal.InForce;
                                    }
                                    break;
                                }
                            case PolicyStatus.Annulled:
                                {
                                    if (policyActionStatus == PolicyActionStatus.Rejected)
                                        lStatus = PolicyStatusLocal.Rejected;
                                    else
                                        lStatus = PolicyStatusLocal.Annulled;

                                    break;
                                }
                            case PolicyStatus.Terminated:
                                {
                                    lStatus = PolicyStatusLocal.Terminated;
                                    break;
                                }
                        }
                        break;
                    }
                case PolicyAction.AppendixIssuance:
                    {
                        switch (policyActionStatus)
                        {
                            case PolicyActionStatus.PreIssued:
                                {
                                    lStatus = PolicyStatusLocal.PreIssued;
                                    break;
                                }
                            case PolicyActionStatus.Issued:
                                {
                                    if (actionStartDate > currentDate)
                                    {
                                        lStatus = PolicyStatusLocal.Issued;
                                    }
                                    else

                                            if (actionEndDate < currentDate)
                                    {
                                        lStatus = PolicyStatusLocal.Expired;
                                    }
                                    else
                                    {
                                        lStatus = PolicyStatusLocal.InForce;
                                    }
                                    break;
                                }
                            case PolicyActionStatus.Annuled:
                                {
                                    lStatus = PolicyStatusLocal.Annulled;
                                    break;
                                }
                            case PolicyActionStatus.Terminated:
                                {
                                    lStatus = PolicyStatusLocal.Terminated;
                                    break;
                                }
                        }
                        break;
                    }
                case PolicyAction.Annulment:
                    {
                        lStatus = PolicyStatusLocal.Annulled;
                        break;
                    }
                case PolicyAction.Termination:
                case PolicyAction.AppendixTermination:
                case PolicyAction.CaseTermination:
                    {
                        lStatus = PolicyStatusLocal.Terminated;
                        break;
                    }
            }

            return lStatus;
        }

        private string  GetArchiveStatus(ArchiveStatusEnum? archiveStatus)
        {
            string statusString = string.Empty;

            switch (archiveStatus)
            {
                case ArchiveStatusEnum.deleted:
                    {
                        statusString = DBResources.GetText("ArchiveStatusFalse", "Arxivdə yoxdur"); ;
                        break;
                    }
                case ArchiveStatusEnum.none:
                    {
                        statusString = DBResources.GetText("ArchiveStatusFalse", "Arxivdə yoxdur"); ;
                        break;
                    }
                case ArchiveStatusEnum.notFound:
                    {
                        statusString = DBResources.GetText("ArchiveStatusFalse", "Arxivdə yoxdur"); ;
                        break;
                    }
                case ArchiveStatusEnum.found:
                    {
                        statusString = DBResources.GetText("ArchiveStatusTrue", "Arxivdədir"); ;
                        break;
                    }
            }

            return statusString;
        }
    }
}
