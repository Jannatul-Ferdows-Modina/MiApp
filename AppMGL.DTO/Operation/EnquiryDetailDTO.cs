using System;
using AppMGL.DTO.DataManagement;
using System.Collections.Generic;

namespace AppMGL.DTO.Operation
{
    public class EnquiryDetailDTO
    {
        public string ExpAdd { get; set; }
        public string ExpAddress { get; set; }
        public string CustomerInqNo { get; set; }
        public string OriginDoorID { get; set; }
        public string OriginDoorAddress { get; set; }
        public string OriginDoorName { get; set; }
        public string DestinationDoorID { get; set; }
        public string DestinationDoorAddress { get; set; }
        public string DestinationDoorName { get; set; }
        public decimal? LastEnquiryNo { get; set; }
        public decimal? EnquiryControlNo { get; set; }
        public int? DepartmentID { get; set; }
        public decimal EnquiryID { get; set; }
        public string EnquiryNo { get; set; }
        public string EnquiryDate { get; set; }
        public string ShipmentMoveDate { get; set; }
        public string PickupRemark { get; set; }
        public int? fkCompanyID { get; set; }
        public int? fkCountryID { get; set; }
        public int? StateID { get; set; }
        public int? UserID { get; set; }
        public string Remarks { get; set; }
        public int? EnquiryTypeID { get; set; }
        public string PickupType { get; set; }
        public int? NoOfContainer { get; set; }
        public string TypeOfEnquiry { get; set; }
        public string ReceivedBy { get; set; }
        public int? ReceivedByID { get; set; }
        public string CompanyName { get; set; }
        public string TelNo { get; set; }
        public string Address { get; set; }
        public int? BillTo { get; set; }
        public int? BillToCompanyId { get; set; }
        public string BillToCompanyName { get; set; }
        public string BillToAddress { get; set; }
        public string City { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public string WebSite { get; set; }
        public string ZipCode { get; set; }
        public string Fax { get; set; }
        public bool? IsHaz { get; set; }
        public string Description { get; set; }
        public double? Class { get; set; }
        public string UNNo { get; set; }
        public string PackingType { get; set; }
        public string HazRemarks { get; set; }
        public double? Hazweight { get; set; }
        public double? HazVolume { get; set; }
        public int OriginID { get; set; }
        public int DischargeID { get; set; }
        public decimal QuotationID { get; set; }
        public int? OriginCountryID { get; set; }
        public string OriginCountry { get; set; }
        public int? DestinationCountryID { get; set; }
        public string DischargeCountry { get; set; }
        public int? OriginCityID { get; set; }
        public string OriginCity { get; set; }
        public int? DestinationCityID { get; set; }
        public string DischargeCity { get; set; }
        public int? OrignStateID { get; set; }
        public string OriginState { get; set; }
        public int? DestinationStateID { get; set; }
        public string DischargeState { get; set; }
        public string Origin { get; set; }
        public string Discharge { get; set; }
        public int? GalRepID { get; set; }
        public string RailRamp { get; set; }
        public string Terminal { get; set; }
        public int? OrgnRailRampId { get; set; }
        public int? DestnTerminalId { get; set; }
        public int? OrgnRailRampId1 { get; set; }
        public int? DestnTerminalId1 { get; set; }
        public int? LicenseType { get; set; }
        public string RailRamp1 { get; set; }
        public string Terminal1 { get; set; }
        public string LastRemarks { get; set; }
        public DateTime? LastRemarkDate { get; set; }
        public string NextActionRemarks { get; set; }
        public DateTime? NextActionDueDate { get; set; }
        public int? IsComplete { get; set; } //0-Draft enquiry 1-incomplete Draft enquiry
        public int? IsDraft { get; set; } //0-sent to Quotation 1-Draft enquiry
        public decimal SiteId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string ModeOfService { get; set; }

        //public ICollection<CommodityDTO> CommodityDTOList;
        public CommodityDTO[] CommodityDTOList { get; set; }
        public EnquiryContainerServiceDTO[] EnquiryContainerServiceDTOList { get; set; }
        public EnquiryAIRServiceDTO[] EnquiryAIRServiceDTOList { get; set; }
        public EnquiryBreakBulkServiceDTO[] EnquiryBreakBulkServiceDTOList { get; set; }
        public EnquiryFCLServiceDTO[] EnquiryFCLServiceDTOList { get; set; }
        public EnquiryLCLServiceDTO[] EnquiryLCLServiceDTOList { get; set; }
        public EnquiryROROServiceDTO[] EnquiryROROServiceDTOList { get; set; }
        public NextActionRemarksDTO[] NextActionRemarksDTOList { get; set; }

        public DateTime? PrevActionDueDate { get; set; }

        public string NewSiteId { get; set; }

        public int? isapproved_enq { get; set; }

        public string sourceofcontact { get; set; }
        public string location { get; set; }
    }
        public class ContainerCategory
    {
        public decimal? CategoryID { get; set; }
        public decimal? ModeOfServiceID { get; set; }
        public string CategoryName { get; set; }
    }

    public class EnquiryContainerServiceDTO
    {
        public decimal ServiceRefAllID { get; set; }
        public decimal ServiceRefID { get; set; }
        public int RefID { get; set; }
        public string RefType { get; set; }
        public int? ServiceRequiredID { get; set; }
        public int? NoofContainer { get; set; }
        public int? ContainerTypeID { get; set; }
        public int? Quantity { get; set; }
        public string CategoryName { get; set; }
        public string ContainerName { get; set; }
        public string ModeOfService { get; set; }
    }


}
