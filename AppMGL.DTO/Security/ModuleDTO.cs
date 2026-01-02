using System;

namespace AppMGL.DTO.Security
{
    public class ModuleDTO
    {
        public decimal ModId { get; set; }
        public decimal MteId { get; set; }
        public string ModName { get; set; }
        public string ModCaption { get; set; }
        public DateTime? ModCreatedTs { get; set; }
        public DateTime? ModUpdatedTs { get; set; }
        public decimal? ModCreatedBy { get; set; }
        public decimal? ModUpdatedBy { get; set; }
    }
}