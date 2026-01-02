using System;

namespace AppMGL.DTO.Security
{
    public class RoleMapDTO
    {
        public decimal MrmId { get; set; }
        public decimal RleId { get; set; }
        public decimal ModId { get; set; }
        public decimal ActId { get; set; }
        public bool? MamIsEnable { get; set; }
        public DateTime? MrmCreatedTs { get; set; }
        public DateTime? MrmUpdatedTs { get; set; }
        public decimal? MrmCreatedBy { get; set; }
        public decimal? MrmUpdatedBy { get; set; }

        public string ModCaption { get; set; }
        public string ActCaption { get; set; }
    }
}