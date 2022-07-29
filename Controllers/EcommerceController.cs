using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecommerce_API.Models;
using Ecommerce_API.Methods;
namespace Ecommerce_API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class EcommerceController : ControllerBase
    {
        private readonly IUserRegistrationAndLogin _userregistration;
        private readonly ICategoryAndItem _categoryAndItem;
        private readonly ICartAndCheckout _cartAndCheckout;
        public EcommerceController(IUserRegistrationAndLogin userregistration, ICategoryAndItem categoryAndItem, ICartAndCheckout cartAndCheckout)
        {
            _userregistration = userregistration;
            _categoryAndItem = categoryAndItem;
            _cartAndCheckout = cartAndCheckout;
        }
        [HttpPost]
        public async Task<IActionResult> Register(NewUserRegistrationRequest newuserdetails)
        {
            if (ModelState.IsValid)
            {
                NewUserRegistrationResponse response = await _userregistration.UserRegistration(newuserdetails);
                return Ok(response);
            }
            else
            {
                NewUserRegistrationResponse response = new NewUserRegistrationResponse();
                response.responsecode = "02";
                response.responsemessage = "Invalid Entry";
                return BadRequest(response);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                LoginResponse loginresponse = await _userregistration.Login(loginRequest);
                return Ok(loginresponse);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetItemCategory()
        {
            if (ModelState.IsValid)
            {
                List<CategoryResponse> categorylist = await _categoryAndItem.GetCategoriesAsync();
                return Ok(categorylist);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetItemByCategory(int categoryid)
        {
            if (categoryid != 0)
            {
                List<Item> itemlist = await _categoryAndItem.GetItemByCategoryAsync(categoryid);
                return Ok(itemlist);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(CartRequest cartdetails)
        {
            if (ModelState.IsValid)
            {
                CartResponse cartresponse = await _cartAndCheckout.AddToCart(cartdetails);
                return Ok(cartresponse);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCartDetails(int userid)
        {
            if (userid != 0)
            {
                List<CartDetails> cartdetails = await _cartAndCheckout.GetCartDetails(userid);
                return Ok(cartdetails);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Order(List<OrderRequest> order)
        {
            if (ModelState.IsValid)
            {
                CartResponse response = await _cartAndCheckout.InsertOrderdetails(order);
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
