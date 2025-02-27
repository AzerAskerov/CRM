

namespace CRM.Operation.Models.Login
{
    public class RoleModel
    {
        public bool Full { get; set; }
        public bool Basic { get; set; }
        public bool CRMCanEdit { get; set; }
        public bool CRMCanCreate { get; set; }
        public bool IsOwnClient { get; set; }
    }
}
