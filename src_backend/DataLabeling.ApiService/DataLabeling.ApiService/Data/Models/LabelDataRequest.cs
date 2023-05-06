using DataLabeling.Api.Common.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DataLabeling.ApiService.Data.Models
{
    public class LabelDataRequest
    {
        public int DataId { get; set; }

        [Required]
        public string Variant { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [AllowedExtension(".jpg", ".jpeg", ".png", ".bmp", ".raw", ".tif", ".tiff")]
        public IFormFile ImageFile { get; set; }
    }
}
