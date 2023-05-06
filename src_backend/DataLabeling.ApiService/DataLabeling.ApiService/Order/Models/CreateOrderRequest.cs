using DataLabeling.Api.Common.Attributes;
using DataLabeling.Common.Order;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataLabeling.ApiService.Order.Models
{
    public class CreateOrderRequest : IValidatableObject
    {
        [Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Requirements { get; set; }

        public int DatSetRequiredCount { get; set; }

        public OrderType Type { get; set; }

        public OrderPriority Priority { get; set; }

        public DateTime Deadline { get; set; }

        public ICollection<string> Variants { get; set; }

        [DataType(DataType.Upload)]
        [AllowedExtension(".jpg", ".jpeg", ".png", ".bmp", ".raw", ".tif", ".tiff")]
        public ICollection<IFormFile> ImageFiles { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            Validator.TryValidateProperty(Name, 
                                          new ValidationContext(this, null, null) { MemberName = nameof(Name) }, 
                                          results);

            Validator.TryValidateProperty(Requirements,
                                          new ValidationContext(this, null, null) { MemberName = nameof(Requirements) },
                                          results);

            if (DatSetRequiredCount <= 0)
            {
                results.Add(new ValidationResult("DatSet count should be greater than zero."));
            }

            if (Deadline <= DateTime.UtcNow)
            {
                results.Add(new ValidationResult("Deadline should be greater than today."));
            }

            if (Type == OrderType.LabelData)
            {
                if (Variants is null || !Variants.Any())
                {
                    results.Add(new ValidationResult("Variants are required."));
                }

                if (ImageFiles is null || !ImageFiles.Any())
                {
                    results.Add(new ValidationResult("Raw image files are required."));
                }
                else
                {
                    if (ImageFiles.Count != DatSetRequiredCount)
                    {
                        results.Add(new ValidationResult("Dataset count mismatch with uploaded files count."));
                    }
                }
            }

            Validator.TryValidateProperty(ImageFiles,
                                          new ValidationContext(this, null, null) { MemberName = nameof(ImageFiles) },
                                          results);

            return results;
        }
    }
}
