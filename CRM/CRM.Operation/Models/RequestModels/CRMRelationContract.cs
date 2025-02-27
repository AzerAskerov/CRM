using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models.RequestModels
{
    public class CRMRelationContract : RelationContract
    {
        [Attributes.RequiredLocalized]
        public override int? ClientINN { get; set; }
    }
}
