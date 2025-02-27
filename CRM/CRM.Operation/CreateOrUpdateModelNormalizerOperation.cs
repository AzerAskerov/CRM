using CRM.Operation.Attributes;
using CRM.Operation.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class CreateOrUpdateModelNormalizerOperation : BusinessOperation<ClientDataPayload>
       
    {
        ClientDataPayload normalizedModel = new ClientDataPayload();
        public CreateOrUpdateModelNormalizerOperation(DbContext db) : base(db)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            normalizedModel = Parameters;
        }
        protected override void DoExecute()
        {

            Dictionary<PropertyInfo, int> _orderedproperydic = new Dictionary<PropertyInfo, int>();

            Type incomingobject = Parameters.GetType();
            IList<PropertyInfo> allprops = new List<PropertyInfo>(incomingobject.GetProperties());

            foreach (var property in allprops)
            {
                PropertyInfo propertyInfo = (PropertyInfo)property;

                NormalizeAttribute attr = propertyInfo.GetCustomAttribute<NormalizeAttribute>();

                if (attr != null)
                {
                    _orderedproperydic.Add(property, attr.Order);
                }  
            }
        }
    }
}
