using DataLabeling.Api.Common.Attributes;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLabeling.ApiService.Data.Models
{
    public class UploadRawDataRequest
    {
        public int OrderId { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [AllowedExtension(".jpg", ".jpeg", ".png", ".bmp", ".raw", ".tif", ".tiff")]
        public ICollection<IFormFile> ImageFiles { get; set; }
    }
}
