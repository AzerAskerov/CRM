using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Authorization;

using System.Collections.Generic;
using System.Linq;
using CRM.Operation.Models.Login;

namespace CRM.Client.States
{
    public class ServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AppState _appState;
        public UserModel User;
        

        public ServerAuthenticationStateProvider(AppState appState)
        {
            _appState = appState;
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity = new ClaimsIdentity();
            try
            {
                User = await _appState.HasUserLoggedIn();

                if (User.IsAuthenticated)
                {
                    List<Claim> claims = new List<Claim>(){new Claim(ClaimTypes.Name, User.FullName ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, User.Guid.ToString()),
                    new Claim(ClaimTypes.GivenName, User.UserName)};

                    foreach (var permission in User.Permissions)
                    {
                        string claimName = permission.Name + (permission.Product.HasValue ? $"_{permission.Product}" : "");

                        var claim = new Claim(ClaimTypes.Role, claimName);
                        claims.Add(claim);

                        if (permission.Parameters != null)
                            foreach (var param in permission.Parameters)
                                claim.Properties.Add(param.Key, param.Value);
                    }

                    identity = new ClaimsIdentity(claims, "serverauth");

                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task<OperationResult<UserModel>> Login(LoginModel model)
        {
            var result = await _appState.LoginAsync(model);
            if (result.Success)
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return result;
        }

        public async Task Logout()
        {
            await _appState.LogoutAsync();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            User = null;
            
        }


        //private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        //{
        //    var claims = new List<Claim>();
        //    var payload = jwt.Split('.')[1];
        //    var jsonBytes = ParseBase64WithoutPadding(payload);
        //    var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        //    keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

        //    if (roles != null)
        //    {
        //        if (roles.ToString().Trim().StartsWith("["))
        //        {
        //            var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

        //            foreach (var parsedRole in parsedRoles)
        //            {
        //                claims.Add(new Claim(ClaimTypes.Role, parsedRole));
        //            }
        //        }
        //        else
        //        {
        //            claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
        //        }

        //        keyValuePairs.Remove(ClaimTypes.Role);
        //    }

        //    claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

        //    return claims;
        //}

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
        
        public bool? IsUnderwriterUser()
        {
            var permissionParameter = User.Permissions.FirstOrDefault(x => x.Parameters != null && x.Parameters.Any(y=>y.Key.Equals("CRMDealUserType")))?.Parameters;
        
            if (permissionParameter == null)
                return default;
        
            if (permissionParameter["CRMDealUserType"] == "Underwriter")
                return true;
            else if (permissionParameter["CRMDealUserType"] == "Seller")
                return false;

            return default;
        }


        public bool? IsOperuUser()
        {
            var permissionParameter = User.Permissions.FirstOrDefault(x => x.Parameters != null && x.Parameters.Any(y => y.Key.Equals("CRMDealUserType")))?.Parameters;

            if (permissionParameter == null)
                return default;

            if (permissionParameter["CRMDealUserType"] == "Operu")
                return true;
          
            return false;
        }
    }
}
