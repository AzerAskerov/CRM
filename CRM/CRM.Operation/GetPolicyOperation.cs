using CRM.Data.Database;
using CRM.Operation.Models.RequestModels;

namespace CRM.Operation
{
    public class GetPolicyOperation <T> : GenerateProductListOperation<T>
    where T : GetClientPolicyModel
    {
        public GetPolicyOperation(CRMDbContext db) : base(db)
        {
            
        }

        protected override string GenerateQueryParameters()
        {
            return "{\"client_code\":\"" + "null" + "\",\"policy_number\":\"" + Parameters.PolicyNumber + "\"}";
        }
    }
}
