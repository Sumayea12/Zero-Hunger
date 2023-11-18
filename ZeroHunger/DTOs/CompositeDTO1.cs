using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs
{
    public class CompositeDTO1
    {
        public List<CollectionRequestDTO> CollectionRequests { get; set; }
        public List<UserDTO> Users { get; set; }
    }
}