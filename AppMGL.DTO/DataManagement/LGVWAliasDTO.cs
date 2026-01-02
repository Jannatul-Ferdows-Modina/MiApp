using System;

namespace AppMGL.DTO.DataManagement
{
    public class LGVWAliasDTO
    {
        public int? AliasID { get; set; }
        public string Alias { get; set; }
        public int? AliasReferenceID { get; set; }
        public string Type { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Name { get; set; }

        }
}
