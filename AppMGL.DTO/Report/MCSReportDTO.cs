using System;

namespace AppMGL.DTO.Report
{
   public class MCSReportDTO
    {
        public string ExportRef { get; set; }
        public string QuotationNo { get; set; }
        public string FileNo { get; set; }
        public DateTime? BookingDate { get; set; }
        public string BookingNo { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string CompanyName { get; set; }
        public string ShippingLine { get; set; }
        public string Transporter { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ETS { get; set; }
        public string Origin { get; set; }
        public string RailRamp { get; set; }
        public string PortOfLoading { get; set; }
        public string Destination { get; set; }
        public string BLNumber { get; set; }
        public string ContainerNumber { get; set; }
        public string PortOfOrigin { get; set; }
        public string PortOfDischarge { get; set; }
        public int? isContainer40Exits { get; set; }
        public int? isContainer45Exits { get; set; }
        public int? isContainer20Exits { get; set; }

        public decimal? SitId { get; set; }
        public string SitName { get; set; }
        public string Unit { get; set; }
        public double? VolumeInCB { get; set; }
        public int? NoOfContainer { get; set; }
        public double? FREIGHT_SELLING_RATE { get; set; }
        public decimal? INLAND_SELLING_RATE { get; set; }
        public double? COURIER_SELLING_RATE { get; set; }
        public double? OTHER_SELLING_RATE { get; set; }
        public double? TOTAL_SELLING_RATE { get; set; }
        public double? FREIGHT_BUYING_RATE { get; set; }
        public double? FREIGHT_Comm { get; set; }
        public string LineCommType { get; set; }
        public decimal? SPCommissionRate { get; set; }
        public decimal? INLAND_BUYING_RATE { get; set; }
        public double? COURIER_BUYING_RATE { get; set; }
        public double? OTHER_BUYING_RATE { get; set; }
        public double? TOTAL_BUYING_RATE { get; set; }
        public double? MARGIN_AMOUNT { get; set; }

        public string CustomerID { get; set; }

        public string qbid { get; set; }
        public string DocumentCommonID { get; set; }

        public string QBCustomerName { get; set; }

        public string isconsolidatedreport { get; set; }
		public string Invoiceid { get; set; }

        public string QUOTATIONID { get; set; }
        public string EnquiryID { get; set; }

        public string Email { get; set; }
        public string ModeOfService { get; set; }
        
        public int? IsHBL { get; set; }
        public int? IsMBL { get; set; }
        public int? IsCO { get; set; }
        public int? IsDockReceipt { get; set; }
        public int? IsShipper { get; set; }
        public int? IsBooking { get; set; }
        public int? IsOther { get; set; }
        public int? IsAES { get; set; }
        public int? IsLineMBL { get; set; }
        public string DocRequired { get; set; }
        public string IsPortToPort { get; set; }
        public string QBStatus { get; set; }
        public string QBMaturedMonth { get; set; }

        public double? sepataraCommission { get; set; }
        public double? sepastaraCommissionAmount { get; set; }
        public string sepataraPartnerName { get; set; }

    }


    public class MCSReportDTOCompany
    {
        public string ExportRef { get; set; }
        public string QuotationNo { get; set; }
        public string FileNo { get; set; }
        public DateTime? BookingDate { get; set; }
        public string BookingNo { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string CompanyName { get; set; }
        public string ShippingLine { get; set; }
        public string Transporter { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ETS { get; set; }
        public string Origin { get; set; }
        public string RailRamp { get; set; }
        public string PortOfLoading { get; set; }
        public string Destination { get; set; }
        public string BLNumber { get; set; }
        public string ContainerNumber { get; set; }
        public string PortOfOrigin { get; set; }
        public string PortOfDischarge { get; set; }
        public int? isContainer40Exits { get; set; }
        public int? isContainer45Exits { get; set; }
        public int? isContainer20Exits { get; set; }

        public decimal? SitId { get; set; }
        public string SitName { get; set; }
        public string Unit { get; set; }
        public double? VolumeInCB { get; set; }
        public int? NoOfContainer { get; set; }
        public double? FREIGHT_SELLING_RATE { get; set; }
        public decimal? INLAND_SELLING_RATE { get; set; }
        public double? COURIER_SELLING_RATE { get; set; }
        public double? OTHER_SELLING_RATE { get; set; }
        public double? TOTAL_SELLING_RATE { get; set; }
        public double? FREIGHT_BUYING_RATE { get; set; }
        public double? FREIGHT_Comm { get; set; }
        public string LineCommType { get; set; }
        public decimal? SPCommissionRate { get; set; }
        public decimal? INLAND_BUYING_RATE { get; set; }
        public double? COURIER_BUYING_RATE { get; set; }
        public double? OTHER_BUYING_RATE { get; set; }
        public double? TOTAL_BUYING_RATE { get; set; }
        public double? MARGIN_AMOUNT { get; set; }

        public string CustomerID { get; set; }

        public string qbid { get; set; }
        public string DocumentCommonID { get; set; }

        public string QBCustomerName { get; set; }

        public string isconsolidatedreport { get; set; }
        public string Invoiceid { get; set; }

        public string QuotationID { get; set; }
        public string EnquiryID { get; set; }

        public string Email { get; set; }
        public string ModeOfService { get; set; }

        public int? IsHBL { get; set; }
        public int? IsMBL { get; set; }
        public int? IsCO { get; set; }
        public int? IsDockReceipt { get; set; }
        public int? IsShipper { get; set; }
        public int? IsBooking { get; set; }
        public int? IsOther { get; set; }
        public int? IsAES { get; set; }
        public int? IsLineMBL { get; set; }
        public string DocRequired { get; set; }
        public string IsPortToPort { get; set; }
        public string QBStatus { get; set; }
        public string QBMaturedMonth { get; set; }

        public double? sepataraCommission { get; set; }
        public double? sepastaraCommissionAmount { get; set; }
        public string sepataraPartnerName { get; set; }

    }

    public class CommodityGroup_DDL_DTO
    {
        public int? CommodityGroupId { get; set; }
        public string Name { get; set; }
    }
    }
