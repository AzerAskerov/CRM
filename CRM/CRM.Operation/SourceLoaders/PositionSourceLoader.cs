using CRM.Data.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zircon.Core;
using Zircon.Core.OperationModel;
using Zircon.Core.DB;
using System.Linq;
using CRM.Data.Enums;

namespace CRM.Operation.SourceLoaders
{
    public class PositionSourceLoader : SourceLoader
    {
        protected CRMDbContext DbContext { get; set; }
        public PositionSourceLoader()
        {
        } 

        protected override IEnumerable<ISourceItem> DoLoad2(IModel model)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CRMDbContext>();
            optionsBuilder.UseSqlServer(DBConnectionManager.ConnectionString);
            CRMDbContext _context = new CRMDbContext(optionsBuilder.Options);

            List<ISourceItem> list = new List<ISourceItem>();
            foreach (var pos in _context.Positions.Where(x => DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo))
            {
                list.Add(new SourceItem() { Key = pos.Id, Value = pos.PositionName });
            }
            return list;
        }
    }
}
