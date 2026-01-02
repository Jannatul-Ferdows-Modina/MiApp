using System;
using AppMGL.DTO.DataManagement;
using System.Collections.Generic;

namespace AppMGL.DTO.Operation
{
    public class DocumentationDetailDTO
    {

        //Booking properties
        public int DocumentCommonID { get; set; }
        public decimal Doc_Id { get; set; }
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

        public string BookingNo { get; set; }
        public string FileNo { get; set; }        
        public string ShipperCustomerName { get; set; }
        public DateTime? BookingDate { get; set; }
        public int? BookingRequestID { get; set; }
        public string BookingRequest { get; set; }
        public string GALBookingType { get; set; }
        public bool? BookingStatus { get; set; }
        public string CarrierBooking { get; set; }
        public int? GALBookingStatusID { get; set; }
        public string GALBookingStatus { get; set; }        
        public string contractNo { get; set; }
        public int ContactID { get; set; }
        
        public decimal CreatedBy { get; set; }
        public decimal UpdatedBy { get; set; }
       
        public decimal? QuotationID { get; set; }
        public string QuotationNo { get; set; }
        public decimal? EnquiryID { get; set; }
        public string EnquiryNo { get; set; }
        public int? DepartmentID { get; set; }        
        public decimal SiteId { get; set; }
        public string LastRemarks { get; set; }
        public DateTime? LastRemarkDate { get; set; }
        public string NextActionRemarks { get; set; }
        public DateTime? NextActionDueDate { get; set; }

        public NextActionRemarksDTO[] NextActionRemarksDTOList { get; set; }
        public ShipmentDocsDTO[] ShipmentDocsDTOList { get; set; }


    }

}
