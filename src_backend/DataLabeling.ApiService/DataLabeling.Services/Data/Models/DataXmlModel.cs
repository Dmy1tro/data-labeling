using System;
using System.Xml.Serialization;

namespace DataLabeling.Services.Data.Models
{
    [Serializable]
    public class DataXmlModel
    {
        public DataXmlModel()
        {
        }

        public DataXmlModel(string variant, string imageInBase64)
        {
            Variant = variant;
            ImageInBase64 = imageInBase64;
        }

        [XmlElement("Variant")]
        public string Variant { get; set; }

        [XmlElement("ImageBytesInBase64")]
        public string ImageInBase64 { get; set; }
    }
}
