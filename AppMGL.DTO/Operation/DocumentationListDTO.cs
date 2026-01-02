using System;

namespace AppMGL.DTO.Operation
{
    public class DocumentationListDTO
    {
        public decimal Doc_Id { get; set; }
        public int DocumentCommonID { get; set; }
        public string FileNo { get; set; }
        public string ShippingLine { get; set; }
        public int? Dos_Id { get; set; }
        public DateTime? Doc_ReceiptDate { get; set; }
	    public Boolean? Doc_IS_AES_ITN_REQ { get; set; }
	    public string Doc_AES_ITN { get; set; }
        public DateTime? Doc_ReceiptBLInstructionDate { get; set; }
        public DateTime? Doc_BLInstructionLineDate { get; set; }
        public DateTime? Doc_DraftBLLineDate { get; set; }
        public DateTime? Doc_DraftBLCustomerDate { get; set; }
        public DateTime? Doc_DraftBLApprovalDate { get; set; }
        public DateTime? Doc_ApprovedDraftBLToLineDate { get; set; }
        public DateTime? Doc_BLReleaseAwaitedFromLineDate { get; set; }
        public decimal QuotationID { get; set; }
        public string QuotationNo { get; set; }
        public decimal EnquiryID { get; set; }
        public string EnquiryNo { get; set; }
        public string BookingNo { get; set; }
        public DateTime? BookingDate { get; set; }
        public string ShipperAttachFile { get; set; }
        public string ShipperCustomerName { get; set; }       
        public DateTime? EarliestPickUpDate { get; set; }       
        public string GALBookingStatus { get; set; }
        public int? GALBookingStatusID { get; set; }
        public DateTime? EffectFromDate { get; set; }
        public string CompanyName { get; set; }
        public string Shipper { get; set; }
        public DateTime? ShipmentMoveDate { get; set; }
        public DateTime? ShipperConfirmDate { get; set; }       
        public bool IsDeleted { get; set; }            
        public string Remarks { get; set; }
        public DateTime? NextActionDueDate { get; set; }        
        public string Attachment { get; set; }
        public string OptionVale { get; set; }
        public string SearchVale { get; set; }        
        public string GALBookingType { get; set; }        
        public int? DepartmentId { get; set; }        
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
        public decimal CreatedBy { get; set; }
        public decimal UpdatedBy { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string ShipperCommonRemarks { get; set; }       
               
        public NextActionRemarksDTO[] NextActionRemarksDTOList { get; set; }
        public ShipmentDocsDTO[] ShipmentDocsDTOList { get; set; }        

    }

    public class DocumentationStatus
    {
        public int? Dos_Id { get; set; }       
        public string Dos_Name { get; set; }
        public string Dos_Desc { get; set; }
    }
}
