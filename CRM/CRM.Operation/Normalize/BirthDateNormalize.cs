using CRM.Operation.Enums;
using CRM.Operation.Extensions;
using CRM.Operation.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Normalize
{
    public class BirthDateNormalizeOp : Operation<BirthDateNormalizeInputParam>
    {

        public BirthDateNormalizeOutputParam Output;

        protected override void Initialize()
        {
            base.Initialize();
            Output = new BirthDateNormalizeOutputParam();
        }

        protected override void Prepare()
        {
            base.Prepare();
           
         
        }

        protected override void DoExecute()
        {
          
        }


    }

    public class BirthDateNormalizeInputParam
    {
              public ClientType ClientType { get; set; }
        public string BirthDate { get; set; }
    }

    public class BirthDateNormalizeOutputParam
    {
    
    }
}
