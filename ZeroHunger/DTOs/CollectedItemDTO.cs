using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeroHunger.EF;

namespace ZeroHunger.DTOs
{
    public class CollectedItemDTO
    {
        public int Id { get; set; }
        public int ReqId { get; set; }
        public string FoodName { get; set; }
        public string Quantity { get; set; }
        public int EmployeeId { get; set; }
        public System.TimeSpan StoreTime { get; set; }
        public System.TimeSpan ExpireTime { get; set; }
 
    }
}