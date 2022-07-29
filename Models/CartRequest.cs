namespace Ecommerce_API.Models
{
    public class CartRequest
    {
        public int itemid { get; set; }        
        public int quantity { get; set; }        
        public int userId { get; set; }
    }
}
