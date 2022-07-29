using Ecommerce_API.Models;
using System.Data;

namespace Ecommerce_API.Methods
{
    public class CartAndCheckout : ICartAndCheckout
    {
        private readonly IDatabaseConnection _connection;

        public CartAndCheckout(IDatabaseConnection connection)
        {
            _connection = connection;
        }

        public async Task<CartResponse> AddToCart(CartRequest cartdetails)
        {
            CartResponse cart = new CartResponse();
            try
            {
                bool insertresult = await _connection.AddToCart(cartdetails);
                if (insertresult)
                {
                    cart.responsecode = "00";
                    cart.responsemessage = "successful";
                }
                else
                {
                    cart.responsecode = "01";
                    cart.responsemessage = "unsuccessful";
                }
            }
            catch (Exception ex)
            {
                cart.responsecode = "02";
                cart.responsemessage = "server error";
            }
            return cart;
        }

        public async Task<List<CartDetails>> GetCartDetails(int userid)
        {
            List<CartDetails> cartDetails = new List<CartDetails>();
            try
            {
                DataTable insertresult = await _connection.GetCartDetails(userid);
                if (insertresult.Rows.Count > 0)
                {
                    foreach (DataRow row in insertresult.Rows)
                    {
                        CartDetails cart = new CartDetails();
                        try
                        {
                            cart.itemid = Convert.ToInt32(row["ItemId"].ToString());
                            cart.itemname = row["ItemName"].ToString();
                            cart.itemdescription = row["ItemDescription"].ToString();
                            cart.categoryid = Convert.ToInt32(row["CategoryId"].ToString());
                            cart.categoryname = row["CategoryName"].ToString();
                            cart.unitprice = Convert.ToDecimal(row["ItemUnitPrice"].ToString());
                            cart.quantity = Convert.ToInt32(row["Quantity"].ToString());
                            cart.totalprice = (decimal)cart.quantity * cart.unitprice;
                            cart.daterequested = row["OrderTime"].ToString();
                            if (row["ImagePath"] != null)
                            {
                                string filepath = row["ImagePath"].ToString();
                                byte[] bytes = await File.ReadAllBytesAsync(filepath);
                                cart.imagestring = Convert.ToBase64String(bytes);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        cartDetails.Add(cart);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return cartDetails;
        }

        public async Task<CartResponse> InsertOrderdetails(List<OrderRequest> orderdetails)
        {  
            CartResponse response = new CartResponse();
            foreach (OrderRequest orderRequest in orderdetails)
            {
                try
                {
                    decimal totalamount = 0;
                    decimal accountbalance = 0;
                    int userid = 0;
                    OrderDetails order = new OrderDetails();
                    DataTable dt = await _connection.GetOrderDetails(orderRequest);
                    if (dt.Rows.Count > 0)
                    {
                        totalamount = (decimal)Convert.ToInt32(dt.Rows[0]["Quantity"].ToString()) * Convert.ToDecimal(dt.Rows[0]["ItemUnitPrice"].ToString());
                        userid = Convert.ToInt32(dt.Rows[0]["UserId"].ToString());
                    }                    
                    DataTable dt1 = await _connection.GetAccountBalance(userid);
                    if (dt1.Rows.Count > 0)
                    {
                        accountbalance = Convert.ToDecimal(dt1.Rows[0]["AccountBalance"].ToString());
                    }                    
                    if (totalamount > accountbalance || totalamount == 0 || accountbalance == 0)
                    {
                        continue;
                    }
                    TransactionDetails transdetails = new TransactionDetails();
                    transdetails.amount = totalamount;
                    transdetails.userid = userid;
                    transdetails.debitamount = totalamount;
                    transdetails.creditamount = 0;
                    transdetails.transactiontime = DateTime.Now;
                    bool updateaccount = await _connection.UpdateAccountBalance(userid, totalamount);
                    
                    if (updateaccount)
                    {
                        bool insertresult = await _connection.PostTransactionDetails(transdetails);
                        bool updatecart = await _connection.UpdateCartTable(orderRequest.cartId);
                        if (insertresult && updatecart)
                        {
                            response.responsecode = "00";
                            response.responsemessage = "successful";
                        }
                        else
                        {
                            response.responsecode = "01";
                            response.responsemessage = "unsuccessful";
                        }
                    }
                    else
                    {
                        response.responsecode = "01";
                        response.responsemessage = "unsuccessful";
                    }
                }
                catch (Exception ex)
                {

                }                
            }
            return response;
        }
    }
}
