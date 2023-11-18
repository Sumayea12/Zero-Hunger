using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs
{
    public class RegisteredRestaurantDTO
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public string RestaurantName { get; set; }
        public string Location { get; set; }
        public string ContactNumber { get; set; }
    }
}