using System;
using System.Linq;
using System.Globalization;


using CRM.Data.Database;
using CRM.Operation.Enums;

using CRM.Data.Enums;
using System.Security.Cryptography.X509Certificates;

namespace CRM.Operation
{
    public class ClientCreateOperation : ClientOperationBase
    {
        CRMDbContext _db;
        public int NewCreatedClientINN;

        public ClientCreateOperation(CRMDbContext db) : base(db)
        {
            _db = db;
        }

        protected override void DoExecute()
        {
            DateTime validFrom = DateTime.Now;
            DateTime validTo = DateTime.MaxValue;

            base.ClientRef.TagComps.Remove(base.ClientRef.TagComps.FirstOrDefault(x => x.Tag.Name == "FromUI"));
            _db.Add(base.ClientRef);

            CultureInfo userCulture = new CultureInfo("az");

            CultureInfo.DefaultThreadCurrentCulture = userCulture;
            CultureInfo.DefaultThreadCurrentUICulture = userCulture;

           

            _db.SaveChanges();


            //Add INN as document
            Document docInn = new Document()
            {
                DocumentNumber = base.ClientRef.Inn.ToString(),
                DocumentType = (int)DocumentTypeEnum.INN, //INN
                DocumentExpireDate = DateTime.MaxValue,
                ClientRef = base.ClientRef,
                ValidFrom = validFrom,
                ValidTo = validTo
            };
            _db.Documents.Add(docInn);
            _db.SaveChanges();

            NewCreatedClientINN = base.ClientRef.Inn;
        }
    }
}
