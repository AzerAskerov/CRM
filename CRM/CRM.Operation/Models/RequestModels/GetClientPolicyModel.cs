namespace CRM.Operation.Models.RequestModels
{
    public class GetClientPolicyModel : ClientContract
    {
        public GetClientPolicyModel(ClientContract clientContract ,string policyNumber): base(clientContract)
        {
            PolicyNumber = policyNumber;
        }

        public GetClientPolicyModel()
        {
            
        }

        public string PolicyNumber { get; set; }
    }
}