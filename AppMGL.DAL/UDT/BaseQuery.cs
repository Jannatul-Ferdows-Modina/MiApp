namespace AppMGL.DAL.UDT
{
    #region Base Query

    public abstract class BaseQuery
    {
        public virtual string List { get { return string.Empty; } }
        public virtual string Detail { get { return string.Empty; } }
        public virtual string Lookup { get { return string.Empty; } }
        public virtual string Insert { get { return string.Empty; } }
        public virtual string Update { get { return string.Empty; } }
        public virtual string Delete { get { return string.Empty; } }
        public virtual string ChangeStatus { get { return string.Empty; } }

        public static string ValidateAction { get { return "EXEC dbo.UGP_ValidateAction @LoginId, @ModuleId, @ActionId, @statusId, @ResultId OUT"; } }
    }

    #endregion

    //#region Security Queries

    //public class ErrorLogQuery : BaseQuery { }

    //public class ModuleTypeQuery : BaseQuery { }

    //public class ModuleQuery : BaseQuery { }

    //public class ActionQuery : BaseQuery { }

    //public class RoleQuery : BaseQuery { }

    //public class RoleMapQuery : BaseQuery { }

    //public class SiteQuery : BaseQuery { }

    //public class SiteRoleQuery : BaseQuery { }

    //public class UserQuery : BaseQuery { }

    //public class AspNetUsersQuery : BaseQuery { }

    //#endregion

   #region Setup Queries

    //public class ContactQuery : BaseQuery { }

    //public class CountryQuery : BaseQuery { }

    //public class DepartmentQuery : BaseQuery { }

    //public class LocationQuery : BaseQuery { }

    //public class StateQuery : BaseQuery { }

    //public class CityQuery : BaseQuery { }

    //public class CompanyGradationQuery : BaseQuery { }

    //public class SIPLUSer : BaseQuery { }

    //public class CustomerContactQuery : BaseQuery { }

    //public class TimezoneQuery : BaseQuery { }

    //public class TitleQuery : BaseQuery { }

    //public class DashboardQuery : BaseQuery { }

    //public class SIPLDepartment : BaseQuery { }

    #endregion

    #region DataManagement Queries

    //public class ContactCategoryQuery : BaseQuery { }

    //public class CommodityQuery : BaseQuery { }

    //public class CommodityTypeQuery : BaseQuery { }

    //public class ContainerTypeQuery : BaseQuery { }

    //public class LoadTypeQuery : BaseQuery { }

    //public class TradeServiceQuery : BaseQuery { }

    //public class ContinentQuery : BaseQuery { }

    //public class SIPLCountryQuery : BaseQuery { }

    //public class LGVWStateQuery : BaseQuery { }

    //public class LGVWCityQuery : BaseQuery { }

    //public class LGVWAliasQuery : BaseQuery { }

    //public class RailRampQuery : BaseQuery { }

    //public class TerminalQuery : BaseQuery { }

    //public class LGVWPortQuery : BaseQuery { }
    //public class LGVWSIPLContactQuery : BaseQuery { }

    //public class SurchargeGroupQuery : BaseQuery { }

    //public class SIPLDepartmentQuery : BaseQuery { }

    //public class SIPLContactQuery : BaseQuery { }

    //public class SIPLUserQuery : BaseQuery { }

    //public class SIPLStateQuery : BaseQuery { }

    //public class SIPLCityQuery : BaseQuery { }

    //public class SIPLPortQuery : BaseQuery { }

    //public class PortGroupQuery : BaseQuery { }

    //public class ContactWorkTypeQuery : BaseQuery { }

    //public class SIPLBookingStatusQuery : BaseQuery { }

    //public class LGACCTCategoryQuery : BaseQuery { }

    //public class LGSPFEECategoryQuery : BaseQuery { }

    

    #endregion

    #region Operation Queries

    //public class EnquiryListQuery : BaseQuery { }

    //public class EnquiryDetailQuery : BaseQuery { }
    
    //public class QuotationListQuery : BaseQuery { }

    //public class ContractRateQuery : BaseQuery { }

    //public class ContractRateModalQuery : BaseQuery { }


    //public class BookingListQuery : BaseQuery { }
    //public class DocumentationListQuery : BaseQuery { }

    //public class QuickBookListQuery : BaseQuery { }

    //public class DisplayRateQuery : BaseQuery { }

    #endregion

    #region Report Queries

    //public class EnquiryReportQuery : BaseQuery
    //{
    //    public override string List { get { return "EXEC dbo.RPT_GET_ENQUIRY_LIST @PageIndex, @PageSize, @Sort, @Year, @Type, @EnquiryDateFrom, @EnquiryDateTo, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId, @Count OUT"; } }
    //    public static string GetEnquiry { get { return "EXEC dbo.RPT_GET_ENQUIRY @Year, @Type, @EnquiryDateFrom, @EnquiryDateTo, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId"; } }
    //}

    //public class QuotationReportQuery : BaseQuery
    //{
    //    public override string List { get { return "EXEC dbo.RPT_GET_QUOTATION_LIST @PageIndex, @PageSize, @Sort, @Type, @EnquiryDateFrom, @EnquiryDateTo, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId, @Count OUT"; } }
    //    public static string GetQuotation { get { return "EXEC dbo.RPT_GET_QUOTATION @Type, @EnquiryDateFrom, @EnquiryDateTo, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId"; } }
    //    public static string GetQuotationEnquiry { get { return "EXEC dbo.GET_QUOTATION_ENQUIRY @QuotationId"; } }
    //    public static string GetQuotationCharges { get { return "EXEC dbo.GET_QUOTATION_CHARGES @QuotationId, @QuotationCarrierId, @PickupTypeId"; } }
    //}

    //public class BookingReportQuery : BaseQuery
    //{
    //    public override string List { get { return "EXEC dbo.RPT_GET_BOOKING_LIST @PageIndex, @PageSize, @Sort, @DepartmentId, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId, @Count OUT"; } }
    //    public static string GetBooking { get { return "EXEC dbo.RPT_GET_BOOKING @DepartmentId, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId"; } }
    //}

    //public class BookingStatusReportQuery : BaseQuery
    //{
    //    public override string List { get { return "EXEC dbo.RPT_GET_BOOKING_STATUS_LIST @PageIndex, @PageSize, @Sort, @BookingStatusId, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId, @Count OUT"; } }
    //    public static string GetBookingStatus { get { return "EXEC dbo.RPT_GET_BOOKING_STATUS @BookingStatusId, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId"; } }
    //}

    //public class MCSReportQuery : BaseQuery
    //{
    //    public override string List { get { return "EXEC dbo.USP_LG_GET_MCS_BY_UNIT @SiteId, @StartBookingDate, @EndBookingDate, @DeptId"; } }
    //}

    #endregion

    #region Report Queries

    //public class DocumentCommonQuery : BaseQuery
    //{
    //    public static string GetDocumentList { get { return "EXEC dbo.LG_GET_DOCUMENT_LIST @PageIndex, @PageSize, @Sort, @SitId, @DocumentStatus,@Count OUT"; } }
    //    public static string GetDocumentAttachmentDetail { get { return "EXEC dbo.USP_LG_GET_CONTRACT_DOCS @PageIndex, @PageSize, @Sort, @SitId,@contractID, @Count OUT"; } }

    //    public static string GetCommodityDetail { get { return "EXEC dbo.LG_GET_COMMODITY_DETAIL @Type, @DocumentCommonId"; } }
    //    public static string DeleteAllCommodityDetail { get { return "EXEC dbo.LG_DELETE_ALL_COMMODITY_DETAIL @Type, @DocumentCommonId"; } }
    //    public static string InsertCommodityDetail { get { return "EXEC dbo.LG_INSERT_COMMODITY_DETAIL @DocumentCommonId, @Number, @Package, @Commodity, @Weight, @Measuremant, @Type, @WtType, @MtType"; } }

    //    public static string GetFreightDetail { get { return "EXEC dbo.LG_GET_FREIGHT_DETAIL @Type, @HOID"; } }
    //    public static string DeleteAllFreightDetail { get { return "EXEC dbo.LG_DELETE_ALL_FREIGHT_DETAIL @Type, @HOID"; } }
    //    public static string InsertFreightDetail { get { return "EXEC dbo.LG_INSERT_FREIGHT_DETAIL @HOID, @FREIGHTCharge, @PREPAID, @COLLECT, @Type"; } }

    //    public static string GetDockReceipt { get { return "EXEC dbo.LG_GET_DOCK_RECEIPT @MiamiRefNo, @SystemRefNo, @BookingNo, @SitId"; } }
    //    public static string SaveDockReceipt { get { return "EXEC dbo.LG_SAVE_DOCK_RECEIPT @DocumentCommonId, @ExporterId, @ExporterAddress, @DocumentNumber, @BLNumber, @FileNo, @ExportRef, @ConsignedToId, @ConsignedToType, @IsPrintAddress, @FwdAgentId, @FTZNumber, @ConsigneeId, @ExportInstruction, @CarriageBy, @PlaceOfReceipt, @Vessel, @LandingPortId, @LoadingPert, @Voyage, @ForeignPortId, @Transshipment, @MoveType, @DeliveredBy, @ReqStuffingDate, @CheckedBY, @Issued, @Booking, @CutOff, @EmptyPickup, @FullReturn, @AirId, @IsShipper, @SitId, @DeliverOrderId OUT, @ReturnCommonId OUT,@Free_Text"; } }
    //    public static string SaveDocuments { get { return "EXEC dbo.LG_SAVE_DOCUMENTS @DocumentCommonId, @SdDocName, @sdDocType, @SdCreatedTs, @LoginId, @SdUpdatedTs"; } }
    //    public static string SaveContratDocuments { get { return "EXEC dbo.LG_SAVE_CONTRACT_DOCUMENTS @DocumentCommonId, @SdDocName, @sdDocType, @SdCreatedTs, @LoginId, @SdUpdatedTs"; } }

    //    public static string GetHBL { get { return "EXEC dbo.LG_GET_HBL @MiamiRefNo, @SystemRefNo, @BookingNo, @SitId"; } }
    //    public static string SaveHBL { get { return "EXEC dbo.LG_SAVE_HBL @DocumentCommonId, @ExporterId, @ExporterAddress, @DocumentNumber, @BLNumber, @FileNo, @ExportRef, @ConsignedToId, @FwdAgentId, @FTZNumber, @ConsigneeId, @ConsignedToType, @IsPrintAddress, @ExportInstruction, @CarriageBy, @PlaceOfReceipt, @Vessel, @LandingPortId, @LoadingPert, @Voyage, @ForeignPortId, @Transshipment, @MoveType, @Place, @ExportingCarrier, @SignDate, @Type, @AirId, @SitId, @HouseBLID OUT, @ReturnCommonId OUT"; } }

    //    public static string GetMBL { get { return "EXEC dbo.LG_GET_MBL @MiamiRefNo, @SystemRefNo, @BookingNo, @SitId"; } }
    //    public static string SaveMBL { get { return "EXEC dbo.LG_SAVE_MBL @DocumentCommonId, @ExporterId, @ExporterAddress, @DocumentNumber, @BLNumber, @FileNo, @ExportRef, @ConsignedToId, @ConsignedToType, @IsPrintAddress, @FwdAgentId, @FTZNumber, @ConsigneeId, @ExportInstruction, @CarriageBy, @PlaceOfReceipt, @Vessel, @LandingPortId, @LoadingPert, @Voyage, @ForeignPortId, @Transshipment, @MoveType, @Containerized, @Place, @ExportingCarrier, @SignDate, @Type, @AirId, @SitId, @HouseBLID OUT, @ReturnCommonId OUT"; } }

    //    public static string GetCO { get { return "EXEC dbo.LG_GET_CO @MiamiRefNo, @SystemRefNo, @BookingNo, @SitId"; } }
    //    public static string SaveCO { get { return "EXEC dbo.LG_SAVE_CO @DocumentCommonId, @ExporterId, @ExporterAddress, @DocumentNumber, @BLNumber, @FileNo, @ExportRef, @ConsignedToId, @FwdAgentId, @FTZNumber, @ConsigneeId, @ConsignedToType, @IsPrintAddress, @ExportInstruction, @CarriageBy, @PlaceOfReceipt, @LandingPortId, @LoadingPert, @ForeignPortId, @Transshipment, @MoveType, @Containerized, @ExportingCarrier, @Undersigned, @DatedAt, @Dayof1, @Dayof2, @SignDayof1, @SignDayof2, @Chamber, @Stateof, @Secretary, @Type, @AirId, @SitId, @COID OUT, @ReturnCommonId OUT"; } }
    //}

    #endregion
}
