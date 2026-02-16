#region Using

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

#endregion

namespace WiseX.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        [Required]       
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string Currentpassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Newpassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Newpassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string Confirmpassword { get; set; }
        public IdentityResult Result;
        public string Message { get; set; }
    } 
}
