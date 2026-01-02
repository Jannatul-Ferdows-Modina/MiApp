using System;

namespace AppMGL.DTO.Operation
{
   public class QuotationListDTO
    {
        public decimal QuotationID { get; set; }
        public string QuotationNo { get; set; }
        public decimal EnquiryID { get; set; }
        public string EnquiryNo { get; set; }
        public DateTime? EnquiryDate { get; set; }
        public string CompanyName { get; set; }        
        public DateTime? ShipmentMoveDate { get; set; }
        public string PortOfOrigin { get; set; }
        public string PortOfDischarge { get; set; }
        public string PickupRemark { get; set; }
        public string City { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string ZipCode { get; set; }
        public string IsCancelled { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDraft { get; set; }
        public bool IsApproved { get; set; }
        public bool IsBooked { get; set; }
        public string TypeOfEnquiry { get; set; }
        public string SourceCountry { get; set; }
        public string Destinationcountry { get; set; }
        public string OriginCity { get; set; }
        public string OriginState { get; set; }
        public string RailRamp { get; set; }
        public int? MailSend { get; set; }
        public int QuotationStatus { get; set; }
        public string Remarks { get; set; }
        public long? ROWNUM { get; set; }
        public int? TotalCount { get; set; }

        public string CreatedByName { get; set; }
        public string QuotationCreatedByName { get; set; }
        public string SiteId { get; set; }

        public int? CreatedBy { get; set; }
        public string ContainerNo { get; set; }
    }
}
