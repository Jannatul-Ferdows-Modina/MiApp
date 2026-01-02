using AppMGL.DTO.Document;
using System;

namespace AppMGL.DTO.Report
{
   public class BookingReportDTO
    {
        public long? SerialNo { get; set; }
        public DateTime? BookingDate { get; set; }
        public int? SystemRef { get; set; }
        public string MiamiRef { get; set; }
        public string BookingNo { get; set; }
        public string EnquiryNo { get; set; }
        public string QuotationNo { get; set; }
        public string CompanyName { get; set; }
        public string RegionName { get; set; }
        public string CarrierName { get; set; }
        public int? C20 { get; set; }
        public int? C40 { get; set; }
        public int? EQ20 { get; set; }
        public int? FCL { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public DateTime? CutOff { get; set; }
        public DateTime? ETS { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ERD { get; set; }
        public string OriginDoor { get; set; }
        public string PortOfLoading { get; set; }
        public string Destination { get; set; }
        public DateTime? FCRCutoffDate { get; set; }
        public string BillOfLadingNo { get; set; }
        public string VIN { get; set; }
        public string Commodity { get; set; }
        public string CntType { get; set; }
        public string CntNo { get; set; }
        public string SealNo { get; set; }
        public string Shipper { get; set; }
        public string PickupCategory { get; set; }
        public long? ROWNUM { get; set; }
        public int? TotalCount { get; set; }
    }


    public class BookingReportHistoryDTO
    {
        public long? SerialNo { get; set; }
        public DateTime? BookingDate { get; set; }
        public string SystemRef { get; set; }
        public string MiamiRef { get; set; }
        public string BookingNo { get; set; }
        public string EnquiryNo { get; set; }
        public string QuotationNo { get; set; }
        public string CompanyName { get; set; }
        public string RegionName { get; set; }
        public string CarrierName { get; set; }
        public int? C20 { get; set; }
        public int? C40 { get; set; }
        public int? EQ20 { get; set; }
        public int? FCL { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public DateTime? CutOff { get; set; }
        public DateTime? ETS { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ERD { get; set; }
        public string OriginDoor { get; set; }
        public string PortOfLoading { get; set; }
        public string Destination { get; set; }
        public DateTime? FCRCutoffDate { get; set; }
        public string BillOfLadingNo { get; set; }
        public string VIN { get; set; }
        public string Commodity { get; set; }
        public string CntType { get; set; }
        public string CntNo { get; set; }
        public string SealNo { get; set; }
        public string Shipper { get; set; }
        public string PickupCategory { get; set; }
        public long? ROWNUM { get; set; }
        public int? TotalCount { get; set; }
        public string EnquiryID { get; set; }
        public string DocumentCommonID { get; set; }
        public string QuotationID { get; set; }       
					
    }




    public class EESSubmission
    {
        public string Id { get; set; }
        public string DocumentCommonID { get; set; }
        public string siteId { get; set; }
        public string QuotationID { get; set; }
        public string EmailResponseAddress { get; set; }
        public string shipmentNumber { get; set; } 
        public string enquiryNo { get; set; }
        public string quotationNo { get; set; }
        public string bookingNo { get; set; }
        public string companyName { get; set; }
       // public string galBookingType { get; set; }
       // public string PortOfLoading { get; set; }
        public string OriginState { get; set; }
        public string OriginStateCode { get; set; }
        // public string portOfDischarge { get; set; }
        public string CountryOfDestination { get; set; }
        public string CountryCode { get; set; }
        public string DepartureDate { get; set; }
        public string FilingOption { get; set; }
        public string ModeofTransport { get; set; }
        public string IsUltimateConsigneeCompanies { get; set; }
        public string IsHazardousMaterial { get; set; }
        public string InbondType { get; set; }
        public string PortofExport { get; set; }
        public string PortofExportCode { get; set; }
        public string PortofUnloading { get; set; }
        public string PortofUnloadingCode { get; set; }
        public string ForeginTradeZone { get; set; }
        public string ImportEntry { get; set; }
        public string OriginalITN { get; set; }
      //  public string ets { get; set; }
        public string IsRoutedTransaction { get; set; }
      //  public string UsPPI { get; set; }
       // public string HazardousMaterial { get; set; }
      //  public string USPPI_Shiper { get; set; }
        
      //  public string CONSIGNEE { get; set; }
      //  public string FREIGHT_FORWARDER { get; set; }
       // public string INTERMEDIATE_CONSIGNEE { get; set; }
        //public string Export_Information_Code { get; set; }

       // public string ScheduleB { get; set; }

       // public string Commodity_Description { get; set; }
      //  public string FirstQuantity { get; set; }
        //public string SecondQuantity { get; set; }
      //  public string SecondUOM { get; set; }
     //   public string OriginOfGoods { get; set; }
     //   public string ValueOfGoods { get; set; }
      //  public string ShippingWeight { get; set; }

       // public string LicenseTypeCode { get; set; }
      //  public string FilingRequired { get; set; }
      //  public string ECCN { get; set; }
      //  public string HSCODE { get; set; }

      //  public string MachineSrNo { get; set; }
     //   public string Carrier_SCAC_IATA { get; set; }
      //  public string VesselAndVoyage { get; set; }
      //  public string Vessel { get; set; }
     //   public string Voyage { get; set; }

     //  public string Status { get; set; }
     //   public string ResponseMessage { get; set; }
        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
       // public string Address { get; set; }
      //  public string State { get; set; }
       // public string ZipCode { get; set; }
       // public string Country { get; set; }
      //  public string City { get; set; }
        
        public string USPPICompanyName { get; set; }
        public string USPPIIDNumberTypeId { get; set; }
        public string USPPIIDNumber { get; set; }
        public string IRSNumber { get; set; }
        
        public string USPPIFirstName { get; set; }
        public string USPPILastName { get; set; }
        
        public string USPPIPhoneNumber { get; set; }
        public string USPPICity { get; set; }
        public string USPPIStateId { get; set; }
        public string USPPIStateCode { get; set; }
        public string USPPIPostalCodeId { get; set; }
        public string USPPIAddressLine1 { get; set; }
        public string USPPIAddressLine2 { get; set; }
        public string USPPIEmail { get; set; }

        
         public string ConsigneeTypeId { get; set; }
       
        public string IsSoldEnRoute { get; set; }
        public string UltimateIdNumberTypeId { get; set; }
        public string UltimateIdNumber { get; set; }
        
        public string UltimateFirstName { get; set; }
        public string UltimateLastName { get; set; }
        
        public string UltimatePhoneNumber { get; set; }
        public string UltimateCity { get; set; }
        public string UltimateStateId { get; set; }
        public string UltimateStateCode { get; set; }
        public string UltimatePostalCodeId { get; set; }
        public string UltimateCompanyName { get; set; }
        public string UltimateAddressLine1 { get; set; }
        public string UltimateAddressLine2 { get; set; }
        public string UltimateCountryId { get; set; }
        public string UltimateCountryCode { get; set; }
        public string UltimateEmail { get; set; }

        public string InterIdNumberTypeId { get; set; }
        public string InterIdNumber { get; set; }

        public string InterFirstName { get; set; }
        public string InterLastName { get; set; }
        public string InterCountryId { get; set; }
        public string InterCountryCode { get; set; }
        public string InterPhoneNumber { get; set; }
        public string InterCity { get; set; }
        public string InterStateId { get; set; }
        public string InterStateCode { get; set; }
        public string InterPostalCodeId { get; set; }
        public string InterCompanyName { get; set; }
        public string InterAddressLine1 { get; set; }
        public string InterAddressLine2 { get; set; }
        public string InterEmail { get; set; }

        public string FreightIdNumber { get; set; }
        public string FreightIdNumberTypeId { get; set; }
        public string FreightFirstName { get; set; }
        public string FreightLastName { get; set; }
        public string FreightPhoneNumber { get; set; }
        public string FreightCity { get; set; }
        public string FreightStateId { get; set; }
        public string FreightStateCode { get; set; }
        public string FreightPostalCodeId { get; set; }
        public string FreightCompanyName { get; set; }
        public string FreightAddressLine1 { get; set; }
        public string FreightAddressLine2 { get; set; }
        public string FreightEmail { get; set; }

        public string CarrierSCAC { get; set; }
        public string ConveyanceName { get; set; }
        public string TransportationReferenceNumber { get; set; }
        public string AesFileName { get; set; }
        public string AESTYPE { get; set; }
        public string ExporterName { get; set; }
        public string ExporterId { get; set; }
        public string ExporterFirstName { get; set; }
        public string ExporterLastName { get; set; }
        public string ExporterPhoneNumber { get; set; }
        public string ExporterCity { get; set; }
        public string ExporterStateId { get; set; }
        public string ExporterStateCode { get; set; }
        public string ExporterPostalCodeId { get; set; }
        public string ExporterAddress { get; set; }
        public string ExporterAddress2 { get; set; }
        public string ExporterCountryId { get; set; }
        public string ExporterCountryCode { get; set; }
        public string ExporterEmail { get; set; }

        public string etype { get; set; }
        public string isuploaded { get; set; }
        public bool isDraft { get; set; }
        public string Aes_Status { get; set; }
        public string isFreeze { get; set; }
        public string FreezeRemarks { get; set; }
        public string PortId { get; set; }
        public string UnloadPortId { get; set; }
        public string IsTemplate { get; set; }
        public CommodityLine[] CommodityDetail { get; set; }
        public EquipmentLine[] EquipmentLineDetail { get; set; }
        public Vin[] VinDetail { get; set; }
        public FileHistory[] FileList { get; set; }

    }
    public class EESLIST
    {
        public string Id { get; set; }
        public string DocumentCommonID { get; set; }
        public string SiteId { get; set; }
        public string QuotationID { get; set; }
        public string EmailResponseAddress { get; set; }
        public string shipmentNumber { get; set; }
        public string EnquiryNo { get; set; }
        public string QuotationNo { get; set; }
        public string BookingNo { get; set; }
        public string CompanyName { get; set; }
        public string OriginState { get; set; }
        public string OriginStateCode { get; set; }
        public string CountryOfDestination { get; set; }
        public string CountryCode { get; set; }
        public string DepartureDate { get; set; }
        public string FilingOption { get; set; }
        public string ModeofTransport { get; set; }
        public string IsRoutedTransaction { get; set; }
        public string IsUltimateConsigneeCompanies { get; set; }
        public string IsHazardousMaterial { get; set; }
        public string InbondType { get; set; }
        public string PortofExport { get; set; }
        public string PortofExportCode { get; set; }
        public string PortofUnloading { get; set; }
        public string PortofUnloadingCode { get; set; }
        public string ForeginTradeZone { get; set; }
        public string ImportEntry { get; set; }
        public string OriginalITN { get; set; }
        public string USPPICompanyName { get; set; }
        public string USPPIIDNumberTypeId { get; set; }
        public string USPPIIDNumber { get; set; }
        public string IRSNumber { get; set; }
        public string USPPIFirstName { get; set; }
        public string USPPILastName { get; set; }
        public string USPPIPhoneNumber { get; set; }
        public string USPPICity { get; set; }
        public string USPPIStateId { get; set; }
        public string USPPIStateCode { get; set; }
        public string USPPIPostalCodeId { get; set; }
        public string USPPIAddressLine1 { get; set; }
        public string USPPIAddressLine2 { get; set; }
        public string USPPIEmail { get; set; }

        public string ConsigneeTypeId { get; set; }
        public string IsSoldEnRoute { get; set; }
        public string UltimateIdNumberTypeId { get; set; }
        public string UltimateIdNumber { get; set; }
        public string UltimateFirstName { get; set; }
        public string UltimateLastName { get; set; }
        public string UltimatePhoneNumber { get; set; }
        public string UltimateCity { get; set; }
        public string UltimateStateId { get; set; }
        public string UltimateStateCode { get; set; }
        public string UltimatePostalCodeId { get; set; }
        public string UltimateCompanyName { get; set; }
        public string UltimateAddressLine1 { get; set; }
        public string UltimateAddressLine2 { get; set; }
        public string UltimateCountryId { get; set; }
        public string UltimateCountryCode { get; set; }
        public string UltimateEmail { get; set; }

        public string InterIdNumberTypeId { get; set; }
        public string InterIdNumber { get; set; }
        public string InterFirstName { get; set; }
        public string InterLastName { get; set; }
        public string InterPhoneNumber { get; set; }
        public string InterCity { get; set; }
        public string InterStateId { get; set; }
        public string InterStateCode { get; set; }
        public string InterPostalCodeId { get; set; }
        public string InterCompanyName { get; set; }
        public string InterAddressLine1 { get; set; }
        public string InterAddressLine2 { get; set; }
        public string InterCountryId { get; set; }
        public string InterCountryCode { get; set; }
        public string InterEmail { get; set; }

        public string FreightIdNumber { get; set; }
        public string FreightIdNumberTypeId { get; set; }
        public string FreightFirstName { get; set; }
        public string FreightLastName { get; set; }
        public string FreightPhoneNumber { get; set; }
        public string FreightCity { get; set; }
        public string FreightStateId { get; set; }
        public string FreightStateCode { get; set; }
        public string FreightPostalCodeId { get; set; }
        public string FreightCompanyName { get; set; }
        public string FreightAddressLine1 { get; set; }
        public string FreightAddressLine2 { get; set; }
        public string FreightEmail { get; set; }

        public string CarrierSCAC { get; set; }
        public string ConveyanceName { get; set; }
        public string TransportationReferenceNumber { get; set; }
        public string AesFileName { get; set; }
        public string AESTYPE { get; set; }
        public string ExporterName { get; set; }
        public string ExporterId { get; set; }
        public string ExporterFirstName { get; set; }
        public string ExporterLastName { get; set; }
        public string ExporterPhoneNumber { get; set; }
        public string ExporterCity { get; set; }
        public string ExporterStateId { get; set; }
        public string ExporterStateCode { get; set; }
        public string ExporterPostalCodeId { get; set; }
        public string ExporterAddress { get; set; }
        public string ExporterAddress2 { get; set; }
        public string ExporterCountryId { get; set; }
        public string ExporterCountryCode { get; set; }
        public string ExporterEmail { get; set; }

        public string isuploaded { get; set; }
        public string isdownloaded { get; set; }
        public bool isDraft { get; set; }
        public string Aes_Status { get; set; }
        public string isFreeze { get; set; }
        public string freezeRemarks { get; set; }
        public string PortId { get; set; }
        public string UnloadPortId { get; set; }
        
        public CertificationOfOriginODDTO[] CommodityDetail { get; set; }
        public EquipmentLine[] EquipmentLineDetail { get; set; }
        public Vin[] VinDetail { get; set; }
        public int? TotalCount { get; set; }

    }
    public class SLIFORMDATA
    {
        public int Id { get; set; }
        public string documentCommonId { get; set; }
        public string siteId { get; set; }
        public string shipmentNumber { get; set; }
        public string originState { get; set; }
        public string originStateId { get; set; }
        public string originStateCode { get; set; }
        public string countryDestination { get; set; }
        public string countryDestinationId { get; set; }
        public string countryDestinationCode { get; set; }
        public string portofExport { get; set; }
        public string portofExportId { get; set; }
        public string portofExportCode { get; set; }
        public string portofUnloading { get; set; }
        public string portofUnloadingId { get; set; }
        public string portofUnloadingCode { get; set; }
        public string departureDate { get; set; }
        public string inbondType { get; set; }
        public string importEntry { get; set; }
        public string foreginTradeZone { get; set; }
        public string relatedParty { get; set; }
        public string isRoutedTransaction { get; set; }
        public string isTIB { get; set; }
        public string isHazardousMaterial { get; set; }

        public string usppiCompany { get; set; }
        public string usppiFirstName { get; set; }
        public string usppiLastName { get; set; }
        public string usppiAddress { get; set; }
        public string usppiCity { get; set; }
        public string usppiState { get; set; }
        public string usppiStateId { get; set; }
        public string usppiStateCode { get; set; }
        public string usppiPostalCode { get; set; }
        public string usppiPhoneNumber { get; set; }
        public string usppiEmail { get; set; }
        public string usppiEIN { get; set; }
        public string usppiReference { get; set; }

        public string ultimateConsigneeType { get; set; }
        public string ultimateCompany { get; set; }
        public string ultimateFirstName { get; set; }
        public string ultimateLastName { get; set; }
        public string ultimateCountry { get; set; }
        public string ultimateCountryId { get; set; }
        public string ultimateCountryCode { get; set; }
        public string ultimateCity { get; set; }
        public string ultimateState { get; set; }
        public string ultimateStateId { get; set; }
        public string ultimateStateCode { get; set; }
        public string ultimatePostalCode { get; set; }
        public string ultimatePhoneNumber { get; set; }
        public string ultimateAddress { get; set; }



        public string interCompany { get; set; }
        public string interFirstName { get; set; }
        public string interLastName { get; set; }
        public string interCountry { get; set; }
        public string interCountryId { get; set; }
        public string interCountryCode { get; set; }
        public string interState { get; set; }
        public string interStateId { get; set; }
        public string interStateCode { get; set; }
        public string interCity { get; set; }
        public string interPostalCode { get; set; }
        public string interPhoneNumber { get; set; }
        public string interAddress { get; set; }

        public string freightCompany { get; set; }
        public string freightFirstName { get; set; }
        public string freightLastName { get; set; }
        public string freightState { get; set; }
        public string freightStateId { get; set; }
        public string freightStateCode { get; set; }
        public string freightCity { get; set; }
        public string freightPostalCode { get; set; }
        public string freightPhoneNumber { get; set; }
        public string freightAddress { get; set; }

        public string createdBy { get; set; }
        public CommodityLine[] CommodityDetail { get; set; }
        public Vin[] VinDetail { get; set; }

    }



}
