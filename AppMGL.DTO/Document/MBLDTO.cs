using System;

namespace AppMGL.DTO.Document
{
   public class MBLDTO
    {
        public int? HouseBLID { get; set; }
        public string DocumentCommonId { get; set; }
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
        public string ConsignedToType { get; set; }
        public bool IsPrintAddress { get; set; }
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
        public string LandingPortId { get; set; }
        public string LandingPort { get; set; }
        public string LoadingPert { get; set; }
        public string Voyage { get; set; }
        public string ForeignPortId { get; set; }
        public string ForeignPort { get; set; }
        public string Transshipment { get; set; }
        public string MoveType { get; set; }
        public bool Containerized { get; set; }
        public string Place { get; set; }
        public string ExportingCarrier { get; set; }
        public DateTime? SignDate { get; set; }
        public int? AirId { get; set; }
        public bool IsShipper { get; set; }
        public int? SitId { get; set; }
        public int? DeliverOrderId { get; set; }
        public int? ReturnCommonId { get; set; }
        
        public CertificationOfOriginODDTO[] CommodityDetail { get; set; }
        public FreightDTO[] FreightDetail { get; set; }
    }
}
