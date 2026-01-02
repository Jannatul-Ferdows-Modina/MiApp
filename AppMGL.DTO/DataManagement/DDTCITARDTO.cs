using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMGL.DTO.DataManagement
{
    public class DDTCITARDTO
    {
        public int? Id { get; set; }
        public string ddtcitarcode { get; set; }
        public string ddtcitardesc { get; set; }
        public int TotalCount { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        
    }
}
