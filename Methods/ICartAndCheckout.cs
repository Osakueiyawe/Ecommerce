using Ecommerce_API.Models;

namespace Ecommerce_API.Methods
{
    public interface ICartAndCheckout
    {
        Task<CartResponse> AddToCart(CartRequest cartdetails);
        Task<List<CartDetails>> GetCartDetails(int userid);
        Task<CartResponse> InsertOrderdetails(List<OrderRequest> orderdetails);
    }
}