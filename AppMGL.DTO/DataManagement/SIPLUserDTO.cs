using System;

namespace AppMGL.DTO.DataManagement
{
 public  class SIPLUserDTO
    {
        public int? UsrId { get; set; }
        public int? CntId { get; set; }
        public string Name { get; set; }
        public int? SitId { get; set; }
    }
    public class SIPLUserDTOList
    {
        public decimal? UsrId { get; set; }
        public decimal? CntId { get; set; }
        public string Name { get; set; }
       public decimal? SitId { get; set; }
    }

}
