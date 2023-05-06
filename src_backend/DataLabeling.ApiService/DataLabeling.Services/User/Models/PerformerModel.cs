using DataLabeling.Common.Shared;
using DataLabeling.Services.Interfaces.User.Models;

namespace DataLabeling.Services.User.Models
{
    public class PerformerModel : UserModel, IPerformerModel
    {
        public decimal Balace { get; set; }

        public Rating Rating { get; set; }
    }
}
