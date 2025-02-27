using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models
{
    public class Item
    {
        public List<UserGuids> Items { get; set; }
    }

    public class UserGuids
    {
        public string UserGuid { get; set; }
    }
}
