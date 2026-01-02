using System;

namespace AppMGL.DTO.Report
{
   public class EnquiryReportDTO
    {
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
        public string OriginCity { get; set; }
        public string OriginState { get; set; }
        public string OriginRailRamp { get; set; }
    }
}
