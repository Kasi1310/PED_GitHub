#region Using

using System.ComponentModel.DataAnnotations;

#endregion

namespace WiseX.ViewModels.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}