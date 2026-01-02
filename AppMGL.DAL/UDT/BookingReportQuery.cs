namespace AppMGL.DAL.UDT
{
	public class BookingReportQuery : BaseQuery
	{
		public override string List { get { return "EXEC dbo.RPT_GET_BOOKING_LIST @PageIndex, @PageSize, @Sort, @DepartmentId, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId, @Count OUT";}}

        public static string GetBooking { get { return "EXEC dbo.RPT_GET_BOOKING @DepartmentId, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId"; } }
	}
}
