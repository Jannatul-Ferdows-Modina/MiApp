using System;

namespace AppMGL.DTO.Operation
{
    public class BookingListDTO
    {
        public int DocumentCommonID { get; set; }
        public decimal QuotationID { get; set; }
        public string QuotationNo { get; set; }
        public decimal EnquiryID { get; set; }
        public string EnquiryNo { get; set; }
        public string BookingNo { get; set; }
        public string SealNo { get; set; }
        public string FileNo { get; set; }
        public string ExportRef { get; set; }
        public DateTime? BookingDate { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string ShippingLine { get; set; }
        public string Commodity { get; set; }
        public string BookingRequest { get; set; }
        public string ModeName { get; set; }
        public int? MailMode { get; set; }
        public string ShipperAttachFile { get; set; }
        public string ShipperCustomerName { get; set; }
        public string EmptyPickup { get; set; }
        public string EmptyPickupLocation { get; set; }
        public string FullDeliveryLocation { get; set; }
        public string FullReturn { get; set; }
        public DateTime? EarliestPickUpDate { get; set; }
        public DateTime? PortcutoffDate { get; set; }
        public DateTime? CutOffDate { get; set; }
        public DateTime? AutoCutOff { get; set; }
        public DateTime? RampCutOff { get; set; }
        public DateTime? ERD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ETS { get; set; }
        public string GALBookingStatus { get; set; }
        public string BookingStatus { get; set; }
        public int? GALBookingStatusID { get; set; }
        public DateTime? EffectFromDate { get; set; }
        public string CompanyName { get; set; }
        public string Shipper { get; set; }
        public DateTime? ShipmentMoveDate { get; set; }
        public DateTime? ShipperConfirmDate { get; set; }
        public string PortOfOrigin { get; set; }
        public string PortOfDischarge { get; set; }
        public string PickupRemark { get; set; }
        public string City { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string ZipCode { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDraft { get; set; }
        public bool IsApproved { get; set; }
        public bool IsBooked { get; set; }
        public string TypeOfEnquiry { get; set; }
        public string SourceCountry { get; set; }
        public string Destinationcountry { get; set; }
        public string OriginCity { get; set; }
        public string OriginState { get; set; }
        public string Origin { get; set; }
        public string RailRamp { get; set; }
        public int? MailSend { get; set; }
        public int QuotationStatus { get; set; }
        public int fkCarrierID { get; set; }
        public string Remarks { get; set; }
        public DateTime? NextActionDueDate { get; set; }
        public string InvoiceStatus { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string Attachment { get; set; }
        public string OptionVale { get; set; }
        public string SearchVale { get; set; }
        public string OriginDoor { get; set; }
        public string PortOfLoading { get; set; }
        public string Destination { get; set; }
        public string PickupCategory { get; set; }
        public string LineBKGType { get; set; }
        public string GALBookingType { get; set; }
        public string Transporter { get; set; }
        public string AttachFile { get; set; }
        public string AttachFile1 { get; set; }
        public string AttachFile2 { get; set; }
        public int? IsPublic { get; set; }        
        public int? DepartmentId { get; set; }
        public string BLNumber { get; set; }
        public string CNTNO { get; set; }
        public string VIN { get; set; }
        public string LastRemarks { get; set; }
        public string ModeOfService { get; set; }
        public string ContainerName { get; set; }
        public int contactID { get; set; }        
        public DateTime? DockReceiptIssueDate { get; set; }
        public DateTime? LastRemarkDate { get; set; }
        public string NextActionRemarks { get; set; }        
        public decimal SiteId { get; set; }
        public long? ROWNUM { get; set; }
        public int? TotalCount { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string ActionType { get; set; }
        public string ConsigneePONo { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeID { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string ShipperCommonRemarks { get; set; }
        public int shipperId { get; set; }
        public int? Trucking_ContactID { get; set; }
        public string TruckCompanyName { get; set; }
        public int? Loading_ContactID { get; set; }
        public string LoadingLocation { get; set; }
        public DateTime? LoadingDate { get; set; }        
        public Boolean IsSysGenerated { get; set; }
        public string DocType { get; set; }
        public string DocumentType { get; set; }
        public DateTime? ReqStuffingDate { get; set; }
        public DateTime? ConfStuffedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? Dos_Id { get; set; }
        public string DocumentationStatus { get; set; }
        public ExpenseDetailDTO[] ExpenseDetailDTOList { get; set; }
        public DispatchContainerDTO[] DispatchContainerDTOList { get; set; }
        public NextActionRemarksDTO[] NextActionRemarksDTOList { get; set; }
        public ShipmentDocsDTO[] ShipmentDocsDTOList { get; set; }
        public ShipperRemarksDTO[] ShipperRemarksDTOList { get; set; }
        public decimal NewQuotationID { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string USPPIIDNumberTypeId { get; set; }
        public string USPPIIDNumber {  get; set; }
        public string USPPIFirstName { get; set; }
        public string USPPIPhoneNumber { get; set; }
        public int? USPPICity { get; set; }
        public int? USPPIStateId { get; set; }
        public string USPPIPostalCodeId { get; set; }
        public string USPPIAddressLine1 { get; set; }
        public string USPPIAddressLine2 { get; set; }

        public string UltimateIdNumber { get; set; }
        public string UltimateIdNumberTypeId { get; set; }
        public string UltimateFirstName { get; set; }
        public string UltimatePhoneNumber { get; set; }
        public int? UltimateCity { get; set; }
        public int? UltimateStateId { get; set; }
        public string UltimatePostalCodeId { get; set; }
        public string UltimateCompanyName { get; set; }
        public string UltimateAddressLine1 { get; set; }
        public string UltimateAddressLine2 { get; set; }

        public string UltimateCountryId { get; set; }
        public string FreightIdNumber { get; set; }
        public string FreightIdNumberTypeId { get; set; }
        public string FreightFirstName { get; set; }
        public string FreightPhoneNumber { get; set; }
        public int? FreightCity { get; set; }
        public int? FreightStateId { get; set; }
        public string FreightPostalCodeId { get; set; }
        public string FreightCompanyName { get; set; }
        public string FreightAddressLine1 { get; set; }
        public string FreightAddressLine2 { get; set; }
        public string  CreatedByName { get; set; }
        public string filingOption { get; set; }
        public string ModeofTransport { get; set; }
        public string IsUltimateConsigneeCompanies { get; set; }
        public string IsIsHazardousMaterial { get; set; }
        public string InbondType { get; set; }
        public string PortofExport { get; set; }
        public string PortofUnloading { get; set; }
        public string ForeginTradeZone { get; set; }
        public string ImportEntry { get; set; }
        public string OriginalITN { get; set; }
        
        public string IsSoldEnRoute { get; set; }
        public string SiteName { get; set; }
        public string NewSiteName { get; set; }
        public string AesFileName { get; set; }
        public string etype { get; set; }
        public string essstatus { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }

        public string Destuffed { get; set; }
        public string DateofDestuffed { get; set; }

        public string CommodityGroup { get; set; }



    }

    public class ExpenseDetailDTO
    {
        public int? ExpID { get; set; }

        public string VendorName { get; set; }

        public string DocNo { get; set; }
       
        public DateTime? DocDate { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ETS { get; set; }
        public string Amount { get; set; }
        public decimal? SellingAmount { get; set; }
        public decimal? BuyingAmount { get; set; }
        public int? ExpenseHeadID { get; set; }
        public string ExpenseHead { get; set; }

        public string Narration { get; set; }

        public string Remarks { get; set; }

        public DateTime? LastUpdatedDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? DocumentCommonID { get; set; }
        public string QuotationNo { get; set; }
        public string FileNo { get; set; }
        public string BookingNo { get; set; }        
        public string ExportRef { get; set; }
        public DateTime? BookingDate { get; set; }
        public string InvoiceStatus { get; set; }
        public string CustomerName { get; set; }
        public Boolean IsActive { get; set; }
        public int? TotalCount { get; set; }
    }
    public class ShipperRemarksDTO
    {
        public int contactID { get; set; }
        public string Remarks { get; set; }
        public string ShipperCommonRemarks { get; set; }
    }
    public class ExpenseHeadDTO
    {
        public int ExpenseId { get; set; }
        public string ExpenseName { get; set; }
    }
    public class ShipmentDocsDTO
    {
        public decimal? SdId { get; set; }
        public int? DocumentCommonID { get; set; }
        public string  DocName { get; set; }
        public Boolean IsSysGenerated { get; set; }
        public string DocType { get; set; }
        public string DocumentType { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class DispatchContainerDTO
    {
        public int? DocumentCommonID { get; set; }
        public string SealNo { get; set; }
        public string BLNumber { get; set; }
        public string CNTNo { get; set; }
        public int? ContainerID { get; set; }
        public int? ServiceReqdID { get; set; }
        public string Name { get; set; }
        public int? TotalQty { get; set; }
        public int? RefID { get; set; }
        public int? SeqNo { get; set; }
        public string DispatchRemarks { get; set; }
        public bool? IsSelected { get; set; }
        public string ContainerOwnerName { get; set; }

        public string Destuffed { get; set; }
        public string DateofDestuffed { get; set; }

    }

    
}
