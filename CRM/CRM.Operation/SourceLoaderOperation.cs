using CRM.Data.Database;
using CRM.Operation.Enums;
using CRM.Operation.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Zircon.Core.OperationModel;
using CRM.Operation.Models;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using CRM.Operation.Attributes;
using CRM.Operation.SourceLoaders;
using Zircon.Core.Attributes;
using Zircon.Core;

namespace CRM.Operation
{
    public class SourceLoaderOperation : BusinessOperation<SourceLoaderModel>
    {
        public IEnumerable<SourceItem> Model { get; set; }
        private SourceAttribute _sourceAttribute;

        CRMDbContext _db;

        public SourceLoaderOperation(CRMDbContext db) : base(db)
        {
            _db = db;
        }

        protected override void DoExecute()
        {
            var parentPropertyType = Type.GetType(Parameters.ParentPropertyNamespace);
            var propertyInfo = parentPropertyType.GetProperty(Parameters.PropertyName);
            _sourceAttribute = GetAttributeFrom<SourceAttribute>(propertyInfo);
            
            List<SourceItem> items = new List<SourceItem>();
            if (_sourceAttribute != null )
            {
                foreach (SourceItem item in _sourceAttribute?.Load(Parameters.Parent, null)?.OrderBy(GetCustomOrderExpression))
                {
                    item.Selected = false;
                    items.Add(item);
                }
            }
            Model = items;
        }

        public static T GetAttributeFrom<T>(PropertyInfo propertyInfo, bool inherited = true)
          where T : Attribute
        {
            // do not use propertyInfo.GetCustomAttributes, it does not do attribute inheritance at all
            return propertyInfo != null
                ? (T)Attribute.GetCustomAttributes(propertyInfo, typeof(T), inherited).FirstOrDefault()
                : default(T);
        }
        
        /// <summary>
        /// This method is for custom order for each source loader.
        /// Since we have default order logic in source loader class, we have to use such approach..
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private dynamic GetCustomOrderExpression(ISourceItem item) => _sourceAttribute.SourceLoader switch
        {
            VehicleBrandsSourceLoader _ => item.Value,
            VehicleModelsSourceLoader _ => item.Value,
            OfferPolicyPeriodSourceLoader _ => item.Sequence,
            _ => null
        };
    } 
}
