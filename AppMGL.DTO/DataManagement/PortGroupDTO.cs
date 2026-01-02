using System;

namespace AppMGL.DTO.DataManagement
{
    public class PortGroupDTO
    {

        public int? PortGroupId { get; set; }
        public string Name { get; set; }
        public string ShortCode { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool Flag { get; set; }


    }
}