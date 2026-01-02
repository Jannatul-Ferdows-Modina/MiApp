using System;

namespace AppMGL.DTO.Setup
{
    public class TitleDTO
    {
        public decimal TtlId { get; set; }
        public string TtlName { get; set; }
        public bool? TtlStatus { get; set; }
        public DateTime? TtlCreatedTs { get; set; }
        public DateTime? TtlUpdatedTs { get; set; }
        public decimal? TtlCreatedBy { get; set; }
        public decimal? TtlUpdatedBy { get; set; }
    }
}