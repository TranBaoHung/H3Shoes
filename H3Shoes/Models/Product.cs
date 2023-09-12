namespace H3Shoes.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public int MaxSale { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }
        public bool IsActive { get; set; }
    }
}
