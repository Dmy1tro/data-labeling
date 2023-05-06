using DataLabeling.Common.Shared;

namespace DataLabeling.Services.Interfaces.User.Models
{
    public interface IPerformerModel : IUserModel
    {
        public decimal Balace { get; }

        public Rating Rating { get; }
    }
}
