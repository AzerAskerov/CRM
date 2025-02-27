using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models
{
    public class QueryResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string Data { get; set; }
    }
}
