namespace DataLabeling.Entities
{
    public class OrderPayment : AuditEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public decimal Price { get; set; }
    }
}
