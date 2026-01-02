using System;

namespace AppMGL.DTO.Setup
{
    public class TimezoneDTO
    {
        public decimal TmzId { get; set; }
        public string TmzName { get; set; }
        public string TmzOffset { get; set; }
        public string TmzAbbrev { get; set; }
        public string TmzLocation { get; set; }
        public bool? TmzStatus { get; set; }
        public DateTime? TmzCreatedTs { get; set; }
        public DateTime? TmzUpdatedTs { get; set; }
        public decimal? TmzCreatedBy { get; set; }
        public decimal? TmzUpdatedBy { get; set; }
    }
}