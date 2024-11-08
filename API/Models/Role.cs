using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        [Required, MaxLength(50)]
        public string RoleName { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}