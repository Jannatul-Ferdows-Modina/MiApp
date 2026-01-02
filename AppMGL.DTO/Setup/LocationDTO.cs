using System;

namespace AppMGL.DTO.Setup
{
    public class LocationDTO
    {
        public decimal LcnId { get; set; }
        public decimal? CntId { get; set; }
        public decimal? TmzId { get; set; }
        public decimal? CcyId { get; set; }
        public string LcnInstitutionName { get; set; }
        public string LcnContactRole { get; set; }
        public string LcnAddress1 { get; set; }
        public string LcnAddress2 { get; set; }
        public string LcnAddress3 { get; set; }
        public string LcnCity { get; set; }
        public string LcnPostalCode { get; set; }
        public string LcnForiegnProvince { get; set; }
        public decimal? UstId { get; set; }
        public decimal? CryId { get; set; }
        public string LcnLatitute { get; set; }
        public string LcnLongitude { get; set; }
        public string LcnPhone { get; set; }
        public string LcnPhoneExt { get; set; }
        public string LcnFax { get; set; }
        public string LcnMobilePhone { get; set; }
        public decimal? LcnStatus { get; set; }
        public string LcnReason { get; set; }
        public DateTime? LcnCreatedTs { get; set; }
        public DateTime? LcnUpdatedTs { get; set; }
        public decimal? LcnCreatedBy { get; set; }
        public decimal? LcnUpdatedBy { get; set; }
        public string CntTitle { get; set; }
        public string CntFirstName { get; set; }
        public string CntLastName { get; set; }
        public string CntMiddleName { get; set; }
        public string CntAddress { get; set; }
        public string CntCellNo { get; set; }
        public string CntFax { get; set; }
        public string CntTelNo { get; set; }
        public int? CntContinentId { get; set; }
        public int? CntCountryId { get; set; }
        public int? CntStateId { get; set; }
        public int? CntCityId { get; set; }
        public string CntZipCode { get; set; }
        public string CntEmail { get; set; }
        public string CntWebsite { get; set; }
    }
}