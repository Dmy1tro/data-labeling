using DataLabeling.Services.Interfaces.User.Models;

namespace DataLabeling.Services.User.Models
{
    public class PerformerStatistic : IPerformerStatistic
    {
        public int RawDataCount { get; set; }

        public int LabelDataCount { get; set; }
    }
}
