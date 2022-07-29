using Ecommerce_API.Models;

namespace Ecommerce_API.Methods
{
    public interface ICategoryAndItem
    {
        Task<List<CategoryResponse>> GetCategoriesAsync();
        Task<List<Item>> GetItemByCategoryAsync(int categoryid);        
    }
}