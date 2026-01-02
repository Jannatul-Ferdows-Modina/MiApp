using System;

namespace AppMGL.DTO.Setup
{
    public class CountryDTO
    {
        public decimal CryId { get; set; }
        public string CryName { get; set; }
        public string CryAbbrev { get; set; }
        public bool? CryStatus { get; set; }
        public DateTime? CryCreatedTs { get; set; }
        public DateTime? CryUpdatedTs { get; set; }
        public decimal? CryPhoneCode { get; set; }
        public decimal? CryCreatedBy { get; set; }
        public decimal? CryUpdatedBy { get; set; }
    }
}