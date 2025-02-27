using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    /// <summary>
    /// SELECT * FROM [CRM].[Classifier].[CodeType] where id = 2
    /// SELECT TOP(1000) * FROM[CRM].[Classifier].[Code] where type_oid = 2
    /// if you need get Tag Collection by Include() from code you must considering up
    /// </summary>
    public partial class Tag
    {
        public Tag()
        {
            Relations = new HashSet<Relation>();
            TagComps = new HashSet<TagComp>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public int? CodeId { get; set; }

        public virtual Code CodeNavigation { get; set; }

        public virtual ICollection<Relation> Relations { get; set; }
        public virtual ICollection<TagComp> TagComps { get; set; }
    }
}