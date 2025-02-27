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
    public class OfferPeriodTypeSourceLoader : SourceLoader
    {
        public OfferPeriodTypeSourceLoader()
        {
        }

        protected override IEnumerable<ISourceItem> DoLoad2(IModel model)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CRMDbContext>();
            optionsBuilder.UseSqlServer(DBConnectionManager.ConnectionString);
            CRMDbContext context = new CRMDbContext(optionsBuilder.Options);

            List<ISourceItem> list = new List<ISourceItem>();
            foreach (var code in context.Codes.Where(x =>
                x.TypeOid == (int) CodeTypeEnum.OFFER_PERIOD_TYPE && DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo))
            {
                list.Add(new SourceItem() {Key = code.CodeOid, Value = code.Value});
            }

            return list;
        }
    }
}