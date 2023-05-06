using DataLabeling.Common.Order;
using DataLabeling.Services.Interfaces.Sallary.Models;
using System;
using System.Collections.Generic;

namespace DataLabeling.Services.Sallary.Models
{
    public class OrderParametersModel : IOrderParametersModel
    {
        public int DatSetRequiredCount { get; set; }

        public OrderType Type { get; set; }

        public OrderPriority Priority { get; set; }

        public DateTime Deadline { get; set; }
    }
}
