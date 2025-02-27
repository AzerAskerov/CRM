using CRM.Data.Database;
using CRM.Operation.Enums;
using CRM.Operation.Extensions;
using CRM.Operation.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Normalize
{
    public class DocumentNumberNormalizeOp : Operation<DocumentNumberNormalizeInputParam>
    {
        CRMDbContext _db;
  
        public DocumentNumberNormalizeOp(CRMDbContext db) : base(db)
        {
            _db = db;
        }
        public DocumentNumberNormalizeOutputParam Output;

        protected override void Initialize()
        {
            base.Initialize();
            Output = new DocumentNumberNormalizeOutputParam();
        }

        protected override void Prepare()
        {
            base.Prepare();
           
            Output.ModifiedDocList = new List<DocumentData>();
        }

        protected override void DoExecute()
        {
            foreach (var item in Parameters.DocList.Distinct())
            {
               
                string documentNumber = item.DocumentNumber;
                
                if (String.IsNullOrEmpty(documentNumber))
                    continue;
                documentNumber = documentNumber.Trim().ToUpper();

                if (documentNumber.Length==0)
                    continue;
                if (_db.IgnoredDocs.Any(x=>x.IgnoredDocType==item.DocumentType && x.IgnoredDocNumber==item.DocumentNumber))
                {
                    continue;
                }

                if (Output.ModifiedDocList.Any(x => x.DocumentNumber == item.DocumentNumber && x.DocumentType == item.DocumentType))
                    continue;

                switch (item.DocumentType)
                {
                    case (int)DocumentTypeEnum.MedicineCard:
                        break;
                    case (int)DocumentTypeEnum.INN:
                        if (item.DocumentNumber.IsNumber())
                            Output.ModifiedDocList.Add(item);
                        else
                            Output.ModifiedDocList.Add(new DocumentData() { DocumentExpireDate=null, DocumentNumber= documentNumber, DocumentType=(int)DocumentTypeEnum.Other});
                        break;

                    case (int)DocumentTypeEnum.Tin:

                      if( item.DocumentNumber.Length!=10)
                            Output.ModifiedDocList.Add(new DocumentData() { DocumentExpireDate = null, DocumentNumber = documentNumber, DocumentType = (int)DocumentTypeEnum.Other });
                        else if (documentNumber.Substring(documentNumber.Length - 1) == "1" )
                        {
                            Parameters.ClientType = ClientType.Company;
                            Output.ModifiedDocList.Add(item);
                        }               
                      else if(documentNumber.Substring(documentNumber.Length - 1) == "2" )
                        {
                            Parameters.ClientType = ClientType.Pyhsical;
                            Output.ModifiedDocList.Add(item);
                        }
                      else if((documentNumber.Substring(documentNumber.Length - 1)=="1" && ClientType.Company !=Parameters.ClientType) 
                            ||(documentNumber.Substring(documentNumber.Length - 1) == "2" && ClientType.Pyhsical != Parameters.ClientType))
                            Output.ModifiedDocList.Add(new DocumentData() { DocumentExpireDate = null, DocumentNumber = documentNumber, DocumentType = (int)DocumentTypeEnum.Other });
                        else
                            Output.ModifiedDocList.Add(item);

                        break;


                    case (int)DocumentTypeEnum.IdCard:
                    case (int)DocumentTypeEnum.Military:
                    case (int)DocumentTypeEnum.PermenantLiving:
                    case (int)DocumentTypeEnum.TemproryLiving:
                    case (int)DocumentTypeEnum.RussianPassport:

                        var converteddoc = PinNumberNormalizeOp.ConvertToAppropiateDocument(documentNumber);
                        if (converteddoc.Any())
                   
                            Output.ModifiedDocList.Add(new DocumentData() { DocumentExpireDate = null, DocumentNumber = converteddoc.First().DocumentNumber, DocumentType = converteddoc.First().DocumentType });
                        else
                            Output.ModifiedDocList.Add(new DocumentData() { DocumentExpireDate = null, DocumentNumber = documentNumber, DocumentType = (int)DocumentTypeEnum.Other });

                        break;

                    default:
                        Output.ModifiedDocList.Add(item);
                        break;

                }

                


            }
        }


    }

    public class DocumentNumberNormalizeInputParam
    {
              public ClientType ClientType { get; set; }
        public List<DocumentData> DocList { get; set; }
    }

    public class DocumentNumberNormalizeOutputParam
    {
        public List<DocumentData> ModifiedDocList { get; set; }
    }
}
