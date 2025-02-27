using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class Comment
    {
        public Comment()
        {
            CommentComps = new HashSet<CommentComp>();
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreateTimestamp { get; set; }
        public string Creator { get; set; }
        public string FullName { get; set; }

        public virtual ICollection<CommentComp> CommentComps { get; set; }
    }
}
