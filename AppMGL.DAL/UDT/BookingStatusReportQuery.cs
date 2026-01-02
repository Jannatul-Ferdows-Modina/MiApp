namespace AppMGL.DAL.UDT
{
	public class BookingStatusReportQuery : BaseQuery
	{
		public override string List { get { return "EXEC dbo.RPT_GET_BOOKING_STATUS_LIST @PageIndex, @PageSize, @Sort, @BookingStatusId, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId, @Count OUT";}}

        public static string GetBookingStatus { get { return "EXEC dbo.RPT_GET_BOOKING_STATUS @BookingStatusId, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId"; } }
	}
}
