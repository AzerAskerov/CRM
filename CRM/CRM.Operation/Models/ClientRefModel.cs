using System.Collections.Generic;
using CRM.Operation.Models.RequestModels;

namespace CRM.Operation.Models
{
    public class ClientRefModel
    {
        public List<DealListClientContract> Companies { get; set; }
        public List<DealListClientContract> PhysicalPeople { get; set; }
    }
}