using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.ViewModels.Admin.Users
{
    public class AppUserCreateDTO
    {
        public string? Id { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Introduce Username")]
        
        [Remote(action: "IsUsernameAvialiable", controller: "Validation", 
            HttpMethod = "Get",  
            ErrorMessage = "Username already exist", 
            AdditionalFields = nameof(UserName))]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}", 
            ErrorMessage = "Introduct password with the length of 8 " +
            "- 1 letter, 1 number and 1 symbol")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required, Compare(nameof(Password)), MinLength(6)]
        public string ConfirmedPassword { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool IsModerator { get; set; } = false;
    }
}
