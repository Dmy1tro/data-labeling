using DataLabeling.Services.Interfaces.Data.Models;

namespace DataLabeling.Services.Data.Models
{
    public class DataModel : IDataModel
    {
        public int Id { get; set; }

        public string RawImageSource { get; set; }
        public string RawImageHash { get; set; }

        public string LabeledImageSource { get; set; }
        public string Variant { get; set; }

        public int OrderId { get; set; }

        public int? PerformerId { get; set; }
    }
}
