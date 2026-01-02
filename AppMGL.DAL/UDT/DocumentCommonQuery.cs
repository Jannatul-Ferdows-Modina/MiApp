namespace AppMGL.DAL.UDT
{
	public class DocumentCommonQuery : BaseQuery
	{
		public static string GetDocumentList { get { return "EXEC dbo.LG_GET_DOCUMENT_LIST_New @PageIndex, @PageSize, @Sort, @SitId, @DocumentStatus,@Count OUT,@FileNo"; } }

		public static string GetDocumentAttachmentDetail { get { return "EXEC dbo.USP_LG_GET_CONTRACT_DOCS @PageIndex, @PageSize, @Sort, @SitId,@contractID, @Count OUT"; } }

		public static string GetCommodityDetail { get { return "EXEC dbo.LG_GET_COMMODITY_DETAIL @Type, @DocumentCommonId"; } }

		public static string DeleteAllCommodityDetail { get { return "EXEC dbo.LG_DELETE_ALL_COMMODITY_DETAIL @Type, @DocumentCommonId"; } }

		public static string InsertCommodityDetail { get { return "EXEC dbo.LG_INSERT_COMMODITY_DETAIL @DocumentCommonId, @Number, @Package, @Commodity, @Weight, @Measuremant, @Type, @WtType, @MtType"; } }

		public static string GetFreightDetail { get { return "EXEC dbo.LG_GET_FREIGHT_DETAIL @Type, @HOID"; } }

		public static string DeleteAllFreightDetail { get { return "EXEC dbo.LG_DELETE_ALL_FREIGHT_DETAIL @Type, @HOID"; } }

		public static string InsertFreightDetail { get { return "EXEC dbo.LG_INSERT_FREIGHT_DETAIL @HOID, @FREIGHTCharge, @PREPAID, @COLLECT, @Type"; } }
		public static string GetDockReceiptList { get { return "EXEC dbo.LG_GET_DOCKRECEIPT_LIST @PageIndex, @PageSize, @Sort, @SitId, @DocumentStatus,@Count OUT,@FileNo"; } }
		public static string GetDockReceipt { get { return "EXEC dbo.LG_GET_DOCK_RECEIPT @MiamiRefNo, @SystemRefNo, @BookingNo, @SitId"; } }

		public static string SaveDockReceipt { get { return "EXEC dbo.LG_SAVE_DOCK_RECEIPT @DocumentCommonId, @ExporterId, @ExporterAddress, @DocumentNumber, @BLNumber, @FileNo, @ExportRef, @ConsignedToId,@ConsignedToAddress, @ConsignedToType, @IsPrintAddress, @FwdAgentId,@FwdAgentAddress, @FTZNumber, @ConsigneeId,@ConsigneeAddress, @ExportInstruction, @CarriageBy, @PlaceOfReceipt, @Vessel, @LandingPortId, @LoadingPert, @Voyage, @ForeignPortId, @Transshipment, @MoveType, @DeliveredBy, @ReqStuffingDate, @CheckedBY, @Issued, @Booking, @CutOff, @EmptyPickup, @FullReturn, @AirId, @IsShipper, @SitId, @DeliverOrderId OUT, @ReturnCommonId OUT,@Free_Text,@EmptyPickupId,@fullReturnId,@IsDockTemplate"; } }

		public static string SaveDocuments { get { return "EXEC dbo.LG_SAVE_DOCUMENTS @DocumentCommonId, @SdDocName, @sdDocType, @SdCreatedTs, @LoginId, @SdUpdatedTs"; } }

		public static string SaveContratDocuments { get { return "EXEC dbo.LG_SAVE_CONTRACT_DOCUMENTS @DocumentCommonId, @SdDocName, @sdDocType, @SdCreatedTs, @LoginId, @SdUpdatedTs"; } }

		public static string GetHBL { get { return "EXEC dbo.LG_GET_HBL @SystemRefNo, @BookingNo, @SitId"; } }

		//public static string SaveHBL { get { return "EXEC dbo.LG_SAVE_HBL @DocumentCommonId, @ExporterId, @ExporterAddress, @DocumentNumber, @BLNumber, @FileNo, @ExportRef, @ConsignedToId, @ConsignedToType,@IsPrintAddress, @FwdAgentId, @FTZNumber, @ConsigneeId, @ConsignedToType, @IsPrintAddress, @ExportInstruction, @CarriageBy, @PlaceOfReceipt, @Vessel, @LandingPortId, @LoadingPert, @Voyage, @ForeignPortId, @Transshipment, @MoveType, @Place, @ExportingCarrier, @SignDate, @Type, @AirId, @SitId, @HouseBLID OUT, @ReturnCommonId OUT"; }}

		public static string SaveHBL { get { return "EXEC dbo.LG_SAVE_HBL    @DocumentCommonId , @ExporterId , @ExporterAddress, @DocumentNumber , @BLNumber , @FileNo ,@ExportRef ,@ConsignedToId,@ConsignedToAddress , @ConsignedToType,@IsPrintAddress , @FwdAgentId,@FwdAgentAddress,@FTZNumber , @ConsigneeId,@ConsigneeAddress ,  @ExportInstruction ,    @CarriageBy ,    @PlaceOfReceipt , @Vessel ,    @LandingPortId ,    @LoadingPert ,    @Voyage ,    @ForeignPortId ,    @Transshipment ,    @MoveType, @Place, @ExportingCarrier , @SignDate ,@Type ,@AirId ,  @SitId ,@HouseBLID ,@ReturnCommonId OUT,@IsHBLTemplate"; } }

		public static string GetMBL { get { return "EXEC dbo.LG_GET_MBL  @SystemRefNo, @BookingNo, @SitId"; } }

		public static string SaveMBL { get { return "EXEC dbo.LG_SAVE_MBL @DocumentCommonId, @ExporterId, @ExporterAddress, @DocumentNumber, @BLNumber, @FileNo, @ExportRef, @ConsignedToId,@ConsignedToAddress, @ConsignedToType, @IsPrintAddress, @FwdAgentId,@FwdAgentAddress, @FTZNumber, @ConsigneeId,@ConsigneeAddress, @ExportInstruction, @CarriageBy, @PlaceOfReceipt, @Vessel, @LandingPortId, @LoadingPert, @Voyage, @ForeignPortId, @Transshipment, @MoveType, @Containerized, @Place, @ExportingCarrier, @SignDate, @Type, @AirId, @SitId, @HouseBLID OUT, @ReturnCommonId OUT"; } }

		public static string GetCO { get { return "EXEC dbo.LG_GET_CO @SystemRefNo, @BookingNo, @SitId"; } }

        //public static string SaveCO { get { return "EXEC dbo.LG_SAVE_CO @DocumentCommonId, @ExporterId, @ExporterAddress, @DocumentNumber, @BLNumber, @FileNo, @ExportRef, @ConsignedToId, @FwdAgentId, @FTZNumber, @ConsigneeId, @ConsignedToType, @IsPrintAddress, @ExportInstruction, @CarriageBy, @PlaceOfReceipt, @LandingPortId, @LoadingPert, @ForeignPortId, @Transshipment, @MoveType, @Containerized, @ExportingCarrier, @Undersigned, @DatedAt, @Dayof1, @Dayof2, @SignDayof1, @SignDayof2, @Chamber, @Stateof, @Secretary, @Type, @AirId, @SitId, @COID OUT, @ReturnCommonId OUT";}}
        public static string SaveCO { get { return "EXEC dbo.LG_SAVE_CO  @DocumentCommonId, @ExporterId, @ExporterAddress , @DocumentNumber, @BLNumber, @FileNo, @ExportRef, @ConsignedToId,@ConsignedToAddres, @ConsignedToType, @IsPrintAddress, @FwdAgentId,@FwdAgentAddress, @FTZNumber , @ConsigneeId ,@ConsigneeAddress, @ExportInstruction , @CarriageBy , @PlaceOfReceipt , @LandingPortId , @LoadingPert , @ForeignPortId , @Transshipment , @MoveType , @Containerized , @ExportingCarrier ,@Undersigned , @DatedAt , @Dayof1 , @Dayof2 , @SignDayof1 , @SignDayof2 , @Chamber , @Stateof , @Secretary , @Type , @AirId , @SitId , @COID OUT , @ReturnCommonId OUT,@Voyage,@Vessel"; } }

		public static string GetEquipmentLineDetail { get { return "EXEC dbo.AES_GetEquipmentLineDetail_SP @DocumentCommonId,@SiteId,@AesId"; } }

		public static string GetCommodityLineDetail { get { return "EXEC dbo.AES_GetCommodityLineDetail_SP @AesId,@SiteId,@etype, @DocumentCommonId"; } }
		public static string GetVinDetail { get { return "EXEC dbo.AES_GetVINDetail_SP @DocumentCommonId,@SiteId,@etype,@AesId"; } }
		public static string GetFilesList { get { return "EXEC dbo.ShoweesFiles_SP @DocumentCommonId,@AesId"; } }
		public static string GetConsolidateDockReceiptList { get { return "EXEC dbo.LG_ConsolidateDockReceiptList @PageIndex, @PageSize, @Sort, @SitId,@Count OUT,@FileNo"; } }
		public static string GetConsolidateDockReceipt { get { return "EXEC dbo.LG_ConsolidateDockReceipt @BookedId"; } }
		
		public static string SaveAddDockReceipt { get { return "EXEC dbo.LG_ADD_DOCK_RECEIPT @DocumentCommonId, @ExporterId, @ExporterAddress, @DocumentNumber, @BLNumber, @FileNo, @ExportRef, @ConsignedToId,@ConsignedToAddress, @ConsignedToType, @IsPrintAddress, @FwdAgentId,@FwdAgentAddress, @FTZNumber, @ConsigneeId,@ConsigneeAddress, @ExportInstruction, @CarriageBy, @PlaceOfReceipt, @Vessel, @LandingPortId, @LoadingPert, @Voyage, @ForeignPortId, @Transshipment, @MoveType, @DeliveredBy, @ReqStuffingDate, @CheckedBY, @Issued, @Booking, @CutOff,@DepartureDate, @EmptyPickup, @FullReturn, @AirId, @IsShipper, @SitId, @DeliverOrderId,@Free_Text,@EmptyPickupId,@fullReturnId,@IsDockTemplate,@CustomerId,@CustomerName,@CustFirstName,@CustLastName,@CustPhoneNumber,@CustAddressLine1,@CustAddressLine2,@CustCountryName,@CustCountryId,@CustPostalCode,@CustCityId,@CustCityName,@CustStateName,@CustStateId,@CreatedBy,@ModifiedBy"; } }
		public static string GetAddDockReceipt { get { return "EXEC dbo.LG_GETADD_DOCK_RECEIPT @DocumentCommonId"; } }
		public static string GetAddDockReceiptList { get { return "EXEC dbo.LG_GET_ADDDOCKRECEIPT_LIST @PageIndex, @PageSize, @Sort, @SitId, @DocumentStatus,@Count OUT,@FileNo"; } }
		

	}
}
