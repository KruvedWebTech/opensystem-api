namespace opensystem_api.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
      
        public DateTime CreatedAt { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
