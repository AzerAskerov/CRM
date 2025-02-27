using System;
using System.Collections.Generic;

namespace CRM.Data.Database
{
    public partial class Position
    {
        public Position()
        {
            PhysicalPeople = new HashSet<PhysicalPerson>();
        }

        public int Id { get; set; }
        public string PositionName { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Category { get; set; }
        public virtual ICollection<PhysicalPerson> PhysicalPeople { get; set; }

        public override string ToString()
        {
            return PositionName;
        }
    }
}
