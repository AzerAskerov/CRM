using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CRM.Operation.Models.RequestModels
{
    public class CRMClientContract : ClientContract
    {
        public CRMClientContract()
        {
            Relations = new List<CRMRelationContract>();
        }

        [ValidateComplexType]
        public override IEnumerable<RelationContract> Relations { get; set; }
    }
}
