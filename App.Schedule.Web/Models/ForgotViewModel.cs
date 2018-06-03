using System.ComponentModel.DataAnnotations;

namespace App.Schedule.Web.Models
{
    public class ForgotViewModel
    {
        [Required(ErrorMessage = "Please enter your email id")]
        public string Email { get; set; }
    }
}