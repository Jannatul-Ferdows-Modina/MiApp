namespace AppMGL.DAL.UDT
{
	public class EnquiryReportQuery : BaseQuery
	{
		public override string List { get { return "EXEC dbo.RPT_GET_ENQUIRY_LIST @PageIndex, @PageSize, @Sort, @Year, @Type, @EnquiryDateFrom, @EnquiryDateTo, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId, @Count OUT";}}

        public static string GetEnquiry { get { return "EXEC dbo.RPT_GET_ENQUIRY @Year, @Type, @EnquiryDateFrom, @EnquiryDateTo, @EnquiryNo, @CompanyName, @QuotationNo, @SystemRefNo, @MiamiRefNo, @BookingNo, @SitId"; } }
	}
}
