using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AutoMapper;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Helpers;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using Zircon.Core.Config;
using Zircon.Core.Extensions;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class UpdateSurveyOperation: BusinessOperation<UpdateSurveyRequestModel>
    {
        private readonly CRMDbContext _dbContext;
        private readonly IMapper _mapper;
        private Deal _deal;

        public UpdateSurveyOperation(CRMDbContext db, IMapper mapper) : base(db)
        {
            _dbContext = db;
            _mapper = mapper;
        }

        protected override void DoExecute()
        {
            _deal = _dbContext.Deal.SingleOrDefault(x=>x.DealNumber.Equals(Parameters.DealNumber));
            
            if(_deal is null)
            {
                Result.AddError($"Deal not found in db : {Parameters.DealNumber}");
                return;
            }

            _deal.Survey.Status = (int)SurveyStatusEnum.Successful;
            _deal.Survey.Description = Parameters.Description;
            _deal.Survey.SurveyerLogonName = Parameters.SurveyerLogonName;
            _deal.ResponsiblePersonType = (int)DealResponsiblePersonTypeEnum.Underwriter;
            _deal.CurrentUserGuid = Parameters.CurrentUserGuid;
            _deal.CurrentUserName = Parameters.CurrentUserName;
            _deal.DealStatus = (int)DealStatus.PendingUnderwriting;

            PutMessageToDiscussion();
            
            _dbContext.SaveChanges();
        }

        private void SendSurveyToSurveyer()
        {
            var generateTokenRequest = new FileManagementGenerateTokenRequest
            {
                ObjectType = "DealSurvey",
                ViewMode = 11,
                ObjectIdentificationNumber = Parameters.DealNumber,
                DocumentType =
                    SurveyDocTypesMapping.SurveyDocTypes.ContainsKey((OfferTypeEnum) _deal.DealType!)
                        ? SurveyDocTypesMapping.SurveyDocTypes[(OfferTypeEnum) _deal.DealType!]
                        : new List<string>()
            };

            GenerateFileUploadTokenOperation generateFileUploadTokenOperation = new GenerateFileUploadTokenOperation();
            generateFileUploadTokenOperation.Execute(generateTokenRequest);

            var token = generateFileUploadTokenOperation.Response.Token;

            var surveyLink = $"{Settings.GetString("BaseUrl").Replace("/Api","")}/FileManagement/Uploader/Index?token={token}";

            var surveyLinkShorten = ShortenLinkHelper.EncodeLink(surveyLink);
            
            using (HttpClient httpClient = HttpClientHelper.GetHttpClientForWebIms())
            {
                var emailModel = new EmailModel
                {
                    Body = string.Format("SurveyLink.Body".Translate(), surveyLinkShorten),
                    From = "",
                    To = Settings.GetString("SurveyerGroupEmail"),
                    Subject = "SurveyLink.Subject".Translate(),
                    SystemOid = 8,
                };

                var requestUrl = "Messaging/SendEmail";

                var response = Zircon.Core.HttpContextHelper.HttpClientHelper
                    .PostJsonAsync<EmailModel, object>(httpClient, requestUrl, emailModel).Result;

                Result.Merge(response);
            }

            _deal.Survey.SurveyLink = surveyLink;
        }

        private void PutMessageToDiscussion()
        {
            _deal.Discussion.Add(new Discussion
            {
                DateTime = DateTime.UtcNow,
                DealGuid = _deal.DealGuid,
                SenderGuid = _deal.CreatedByUserGuid,
                ReceiverGuid = _deal.UnderwriterUserGuid,
                Content = "UpdateSurveyOperation.SurveyCompleted".Translate()
            });
        }
    }
}