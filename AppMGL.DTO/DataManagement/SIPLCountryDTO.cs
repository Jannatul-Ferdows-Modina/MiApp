using System;

namespace AppMGL.DTO.DataManagement
{
    public class SIPLCountryDTO
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public int? ContinentID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Flag { get; set; }

        public string ContinentName { get; set; }
        
        public ContinentDTO Continent { get; set; }
    }
}
