namespace AppMGL.DAL.Helper.Logging
{
    /// <summary>
    /// A File Source base, log factory
    /// </summary>
    public class FileSourceLogFactory
        : ILoggerFactory
    {
        /// <summary>
        /// Create the file source log
        /// </summary>
        /// <returns>New ILog based on File Source infrastructure</returns>
        public ILogger Create()
        {
            return new FileSourceLog();
        }
    }
}
