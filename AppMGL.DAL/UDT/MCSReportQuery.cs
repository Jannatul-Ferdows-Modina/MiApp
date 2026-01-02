namespace AppMGL.DAL.UDT
{
	public class MCSReportQuery : BaseQuery
	{
       // public override string List { get { return "EXEC dbo.USP_LG_GET_MCS_BY_UNIT_Report @SiteId, @StartBookingDate, @EndBookingDate, @DeptId,@isinvoiceready,@SystemRefNo"; } }
        public override string List { get { return "EXEC dbo.USP_LG_GET_MCS_BY_UNIT_Report @SiteId, @StartBookingDate, @EndBookingDate, @DeptId,@isinvoiceready,@SystemRefNo,@QBStatus,@QBMaturedMonth,@UserId"; } }

        public  string SepataraList { get { return "EXEC dbo.[USP_LG_GET_MCS_Sepatara @SiteId, @StartBookingDate, @EndBookingDate, @DeptId,@isinvoiceready,@SystemRefNo,@QBStatus,@QBMaturedMonth"; } }
    }
}
