using AutoMapper;
using CRM.Data.Database;
using CRM.Operation.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Zircon.Core.Authorization;
using Zircon.Core.OperationModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CRM.Operation.Models.DealOfferModels;

namespace CRM.Operation
{
    public class GetScopedUserHavingDealsOperation : BusinessOperation<GetScopeUsersRequestModel>
    {
        private CRMDbContext _context;
        private readonly IMapper _mapper;
        public IQueryable<DealListItem> Deals { get; set; }
        List<Deal> dealList = new List<Deal>();
        public GetScopedUserHavingDealsOperation(CRMDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _context = dbContext;
            _mapper = mapper;
        }

        protected override void DoExecute()
        {
            if (Parameters.IsSeller)
            {
                GetScopeUserGuidsOperation unitUserGuidsOperation = new GetScopeUserGuidsOperation(_context);
                unitUserGuidsOperation.Execute(Parameters);


                foreach (var item in unitUserGuidsOperation.UserGuids_)
                {
                    dealList.AddRange(_context.Deal.Where(x => x.CreatedByUserGuid.ToString() == item.UserGuid).ToList());
                }
            }
            else
            {
                dealList = (from
                       deal in _context.Deal
                            select deal).ToList();
            }   
            

             if (Parameters.ClientInn.HasValue)
            {
                dealList = dealList.Where(d => d.ClientInn == Parameters.ClientInn).ToList();
            }

            if (!string.IsNullOrEmpty(Parameters.FullName))
            {
                dealList = dealList.Where(d =>
                    d.CreatedByUserFullName != null && d.CreatedByUserFullName.Contains(Parameters.FullName)).ToList();
            }

            if (!string.IsNullOrEmpty(Parameters.DiscussionText))
            {
                dealList = dealList.Where(d =>
                    d.Discussion != null && d.Discussion.Any(di =>
                        di.Content != null && di.Content.Contains(Parameters.DiscussionText))).ToList();
            }

            if (!string.IsNullOrEmpty(Parameters.MediatorUserName))
            {
                dealList = dealList.Where(d =>
                    d.CreatedByUserFullName != null && d.CreatedByUserFullName.Contains(Parameters.MediatorUserName))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(Parameters.UnderwriterUserName))
            {
                dealList = dealList.Where(d =>
                    d.UnderwriterUserFullName != null && d.UnderwriterUserFullName.Contains(Parameters.UnderwriterUserName))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(Parameters.PinNumber))
            {
                dealList = dealList.Where(d =>
                    d.ClientInnNavigation.PhysicalPeople.Any(pp => pp.Pin == Parameters.PinNumber))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(Parameters.DealNumber))
            {
                dealList = dealList.Where(d => d.DealNumber != null && d.DealNumber == Parameters.DealNumber)
                    .ToList();
            }

            if (Parameters.DealStatus > -1)
            {
                dealList = dealList.Where(d => (int)d.DealStatus == Parameters.DealStatus).ToList();
            }

            if (Parameters.DealType > -1)
            {
                dealList = dealList.Where(d => (int)d.DealType == Parameters.DealType).ToList();
            }

            Deals  = _mapper.Map<List<DealListItem>>(dealList).AsQueryable();
        }

    }
}
