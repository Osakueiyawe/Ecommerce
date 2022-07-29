namespace Ecommerce_API.Models
{
    public class Item
    {
        public int itemid { get; set; }
        public string itemname { get; set; }
        public string itemdescription { get; set; }
        public int category { get; set; }
        public string categoryname { get; set; }
        public decimal itemunitprice { get; set; }
        public string imagestring { get; set; }
    }
}
