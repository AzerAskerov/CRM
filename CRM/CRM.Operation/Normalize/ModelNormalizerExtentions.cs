using CRM.Operation.Attributes;
using CRM.Operation.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Normalize
{
    public static class ModelNormalizerExtentions
    {
        public static OperationResult _result;
        public static Tmodel Normalize<Tmodel>(this IModel model, DbContext dbContext)
        {
            _result = new OperationResult();
            Type incomingobject = model.GetType();

            //if current model has attribute Normalize
            if (incomingobject.GetCustomAttributes(typeof(NormalizeAttribute), false)!=null)
            {
                // try to call appropiate normalize operation for current model if exist: Normalization Operation naming format should be {ModelName}NormalizerOperation
                String _operationfullnamename = $"CRM.Operation.Normalize.{incomingobject.Name}NormalizerOperation";

                Type t = Type.GetType(_operationfullnamename);

                if (t != null)
                {
                    var opinstance = Activator.CreateInstance(t, new object[] { dbContext });

                    MethodInfo method = t.GetMethod("Execute", new[] { typeof(Tmodel)});
                    _result = (OperationResult) method.Invoke(opinstance, new object[] { model });
                }
                


            } ;



            return (Tmodel)model;
        }
    }
}
