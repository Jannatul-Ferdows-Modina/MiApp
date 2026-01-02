using System;

namespace AppMGL.DAL.Helper.Helper
{
    public class ClientException : Exception
    {
        private readonly string _stackTrace;

        public ClientException(string message, string stackTrace)
            : base(message)
        {
            _stackTrace = stackTrace;
        }

        public override string StackTrace
        {
            get
            {
                return _stackTrace;
            }
        }
    }
}
