using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs
{
    public class CompositeDTO2
    {
        public List<CollectedItemDTO> CollectedItems { get; set; }
        public List<CollectionRequestDTO> CollectionRequests { get; set; }
        
    }
}