using CRM.Operation.Enums;
using CRM.Operation.Extensions;
using CRM.Operation.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Zircon.Core.Extensions;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Normalize
{
    public class PinNumberNormalizeOp : Operation<PinNumberNormalizeInputParam>
    {
        public PinNumberNormalizeOutputParam Output;

        protected override void Initialize()
        {
            base.Initialize();
            Output = new PinNumberNormalizeOutputParam();

        }
        protected override void Prepare()
        {
            base.Prepare();
            Parameters.pinNumber = Parameters.pinNumber?.ToUpperInvariant().Trim();
            Output.ModifiedPinNumber = Parameters.pinNumber;
            Output.ModifiedDocList = Parameters.DocList?? new List<DocumentData>();
        }
        protected override void DoExecute()
        {
           
            //if (Parameters.ClientType==ClientType.Pyhsical)
            //{
                //if pin number empty or null nothing need to do. just trim empty value and finish operation
                if (string.IsNullOrEmpty( Parameters.pinNumber?.Trim()))
                {
                    Output.ModifiedPinNumber = null;
                    return;
                }

            //if pin number IMS non defined PIN default value
            if (Parameters.pinNumber== "UIDPERS")
            {
                Result.AddException(new System.Exception("UIDPERS is not client"));
                Output.ModifiedPinNumber = null;
                return;
            }

            //if pin number also exist in modified doc list then remove it from doc list
            if (Output.ModifiedDocList.Any(x => x.DocumentNumber == Parameters.pinNumber))
                Output.ModifiedDocList.Remove(Output.ModifiedDocList.FirstOrDefault(x => x.DocumentNumber == Parameters.pinNumber));

            // if pinNumber regex validation true means all are OK nothing need to do.
            Regex pinregex = new Regex("[A-Z0-9]{7}");
                if (pinregex.IsMatch(Parameters.pinNumber) && Parameters.pinNumber.Length==7)
                {                   
                    return;
                }
                //if pin exist but not in correct format need try to idendify its type or null at all
                else
                 {
                    Output.ModifiedPinNumber = null;
                    var defineddoc = ConvertToAppropiateDocument(Parameters.pinNumber);
                //document type identified by format
                    if ( defineddoc.Count>0)
                    {
                    // for each deefined type add to modified doc list and in case of doc type define client type if client tpye is undefined. defination of client type must be outside of loop.
                    // if any TIN then client type Company else Physcal
                    Output.ModifiedDocList.AddRange(defineddoc);
                    if (defineddoc.Any(x => x.DocumentType == (int)DocumentTypeEnum.Tin))

                        Parameters.ClientType = ClientType.Company;
                    else
                        Parameters.ClientType = ClientType.Pyhsical;


                    foreach (var item in defineddoc)
                    {
                        Output.ModifiedDocList.Add(item);
                        if (Parameters.ClientType == ClientType.Undefined)
                                Parameters.ClientType = item.DocumentType == (int)DocumentTypeEnum.Tin ? ClientType.Company : ClientType.Pyhsical;
                        }
                      
                    }
                    // exist pin but format not not for PIN and document type not identified
                    else
                        Output.ModifiedDocList.Add(new DocumentData() { DocumentNumber = Parameters.pinNumber, DocumentType = (int)DocumentTypeEnum.Other });
                }

            

        }

        public static List<DocumentData> ConvertToAppropiateDocument(string inputstr)
        {
            List<DocumentData> doclist ;


            // check if ID cart old version document
            Regex Idcardregex = new Regex("^([AZE]{3})|([0-9]){8}$");
            var match = Idcardregex.Matches(inputstr);
            if (!match.Any(x => !x.Success) && match.Count == 2)
            {
                doclist = new List<DocumentData>();
                doclist.Add(new DocumentData() { DocumentNumber = $"{match[0].Value}{match[1].Value}", DocumentType = (int)DocumentTypeEnum.IdCard });
                return doclist;
            }

         

            // check if ID cart new version document
            Regex Idcardnewregex = new Regex("^(AA|AB|AC|AD)|([0-9]){7}$");
            var matchidnew = Idcardnewregex.Matches(inputstr);
            if (!matchidnew.Any(x => !x.Success) && matchidnew.Count == 2)
            {
                doclist = new List<DocumentData>();
                doclist.Add(new DocumentData() { DocumentNumber = $"{matchidnew[0].Value}{matchidnew[1].Value}", DocumentType = (int)DocumentTypeEnum.IdCard });
                return doclist;
            }


            // check if Military document

            Regex militaryregex = new Regex("^(MN)|([0-9]){7}$");

            var militarymatch = militaryregex.Matches(inputstr);
            if (!militarymatch.Any(x => !x.Success) && militarymatch.Count == 2)
            {
                doclist = new List<DocumentData>();
                doclist.Add(new DocumentData() { DocumentNumber = $"{militarymatch[0].Value}{militarymatch[1].Value}", DocumentType = (int)DocumentTypeEnum.Military });
                return doclist;
            }

            // check if Temprory resident document
            Regex temproryregex = new Regex("^([MYI|MYİ]{3})|([0-9]{7})$");

            var temprorymatch = temproryregex.Matches(inputstr);
            if (!temprorymatch.Any(x=>!x.Success) && temprorymatch.Count == 2)
            {
                doclist = new List<DocumentData>
                {
                    new DocumentData() { DocumentNumber = $"{temprorymatch[0].Value}{temprorymatch[1].Value}", DocumentType = (int)DocumentTypeEnum.TemproryLiving }
                };
                return doclist;
            }

            // check if Permenant resident document
            Regex permenantregex = new Regex("^([DYI|DYİ]{3})|([0-9]{7})$");
            var permenantmatch = permenantregex.Matches(inputstr);
            if (!permenantmatch.Any(x => !x.Success) && permenantmatch.Count == 2)
            {
                doclist = new List<DocumentData>
                {
                    new DocumentData() { DocumentNumber = $"{permenantmatch[0].Value}{permenantmatch[1].Value}", DocumentType = (int)DocumentTypeEnum.PermenantLiving }
                };
                return doclist;
            }

            // check if Russian foreign  passport
            Regex RussianForeignregex = new Regex("^([RUS]{3})|([0-9]{7,9})$");
            var RussianForeignregexmatch = RussianForeignregex.Matches(inputstr);
            if (!RussianForeignregexmatch.Any(x => !x.Success) && RussianForeignregexmatch.Count == 2)
            {
                doclist = new List<DocumentData>
                {
                    new DocumentData() { DocumentNumber = $"{RussianForeignregexmatch[0].Value}{RussianForeignregexmatch[1].Value}", DocumentType = (int)DocumentTypeEnum.RussianPassport }
                };
                return doclist;
            }
            // check if United Kingdom  foreign  passport
            Regex GBForeignregex = new Regex("^([GBR]{3})|([0-9]{7,9})$");
            var GBForeignregexmatch = GBForeignregex.Matches(inputstr);
            if (!GBForeignregexmatch.Any(x => !x.Success) && GBForeignregexmatch.Count == 2)
            {
                doclist = new List<DocumentData>
                {
                    new DocumentData() { DocumentNumber = $"{GBForeignregexmatch[0].Value}{GBForeignregexmatch[1].Value}", DocumentType = (int)DocumentTypeEnum.GBPassport }
                };
                return doclist;
            }
            // check if TIN
            if (inputstr.IsNumber() && inputstr.Length==10 && inputstr.Substring(inputstr.Length - 1).In("2","1"))
            {
                doclist = new List<DocumentData>();
                doclist.Add(new DocumentData() { DocumentNumber = inputstr, DocumentType = (int)DocumentTypeEnum.Tin });
                return doclist;
            }



            doclist = new List<DocumentData>();
            return doclist;
        }
    }

    public class PinNumberNormalizeInputParam
    {
        public string pinNumber { get; set; }
        public ClientType ClientType { get; set; }
        public List<DocumentData> DocList { get; set; }
    }

    public class PinNumberNormalizeOutputParam
    {
        public string ModifiedPinNumber { get; set; }
        public List<DocumentData> ModifiedDocList { get; set; }
    }
}