using CRM.Data.Database;
using CRM.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zircon.Core;
using Zircon.Core.DB;
using Zircon.Core.OperationModel;

namespace CRM.Operation.SourceLoaders
{
    public class TenderSourceLoader : SourceLoader
    {
        public TenderSourceLoader()
        {
        }

        protected override IEnumerable<ISourceItem> DoLoad2(IModel model)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CRMDbContext>();
            optionsBuilder.UseSqlServer(DBConnectionManager.ConnectionString);
            CRMDbContext context = new CRMDbContext(optionsBuilder.Options);

            List<ISourceItem> list = new List<ISourceItem>();
            foreach (var code in context.Codes.Where(x =>
                x.TypeOid == (int)CodeTypeEnum.TENDER_TYPES && DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo))
            {
                list.Add(new SourceItem() { Key = code.CodeOid, Value = code.Value });
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

            var item = context.Codes.FirstOrDefault(x => x.CodeOid == int.Parse(key.ToString()) && x.TypeOid == (int)CodeTypeEnum.TENDER_TYPES && DateTime.Now > x.ValidFrom &&
                                                         DateTime.Now < x.ValidTo);

            return item is null ? null : new SourceItem() { Key = item.CodeOid, Value = item.Value };
        }
    }
}
