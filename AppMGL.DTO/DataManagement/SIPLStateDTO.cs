using System;


namespace AppMGL.DTO.DataManagement
{
    public class SIPLStateDTO
    {
        public int StateId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? fkCountryId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Flag { get; set; }
        public string Abb { get; set; }
}
}
