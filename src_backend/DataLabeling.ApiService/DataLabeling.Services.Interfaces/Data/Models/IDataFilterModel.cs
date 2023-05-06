namespace DataLabeling.Services.Interfaces.Data.Models
{
    public interface IDataFilterModel
    {
        public int OrderId { get; }

        public int? PerformerId { get; }

        public int? CustomerId { get; }

        public int Skip { get; }

        public int Take { get; }
    }
}
