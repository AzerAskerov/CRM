using System.Collections.Generic;
using System.Linq;
using CRM.Data.Database;
using CRM.Operation.Enums;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Models.Terror;
using Zircon.Core.Authorization;
using Zircon.Core.Enums;

namespace CRM.Operation.Maps
{
    public class MapHelper
    {
        public static void MapModelToUserContext(UserSummaryModel model, UserContext user)
        {
            user.UserName = model.Login;
            user.FullName = model.FullName;
            user.UserGuid = model.UserGuid;
            user.validTill = model.TillValidDate;
            user.IsAuthenticated = model.IsAuthenticated;
            user.Permissions = model.Permissions;
            user.UserEmail = model.UserEmail;
        }


        public static void MapModeToTerrorPersonModel(ClientDataPayload physicalPerson, TerrorPersonModel terrorPersonModel)
        {
            terrorPersonModel.Inn = physicalPerson.INN;
            var clientPhoneNumbers = physicalPerson.ContactsInfo.Where(x => x.Type == (int)ContactInfoType.MainPhone).Select(d => d.Value).ToList();
            var clientEmails = physicalPerson.ContactsInfo.Where(x => x.Type == (int)ContactInfoType.Email).Select(e => e.Value).ToList();

            terrorPersonModel.FirstName = physicalPerson.FirstName;
            terrorPersonModel.LastName = physicalPerson.LastName;
            terrorPersonModel.FullName = $"{physicalPerson.FirstName} {physicalPerson.LastName}";
            terrorPersonModel.Birthdate = physicalPerson.BirthDate;
            terrorPersonModel.PassportNr = physicalPerson.Documents.FirstOrDefault(x => x.DocumentType == (int)DocumentTypeEnum.Passport)?.DocumentNumber;
            terrorPersonModel.Addresses = new List<string>() {physicalPerson.Address?.additionalInfo};
            terrorPersonModel.PhoneNumber = clientPhoneNumbers; 
            terrorPersonModel.Email = clientEmails;
            terrorPersonModel.Pin = physicalPerson.PinNumber;
        }
    }
}
