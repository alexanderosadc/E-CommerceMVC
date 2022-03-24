using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.CustomAttributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        public string FileListName;
        public string[] Extensions;
        

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var fileListProperty = validationContext.ObjectType.GetProperty(FileListName);
            if (fileListProperty == null)
                throw new ArgumentException("Property with this name not found");

            var files = fileListProperty.GetValue(validationContext.ObjectInstance) as List<IFormFile>;
            if (files == null)
                throw new ArgumentException("Files is null");
            foreach (var item in files)
            {
                var file = item as IFormFile;
                var extension = Path.GetExtension(file.FileName);
                if (file != null)
                {
                    if (!Extensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Your image's filetype is not valid.";
        }
    }
}
