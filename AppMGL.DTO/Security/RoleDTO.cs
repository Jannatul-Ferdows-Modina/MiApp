using System;

namespace AppMGL.DTO.Security
{
    public class RoleDTO
    {
        public decimal RleId { get; set; }
        public string RleName { get; set; }
        public bool? RleStatus { get; set; }
        public DateTime? RleCreatedTs { get; set; }
        public DateTime? RleUpdatedTs { get; set; }
        public bool? RleDisplaly { get; set; }
        public string RleType { get; set; }
        public decimal? RleCreatedBy { get; set; }
        public decimal? RleUpdatedBy { get; set; }

        public int? TotalCount { get; set; }

    }
}