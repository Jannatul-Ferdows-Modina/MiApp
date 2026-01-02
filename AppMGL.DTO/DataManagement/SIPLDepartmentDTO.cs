using System;

namespace AppMGL.DTO.DataManagement
{
    public class SIPLDepartmentDTO
    {
        public int DepartmentID { get; set; }
        public string Department { get; set; }
        public int? fkTransportModeID { get; set; }
        public int? fkOperationTypeID { get; set; }
        public string Prefix { get; set; }
        public decimal LastEnquiryNo { get; set; }
        public decimal EnquiryControlNo { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
