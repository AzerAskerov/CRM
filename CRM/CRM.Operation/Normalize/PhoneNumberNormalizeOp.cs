using CRM.Operation.Enums;
using CRM.Operation.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper.Internal;
using CRM.Operation.Helpers;
using Microsoft.IdentityModel.Tokens;
using Zircon.Core.Enums;
using Zircon.Core.OperationModel;
using ClientType = CRM.Operation.Models.RequestModels.ClientType;

namespace CRM.Operation.Normalize
{
    public class PhoneNumberNormalizeOp : Operation<PhoneNumberNormalizeInputParam>
    {
        public PhoneNumberNormalizeOutputParam Output;

        protected override void Initialize()
        {
            base.Initialize();
            Output = new PhoneNumberNormalizeOutputParam();
        }

        protected override void Prepare()
        {
            base.Prepare();
            Output.ModifiedPhoneNumber = new List<ContactInfoNormalize>();
        }

        protected override void DoExecute()
        {
            foreach (var phoneNumber in Parameters.PhoneNumbers)
            {
                //if phone number value is 994500000000 then ignore it

                if (phoneNumber.Value == "994500000000")
                {
                    continue;
                }


                var formatterNumber = ConversionHelper.ParsePhoneNumber(phoneNumber.Value);
                Output.ModifiedPhoneNumber.Add(new ContactInfoNormalize()
                {
                    ContactInfoType = phoneNumber.ContactInfoType,
                    OldValue = phoneNumber.Value,
                    Value = formatterNumber.Result
                });
            }

        }
    }
}

public class PhoneNumberNormalizeInputParam
{
    public List<ContactInfoNormalize> PhoneNumbers { get; set; }
    public ClientType ClientType { get; set; }
    public List<DocumentData> DocList { get; set; }

    public PhoneNumberNormalizeInputParam()
    {
        PhoneNumbers = new List<ContactInfoNormalize>();
    }
}

public class ContactInfoNormalize
{
    public ContactInfoType ContactInfoType { get; set; }

    public string OldValue { get; set; }

    public string Value { get; set; }


}

public class PhoneNumberNormalizeOutputParam
{
    public List<ContactInfoNormalize> ModifiedPhoneNumber { get; set; }
}

