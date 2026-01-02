using System;
using AppMGL.DTO.Security;

namespace AppMGL.DTO.Security
{
    public class ContactWorkTypeDTO
    {
        public decimal CwtId { get; set; }
        public string CwtName { get; set; }
        public string CwtStatus { get; set; }
        public DateTime? CntCreatedTs { get; set; }
        public decimal CwtCreatedBy { get; set; }
        public DateTime? CntUpdatedTs { get; set; }
        public decimal CwtUpdatedBy { get; set; }
        public int? TotalCount { get; set; }

    }
}
