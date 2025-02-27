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
    public class TagSourceLoader : SourceLoader
    {
        protected CRMDbContext DbContext { get; set; }
        public TagSourceLoader()
        {
        }
        protected override IEnumerable<ISourceItem> DoLoad2(IModel model)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CRMDbContext>();
            optionsBuilder.UseSqlServer(DBConnectionManager.ConnectionString);
            CRMDbContext _context = new CRMDbContext(optionsBuilder.Options);

            List<ISourceItem> list = new List<ISourceItem>();
            foreach (var code in _context.Tags.Where(x => DateTime.Now < x.ValidTo))
            {
                list.Add(new SourceItem() { Key = code.Id, Value = code.Name });
            }
            return list;
        }
    }
}
