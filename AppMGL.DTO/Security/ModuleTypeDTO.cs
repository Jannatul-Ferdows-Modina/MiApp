using System;

namespace AppMGL.DTO.Security
{
    public class ModuleTypeDTO
    {
        public decimal MteId { get; set; }
        public string MteName { get; set; }
        public string MteCaption { get; set; }
        public DateTime? MteCreatedTs { get; set; }
        public DateTime? MteUpdatedTs { get; set; }
        public decimal? MteCreatedBy { get; set; }
        public decimal? MteUpdatedBy { get; set; }
    }
}