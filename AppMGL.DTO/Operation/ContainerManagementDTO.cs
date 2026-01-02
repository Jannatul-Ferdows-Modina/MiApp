using System;
using AppMGL.DTO.DataManagement;
using System.Collections.Generic;

namespace AppMGL.DTO.Operation
{
    public class ContainerManagementDTO
    {
        public int? ContainerId { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerLineNo { get; set; }
        public string ContainerDescription { get; set; }
        public int? SiteId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UserId { get; set; }
        public int? TotalCount { get; set; }
        public string CreatedBy { get; set; }
    }
}
