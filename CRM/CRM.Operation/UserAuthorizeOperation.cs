using System;
using System.Net.Http;
using System.Threading.Tasks;

using Zircon.Core.Authorization;
using Zircon.Core.Config;
using Zircon.Core.HttpContextHelper;
using Zircon.Core.Log;
using Zircon.Core.OperationModel;

using CRM.Data.Database;
using CRM.Operation.Maps;
using CRM.Operation.Models;
using CRM.Operation.Models.Login;
using System.Linq;
using Zircon.Core.Extensions;

namespace CRM.Operation
{
    public class UserAuthorizeOperation : Operation<LoginModel>
    {
        private CRMDbContext _db;
        public UserContext User { get; private set; }


        //ToDo make dbcontext setted from startup
        public UserAuthorizeOperation(CRMDbContext db) : base(db)
        {
            _db = db;
        }

        protected override void DoExecute()
        {
            HttpClient httpClient = new HttpClient();

            var result = Task.Run(async () => await httpClient.PostJsonAsync<LoginModel, UserSummaryModel>
                ($"{Settings.GetString("BaseUrl")}/authorize/authorizeUser/", Parameters)).Result;

            if (!result.Success)
            {
                Result.Merge(result);
                return;
            }

            User = new UserContext();

            result.Model.Permissions.RemoveAll(x => !(x.Name == "CRMRole" || x.Name == "CRMCanCreate" || x.Name == "CRMCanEdit"));

            MapHelper.MapModelToUserContext(result.Model, User);

            var crmRole = User.Permissions.FirstOrDefault(x => x.Name == "CRMRole");
            
            if(crmRole == null)
            {
                Result.AddError("UserAuthorizeOperation.NotPermision".Translate());
                return;
            }
            
            if (crmRole.Parameters.ContainsValue("Underwriter") && crmRole.Parameters.ContainsValue("Seller") && crmRole.Parameters.ContainsValue("Operu"))
            {
                Result.AddError("UserAuthorizeOperation.UserCantHasUnderwriterAndSellerPermission".Translate());
            }
            TechnicalLog.WriteInformation(new Message(this.Name + ":" + "User {Login} has logged in", Parameters), User.UserGuid);
        }

        protected override void DoFinalize()
        {
            foreach (Issue issue in Result.Issues)
            {
                TechnicalLog.WriteError(issue.Message, objectId: User != null ? User.UserGuid : (Guid?)null);
            }
        }
    }
}
