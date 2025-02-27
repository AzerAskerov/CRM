namespace CRM.Operation.Models.RequestModels
{
    public class GetUsersWithPermissionParameterValueContract
    {
        public string PermissionName { get; set; }

        public string ParameterName { get; set; }

        public string ParameterValue { get; set; }

        public int? LobOid { get; set; }
    }
}