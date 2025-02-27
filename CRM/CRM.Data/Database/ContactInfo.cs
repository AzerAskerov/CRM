using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class ContactInfo
    {
        public ContactInfo()
        {
            CallHistories = new HashSet<CallHistory>();
            ClientContactInfoComps = new HashSet<ClientContactInfoComp>();
            CommentComps = new HashSet<CommentComp>();
        }

        public int Id { get; set; }
        public int Type { get; set; }
        public string Value { get; set; }

        public virtual ICollection<CallHistory> CallHistories { get; set; }
        public virtual ICollection<ClientContactInfoComp> ClientContactInfoComps { get; set; }
        public virtual ICollection<CommentComp> CommentComps { get; set; }
    }
}
