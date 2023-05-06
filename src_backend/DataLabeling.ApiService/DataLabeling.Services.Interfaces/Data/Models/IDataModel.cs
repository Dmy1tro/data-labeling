namespace DataLabeling.Services.Interfaces.Data.Models
{
    public interface IDataModel
    {
        public int Id { get; }

        public string RawImageSource { get; }
        public string RawImageHash { get; }

        public string Variant { get; }
        public string LabeledImageSource { get; }

        public int OrderId { get; }

        public int? PerformerId { get; }
    }
}
