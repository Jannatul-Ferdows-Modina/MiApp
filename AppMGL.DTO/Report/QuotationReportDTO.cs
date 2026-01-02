using System;

namespace AppMGL.DTO.Report
{
   public class QuotationReportDTO
    {
        public decimal QuotationID { get; set; }
        public decimal EnquiryID { get; set; }
        public long SerialNo { get; set; }
        public string SystemRef { get; set; }
        public string MiamiRef { get; set; }
        public string BookingNo { get; set; }
        public string EnquiryNo { get; set; }
        public string QuotationNo { get; set; }
        public DateTime? EnquiryDate { get; set; }
        public string CompanyName { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string SourceCountry { get; set; }
        public string DestinationCountry { get; set; }
        public string PortOfOrigin { get; set; }
        public string PortOfDestination { get; set; }
        public string PickupRemark { get; set; }
        public DateTime? QuotationSentDate { get; set; }
        public int? TotalCount { get; set; }
    }
}
