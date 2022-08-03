﻿using System.ComponentModel.DataAnnotations;

namespace Authenticate.ViewModels
{
    public class Register
    {
        [Key]
        public int UserID { set; get; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


        public string FullName { set; get; }
        public string Mobile { set; get; }
        public int Role { set; get; }
    }
}
