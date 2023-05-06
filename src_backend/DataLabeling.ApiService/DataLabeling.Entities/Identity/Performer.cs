using DataLabeling.Common.Shared;
using System.Collections.Generic;

namespace DataLabeling.Entities
{
    public class Performer : User
    {
        public Rating Rating { get; set; }

        public decimal Balance { get; set; }

        public ICollection<Data> CompletedJobs { get; set; }

        public ICollection<JobPayment> JobPayments { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
