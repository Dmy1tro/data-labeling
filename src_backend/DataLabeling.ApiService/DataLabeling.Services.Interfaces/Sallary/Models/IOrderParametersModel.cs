using DataLabeling.Common.Order;
using System;

namespace DataLabeling.Services.Interfaces.Sallary.Models
{
    public interface IOrderParametersModel
    {
        public int DatSetRequiredCount { get; }

        public OrderType Type { get; }

        public OrderPriority Priority { get; }

        public DateTime Deadline { get; }
    }
}
