using System.Collections.Generic;
using CRM.Data.Database;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.DealOfferModels
{
    public class DealDiscussionModel : IModel
    {
        public DealDiscussionModel()
        {
            DiscussionHistory = new List<DiscussionViewModel>();
        }
        public IList<DiscussionViewModel> DiscussionHistory { get; set; }

        public string NewDiscussionContent { get; set; }
    }
}