using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMGL.DTO.DataManagement
{
    public class DDTCUSMLDTO
    {
        public int? Id { get; set; }
        public string ddtcusmlcode { get; set; }
        public string ddtcusmldesc { get; set; }
        public int TotalCount { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
    }
}
