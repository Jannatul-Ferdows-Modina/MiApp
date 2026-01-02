using System;

namespace AppMGL.DTO.Setup
{
    public class DepartmentDTO
    {
        public decimal DptId { get; set; }
        public string DptName { get; set; }
        public bool? DptStatus { get; set; }
        public DateTime? DptCreatedTs { get; set; }
        public DateTime? DptUpdatedTs { get; set; }
        public decimal? DptCreatedBy { get; set; }
        public decimal? DptUpdatedBy { get; set; }
    }
}