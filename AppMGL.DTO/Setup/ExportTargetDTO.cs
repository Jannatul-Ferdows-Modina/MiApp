using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMGL.DTO.Setup
{
    public class ExportTargetDTO
    {
        public int? TargetId { get; set; }
        public int? FranchiseId { get; set; }
        public string FranchiseName { get; set; }
        public string TargetYear { get; set; }
        public int? TargetJan { get; set; }
        public int? TargetFeb { get; set; }
        public int? TargetMar { get; set; }
        public int? TargetApr { get; set; }
        public int? TargetMay { get; set; }
        public int? TargetJun { get; set; }
        public int? TargetJul { get; set; }
        public int? TargetAug { get; set; }
        public int? TargetSep { get; set; }
        public int? TargetOct { get; set; }
        public int? TargetNov { get; set; }
        public int? TargetDec { get; set; }
        public bool IsDel { get; set; }
        public int TotalCount { get; set; }
        public int? CreatedBy { get; set; }
    }
    public class ExportTargetListDTO
    {
        public int? TargetId { get; set; }
        public string FranchiseName { get; set; }
        public int? TargetYear { get; set; }
        public bool IsDel { get; set; }
        public int TotalCount { get; set; }
    }
}
