using System;

namespace AppMGL.DTO.Report
{
   public class QuotationEnquiryDTO
    {
        public decimal QuotationId { get; set; }
        public string EnquiryNo { get; set; }
        public string QuotationNo { get; set; }
        public string OriginPort { get; set; }
        public string DesinationPort { get; set; }
        public string CustomerName { get; set; }
        public string CommodityName { get; set; }
        public string PickupTypeId { get; set; }
        public string ContainerTypeName { get; set; }
        public decimal QuotationCarrierId { get; set; }
        public string CarrierName { get; set; }
    }
}
