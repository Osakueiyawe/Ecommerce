namespace Ecommerce_API.Models
{
    public class CartDetails
    {
        public int itemid { get; set; }
        public string itemname { get; set; }
        public string itemdescription { get; set; }
        public int categoryid { get; set; }
        public string categoryname { get; set; }
        public string daterequested { get; set; }
        public decimal unitprice { get; set; }
        public int quantity { get; set; }
        public decimal totalprice { get; set; }
        public string imagestring { get; set; }
    }
}
