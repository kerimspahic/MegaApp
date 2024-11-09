using System.ComponentModel.DataAnnotations;

namespace API.Data.Dto.Auth
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; } 
        [Required]  
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        
    }
}