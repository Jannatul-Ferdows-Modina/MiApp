using System;

namespace AppMGL.DTO.Operation
{
    public class CustomerContactDTO
    {
        public int? ID { get; set; }
        public int ContactID { get; set; }
        public string Address { get; set; }
        public string CellNo { get; set; }        
        public string CompanyName { get; set; }       
        public string ContactCategoryID { get; set; }
        public string ForwarderNetworkId { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Remarks { get; set; }       
        public string TelNo { get; set; }
        public string WebSite { get; set; }
        public string ZipCode { get; set; }
        public string IDNumberType { get; set; }

        public string IDNumberTypeValue { get; set; }

        public string IDNumber { get; set; }

        public string IRSNumber { get; set; }
        

        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Type { get; set; }
        public string AccountDetail { get; set; }
        public string Attachment { get; set; }
        public string companyGradation { get; set; }
        public string CustomerCode { get; set; }
        public string RepresentativeID { get; set; }       
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public int? StateId { get; set; }
        public string City { get; set; }
        public int? CityId { get; set; }
        public string Continent { get; set; }
        public int? ContinentId { get; set; }
        public string MobNo { get; set; }        
        public string TaxID { get; set; }
        public string AccountNo { get; set; }       
        public bool? IsDeleted { get; set; } 
        public string GalRepresentative { get; set; }
        public string BranchName { get; set; }
        public int? AcyID { get; set; }
        public decimal SiteId { get; set; }
        public AdditionalContactDTO[] AdditionalContactDTOList { get; set; }
        public ContactBranchDetailDTO[] ContactBranchDetailDTOList { get; set; }
        public ContactCommodityDTO[] ContactCommodityDTOList { get; set; }
        public ContactOrigionDTO[] ContactOrigionDTOList { get; set; }
        public int? isconsolidatedreport { get; set; }
        public string SCAC { get; set; }
        public string IATA  { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonLastName { get; set; }
        public string EnquiryUrl { get; set; }
        public string siteids { get; set; }
        public int? isapproved { get; set; }
        public string sourceofcontact { get; set; }
        public string createdfrom { get; set; }
		public string isquickbook { get; set; }
        public string qbid { get; set; }
        public string designation { get; set; }
        public string IsSepatraCustomer { get; set; }
        public string IsSepatraPartner { get; set; }
        public string SepatraCustomer { get; set; }
        public string SepatraCustomerId { get; set; }

        public string contactpersonphone { get; set; }
        public string contactpersonemail { get; set; }

    }

    public class AdditionalContactDTO
    {
        public int ContactID { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string contactEmail { get; set; }
        public string ContactNo { get; set; }
        public string ContactCellNo { get; set; }       

    }
    public class ContactBranchDetailDTO
    {
        public int ContactID { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string BranchCellNo { get; set; }        
        public string BranchContactPerson { get; set; }
        public string BranchEmail { get; set; }
        public string BranchFax { get; set; }
        public string BranchRegion { get; set; }
        public int? BranchRegionID { get; set; }
        public int? BranchCountryID { get; set; }
        public string BranchCountry { get; set; }
        public int? BranchStateID { get; set; }
        public string BranchState { get; set; }
        public int? BranchCityID { get; set; }
        public string BranchCity { get; set; }
        public string BranchTaxID { get; set; }
        public string BranchTelNo { get; set; }
        public string BranchZipCode { get; set; }              

    }
    public class ContactCommodityDTO
    {
        public int ContactID { get; set; }
        public int? CommodityID { get; set; }
       
    }
    public class ContactOrigionDTO
    {
        public int ContactID { get; set; }
        public int? OrigionID { get; set; }
        public string CustomerCode { get; set; }

    }

    public class CustomerContactDetailDTO
    {

        public int ContactID { get; set; }
        public string CompanyName { get; set; }
        public string TelNo { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string AccountDetail { get; set; }
        public string WebSite { get; set; }
        public string ZipCode { get; set; }
        public string MobNo { get; set; }
        public string Fax { get; set; }
        public string TaxID { get; set; }
        public string AccountNo { get; set; }
        public string CustomerCode { get; set; }
        public bool? IsDeleted { get; set; }
        public string Remarks { get; set; }
        public string Attachment { get; set; }
        public string CompanyGradation { get; set; }
        public string RepresentativeID { get; set; }
        public string ContactCategoryID { get; set; }
        public string Continent { get; set; }
        public string State { get; set; }
        public string CountryName { get; set; }        
        public string GalRepresentative { get; set; }
        public string BranchName { get; set; }


    }

    public class AccountCatagoriesDTO
    {
        public int AcyID { get; set; }
        public string AcyName { get; set; }
       
    }
    public class FranchiseEmailData
    {
       
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string Emailcc { get; set; }
        public string EmailBcc { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string CreatedBy { get; set; }
        public string CntEmail { get; set; }
        public string CntName { get; set; }
        public string JobRole { get; set; }
        public string TelNo { get; set; }
        

    }
    public class ContactSycDTO
    {
        public int? ID { get; set; }
        public int ContactID { get; set; }
        public string Address { get; set; }
        public string CellNo { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Remarks { get; set; }
        public string TelNo { get; set; }
        public string WebSite { get; set; }
        public string ZipCode { get; set; }
        public string CustomerCode { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public int? StateId { get; set; }
        public string City { get; set; }
        public int? CityId { get; set; }
        public string MobNo { get; set; }
        public string TaxID { get; set; }
        public decimal SiteId { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonLastName { get; set; }
        public string siteids { get; set; }
        public string isquickbook { get; set; }
        public string qbid { get; set; }

        public string quickCompanyName { get; set; }
        public string quickContactNumber { get; set; }
        public string quickContactPerson { get; set; }
        public string quickEmail { get; set; }
        public string quickCountry { get; set; }
        public string quickCity { get; set; }
        public string quickAddress { get; set; }
        public string quickZipCode { get; set; }
    }
}