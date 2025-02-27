using System.Collections.Generic;
using System.Linq;
using AutoMapper.Internal;
using CRM.Client.States;
using CRM.Data.Database;
using CRM.Operation;
using CRM.Operation.Models.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zircon.Core.Authorization;

namespace CRM.Server.Attributes
{
    public class WebApiAuthenticateAttribute : ActionFilterAttribute
    {
        private bool success = true;
        private OperationResult result;
        private string username;
        private string password;
        private string staticapikey;


        public WebApiAuthenticateAttribute()
        {
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            success = CommonUserContextManager.CurrentUser != null && CommonUserContextManager.CurrentUser.IsAuthenticated;

            if (success)
                return;

            

            ValidateHeaders(context.HttpContext.Request);


            if (success)
                return;

            if (!success)
            {
                context.Result = new UnauthorizedObjectResult(result);
                return;
            }


            AuthenticateUser(new LoginModel() { Login = username, Password = password });
            if (!success)
            {
                context.Result = new UnauthorizedObjectResult(result);
                return;
            }
        }


        private void ValidateHeaders(HttpRequest Request)
        {
            // result = new OperationResult();
            var headers = Request.Headers;
            var user = headers.FirstOrDefault(x => x.Key == "username");
            var pass = headers.FirstOrDefault(x => x.Key == "password");
            var key = headers.FirstOrDefault(x => x.Key == "SecretCrmkey");

            if (!string.IsNullOrEmpty(key.Value))
            {
                if (key.Value== "Fornowtempvalue")
                {
                    success = true;
                    return;
                }
               
            }

            if (string.IsNullOrEmpty(user.Value))
            {
                success = false;
                result = new OperationResult()
                {
                    Name = "",
                    Issues = new List<Issue>()
                        { new Issue() { Severity = IssueSeverity.ValidationError, Message = new Message() { Text = "User name is not provided" } } }
                };
                return;
            }

            if (string.IsNullOrEmpty(pass.Value))
            {
                success = false;
                result = new OperationResult()
                {
                    Name = "",
                    Issues = new List<Issue>()
                        { new Issue() { Severity = IssueSeverity.ValidationError, Message = new Message() { Text = "Password not provided" } } }
                };
                return;
            }

            username = headers.GetOrDefault("UserName").First();
            password = headers.GetOrDefault("password").First();
           
            success = true;
        }

        private void AuthenticateUser(LoginModel model)
        {
            UserAuthorizeOperation opt = new UserAuthorizeOperation(new CRMDbContext());
            opt.Execute(model);
            success = opt.User.IsAuthenticated;
        }
    }
}