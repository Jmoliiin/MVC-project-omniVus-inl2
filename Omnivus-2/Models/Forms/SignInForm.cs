using System.ComponentModel.DataAnnotations;

namespace Omnivus_2.Models.Forms
{
    public class SignInForm
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Must cointain a valid email address")] //inbygd validering 
        public string Email { get; set; } = "";

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "this field is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Must cointain a valid password")]
        public string Password { get; set; } = "";

        public string ErrorMessage { get; set; } = "";
        public string ReturnUrl { get; set; } = "/";
    }
}
