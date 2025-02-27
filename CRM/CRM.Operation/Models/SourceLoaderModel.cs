using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Zircon.Core.Attributes;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models
{
    public class SourceLoaderModel
    {
        public string ParentPropertyNamespace { get; set; }
         
        public string PropertyName { get; set; }

        public string ParentAsJson { get; set; }

        /// <summary>
        /// This model deserializing from 'ParentAsJson' field in ServerSide - SourceLoaderController. 
        /// </summary>
        public IModel Parent { get; set; }
    }
}
