using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Client.Shared.Components
{
    public partial class CustomTitle
    {
        [Parameter]
        public string Icon { get; set; }

        [Parameter]
        public string Main { get; set; }

        [Parameter]
        public string Sub { get; set; }

        [Parameter]
        public string ButtonIcon { get; set; } = "plus";

        [Parameter]
        public string ButtonText { get; set; }

        [Parameter]
        public Action ButtonClick { get; set; }

        [Parameter]
        public Action SearchClick { get; set; }

        [Parameter]
        public string SearchPlaceHolder { get; set; }

       // [Parameter] 

    }
}
