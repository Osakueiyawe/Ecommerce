using Ecommerce_API.Models;
using System.Data;

namespace Ecommerce_API.Methods
{
    public class CategoryAndItem : ICategoryAndItem
    {
        private readonly IDatabaseConnection _databaseconnection;

        public CategoryAndItem(IDatabaseConnection databaseconnection)
        {
            _databaseconnection = databaseconnection;
        }

        public async Task<List<CategoryResponse>> GetCategoriesAsync()
        {
            List<CategoryResponse> categorylist = new List<CategoryResponse>();
            DataTable dt = await _databaseconnection.GetItemCategories();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        CategoryResponse category = new CategoryResponse();
                        category.CategoryId = Convert.ToInt32(dr["CategoryId"].ToString());
                        category.CategoryName = dr["CategoryName"].ToString();
                        categorylist.Add(category);
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
            return categorylist;
        }

        public async Task<List<Item>> GetItemByCategoryAsync(int categoryid)
        {
            List<Item> itemlist = new List<Item>();
            DataTable dt = await _databaseconnection.GetItemByCategory(categoryid);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        Task<byte[]>? bytes = null;
                        Item item = new Item();
                        if (dr["ImagePath"] != null)
                        {
                            bytes = File.ReadAllBytesAsync(dr["ImagePath"].ToString());
                        }
                        item.itemid = Convert.ToInt32(dr["ItemId"].ToString());
                        item.itemname = dr["ItemName"].ToString();
                        if (dr["ItemDescription"] != null)
                        {
                            item.itemdescription = dr["ItemDescription"].ToString();
                        }                        
                        item.category = Convert.ToInt32(dr["Category"].ToString());
                        item.categoryname = dr["CategoryName"].ToString();
                        item.itemunitprice = Convert.ToDecimal(dr["ItemUnitPrice"].ToString());
                        if(bytes != null)
                        {
                            item.imagestring = Convert.ToBase64String(await bytes);
                        }
                        
                        itemlist.Add(item);
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
            return itemlist;
        }
    }
}
