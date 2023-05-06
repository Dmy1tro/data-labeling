using DataLabeling.Common.User;
using System.ComponentModel.DataAnnotations;

namespace DataLabeling.ApiService.User.Models
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public UserType UserType { get; set; }
    }
}
