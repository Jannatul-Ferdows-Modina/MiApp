using System;

namespace AppMGL.DTO.DataManagement
{
   public  class LGVWModeofTransportDTO
    {
        public int? TransportModeID { get; set; }
        public string TransportMode { get; set; }
        public string TransportationCode { get; set; }
        public string TransCode { get; set; }
        public int TotalCount { get; set; }
        public bool IsActive { get; set; }
        
    }
}
