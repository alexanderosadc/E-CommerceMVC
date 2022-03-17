using System.ComponentModel.DataAnnotations;

namespace Ebay.Infrastructure.ViewModels
{
    public class AppUserLogInDTO
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Introduct Username")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Introduce your Password")]
        public string Password { get; set; }
    }
}
