namespace AppMGL.DAL.UDT
{
	public class MCSQBQuery : BaseQuery
	{
        public override string List { get { return "EXEC dbo.USP_LG_QUICKBOOK_INVOCE_RECEIPT @Documentno ,@userid"; } }
	}
}
