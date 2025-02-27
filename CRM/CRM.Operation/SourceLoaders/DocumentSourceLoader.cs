﻿using CRM.Data.Database;
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
    public class DocumentSourceLoader : SourceLoader
    {
        public DocumentSourceLoader()
        {
        } 
        protected override IEnumerable<ISourceItem> DoLoad2(IModel model)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CRMDbContext>();
            optionsBuilder.UseSqlServer(DBConnectionManager.ConnectionString);
            CRMDbContext _context = new CRMDbContext(optionsBuilder.Options);

            List <ISourceItem> list = new List<ISourceItem>();
            foreach (var code in _context.Codes.Where(x => x.TypeOid == (int)CodeTypeEnum.DBO_DOCUMENT_DOCUMENT_TYPE && DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo))
            {
                list.Add(new SourceItem() { Key = code.CodeOid, Value = code.Value });
            }

            DeleteItem(list, 3); // 3 is code_oid of 'INN'
            
            return list;
        }

        private void DeleteItem(List<ISourceItem> sourceItems, int deletableItemCodeOid)
        {
            var item = sourceItems.Where(x => (int)x.Key == deletableItemCodeOid).FirstOrDefault();
            sourceItems.Remove(item);
        }
    }
}
