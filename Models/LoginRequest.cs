using System.ComponentModel.DataAnnotations;

namespace Ecommerce_API.Models
{
    public class LoginRequest
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
