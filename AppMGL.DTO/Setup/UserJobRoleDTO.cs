using System;

namespace AppMGL.DTO.Setup
{
    public class UserJobRoleDTO
    {

        public int? Id { get; set; }
        public string JobRole { get; set; }
        public string JobRoleDesc { get; set; }
        public bool IsDel { get; set; }
        public int TotalCount { get; set; }
        public int? CreatedBy { get; set; }
    }
}