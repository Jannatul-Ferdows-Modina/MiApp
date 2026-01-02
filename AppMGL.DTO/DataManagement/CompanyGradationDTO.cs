using System;

namespace AppMGL.DTO.DataManagement
{
    public class CompanyGradationDTO
    {
        public int GradationID { get; set; }
        public string CompanyGradation { get; set; }
        public string Remark { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime? CreatedOn { get; set; }
        public System.DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
    }

}