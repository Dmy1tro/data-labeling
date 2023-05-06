namespace DataLabeling.Entities
{
    public class JobPayment : AuditEntity
    {
        public int Id { get; set; }

        public int PerformerId { get; set; }
        public Performer Performer { get; set; }

        public string BankCardNumber { get; set; }

        public decimal Price { get; set; }
    }
}
