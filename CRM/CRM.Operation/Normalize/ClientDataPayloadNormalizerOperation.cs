using CRM.Data.Database;
using CRM.Operation.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zircon.Core.Enums;
using Zircon.Core.Extensions;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Normalize
{
    public class ClientDataPayloadNormalizerOperation : Operation<ClientDataPayload>
    {

        CRMDbContext _db;
     
        public ClientDataPayloadNormalizerOperation(CRMDbContext db) : base(db)
        {
            _db = db;
        }
        protected override void DoExecute()
        {
            //Normalize Physcal person Pin number

            PinNumberNormalizeOp _pinop = new PinNumberNormalizeOp();
            Result.Merge(_pinop.Execute(new PinNumberNormalizeInputParam() { pinNumber = Parameters.PinNumber, ClientType = Parameters.ClientType, DocList = Parameters.Documents }));
            Parameters.PinNumber = _pinop.Output.ModifiedPinNumber;
            Parameters.Documents = _pinop.Output.ModifiedDocList;

            //Normalize Docs

            DocumentNumberNormalizeOp _docop = new DocumentNumberNormalizeOp(_db);
            Result.Merge(_docop.Execute(new DocumentNumberNormalizeInputParam() { ClientType = Parameters.ClientType, DocList = Parameters.Documents }));
            Parameters.Documents = _docop.Output.ModifiedDocList;
            Parameters.ClientType = _docop.Parameters.ClientType;


            //Normalize Phone number

            var phoneNumbers = Parameters.ContactsInfo.Where(x => x.Type == (int)ContactInfoType.MobilePhone || x.Type == (int)ContactInfoType.MainPhone)
                .Select(x=> new ContactInfoNormalize(){ ContactInfoType = (ContactInfoType)x.Type, Value = x.Value}).ToList();
            
            PhoneNumberNormalizeOp phoneOp = new PhoneNumberNormalizeOp();
            Result.Merge(phoneOp.Execute(new PhoneNumberNormalizeInputParam(){ PhoneNumbers = phoneNumbers}));

            foreach (var phoneNumberItem in phoneOp.Output.ModifiedPhoneNumber)
            {
                if (phoneNumberItem.Value is null)
                {
                    Parameters.ContactsInfo.Remove(Parameters.ContactsInfo.FirstOrDefault(x => x.Type == (int)phoneNumberItem.ContactInfoType && x.Value == phoneNumberItem.OldValue));
                }
                else
                {
                    Parameters.ContactsInfo.FirstOrDefault(x => x.Type == (int)phoneNumberItem.ContactInfoType && x.Value == phoneNumberItem.OldValue).Value = phoneNumberItem.Value;
                }
            }
               
        }
    }
}
