﻿using System.ComponentModel.DataAnnotations;

namespace Ebay.Presentation.Models.ViewModels
{
    public class AppUserViewModel
    {
        [Required(ErrorMessage = "Introduce your User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Introduce your Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
        public string? ErrorMessage { get; set; }
    }
}
