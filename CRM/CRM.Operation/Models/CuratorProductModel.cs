using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models
{
    public class CuratorProductModel
    {
        public string ProductName { get; set; }
        public int ClientInn { get; set; }
        public string CuratorSurname { get; set; }
        public string CuratorName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CuratorFullName { get => $"{CuratorName} {CuratorSurname}"; }
    }
}
