using System.Collections.Generic;

namespace DataLabeling.Entities
{
    public class Customer : User
    {
        public ICollection<Order> Orders { get; set; }

        public ICollection<OrderPayment> OrderPayments { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
