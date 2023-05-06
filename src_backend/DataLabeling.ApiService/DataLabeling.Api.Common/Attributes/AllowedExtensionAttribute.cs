using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace DataLabeling.Api.Common.Attributes
{
    public class AllowedExtensionAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionAttribute(params string[] allowedExtensions)
        {
            _extensions = allowedExtensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var files = new List<IFormFile>();

            if (value is null)
            {
                return ValidationResult.Success;
            }

            if (value is IEnumerable<IFormFile> enumerable)
            {
                files.AddRange(enumerable);
            }

            else if (value is Array array && array.OfType<IFormFile>().Any())
            {
                files.AddRange(array.OfType<IFormFile>());
            }

            else if (value is IFormFile file)
            {
                files.Add(file);
            }

            if (!files.Any())
            {
                return new ValidationResult("File(s) have wrong format.");
            }

            var errors = files.Select(f => Validate(f)).Where(r => !r.IsValid).ToList();

            if (errors.Any())
            {
                var errorMessage = string.Join("\n", errors.Select(e => e.Message));

                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }

        private FileValidationResult Validate(IFormFile file)
        {
            if (file is null)
            {
                return new(false, "File not found.");
            }

            var fileExtension = Path.GetExtension(file.FileName);
            var isValidExtension = _extensions.Any(e => e.ToUpper() == fileExtension.ToUpper() || 
                                                        e.ToUpper().Contains(file.ContentType.ToUpper()) ||
                                                        file.ContentType.ToUpper().Contains(e.ToUpper()));

            if (!isValidExtension)
            {
                return new(false, "Invalid file extension.");
            }

            return new(true, string.Empty);
        }

        private record FileValidationResult(bool IsValid, string Message);
    }
}
