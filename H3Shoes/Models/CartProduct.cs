namespace H3Shoes.Models
{
    public class CartProduct
    {
        public int ID { get; set; }
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
        public double Summary { get; set; }
        public int SalePercent { get; set; }
    }
}
