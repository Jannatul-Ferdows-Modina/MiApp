namespace AppMGL.DTO.Document
{
    public class CertificationOfOriginODDTO
    {
        public int COOtherID { get; set; }
        public int COID { get; set; }
        public string Number { get; set; }
        public string Package { get; set; }
        public string Commodity { get; set; }
        public string Weight { get; set; }
        public string Measuremant { get; set; }
        public string Type { get; set; }
        public string WtType { get; set; }
        public string MtType { get; set; }
        public string DocumentCommonId { get; set; }
    }
    public class EquipmentLine
    {
        public string LineNo { get; set; }
        public string EquipmentNumber { get; set; }
        public string SealNumber { get; set; }
        public string DocumentCommonId { get; set; }
        public string SiteId { get; set; }
    }
    public class CommodityLine
    {
        public string idd { get; set; }
        public string ctype { get; set; }
        public string IsType { get; set; }
        public string ExportCode { get; set; }
        public string ScheduleB { get; set; }
        public string FirstQuantity { get; set; }
        public string FirstUOM { get; set; }
        public string SecondQuantity { get; set; }
        public string SecondUOM { get; set; }
        public string OriginofGoods { get; set; }
        public string ValueofGoods { get; set; }
        public string ShippingWeight { get; set; }
        public string Eccn { get; set; }
        public string LicenseTypeCode { get; set; }
        public string ExpLic { get; set; }
        public string LicValueAmount { get; set; }
        
        public string IsGovermentAgency { get; set; }
        public string CommodityDescription { get; set; }
        public string SiteId { get; set; }
        public string DocumentCommonId { get; set; }

        public string dDTCITAR { get; set; }
        public string dDTCReg { get; set; }
        public string dDTCSignificant { get; set; }
        public string dDTCEligible { get; set; }
        public string dDTCUSML { get; set; }
        public string dDTCUnit { get; set; }
        public string dDTCQuantity { get; set; }
        public string dDTCLicense { get; set; }
        public string htsCodeId { get; set; }
        
    }
    public class Vin
    {
        public string VIN { get; set; }
        public string VINNumber { get; set; }
        public string VehicleTitleNum { get; set; }
        public string VehicleTitleState { get; set; }
        public string SiteId { get; set; }
        public string DocumentCommonId { get; set; }
        public string AesId { get; set; }
        public string Line_No { get; set; }
        public string HtsCode { get; set; }
        public string HtsCodeId { get; set; }

    }
    public class FileHistory
    {
        public string Id { get; set; }
        public string DocumentCommonId { get; set; }
        public string ShipmentReferenceNumber { get; set; }
        public string InputFile { get; set; }
        public string OutputFile { get; set; }
        public string isuploaded { get; set; }
        public string isdownloaded { get; set; }
        public bool isDraft { get; set; }
        public string itanno { get; set; }
        public string SubmitDate { get; set; }
        public string UploadDate { get; set; }
    }
}
