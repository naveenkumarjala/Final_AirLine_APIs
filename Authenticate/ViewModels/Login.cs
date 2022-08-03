using System.ComponentModel.DataAnnotations;

namespace Authenticate.ViewModels
{
    public class Login
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
