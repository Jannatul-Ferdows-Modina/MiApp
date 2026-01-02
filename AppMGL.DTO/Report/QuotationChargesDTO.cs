using System;

namespace AppMGL.DTO.Report
{
   public class QuotationChargesDTO
    {
        public string ItemType { get; set; }
        public double SellingPrice { get; set; }
        public double BuyingPrice { get; set; }
        public string Remarks { get; set; }
        public bool ApprovedForBooking { get; set; }
    }
}
