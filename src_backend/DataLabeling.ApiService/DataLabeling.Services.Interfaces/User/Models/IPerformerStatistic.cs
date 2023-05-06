namespace DataLabeling.Services.Interfaces.User.Models
{
    public interface IPerformerStatistic
    {
        public int RawDataCount { get; }

        public int LabelDataCount { get; }

        public int TotalJobs => RawDataCount + LabelDataCount;
    }
}
