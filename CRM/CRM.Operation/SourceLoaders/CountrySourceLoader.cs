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
    public class CountrySourceLoader : SourceLoader
    {
        protected CRMDbContext DbContext { get; set; }
        public CountrySourceLoader()
        {
        }
        protected override IEnumerable<ISourceItem> DoLoad2(IModel model)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CRMDbContext>();
            optionsBuilder.UseSqlServer(DBConnectionManager.ConnectionString);
            CRMDbContext _context = new CRMDbContext(optionsBuilder.Options);

            List<ISourceItem> list = new List<ISourceItem>();
            foreach (var code in _context.Countries.Where(x => DateTime.Now < x.ValidTill))
            {
                list.Add(new SourceItem() { Key = code.Id, Value = code.Name });
            }
            return list;
        }
    }
}
