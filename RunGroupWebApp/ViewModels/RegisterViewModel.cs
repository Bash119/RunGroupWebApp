using System.ComponentModel.DataAnnotations;

namespace RunGroupWebApp.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name ="Email address")]
        [Required(ErrorMessage ="Email Address is required")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="Confirm Password")]
        [Required(ErrorMessage ="Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword {  get; set; }
    }
}
