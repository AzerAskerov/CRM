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
    public class RelationTypeSourceLoader : SourceLoader
    {
        CRMDbContext _db;

        protected CRMDbContext DbContext { get; set; }
        public RelationTypeSourceLoader()
        {
        }



        //public PositionSourceLoader(CRMDbContext db) 
        //{
        //    _db = db;
        //}
        protected override IEnumerable<ISourceItem> DoLoad2(IModel model)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CRMDbContext>();
            optionsBuilder.UseSqlServer(DBConnectionManager.ConnectionString);
            CRMDbContext _context = new CRMDbContext(optionsBuilder.Options);

            List <ISourceItem> list = new List<ISourceItem>();
            foreach (var code in _context.Codes.Where(x => x.TypeOid == (int)CodeTypeEnum.CLASSIFIER_RELATION_TAG_ID && DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo))
            {
                list.Add(new SourceItem() { Key = code.CodeOid, Value = code.Value });
            }

            return list;
        }

        //private void DeleteItemFromSource(List<ISourceItem> sourceItems, int deletableItemCodeOid)
        //{
        //    var entityDeletableItem = sourceItems.Where(x => (int)x.Key == deletableItemCodeOid).FirstOrDefault();
        //    sourceItems.Remove(entityDeletableItem);
        //}
    }
}
