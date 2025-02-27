using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models
{
    public class ClaimModel
    {
        public List<ClaimListItemModel> Claims { get; set; }
        public int? NonClaimDayCount { get; set; }
        public int? AccidentCount { get; set; }
    }
}
