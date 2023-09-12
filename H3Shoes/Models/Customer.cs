namespace H3Shoes.Models
{
    public class Customer
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string ShowName { get; set; }
        public List<Role> Roles { get; set; }
        public string PhoneNumber { get; set; }
        public int Category { get; set; }
    }
}
