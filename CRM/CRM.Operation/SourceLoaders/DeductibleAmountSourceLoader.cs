using CRM.Data.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Zircon.Core;
using Zircon.Core.OperationModel;
using Zircon.Core.DB;
using System.Linq;
using CRM.Data.Enums;

namespace CRM.Operation.SourceLoaders
{
    public class DeductibleAmountSourceLoader : SourceLoader
    {
        public DeductibleAmountSourceLoader()
        {
        }

        protected override IEnumerable<ISourceItem> DoLoad2(IModel model)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CRMDbContext>();
            optionsBuilder.UseSqlServer(DBConnectionManager.ConnectionString);
            CRMDbContext context = new CRMDbContext(optionsBuilder.Options);

            List<ISourceItem> list = new List<ISourceItem>();
            foreach (var code in context.Codes.Where(x =>
                x.TypeOid == (int) CodeTypeEnum.OFFER_DEDUCTIBLE_AMOUNTS && DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo).ToList().OrderBy(x=>decimal.Parse(x.Value)))
            {
                list.Add(new SourceItem() {Key = code.CodeOid, Value = code.Value});
            }

            return list;
        }
        
        public ISourceItem GetItem(object key, CRMDbContext context = null)
        {
            if (context == null)
            {
                var optionsBuilder = new DbContextOptionsBuilder<CRMDbContext>();
                optionsBuilder.UseSqlServer(DBConnectionManager.ConnectionString);
                context = new CRMDbContext(optionsBuilder.Options);
            }
            
            var item = context.Codes.FirstOrDefault(x => x.CodeOid == int.Parse(key.ToString()) && x.TypeOid == (int)CodeTypeEnum.OFFER_DEDUCTIBLE_AMOUNTS && DateTime.Now > x.ValidFrom &&
                                                         DateTime.Now < x.ValidTo);

            return item is null ? null : new SourceItem() {Key = item.CodeOid, Value = item.Value};
        }
    }
}