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

    public class ContactTypeSourceLoader : SourceLoader
    {
        CRMDbContext _db;

        protected CRMDbContext DbContext { get; set; }
        public ContactTypeSourceLoader()
        {
        }
        protected override IEnumerable<ISourceItem> DoLoad2(IModel model)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CRMDbContext>();
            optionsBuilder.UseSqlServer(DBConnectionManager.ConnectionString);
            CRMDbContext _context = new CRMDbContext(optionsBuilder.Options);

            List<ISourceItem> list = new List<ISourceItem>();
            foreach (var code in _context.Codes.Where(x => x.TypeOid == (int)CodeTypeEnum.DBO_CONTACTINFO_TYPE && DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo))
            {
                list.Add(new SourceItem() { Key = code.CodeOid, Value = code.Value });
            }
            return list;
        }
    }
}
