using System;

namespace AppMGL.DTO.Security
{
    public class SiteRoleDTO
    {
        public decimal ScrId { get; set; }
        public decimal? SitId { get; set; }
        public decimal CntId { get; set; }
        public decimal RleId { get; set; }
        public DateTime? ScrCreatedTs { get; set; }
        public DateTime? ScrUpdatedTs { get; set; }
        public decimal ScrCreatedBy { get; set; }
        public decimal ScrUpdatedBy { get; set; }

        public string FullName { get; set; }
        public string RleName { get; set; }
    }
}