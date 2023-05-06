using DataLabeling.Common.Shared;
using System.Collections.Generic;

namespace DataLabeling.Entities
{
    public class Review : AuditEntity
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int PerformerId { get; set; }
        public Performer Performer { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public Rating Rating { get; set; }

        public string Message { get; set; }
    }
}
