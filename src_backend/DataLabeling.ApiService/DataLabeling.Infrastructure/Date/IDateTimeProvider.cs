using System;

namespace DataLabeling.Infrastructure.Date
{
    public interface IDateTimeProvider
    {
        public DateTime UtcNow { get; }
    }
}
