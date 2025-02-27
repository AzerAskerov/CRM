using System;
using System.Text.Json;
using AutoMapper;
using CRM.Data.Database;
using CRM.Operation;
using CRM.Operation.JsonConverters;
using CRM.Operation.Models;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.RequestModels;
using CRM.Server.Attributes;
using Microsoft.AspNetCore.Mvc;
using Zircon.Core.Authorization;
using Zircon.Core.Authorization.SystemProvider;
using Zircon.Core.HttpContextHelper;

namespace CRM.Server.Controllers
{
    public class DealController : BaseController
    {
        private readonly IMapper _mapper;

        public DealController(IMapper mapper, CRMDbContext dbContext)
        {
            _mapper = mapper;
            DbContext = dbContext;
        }

        [HttpGet("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetDealByDealGuid(Guid dealGuid)
        {
            var op = new LoadDealByDealGuidOperation(DbContext, _mapper);
            op.Execute(dealGuid);
            return new JsonResult (op.DealModel,new JsonSerializerOptions(){Converters = { new DealOfferFormJsonConverter() }});
        }

        [HttpGet("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetDealByDealNumber(string dealNumber)
        {
            var op = new LoadDealByDealNumberOperation(DbContext, _mapper);
            op.Execute(dealNumber);
            return new JsonResult (op.DealModel,new JsonSerializerOptions(){Converters = { new DealOfferFormJsonConverter() }});
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetClientDeals([FromBody] int inn)
        {
            var op = new LoadDealByClientInnOperation(DbContext, _mapper);
            op.Execute(inn);
            return OperationResult(op.Result, op.DealModels);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult SaveDealAsDraft(DealModel model)
        {
            model.CurrentUserName = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserName;
            model.CurrentUserGuid = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserGuid;
            var op = new SaveDealAsDraftOperation(DbContext, _mapper);
            var operationResult = op.Execute(model);
            return OperationResult(operationResult, model);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult SendDealToUnderWriter(DealModel model)
        {
            model.CurrentUserName = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserName;
            model.CurrentUserGuid = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserGuid;
            var op = new SendDealToUnderWriterOperation(DbContext, _mapper);
            var operationResult = op.Execute(model);
            return OperationResult(operationResult, model);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult SubmitDiscussion(DiscussionViewModel model)
        {
            model.CurrentUserName = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserName;
            model.CurrentUserGuid = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserGuid;
            var op = new DiscussionSubmitOperation(DbContext, _mapper);
            var operationResult = op.Execute(model);
            return OperationResult(operationResult);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult SubmitOffers(DealModel model)
        {
            model.CurrentUserName = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserName;
            model.CurrentUserGuid = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserGuid;
            var op = new SubmitOffersOperation(DbContext, _mapper);
            var operationResult = op.Execute(model);
            return OperationResult(operationResult);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult ChangeDealStatus(ChangeDealStatusRequest request)
        {
            request.CurrentUserName = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserName;
            request.CurrentUserGuid = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserGuid;
            var op = new ChangeDealStatusOperation(DbContext);
            var operationResult = op.Execute(request);
            return OperationResult(operationResult);
        }

        [HttpGet("[action]")]
        [WebApiAuthenticate]
        public IActionResult SubmitAgreedOffer(string offerNumber)
        {
            var op = new SubmitAgreedOfferOperation(DbContext);
            var operationResult = op.Execute(new SubmitAgreedOfferModel { 
                OfferNumber = offerNumber,
                CurrentUserName= Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserName,
                CurrentUserGuid = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserGuid

            });
            return OperationResult(operationResult);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult LinkPolicyToDeal(DealPolicyLinkViewModel model)
        {
            model.CurrentUserName = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserName;
            model.CurrentUserGuid = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserGuid;
            var op = new LinkPolicyToDealOperation(DbContext,_mapper);
            var operationResult = op.Execute(model);
            return OperationResult(operationResult);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult MarkUserMessagesAsRead(MarkUserDealMessagesAsReadModel model)
        {
            var op = new MarkUserDealMessagesAsReadOperation(DbContext,_mapper);
            var operationResult = op.Execute(model);
            return OperationResult(operationResult);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult SubmitSurvey(DealModel model)
        {
            model.CurrentUserName = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserName;
            model.CurrentUserGuid = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserGuid;
            var op = new SubmitSurveyOperation(DbContext, _mapper);
            var operationResult = op.Execute(model);
            return OperationResult(operationResult, model);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult UpdateSurvey(UpdateSurveyRequestModel model)
        {
            model.CurrentUserName = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserName;
            model.CurrentUserGuid = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserGuid;
            var op = new UpdateSurveyOperation(DbContext, _mapper);
            var operationResult = op.Execute(model);
            return OperationResult(operationResult);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult SetUnderwriterForDeal(DealModel model)
        {
            model.CurrentUserName = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserName;
            model.CurrentUserGuid = Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER).UserGuid;
            var op = new SetUnderwriterForDealOperation(DbContext, _mapper);
            var operationResult = op.Execute(model);
            return OperationResult(operationResult);
        }




        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetDealSPecificInfo(DealOfferDropdownDataModel model)
        {
            var op = new GetDealGeneralSourceDataInfo(DbContext);
          op.Execute(model);
            return OperationResult(op.Result, op.Data);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetMovableNImmovableProperty(DealOfferDropdownDataModel model)
        {
            var op = new GetPropertySourceDataInfo(DbContext);
            op.Execute(model);
            return OperationResult(op.Result, op.Data);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetCountryNCity(DealOfferDropdownDataModel model)
        {
            var op = new GetPropertySourceDataInfo(DbContext);
            op.Execute(model);
            return OperationResult(op.Result, op.Data);
        }


        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetDealVehicleInfo(DealOfferDropdownDataModel model)
        {
            var op = new GetVehicleInsDealDataInfo(DbContext);
            op.Execute(model);
            return OperationResult(op.Result, op.Data);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetVehiclesForInsurance([FromBody]Guid dealGuid)
        {
            var op = new GetVehiclesForInsuranceOperation(DbContext, _mapper);
            op.Execute(dealGuid);
            return OperationResult(op.Result, op.Vehicles);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetVehiclesForCarAndEquipInsurance([FromBody] Guid dealGuid)
        {
            var op = new GetVehiclesAndEquipForInsuranceOperation(DbContext, _mapper);
            op.Execute(dealGuid);
            return OperationResult(op.Result, op.Vehicles);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetVolunteerDataInsurance(DealOfferDropdownDataModel model)
        {
            var op = new GetVolunteerDataOperation(DbContext, _mapper);
            op.Execute(model);
            return OperationResult(op.Result, op.Data);
        }


        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetCargoDataInsurance(DealOfferDropdownDataModel model)
        {
            var op = new GetCargoDataOperation(DbContext, _mapper);
            op.Execute(model);
            return OperationResult(op.Result, op.Data);
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetScopeUserGuids(GetScopeUsersRequestModel request)
        {
            var unitUserGuidsOperation = new GetScopeUserGuidsOperation(DbContext);
            unitUserGuidsOperation.Execute(request);

            return new JsonResult(new DealScopeModel
            {

                Guids = unitUserGuidsOperation.UserGuids_,
            }, new JsonSerializerOptions() { Converters = { new DealOfferFormJsonConverter() } });
        }

        [HttpGet("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetOrgazinationUserGuids()
        {
            var unitUserGuidsOperation = new GetOrganizationUserGuidsOperation(DbContext);
            unitUserGuidsOperation.Execute();

            return new JsonResult(new DealScopeModel
            {

                Guids = unitUserGuidsOperation.UserGuids_,
            }, new JsonSerializerOptions() { Converters = { new DealOfferFormJsonConverter() } });
        }

        [HttpPost("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetScopedUserHavingDealsOperation(GetScopeUsersRequestModel requestComing)
        {
           
           var dealOperation = new GetScopedUserHavingDealsOperation(DbContext, _mapper);
            dealOperation.Execute(requestComing);


            return OperationResult(dealOperation.Result, dealOperation.Deals);
        }


    }
}