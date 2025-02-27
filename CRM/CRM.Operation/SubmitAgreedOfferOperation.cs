using System.Collections.Generic;
using System.Linq;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using Zircon.Core.Config;
using Zircon.Core.Extensions;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class SubmitAgreedOfferOperation : BusinessOperation<SubmitAgreedOfferModel>, ISendEmailOperation
    {
        private readonly CRMDbContext _crmDbContext;
        private Deal _deal;
        public SubmitAgreedOfferOperation(DbContext db) : base(db)
        {
            _crmDbContext = (CRMDbContext)db;
        }

        protected override void DoExecute()
        {
            _deal = _crmDbContext.Offer.AsTracking().FirstOrDefault(x => x.OfferNumber.Equals(Parameters.OfferNumber))?.Deal;

            if (_deal is null)
            {
                Result.AddError($"Offer not found with number {Parameters.OfferNumber}");
                return;
            }

            if (_deal.DealStatus != (int)DealStatus.Offered)
            {
                Result.AddError("For this operation, Deal must be in 'Offered' status.");
                return;
            }

            foreach (var offer in _deal.Offer)
            {
                offer.IsAgreed = offer.OfferNumber == Parameters.OfferNumber;
            }

            _deal.DealStatus = (int)DealStatus.Agreed;
            _deal.ResponsiblePersonType = (int)DealResponsiblePersonTypeEnum.Underwriter;
            _deal.CurrentUserGuid = Parameters.CurrentUserGuid;
            _deal.CurrentUserName = Parameters.CurrentUserName;
            SendEmailToUnderwriter();
            SendEmailToOperu();
            
            _crmDbContext.SaveChanges();
        }
        
        private void SendEmailToUnderwriter()
        {
            var baseUrl = Settings.GetString("CRM_BASE_URL");
            
            var emailModel = new EmailModel
            {
                Body = string.Format("SendEmailToUnderwriting.Body".Translate(), _deal.DealNumber, $"{baseUrl}opendeal/{_deal.DealNumber}", ""),
                From = Settings.GetString("MAILSERVER_MAIL_FROM"),
                To = Settings.GetString("UNDERWRITER_GROUP_MAIL"),
                Subject = "SendEmailToUnderwriting.Subject".Translate(),
                SystemOid = 8,
            };

            Result.Merge((this as ISendEmailOperation).SendEmail(emailModel));
        }
        
        private void SendEmailToOperu()
        {
            var offerType = (OfferTypeEnum)_deal.DealType;
            
            var emailModel = new EmailModel
            {
                Body = string.Format("SendEmailToOperu.Body".Translate(), _deal.CreatedByUserFullName,
                    _deal.UnderwriterUserFullName, offerType.ToString().Translate(),
                    CreateOpenDealLink()),
                From = Settings.GetString("MAILSERVER_MAIL_FROM"),
                To = Settings.GetString("OPERU_MAIL"),
                Subject = "SendEmailToOperu.Subject".Translate(),
                SystemOid = 8,
            };

            Result.Merge((this as ISendEmailOperation).SendEmail(emailModel));
        }

        private string GetFileDownloadLink()
        {
            var baseUrl = Settings.GetString("CRM_BASE_URL");

            FileManagementGenerateTokenRequest requestModel = new FileManagementGenerateTokenRequest
            {
                DocumentType = new List<string>{"Other"}, // Other
                ObjectType = "DEALS",
                ObjectIdentificationNumber = _deal.DealGuid.ToString(), // Deal number
                ViewMode = 10
            };

            var generateFileUploadTokenOperation = new GenerateFileUploadTokenOperation();
            generateFileUploadTokenOperation.Execute(requestModel);

            return $"{baseUrl}api/filemanagement/downloadattachmentviatoken?token={generateFileUploadTokenOperation.Response}";
        }


        private string CreateOpenDealLink()
        {
            var baseUrl = Settings.GetString("CRM_BASE_URL");

            return $"{baseUrl}opendeal/{_deal.DealNumber}";
        }
    }
}