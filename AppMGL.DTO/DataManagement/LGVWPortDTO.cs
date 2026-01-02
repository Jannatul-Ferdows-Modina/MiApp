using System;

namespace AppMGL.DTO.DataManagement
{
    public class LGVWPortDTO
    {
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public int? PortId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int? fkCountryId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool Flag { get; set; }
        public bool IsAirLine { get; set; }
        public string port_code { get; set; }
        public int TotalCount { get; set; }


    }
}