namespace AppMGL.DAL.Helper
{
    public class ListParams
    {
        public long UserId { get; set; }
        public string SiteId { get; set; }
        public long ModuleId { get; set; }
        public long UserWorkTypeId { get; set; }
        public long OtherId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Sort { get; set; }
        public string Filter { get; set; }

        public decimal CwtId { get; set; }
    }


    public class FilterModel
    {
        public string name { get; set; }
        public string type { get; set; }
        public string fieldName { get; set; }
        public string value { get; set; }
        public string @operator { get; set; }
        public string valueT { get; set; }
    }
}
