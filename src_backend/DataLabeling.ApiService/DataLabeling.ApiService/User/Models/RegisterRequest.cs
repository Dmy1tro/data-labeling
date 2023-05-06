using DataLabeling.Common.User;
using System.ComponentModel.DataAnnotations;

namespace DataLabeling.ApiService.User.Models
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public UserType UserType { get; set; }
    }
}
