using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs
{
    public class DistributionDTO
    {
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public string DistributedLocation { get; set; }
        public System.DateTime Date { get; set; }
        public System.TimeSpan DistributedTime { get; set; }
        public string FoodName { get; set; }
        public string Quantity { get; set; }
        public int EmployeeId { get; set; }

    }
}