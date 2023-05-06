using DataLabeling.Common.Shared;

namespace DataLabeling.ApiService.User.Models
{
    public class PerformerModelResponse
    {
        public string Email { get; set; }

        public string FullName { get; set; }

        public int UserType { get; set; }

        public string Roles { get; set; }

        public decimal Balance { get; set; }

        public Rating Rating { get; set; }
    }
}
