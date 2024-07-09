namespace opensystem_api.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
