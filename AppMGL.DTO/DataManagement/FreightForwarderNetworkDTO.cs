using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMGL.DTO.DataManagement
{
    public class FreightForwarderNetworkDTO
    {
        public int FFNetworkID { get; set; }
        public string FFNetworkName { get; set; }
        public string FFNetworkCode { get; set; }
        public string Address { get; set; }
        public string CellNo { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Remarks { get; set; }
        public string TelNo { get; set; }
        public string WebSite { get; set; }
        public string ZipCode { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Type { get; set; }
        public string Attachment { get; set; }
        public string RepresentativeID { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public int? StateId { get; set; }
        public string City { get; set; }
        public int? CityId { get; set; }
        public int? ContinentId { get; set; }
        public string MobNo { get; set; }
        public string TaxID { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal SiteId { get; set; }
        public string siteids { get; set; }
        
        public AdditionalFFContactDTO[] AdditionalFFContactDTOList { get; set; }

    }


    public class AdditionalFFContactDTO
    {
        public int FFNetworkID { get; set; }
        public string ContactName { get; set; }
        public string Designation { get; set; }
        public string contactEmail { get; set; }
        public string ContactNo { get; set; }
        public string ContactCellNo { get; set; }

    }
    public class FFNetworkCodeDTO
    {
        
        public string FFNetworkCode { get; set; }

    }
    public partial class USP_GET_FFCONTACT_LIST_Result
    {
        public Nullable<long> ROWNUM { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public string FFNetworkName { get; set; }
        public string TelNo { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public string WebSite { get; set; }
        public string ZipCode { get; set; }
        public int FFNetworkID { get; set; }
        public string MobNo { get; set; }
        public string Fax { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> StateID { get; set; }
        public Nullable<int> ContinentId { get; set; }
        public string Continent { get; set; }
        public string CountryName { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string FFNetworkCode { get; set; }
        public Nullable<int> CityId { get; set; }
        public string City { get; set; }
        public string RepresentativeID { get; set; }
        public string GalRepresentative { get; set; }
        public string Attachment { get; set; }
       
    }
    public class FreightForwarderMappingDTO
    {
        public int FFNetworkId { get; set; }
        public int NetworkMapId { get; set; }
        public string FFNetworkName { get; set; }
        public string FFNetworkCode { get; set; }
        public string CompanyName { get; set; }
        public int ContactId { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int SiteId { get; set; }
    }
}
