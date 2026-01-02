using System;

namespace AppMGL.DTO.DataManagement
{
   public  class LGVWLienseexemptionDTO
    {
        public int? Id { get; set; }
        public string LicenseCode { get; set; }
        public string description { get; set; }
        public string licval { get; set; }
        public int TotalCount { get; set; }
        public int? CreatedBy { get; set; }
        public bool isDel { get; set; }
        
    }
}
