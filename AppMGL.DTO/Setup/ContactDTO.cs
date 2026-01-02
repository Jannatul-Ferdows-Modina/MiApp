using System;

namespace AppMGL.DTO.Setup
{
    public class ContactDTO
    {
        public decimal CntId
        {
            get;
            set;
        }

        public decimal DptId
        {
            get;
            set;
        }

        public decimal TtlId
        {
            get;
            set;
        }

        public decimal CntType
        {
            get;
            set;
        }

        public string CntFirstName
        {
            get;
            set;
        }

        public string CntMiddleName
        {
            get;
            set;
        }

        public string CntLastName
        {
            get;
            set;
        }

        public string CntEmail
        {
            get;
            set;
        }

        public bool? CntStatus
        {
            get;
            set;
        }

        public string CntReason
        {
            get;
            set;
        }

        public DateTime? CntCreatedTs
        {
            get;
            set;
        }

        public DateTime? CntUpdatedTs
        {
            get;
            set;
        }

        public decimal CwtId
        {
            get;
            set;
        }

        public decimal? CntCreatedBy
        {
            get;
            set;
        }

        public decimal? CntUpdatedBy
        {
            get;
            set;
        }

        public string CntImageName
        {
            get;
            set;
        }

        public string FullName
        {
            get;
            set;
        }

        public int? TotalCount
        {
            get;
            set;
        }
        public string Address { get; set; }
        public string CellNo { get; set; }
        public string ContactPerson { get; set; }
        public string Fax { get; set; }
        public string TelNo { get; set; }
        public string ZipCode { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? ContinentId { get; set; }
        public string userroleid { get; set; }
        
    }
}