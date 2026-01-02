using System;

namespace AppMGL.DTO.DataManagement
{
   public  class LGVWExportInformationDTO
    {
        public int? Id { get; set; }
        public string Eic { get; set; }
        public string Description { get; set; }
        public int TotalCount { get; set; }
        public bool IsActive { get; set; }
        
    }
}
