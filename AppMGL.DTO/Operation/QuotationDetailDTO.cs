using System;

namespace AppMGL.DTO.Operation
{
   public class QuotationDetailDTO
    {
        public decimal QuotationID { get; set; }
        public string QuotationNo { get; set; }
        public decimal EnquiryID { get; set; }
        public string EnquiryNo { get; set; }
        public DateTime? EnquiryDate { get; set; }
        public string CompanyName { get; set; }
        public int? fkCompanyID { get; set; }
        public DateTime? ShipmentMoveDate { get; set; }       
        public string PickupType { get; set; }
        public string PickupRemark { get; set; }
        public DateTime? ValidTill { get; set; }
        public string City { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string ZipCode { get; set; }
        public int? IsDraft { get; set; }
        public int? IsApproved { get; set; }
        public bool? IsBooked { get; set; }
        public bool? IsDeleted { get; set; }
        public int? TypeOfEnquiry { get; set; }
        public int? SourceCountryID { get; set; }
        public string SourceCountryName { get; set; }
        public int? DestinationCountryID { get; set; }
        public string DestinationCountryName { get; set; }
        public string DischargeCountry { get; set; }
        public int? OriginCountryID { get; set; }
        public string OriginCountry { get; set; }
        public int OriginID { get; set; }
        public int DischargeID { get; set; }
        public string Discharge { get; set; }
        public string Origin { get; set; }

        public int? PortOriginId { get; set; }
        public string PortOriginName { get; set; }
        public int? PortDischargeId { get; set; }
        public string PortDischargeName { get; set; }
        public int? NoOfContainer { get; set; }
        public int ContactID { get; set; }
        public int? EnquiryTypeID { get; set; }
        public string EnquiryType { get; set; }
        public int? GalRepID { get; set; }
        public string GalRepName { get; set; }
        public string RailRamp { get; set; }
        public int? MailSend { get; set; }
        public string PaymentTerms { get; set; }
        public string OtherTerms { get; set; }
        public decimal TruckBuying { get; set; }
        public decimal TruckSelling { get; set; }
        public string TruckingRemarks { get; set; }
        public string EmailRemarks { get; set; }
        public string Commodity { get; set; }
        public string CarriageCurrency { get; set; }
        public string Carrier { get; set; }
        public string LastRemarks { get; set;}
        public DateTime? LastRemarkDate { get; set; }
        public string NextActionRemarks { get; set; }
        public DateTime? NextActionDueDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }        
        public int? selectedCarrierID { get; set; }
        public long? ROWNUM { get; set; }
        public int? TotalCount { get; set; }
        public DateTime? custReqCutOffDate { get; set; }
        public DateTime? custReqSailingDate { get; set; }
        public QuotationCarrierDTO[] CarrierDTOList { get; set; }
        public TruckingCharges[] TruckingChargesList { get; set; }
        public EmailMessage EmailMessageDTO { get; set; }
        public NextActionRemarksDTO[] NextActionRemarksDTOList { get; set; }
        public string SiteId { get; set; }
        public bool? IsConBooking { get; set; }
    }
    public class TruckingCharges
    {
        public decimal TrkQuotationID { get; set; }
        public decimal TrkEnquiryID { get; set; }         
        public decimal TrkBuyingRate { get; set; }
        public decimal TrkSellingRate { get; set; }        
        public int TrkContainerID { get; set; }
        public string ContainerName { get; set; }

    }
    public class QuotationCarrierDTO
    {
        public decimal EnquiryID { get; set; }
        public decimal QuotationCarrierID { get; set; }
        public decimal QuotationID { get; set; }
        public int CarrierID { get; set; }
        public string CarrierName { get; set; }
        public string CompanyName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ValidTill { get; set; }
        public bool Approved { get; set; }
        public bool ApprovedForBooking { get; set; }
        public string Remarks { get; set; }        
        public int ContactID { get; set; }
        public string TransitTime { get; set; }
        public string Frequency { get; set; }
        public string CarriageCurrency { get; set; }
        public int Days { get; set; }
        public string Commodity { get; set; }
        public bool Selected { get; set; }
        public int? SelectedCarrierId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? custReqCutOffDate { get; set; }
        public DateTime? custReqSailingDate { get; set; }
        public CarrierChargesDTO[] CarrierChargesDTOList { get; set; }        
        public CarrierRemarksDTO[] CarrierRemarksDTOList { get; set; }

    }
    public class SIPLCurrencyDTO
    {
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string Symbol { get; set; }
        public string Code { get; set; }
    }
    public class CarrierChargesDTO
    {
        public decimal QuotationID { get; set; }
        public decimal QuotationCarrierId { get; set; }
        public string CarrierName { get; set; }
        public int CarrierID { get; set; }
        public int RefId { get; set; }
        public string RefName { get; set; }
        public decimal BuyingRate { get; set; }
        public decimal SellingRate { get; set; }
        public bool? IsSelected { get; set; }
        public string IsApproved { get; set; }
        public int IsApplicable { get; set; }
        public int fkContainerID { get; set; }
        public string Item { get; set; }
        public string Remarks { get; set; }
        public string ContainerTypeName { get; set; }

        public string itemremark { get; set; }

    }
    public class CarrierRemarksDTO
    {
        public decimal QuotationID { get; set; }
        public int CarrierID { get; set; }
        public decimal RefId { get; set; }
        public string RefName { get; set; }
        public string Remarks { get; set; }
        public bool? IsSelected { get; set; }
    }

    public class SPFeeCatagoryDTO
    {
        public int SfcID { get; set; }        
        public string SfcName { get; set; }
        public string SfcCode { get; set; }        
    }
    public class EmailMessage
    {
        public string QuotationNo { get; set; }
        public decimal EnquiryID { get; set; }
        public string EnquiryNo { get; set; }
        public DateTime? EnquiryDate { get; set; }
        public string ValidTill { get; set; }
        public string CityOfOrigin { get; set; }
        public string OriginDoor { get; set; }
        public string CustomerInqNo { get; set; }
        public string terminalName { get; set; }
        public string PickupType { get; set; }
        public string TodayDate { get; set; }
        public int? SourceCountryID { get; set; }
         
        public int? fkCompanyID { get; set; }
        public DateTime? ShipmentMoveDate { get; set; }
        public int? PortOfOrigin { get; set; }
        public int? PortOfDischarge { get; set; }
        public int? NoOfContainer { get; set; }
        public string Discharge { get; set; }
        public string Origin { get; set; }
        public int ContactID { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string TelNo { get; set; }
        public decimal Truckingcharges { get; set; }
        public string SourceCountry { get; set; }
        public string DestinationCountry { get; set; }
        public string TypeOfEnquiry { get; set; }
        public string OriginDoorAddr { get; set; }
        public string railRamp { get; set; }
        public string Commodity { get; set; }
        public string Equipment { get; set; }
        public string EmailContent { get; set; }
        public string EmailSubject { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string Emailcc { get; set; }
        public string EmailBcc { get; set; }
        public string CurrencyName { get; set; }
        public string EnquiryUrl { get; set; }
    }

    public class QuotationEmailData {
        public decimal QuotationID { get; set; }
        public int DocumentCommonID { get; set; }
        public int? GALBookingStatusID { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string Emailcc { get; set; }
        public string EmailBcc { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public FileAttachementDTO[] FileAttachementDTOList { get; set; }
        public ShipmentDocsDTO[] ShipmentDocsDTOList { get; set; }
        public string SitId { get; set; }
        public string EnquiryUrl { get; set; }
    }
    public class FileAttachementDTO
    {
        public string FileName { get; set; }
    }

    public class NextActionRemarksDTO
    {
        public int rowId { get; set; }
        public int? TotalCount { get; set; }
        public decimal QuotationID { get; set; }
        public decimal EnquiryID { get; set; }
        public int DocumentCommonID { get; set; }
        public string QuotationNo { get; set; }
        public string RefID { get; set; }
        public string RefNo { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Type { get; set; }
        public string CustomerName { get; set; }
        public DateTime? NextActionDate { get; set; }
        public DateTime? CurrentDate { get; set; }
        public string contactpersonemail { get; set; }
        public string contactpersonphone { get; set; }
    }

    public class QuotationRemarksDTO
    {
        public decimal QuotationID { get; set; }
        public DateTime? LastRemarkDate { get; set; }
        public string LastRemarks { get; set; }
        public DateTime? NextActionDueDate { get; set; }
    }
}
