namespace AppMGL.DAL.UDT
{
	public class QuotationReportQuery : BaseQuery
	{
		public override string List { get { return  "EXEC dbo.RPT_GET_QUOTATION_LIST @PageIndex, @PageSize, @Sort, @Type, @EnquiryDateFrom, @EnquiryDateTo, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId, @Count OUT";}}

		public static string GetQuotation {get{ return  "EXEC dbo.RPT_GET_QUOTATION @Type, @EnquiryDateFrom, @EnquiryDateTo, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId";}}

		public static string GetQuotationEnquiry {get{ return  "EXEC dbo.GET_QUOTATION_ENQUIRY @QuotationId";}}

        public static string GetQuotationCharges { get { return "EXEC dbo.GET_QUOTATION_CHARGES @QuotationId, @QuotationCarrierId, @PickupTypeId"; } }
	}
}
