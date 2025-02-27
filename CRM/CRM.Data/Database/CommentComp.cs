using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class CommentComp
    {
        public int Id { get; set; }
        public int Inn { get; set; }
        public int CommentId { get; set; }
        public int? ContactId { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual ContactInfo ContactInfo { get; set; }
        public virtual ClientRef ClientRef { get; set; }
    }
}
