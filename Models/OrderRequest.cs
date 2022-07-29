namespace Ecommerce_API.Models
{
    public class OrderDetails
    {
        public int cartid { get; set; }
        public int userid { get; set; }
        public decimal amount { get; set; }
        public int itemid { get; set; }
        public int categoryid { get; set; }
        public int orderquantity { get; set; }
        public int transactionid { get; set; }
        public string address { get; set; }
    }
    public class OrderRequest
    {
        public int cartId { get; set; }
        public string address { get; set; }
    }

    public class TransactionDetails
    {
        public int userid { get; set; }
        public decimal amount { get; set; }
        public decimal creditamount { get; set; }
        public decimal debitamount { get; set; }
        public DateTime transactiontime { get; set; }
    }
}
