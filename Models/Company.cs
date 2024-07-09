namespace opensystem_api.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string ProfilePic { get; set; }
        public string Name { get; set; }
        
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
