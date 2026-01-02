namespace AppMGL.DTO.Security
{
    public class ErrorLogDTO
    {
        public long ErrorTypeId { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public long ModuleId { get; set; }
    }
}