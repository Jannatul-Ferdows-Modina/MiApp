using System;

namespace AppMGL.DTO.Security
{
    public class ActionDTO
    {
        public decimal ActId { get; set; }
        public string ActName { get; set; }
        public string ActCaption { get; set; }
        public DateTime? ActCreatedTs { get; set; }
        public DateTime? ActUpdatedTs { get; set; }
        public decimal? ActCreatedBy { get; set; }
        public decimal? ActUpdatedBy { get; set; }
    }
}