using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class TerrorLog
    {
        public  int Id { get; set;  }
        public int? Inn { get; set; }
        public  string OperationType { get; set; }
        public string OperationDescription { get; set; }
        public string JsonResult { get; set; }
        public  DateTime RegisteredDateTime { get; set; }
    }
}