using System.ComponentModel.DataAnnotations;

namespace Authenticate.ViewModels
{
    public class Users
    {
        [Key]
        public int UserID { set; get; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
       // [Required]
       // public string Password { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        public string FullName { set; get; }
        public string Mobile { set; get; }
        public int Role { set; get; }
      

    }
}
