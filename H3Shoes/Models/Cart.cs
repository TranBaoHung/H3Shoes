namespace H3Shoes.Models
{
    public class Cart
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }
        public double SessionTime { get; set; }
        public bool IsActive { get; set; }
    }
}
