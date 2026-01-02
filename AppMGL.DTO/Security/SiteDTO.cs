using System;
using AppMGL.DTO.Setup;

namespace AppMGL.DTO.Security
{
    public class SiteDTO
    {
        public decimal SitId { get; set; }
        public decimal LcnId { get; set; }
        public decimal SitType { get; set; }
        public string SitName { get; set; }
        public string SitCode { get; set; }
        public bool? SitStatus { get; set; }
        public string SitReason { get; set; }
        public DateTime? SitCreatedTs { get; set; }
        public DateTime? SitUpdatedTs { get; set; }
        public decimal? SitCreatedBy { get; set; }
        public decimal? SitUpdatedBy { get; set; }
        
        public long? ROWNUM { get; set; }
        public int? TotalCount { get; set; }
        public LocationDTO Location { get; set; }
    }
}