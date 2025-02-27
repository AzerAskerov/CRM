using CRM.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models.RequestModels
{
    public class UpdateMergeModel
    {
        public  ClientRef FoundClient { get; set; }
        public ClientDataPayload IncomingClient { get; set; }
    }
}
