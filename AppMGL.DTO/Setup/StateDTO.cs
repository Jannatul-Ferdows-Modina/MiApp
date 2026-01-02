using System;

namespace AppMGL.DTO.Setup
{
    public class StateDTO
    {
        public decimal UstId { get; set; }
        public string UstName { get; set; }
        public string UstAbbrev { get; set; }
        public bool? UstStatus { get; set; }
        public DateTime? UstCreatedTs { get; set; }
        public DateTime? UstUpdatedTs { get; set; }
        public decimal? UstCreatedBy { get; set; }
        public decimal? UstUpdatedBy { get; set; }
    }
}