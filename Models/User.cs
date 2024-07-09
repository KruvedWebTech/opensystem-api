using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace opensystem_api.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public int? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role? Role { get; set; } // Navigation property
        public string? ProfilePic { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? CompanyId { get; set; }
        public Company? Company { get; set; } // Navigation property
        public DateTime CreatedAt { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("RoleId")]
        public ICollection<Role> Roles { get; set; } // Collection navigation property
    }
}
