using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs
{
    public class CollectionRequestDTO
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string FoodName { get; set; }
        public string Quantity { get; set; }
        public string Status { get; set; }
        public System.DateTime Date { get; set; }
        public System.TimeSpan MaxPreservationTime { get; set; }
        public System.TimeSpan CollectTime { get; set; }
        public Nullable<int> employeeId { get; set; }
        public int userid { get; set; }

    }
}