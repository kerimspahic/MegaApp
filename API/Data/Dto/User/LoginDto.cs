using System.ComponentModel.DataAnnotations;

namespace API.Data.Dto.User
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }  
        [Required]
        public string Password { get; set; } 
    }
}