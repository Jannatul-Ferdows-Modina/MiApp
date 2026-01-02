using System;

namespace AppMGL.DTO.DataManagement
{
    public class LGVWHtsCodeDTO
    {
        public int? Id { get; set; }
        public string HTSNumber { get; set; }
        public string description { get; set; }
        public string FirstUOM { get; set; }
        public string SecondUOM { get; set; }
        public int TotalCount { get; set; }
        public string indent { get; set; }
        public int? CreatedBy { get; set; }
        public bool isvin { get; set; }
        public bool isDel { get; set; }
        public string htsfirst { get; set; }
        public string htssecond { get; set; }
        public string htsthird { get; set; }
        public string htsfourth { get; set; }

    }
    public class HtsCode
    {
        public int? Id { get; set; }
        public string HTSNumber { get; set; }
        public string description { get; set; }
        public string FirstUOM { get; set; }
        public string SecondUOM { get; set; }
        public string indent { get; set; }
        public string isvin { get; set; }




    }
}
