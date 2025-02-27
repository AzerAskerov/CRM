using AutoMapper;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Cache;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.RequestModels;
using Zircon.Core.Config;
using Zircon.Core.Extensions;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class DiscussionSubmitOperation : BusinessOperation<DiscussionViewModel>, ISendEmailOperation
    {
        private readonly CRMDbContext _dbContext;
        private readonly IMapper _mapper;
        private Deal _deal;
    
        public DiscussionSubmitOperation(CRMDbContext db, IMapper mapper) : base(db)
        {
            _dbContext = db;
            _mapper = mapper;
        }

        protected override void DoExecute()
        {
            _deal = _dbContext.Deal.Find(Parameters.DealGuid);

            if (_deal is null)
            {
                Result.AddError($"Deal {Parameters.DealGuid} is not found");
                return;
            }

            _deal.ResponsiblePersonType = _deal.CreatedByUserGuid == Parameters.Sender.UserGuid
                ? (int) DealResponsiblePersonTypeEnum.Underwriter
                : (int) DealResponsiblePersonTypeEnum.Seller;
            _deal.CurrentUserGuid = Parameters.CurrentUserGuid;
            _deal.CurrentUserName = Parameters.CurrentUserName;
            SendEmailAboutDiscussionUpdate();
            
            var discussion = _mapper.Map<Discussion>(Parameters);

            _deal.Discussion.Add(discussion);

            _dbContext.SaveChanges();
        }

        private void SendEmailAboutDiscussionUpdate()
        {
            var baseUrl = Settings.GetString("CRM_BASE_URL");
            var underwriterEmail = _deal.UnderwriterUserGuid.HasValue
                ? InMemoryCache.GetUser(_deal.UnderwriterUserGuid.Value).UserEmail
                : Settings.GetString("UNDERWRITER_GROUP_MAIL");
            
            var emailModel = new EmailModel
            {
                Body = string.Format("SendEmailAboutDiscussion.Body".Translate(), _deal.CreatedByUserFullName, _deal.UnderwriterUserFullName, Parameters.Content),
                From = Settings.GetString("MAILSERVER_MAIL_FROM"),
                To = Parameters.SenderFullname == _deal.CreatedByUserFullName ? underwriterEmail : InMemoryCache.GetUser(_deal.CreatedByUserGuid).UserEmail,
                Subject = string.Format("SendEmailAboutDiscussion.Subject".Translate(), _deal.DealNumber),
                SystemOid = 8,
                IsBodyHtml=true
            };

            Result.Merge((this as ISendEmailOperation).SendEmail(emailModel));
        }
    }
}