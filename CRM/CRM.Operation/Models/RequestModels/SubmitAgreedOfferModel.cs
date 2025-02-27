using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models.RequestModels
{
    public class SubmitAgreedOfferModel
    {
        public string OfferNumber { get; set; }
        public Guid CurrentUserGuid { get; set; }
        public string CurrentUserName
        {
            get; set;
        }
        }
}
