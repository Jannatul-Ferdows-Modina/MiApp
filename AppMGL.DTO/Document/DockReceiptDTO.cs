using System;

namespace AppMGL.DTO.Document
{
   public class DockReceiptDTO
    {
        public int DocumentCommonId { get; set; }
        public int? ExporterId { get; set; }
        public string ExporterName { get; set; }
        public string ExporterAddress { get; set; }
        public string DocumentNumber { get; set; }
        public string BLNumber { get; set; }
        public string FileNo { get; set; }
        public string ExportRef { get; set; }
        public int? ConsignedToId { get; set; }
        public string ConsignedToName { get; set; }
        public string ConsignedToAddress { get; set; }
        public int? ConsignedToType { get; set; }
        public string IsPrintAddress { get; set; }
        public int? FwdAgentId { get; set; }
        public string FwdAgentName { get; set; }
        public string FwdAgentAddress { get; set; }
        public string FTZNumber { get; set; }
        public int? ConsigneeId { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ExportInstruction { get; set; }
        public string CarriageBy { get; set; }
        public string PlaceOfReceipt { get; set; }
        public string Vessel { get; set; }
        public int? LandingPortId { get; set; }
        public string LandingPort { get; set; }
        public string LoadingPert { get; set; }
        public string Voyage { get; set; }
        public int? ForeignPortId { get; set; }
        public string ForeignPort { get; set; }
        public int? Transshipment { get; set; }
        public string Transshipmentdesc { get; set; }
        public string MoveType { get; set; }
        public string DeliveredBy { get; set; }
        public DateTime? ReqStuffingDate { get; set; }
        public int? CheckedBY { get; set; }
        public string CheckedBydesc { get; set; }
        public string Issued { get; set; }
        public string Booking { get; set; }
        public string CutOff { get; set; }
        public string DepartureDate { get; set; }
        public string EmptyPickup { get; set; }
        public string EmptyPickupId { get; set; }
        public string pickupName { get; set; }
        public string FullReturn { get; set; }
        public string fullReturnName { get; set; }
        public string fullReturnId { get; set; }
        
        public int? AirId { get; set; }
        public bool IsShipper { get; set; }
        public decimal SitId { get; set; }
        public int? DeliverOrderId { get; set; }
        public string Free_Text { get; set; }
        public decimal CreatedBy { get; set; }
        public decimal ModifiedBy { get; set; }
        public CertificationOfOriginODDTO[] CommodityDetail { get; set; }
        public bool IsDockTemplate { get; set; }

        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustFirstName { get; set; }
        public string CustLastName { get; set; }
        public string CustPhoneNumber { get; set; }
        public string CustAddressLine1 { get; set; }
        public string CustAddressLine2 { get; set; }
        public string CustCountryName { get; set; }
        public int? CustCountryId { get; set; }
        public string CustPostalCode { get; set; }
        public string CustCityName { get; set; }
        public int? CustCityId { get; set; }
        public string CustStateName { get; set; }
        public int? CustStateId { get; set; }
        public string EnquiryNo { get; set; }
        public string QuotationNo { get; set; }


    }
}
