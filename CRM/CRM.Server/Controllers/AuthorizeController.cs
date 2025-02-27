using System;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using Zircon.Core.Authorization;
using Zircon.Core.Authorization.SystemProvider;
using Zircon.Core.HttpContextHelper;

using CRM.Operation;
using CRM.Data.Database;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Models.Login;
using System.Linq;
using CRM.Server.Attributes;
using CRM.Operation.Models;
using System.Text.Json;
using CRM.Operation.JsonConverters;

namespace CRM.Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthorizeController :  BaseController
    {
        private readonly IConfiguration _configuration;
        public AuthorizeController(CRMDbContext db, IConfiguration configuration)
        {
            DbContext = db;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                CultureInfo userCulture = new CultureInfo("az");
                Session.SetObject<CultureInfo>(CRMApplicationProvider.CURRENT_USER_CULTURE, userCulture);
              
                CultureInfo.DefaultThreadCurrentCulture = userCulture;
                CultureInfo.DefaultThreadCurrentUICulture = userCulture;

                UserAuthorizeOperation operation = new UserAuthorizeOperation(DbContext);
                operation.Execute(model);

                if (!operation.Result.Success)
                    return OperationResult<UserModel>(operation.Result, null);

                Session.SetObject<string>(CRMApplicationProvider.CURRENT_USER_IP, HttpContext.Connection.RemoteIpAddress?.ToString());
                Session.SetObject<UserContext>(CRMApplicationProvider.CURRENT_USER, operation.User);
                Session.SetObject<string>(CRMApplicationProvider.CURRENT_USER_GUID, operation.User.UserGuid.ToString());

                List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, operation.User.UserGuid.ToString()) };

                foreach (var permission in operation.User.Permissions)
                {
                    claims.Add(new Claim(ClaimTypes.Role, permission.Name));
                }

                var authProperties = new AuthenticationProperties
                {
                    // AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(5),
                    // The time at which the authentication ticket expires.
                    // A value set here overrides the ExpireTimeSpan option of CookieAuthenticationOptions set with AddCookie.

                    // IsPersistent = true,
                    // Whether the authentication session is permanent or transient.
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);


                return OperationResult<UserModel>(operation.Result, BuildUserInfo(operation.User));

            }

            return OperationResult<UserModel>(null, null);
        }

        [HttpGet("token")]
        [WebApiAuthenticate]
        public IActionResult GetToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddSeconds(Convert.ToInt32(_configuration["JwtExpiryInMinutes"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                null,
                expires: expiry,
                signingCredentials: creds
            ) ;

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [Authorize]
        [WebApiAuthenticate]
        [HttpPost("logout")]
       
        public async Task<ActionResult> Logout()
        {
            ClearSession();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }

        [HttpGet("auth1")]
       
        public ActionResult<UserModel> IsAuthenticated()
        {
            if (!HttpContext.User.Identity.IsAuthenticated || !CommonUserContextManager.HasUserLoggedIn)
                return Ok(BuildUserInfo(null));

            return Ok(BuildUserInfo(Session.GetObject<UserContext>(CRMApplicationProvider.CURRENT_USER)));
        }


        [HttpGet("[action]")]
        [WebApiAuthenticate]
        public IActionResult GetPermissionParameter()
        {
            var permissionOperation = new GetPermissionParameter();
            permissionOperation.Execute();

            return new JsonResult(new PermissionParameterModel
            {

                Parameters = permissionOperation.PermissionParameters
            }, new JsonSerializerOptions() { Converters = { new DealOfferFormJsonConverter() } });
        }
        private UserModel BuildUserInfo(UserContext user)
        {
            if (user == null)
                return new UserModel { IsAuthenticated = false };

            return new UserModel
            {
                FullName = user.FullName,
                IsAuthenticated = user.IsAuthenticated,
                Guid = user.UserGuid,
                FatherName = user.FatherName,
                Name = user.Name,
                Surname = user.Surname,
                Permissions = user.Permissions,
                UserName = user.UserName
            };
        }


        private void ClearSession()
        {
            Session.Clear();

            //Removing Session Cookies
            CookieOptions options = new CookieOptions();
            if (Request.Cookies[".AspNetCore.Session"] != null)
            {
                options.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append(".AspNetCore.Session", "", options);
            }

        }
        [HttpPost("isownclient")]
        public async Task<ActionResult<RoleModel>> IsOwnClient(ClientContract model)
        {
            var clientExists = await DbContext.UserClientComps.AnyAsync(x => x.Inn == model.INN && x.ULogonName == CommonUserContextManager.CurrentUser.UserName);
            var roleModel = new RoleModel { IsOwnClient = clientExists };
            return roleModel;
        }
    }
}
