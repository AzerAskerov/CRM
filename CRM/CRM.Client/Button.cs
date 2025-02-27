using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Client
{
    public class Button<T>
    {
        public string Text { get; set; }
        public string Name { get; set; }
        public Action<T> ActionName { get; set; }
        public string Icon { get; set; }
       // public ButtonTypeEnum Type { get; set; }
    }
}
