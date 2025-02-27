using CRM.Data.Database;
using CRM.Operation.Localization;
using CRM.Operation.Models.RequestModels;
using MatBlazor;
using System.Collections.Generic;

namespace CRM.Client.Helpers
{

    public static class SearchResultHelper
    {
        public static string BuildCompanyInputValueResult(ClientContract context)
        {
            if (context == null)
                return string.Empty;

            if (string.IsNullOrEmpty(context.CompanyName))
            {

                var identifier = context.TinNumber == null ? "Client.PinNumber".Translate() : "Client.TinNumber".Translate();
                var number = context.TinNumber ?? context.PinNumber;

                return $"{context.FirstName} {context.LastName} {context.FatherName} ({identifier} - {number})";
            }
            else
            {

                var tinLabel = "Client.Tin".Translate();
                return $"{context.CompanyName} ({tinLabel} - {context.TinNumber})";
            }
        }


        public static bool HasCompanyInformation(ClientContract context)
        {
            return !string.IsNullOrEmpty(context.CompanyName);
        }

        public static bool HasPersonalInformation(ClientContract context)
        {
            return !string.IsNullOrEmpty(context.FirstName) ||
                   !string.IsNullOrEmpty(context.LastName) ||
                   !string.IsNullOrEmpty(context.FatherName);
        }

        public static string BuildCompanyInfo(ClientContract context)
        {
            return $"{context.CompanyName} ({("Client.TinNumber".Translate())} - {context.TinNumber})";
        }

        public static string BuildPersonalInfo(ClientContract context)
        {
            return $"{context.FirstName} {context.LastName} {context.FatherName} ({(context.TinNumber == null ? ("Client.PinNumber".Translate()) : ("Client.TinNumber".Translate()))} - {context.TinNumber ?? context.PinNumber})";
        }

    }

}
