using AutoMapper;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Cache;
using CRM.Operation.Extensions;
using CRM.Operation.Helpers;
using CRM.Operation.Models;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Zircon.Core.Config;
using Zircon.Core.Extensions;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class SubmitSurveyOperation : BusinessOperation<DealModel>, ISendEmailOperation
    {
        private readonly CRMDbContext _dbContext;
        private readonly IMapper _mapper;
        public string SurveyLink { get; set; }
        public string ShortenSurveyLink => ShortenLinkHelper.EncodeLink(SurveyLink);

        public SubmitSurveyOperation(CRMDbContext db, IMapper mapper) : base(db)
        {
            _dbContext = db;
            _mapper = mapper;
        }

        private int GetViewMode()
        {
            switch ((SurveyProcessEnum)Parameters.Survey.SurveyerType!)
            {
                case SurveyProcessEnum.ByExpert:
                    return 11;
                case SurveyProcessEnum.ByMediator:
                    return 10;
                case SurveyProcessEnum.ByClient:
                {
                    if ((OfferTypeEnum)Parameters.SelectedOfferType == OfferTypeEnum.VehicleInsurance)
                        return 13;
                    if ((OfferTypeEnum)Parameters.SelectedOfferType == OfferTypeEnum.PropertyInsurance)
                        return 12;
                    return 11;
                }
                default:
                    return 11;
            }
        }
        
        protected override void DoExecute()
        {
            if (Parameters.Survey.SurveyerType!.Value != (int)SurveyProcessEnum.NotNeeded)
            {
                var generateTokenRequest = new FileManagementGenerateTokenRequest
                {
                    ObjectType = "DealSurvey",
                    ViewMode = GetViewMode(),
                    ObjectIdentificationNumber = Parameters.DealNumber,
                    DocumentType =
                        SurveyDocTypesMapping.SurveyDocTypes.ContainsKey((OfferTypeEnum) Parameters.SelectedOfferType!)
                            ? SurveyDocTypesMapping.SurveyDocTypes[(OfferTypeEnum) Parameters.SelectedOfferType!]
                            : new List<string>()
                };

                if ((SurveyProcessEnum)Parameters.Survey.SurveyerType!.Value == SurveyProcessEnum.ByClient)
                    generateTokenRequest.DocumentType.Remove("Survey.VehicleOther". Translate());

                GenerateFileUploadTokenOperation generateFileUploadTokenOperation = new GenerateFileUploadTokenOperation();
                generateFileUploadTokenOperation.Execute(generateTokenRequest);

                var token = generateFileUploadTokenOperation.Response.Token;
            
                //Archive old files.
                ArchiveFilesOperation archiveFilesOperation = new ArchiveFilesOperation();
                archiveFilesOperation.Execute(token);

                SurveyLink = $"{Settings.GetString("BaseUrl").Replace("/Api","")}/FileManagement/Uploader/Index?token={token}";

                SurveyLink = ShortenLinkHelper.EncodeLink(SurveyLink);
            
                Parameters.Survey.SurveyLink = SurveyLink;
            }

            var deal = _dbContext.Deal.First(x=>x.DealGuid.Equals(Parameters.DealGuid));
            deal.UnderwriterUserGuid = Parameters.UnderwriterUser.UserGuid;
            deal.ResponsiblePersonType = (int)DealResponsiblePersonTypeEnum.Seller;
            deal.CurrentUserGuid = Parameters.CurrentUserGuid;
            deal.CurrentUserName = Parameters.CurrentUserName;
            if (Parameters.Survey.SurveyerType != (int)SurveyProcessEnum.NotNeeded)
                deal.DealStatus = (int)DealStatus.SurveySent;

            deal.Survey ??= _mapper.Map<Survey>(Parameters.Survey);
            deal.Survey.SurveyerType = (int)Parameters.Survey.SurveyerType;

            deal.Survey.Status = (SurveyProcessEnum) deal.Survey.SurveyerType switch
            {
                SurveyProcessEnum.NotNeeded => (int) SurveyStatusEnum.SurveyNotNeeded,
                _ => (int) SurveyStatusEnum.AwaitingSurvey
            };

            deal.Survey = deal.Survey;
            
            if(deal.Survey.SurveyerType != (int)SurveyProcessEnum.NotNeeded)
                SendSurveyLinkToRelatedPerson();

            if (!Result.Success)
                return;

            if(deal.Survey.SurveyerType != (int)SurveyProcessEnum.NotNeeded)
                Parameters.DealStatus = DealStatus.SurveySent;
            
            Parameters.ResponsiblePersonType = DealResponsiblePersonTypeEnum.Seller;
            Parameters.Survey.Status = deal.Survey.Status;
            
            _dbContext.SaveChanges();
        }

        protected override void ValidateParameters()
        {
            base.ValidateParameters();
            if (!Parameters.Survey.SurveyerType.HasValue)
            {
                Result.AddError(LocalizationExtensions.Translate("SubmitSurveyOperation.SurveyerRequired"));
            }
        }

        private void SendSurveyLinkToRelatedPerson()
        {
            switch ((SurveyProcessEnum) Parameters.Survey.SurveyerType!.Value)
            {
                case SurveyProcessEnum.ByMediator:
                    SendSurveyLinkToMediatorUser();
                    break;

                case SurveyProcessEnum.ByExpert:
                    SendSurveyLinkToExpert();
                    break;

                case SurveyProcessEnum.ByClient:
                    SendSurveyLinkToClient();
                    break;
            }
        }

        private void SendSurveyLinkToMediatorUser()
        {

            StringBuilder surveyBody = new StringBuilder();
            surveyBody.Append(LocalizationExtensions.Translate($"{Parameters.SelectedOfferType}_SurveyLinkForMediator.Body").ParametrizeExtended(Parameters.FormComponentModel).ParametrizeExtended(Parameters.FormComponentModel)
               .ParametrizeExtended(Parameters)
               .ParametrizeExtended(Parameters.Client)
               .ParametrizeExtended(Parameters.CreatedByUser)
               .ParametrizeExtended(ShortenSurveyLink));

            BodyGenerator(ref surveyBody);
            var emailModel = new EmailModel
            {
                Body = surveyBody.ToString(),
                From = Settings.GetString("MAILSERVER_MAIL_FROM"),
                To = Parameters.CreatedByUser?.UserEmail ?? "n/a",
                Subject = LocalizationExtensions.Translate("SurveyLink.Subject"),
                SystemOid = 8,
            };

            Result.Merge((this as ISendEmailOperation).SendEmail(emailModel));

            AddLinkToTheDiscussions();
        }

        private void SendSurveyLinkToExpert()
        {
            StringBuilder surveyBody = new StringBuilder();
            surveyBody.Append(LocalizationExtensions.Translate($"{Parameters.SelectedOfferType}_SurveyLinkForMediator.Body").ParametrizeExtended(Parameters.FormComponentModel)
               .ParametrizeExtended(Parameters)
               .ParametrizeExtended(Parameters.Client)
               .ParametrizeExtended(Parameters.CreatedByUser)
               .ParametrizeExtended(ShortenSurveyLink));
            BodyGenerator(ref surveyBody);

            var emailModel = new EmailModel
            {
                Body = surveyBody.ToString(),
                From = Settings.GetString("MAILSERVER_MAIL_FROM"),
                To = Settings.GetString("SurveyerGroupEmail"),
                Subject = LocalizationExtensions.Translate("SurveyLink.Subject"),
                SystemOid = 8,
            };


            Result.Merge((this as ISendEmailOperation).SendEmail(emailModel));
        }
        
        private void BodyGenerator(ref StringBuilder surveyBody)
        {
            VehicleInsuranceOfferModel model = Parameters.FormComponentModel as VehicleInsuranceOfferModel;
            if (model != null && Parameters.SelectedOfferType == 1)
            {
                foreach (var item in model.Vehicles)
                {
                    surveyBody.Append(string.Format("\r\nNV-in markası və modeli : {0} {1}\r\nNV-in DQN-ı : {2}\r\nNV-in buraxılış ili : {3}\r\n", item.VehBrand, item.VehModel, item.VehicleRegNumber, item.ManufactureYear));
                }
            }
        }

        private void SendSurveyLinkToClient()
        {
            using (HttpClient httpClient = HttpClientHelper.GetHttpClientForWebIms())
            {
                var messageModel = new MessageModel
                {
                    SenderSystem = 8,
                    Recepient = Parameters.SurveyContactInfo,
                    MsBody = string.Format(LocalizationExtensions.Translate("SurveyLink.Body"), ShortenSurveyLink)
                };

                var requestUrl = "Messaging/SendSMS";

                var response = Zircon.Core.HttpContextHelper.HttpClientHelper
                    .PostJsonAsync<MessageModel, object>(httpClient, requestUrl, messageModel).Result;

                Result.Merge(response);
                
                AddLinkToTheDiscussions();
            }
        }

        private void AddLinkToTheDiscussions()
        {
            var generateTokenRequest = new FileManagementGenerateTokenRequest
            {
                ObjectType = "DealSurvey",
                ViewMode = 10,
                ObjectIdentificationNumber = Parameters.DealNumber,
                DocumentType =
                    SurveyDocTypesMapping.SurveyDocTypes.ContainsKey((OfferTypeEnum) Parameters.SelectedOfferType!)
                        ? SurveyDocTypesMapping.SurveyDocTypes[(OfferTypeEnum) Parameters.SelectedOfferType!]
                        : new List<string>()
            };


            GenerateFileUploadTokenOperation generateFileUploadTokenOperation = new GenerateFileUploadTokenOperation();
            generateFileUploadTokenOperation.Execute(generateTokenRequest);

            var token = generateFileUploadTokenOperation.Response.Token;

            var surveyLinkForMediator = $"{Settings.GetString("BaseUrl").Replace("/Api","")}/FileManagement/Uploader/Index?token={token}";

            surveyLinkForMediator = ShortenLinkHelper.EncodeLink(surveyLinkForMediator);
            
            var surveyBody = string.Format(LocalizationExtensions.Translate("SurveyLink.Body"), surveyLinkForMediator);

            var deal = _dbContext.Deal.Find(Parameters.DealGuid);
            
            deal.Discussion.Add(new Discussion
            {
                Content = surveyBody,
                DateTime = DateTime.Now,
                ReceiverGuid = deal.CreatedByUserGuid,
                SenderGuid = Parameters.UnderwriterUser.UserGuid
            });
            
            var discussion = deal.Discussion.Last();
            var discussionModel = _mapper.Map<DiscussionViewModel>(discussion);
            discussionModel.Sender = InMemoryCache.GetUser(discussion.SenderGuid);
                    
            if(discussion.ReceiverGuid.HasValue)
                discussionModel.Receiver = InMemoryCache.GetUser(discussion.ReceiverGuid.Value);
            
            Parameters.DealDiscussionModel.DiscussionHistory.Add(discussionModel);
            
            Parameters.DealDiscussionModel.DiscussionHistory = Parameters.DealDiscussionModel.DiscussionHistory.OrderByDescending(x=>x.DateTime).ToList();
        }

        protected override void DoFinalize()
        {
            base.DoFinalize();
            if(this.Result.Success)
                Result.AddSuccess(LocalizationExtensions.Translate("MessageBox.SubmitSurveySuccess"));
        }
    }
}