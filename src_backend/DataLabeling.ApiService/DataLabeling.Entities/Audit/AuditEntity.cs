using System;

namespace DataLabeling.Entities
{
    public abstract class AuditEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModified { get; set; }
    }
}
