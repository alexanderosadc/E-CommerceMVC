using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.CustomAttributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        public string FileListName;
        public int MaxFileSize { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var fileListProperty = validationContext.ObjectType.GetProperty(FileListName);
            if(fileListProperty == null)
                throw new ArgumentException("Property with this name not found");

            var files = fileListProperty.GetValue(validationContext.ObjectInstance) as List<IFormFile>;
            foreach (var item in files)
            {
                var file = item as IFormFile;
                var extension = Path.GetExtension(file.FileName);
                if (file != null)
                {
                    if (file.Length > MaxFileSize)
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is { MaxFileSize } bytes.";
        }
    }
}
