using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMGL.DTO.Document
{
    public class LGShipmentDocsDTO
    {
        //public decimal SdId { get; set; }
        public int DocumentCommonID { get; set; }
        public string SdDocName { get; set; }
        public string sdDocType { get; set; }
        public DateTime SdCreatedTs { get; set; }
        public decimal? SdCreatedBy { get; set; }
        public DateTime SdUpdatedTs { get; set; }
        public decimal? SdUpdatedBy { get; set; }

    }
}
