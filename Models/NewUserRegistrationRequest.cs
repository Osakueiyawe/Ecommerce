using System.ComponentModel.DataAnnotations;

namespace Ecommerce_API.Models
{
    public class NewUserRegistrationRequest
    {        
        public string name { get; set; }
        [Required]
        [EmailAddress]
        public string emailaddress { get; set; }
        [Required]
        public string phonenumber { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string gender { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string confirmpassword { get; set; }
    }
}
