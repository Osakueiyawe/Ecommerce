using Ecommerce_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace Ecommerce_API.Methods
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private IConfiguration _configuration;
        public DatabaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> CheckforExistingUser(NewUserRegistrationRequest userdetails)
        {
            bool result = false;
            try
            {
                SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("EcommerceConnectionstring").Value);
                DataTable dt = new DataTable();
                if (sqlcon.State != ConnectionState.Open)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("checkexistinguser", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@emailaddress", SqlDbType.VarChar).Value = userdetails.emailaddress;
                sqlcmd.Parameters.Add("@phonenumber", SqlDbType.VarChar).Value = userdetails.phonenumber;
                SqlDataAdapter sdap = new SqlDataAdapter(sqlcmd);
                sdap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<bool> CreateNewUser(NewUserRegistrationRequest newuser)
        {
            bool result = false;
            try
            {
                SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("EcommerceConnectionstring").Value);
                
                if (sqlcon.State != ConnectionState.Open)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("createnewuser", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@name", SqlDbType.VarChar).Value = newuser.name;
                sqlcmd.Parameters.Add("@emailaddress", SqlDbType.VarChar).Value = newuser.emailaddress;
                sqlcmd.Parameters.Add("@phonenumber", SqlDbType.VarChar).Value = newuser.phonenumber;
                sqlcmd.Parameters.Add("@city", SqlDbType.VarChar).Value = newuser.city;
                sqlcmd.Parameters.Add("@state", SqlDbType.VarChar).Value = newuser.state;
                sqlcmd.Parameters.Add("@country", SqlDbType.VarChar).Value = newuser.country;
                sqlcmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = newuser.gender;
                sqlcmd.Parameters.Add("@password", SqlDbType.VarChar).Value = newuser.password;
                sqlcmd.ExecuteNonQuery();                
                result = true;               

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<int> CheckUserDetails(LoginRequest logindetails)
        {
            int result = 0;
            try
            {
                SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("EcommerceConnectionstring").Value);
                DataTable dt = new DataTable();
                if (sqlcon.State != ConnectionState.Open)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("getuserdetails", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@username", SqlDbType.VarChar).Value = logindetails.username;
                sqlcmd.Parameters.Add("@password", SqlDbType.VarChar).Value = logindetails.password;
                SqlDataAdapter sdap = new SqlDataAdapter(sqlcmd);
                sdap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = Convert.ToInt32(dt.Rows[0]["Userid"].ToString());
                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<bool> LogLogindetails(LoginDetails newlogin)
        {
            bool result = false;
            try
            {
                SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("EcommerceConnectionstring").Value);

                if (sqlcon.State != ConnectionState.Open)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("createlogindetails", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@userid", SqlDbType.Int).Value = newlogin.userid;
                sqlcmd.Parameters.Add("@logintime", SqlDbType.DateTime).Value = newlogin.logintime;                
                sqlcmd.ExecuteNonQuery();
                result = true;

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<DataTable> GetItemCategories()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("EcommerceConnectionstring").Value);
                
                if (sqlcon.State != ConnectionState.Open)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("Getcategories", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;                
                SqlDataAdapter sdap = new SqlDataAdapter(sqlcmd);
                sdap.Fill(dt);           

            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public async Task<DataTable> GetItemByCategory(int categoryid)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("EcommerceConnectionstring").Value);

                if (sqlcon.State != ConnectionState.Open)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("getitembycategory", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@categoryid", SqlDbType.Int).Value = categoryid;
                SqlDataAdapter sdap = new SqlDataAdapter(sqlcmd);
                sdap.Fill(dt);

            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public async Task<bool> AddToCart(CartRequest cartdetails)
        {
            int categoryid = 0;
            bool result = false;
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("EcommerceConnectionstring").Value);

                if (sqlcon.State != ConnectionState.Open)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd1 = new SqlCommand("getcategorybyitem", sqlcon);
                sqlcmd1.CommandType = CommandType.StoredProcedure;
                sqlcmd1.Parameters.Add("@itemid", SqlDbType.Int).Value = cartdetails.itemid;
                SqlDataAdapter sdap = new SqlDataAdapter(sqlcmd1);
                sdap.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    categoryid = Convert.ToInt32(dt.Rows[0]["Category"].ToString());
                    SqlCommand sqlcmd = new SqlCommand("addtocart", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@itemid", SqlDbType.Int).Value = cartdetails.itemid;
                    sqlcmd.Parameters.Add("@ordertime", SqlDbType.DateTime).Value = DateTime.Now;
                    sqlcmd.Parameters.Add("@quantity", SqlDbType.Int).Value = cartdetails.quantity;
                    sqlcmd.Parameters.Add("@categoryid", SqlDbType.Int).Value = categoryid;
                    sqlcmd.Parameters.Add("@userid", SqlDbType.Int).Value = cartdetails.userId;
                    sqlcmd.ExecuteNonQuery();
                    result = true;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<DataTable> GetCartDetails(int userid)
        {            
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("EcommerceConnectionstring").Value);

                if (sqlcon.State != ConnectionState.Open)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("getcartdetails", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@userid", SqlDbType.Int).Value = userid;                
                SqlDataAdapter sdap = new SqlDataAdapter(sqlcmd);
                sdap.Fill(dt);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public async Task<DataTable> GetOrderDetails(OrderRequest orderdetails)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("EcommerceConnectionstring").Value);

                if (sqlcon.State != ConnectionState.Open)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("getcartdetailsfororder", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@cartid", SqlDbType.Int).Value = orderdetails.cartId;
                SqlDataAdapter sdap = new SqlDataAdapter(sqlcmd);
                sdap.Fill(dt);

            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public async Task<DataTable> GetAccountBalance(int userid)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("EcommerceConnectionstring").Value);

                if (sqlcon.State != ConnectionState.Open)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("getaccountbalance", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@userid", SqlDbType.Int).Value = userid;
                SqlDataAdapter sdap = new SqlDataAdapter(sqlcmd);
                sdap.Fill(dt);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }


        public async Task<bool> PostTransactionDetails(TransactionDetails transdetails)
        {
            bool result = false;
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("EcommerceConnectionstring").Value);

                if (sqlcon.State != ConnectionState.Open)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("posttransactiondetails", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@userid", SqlDbType.Int).Value = transdetails.userid;
                sqlcmd.Parameters.Add("@totalamount", SqlDbType.Decimal).Value = transdetails.amount;
                sqlcmd.Parameters.Add("@creditamount", SqlDbType.Decimal).Value = transdetails.creditamount;
                sqlcmd.Parameters.Add("@debitamount", SqlDbType.Decimal).Value = transdetails.debitamount;
                sqlcmd.Parameters.Add("@transactiontime", SqlDbType.DateTime).Value = transdetails.transactiontime;                
                sqlcmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<bool> UpdateAccountBalance(int userid, decimal amount)
        {
            bool result = false;
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("EcommerceConnectionstring").Value);

                if (sqlcon.State != ConnectionState.Open)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("updateaccountbalance", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@balance", SqlDbType.Decimal).Value = amount;
                sqlcmd.Parameters.Add("@userid", SqlDbType.Int).Value = userid;                
                sqlcmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<bool> UpdateCartTable(int cartid)
        {

            bool result = false;
            try
            {
                SqlConnection sqlcon = new SqlConnection(_configuration.GetSection("EcommerceConnectionstring").Value);

                if (sqlcon.State != ConnectionState.Open)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("updatecartdetails", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@cartid", SqlDbType.BigInt).Value = cartid;
                sqlcmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
