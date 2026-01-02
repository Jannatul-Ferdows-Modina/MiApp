using System;

namespace AppMGL.DTO.Operation
{
   public class EnquiryListDTO
    {
        public decimal EnquiryID { get; set; }
        public string EnquiryNo { get; set; }
        public DateTime? EnquiryDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public int? NoOfContainer { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public long? ROWNUM { get; set; }
        public int? TotalCount { get; set; }        
        public DateTime? ShipmentMoveDate { get; set; }        
        public string Commodity { get; set; }
        public string Remarks { get; set; }
        public string State { get; set; }
        public int? IsComplete { get; set; } //0-Draft enquiry 1-incomplete Draft enquiry
        public int? IsDraft { get; set; } //0-sent to Quotation 1-Draft enquiry
        public decimal QuotationID { get; set; }

        public DateTime? LastRemarkDate { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? EnqCreatedOn { get; set; }
        public DateTime? EnqModifiedOn { get; set; }
        public string CompanyId { get; set; }
        

    }
}
