using DataLabeling.Services.Interfaces.Data.Models;

namespace DataLabeling.ApiService.Data.Models
{
    public class DataWithImageResponse
    {
        public IDataModel Data { get; set; }

        public byte[] Image { get; set; }

        public string ContentType { get; set; }
    }
}
