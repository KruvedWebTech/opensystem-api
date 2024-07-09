namespace opensystem_api.Models
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int CompanyId { get; set; } // Add this field if your User model requires it
    }
}
