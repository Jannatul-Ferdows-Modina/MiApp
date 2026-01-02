using System;
using AppMGL.DTO.DataManagement;

namespace AppMGL.DTO.DataManagement
{
    public class LGVWSIPLContactDTO
    {
        public int ContactID { get; set; }
        public string CompanyName { get; set; }
        public string TelNo { get; set; }
        public string Address { get; set; }
        public string MobNo { get; set; }
        public string Fax { get; set; }
        public string WebSite { get; set; }
        public int? CityID { get; set; }
        public string ContactPerson { get; set; }
        public string TaxID { get; set; }
        public string Email { get; set; }
        public string ZipCode { get; set; }
        public string Remarks { get; set; }
        public int? StateID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Flag { get; set; }
        public string AccountDetail { get; set; }
        public string AccountNo { get; set; }
        public string Attachment { get; set; }
        public string CompanyGradation { get; set; }
        public string CustomerCode { get; set; }
        public string ContactCategoryID { get; set; }
        public string RepresentativeID { get; set; }
        public int? ContinentId { get; set; }
        public int? CountryId { get; set; }
        public int? BillTo { get; set; }
        public string BillToCompanyName { get; set; }
        public string BillToAddress { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }

    }
}
    