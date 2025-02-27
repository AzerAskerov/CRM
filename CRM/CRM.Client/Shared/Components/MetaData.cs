using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Client.Shared.Components
{
        public class MetaData
        {
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public int PageSize { get; set; }
            public int Skip { get; set; }
            public bool HasPrevious => CurrentPage > 1;
            public bool HasNext => CurrentPage < (int)Math.Ceiling(TotalPages / (double)PageSize);
        }
    
}
