using Ecommerce_API.Models;
using System.Data;

namespace Ecommerce_API.Methods
{
    public interface IDatabaseConnection
    {
        Task<bool> CheckforExistingUser(NewUserRegistrationRequest userdetails);
        Task<bool> CreateNewUser(NewUserRegistrationRequest newuser);
        Task<int> CheckUserDetails(LoginRequest logindetails);
        Task<bool> LogLogindetails(LoginDetails newlogin);
        Task<DataTable> GetItemCategories();
        Task<DataTable> GetItemByCategory(int categoryid);
        Task<bool> AddToCart(CartRequest cartdetails);
        Task<DataTable> GetCartDetails(int userid);
        Task<DataTable> GetOrderDetails(OrderRequest orderdetails);
        Task<DataTable> GetAccountBalance(int userid);
        Task<bool> PostTransactionDetails(TransactionDetails transdetails);
        Task<bool> UpdateAccountBalance(int userid, decimal amount);
        Task<bool> UpdateCartTable(int cartid);
    }
}