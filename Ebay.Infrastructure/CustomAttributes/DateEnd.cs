using Ebay.Infrastructure.ViewModels.Admin.Index;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.CustomAttributes
{
    public sealed class DateEnd : ValidationAttribute
    {
        //public string DateStartProperty { get; set; }
        public string DateStartPropertyName { get; set; }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime endDate = (DateTime)value;

            var startDateProperty = validationContext.ObjectType.GetProperty(DateStartPropertyName);

            if (startDateProperty == null)
                throw new ArgumentException("Property with this name not found");

            var startDate = (DateTime)startDateProperty.GetValue(validationContext.ObjectInstance);

            if (endDate < startDate)
                return new ValidationResult("End Date is smaller than Start Date");

            return ValidationResult.Success;
        }
    }
}
