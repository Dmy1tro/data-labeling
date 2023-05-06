namespace DataLabeling.Entities
{
    public class Data : AuditEntity
    {
        public int Id { get; set; }

        public string RawImageSource { get; set; }
        public string RawImageHash { get; set; }

        public string Variant { get; set; }
        public string LabeledImageSource { get; set; }
        public string LabeledXmlSource { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int? PerformerId { get; set; }
        public Performer Performer { get; set; }
    }
}
