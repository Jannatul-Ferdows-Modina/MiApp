using System;
using System.Web;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Helper.Logging;

namespace AppMGL.DAL.UDT
{
    public class OperationResult
    {
        #region  Properties

        public object Data { get; set; }
        public int Count { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public EnumResult SqlException { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string ExceptionInnerMessage { get; set; }
        public string ExceptionInnerStackTrace { get; set; }

        #endregion

        #region Contructors

        public OperationResult(object data, object[] parameters)
        {
            Data = data;
            Count = Utility.GetParamValue(parameters, "Count", typeof(int));
            Status = true;
            SqlException = (EnumResult)Utility.GetParamValue(parameters, "ResultId");
        }

        public OperationResult(object data, int count, object[] parameters)
        {
            Data = data;
            Count = count;
            Status = true;
            SqlException = (EnumResult)Utility.GetParamValue(parameters, "ResultId");
        }

        public OperationResult(EnumAction action, object data, object[] parameters)
        {
            Data = data;
            Count = (data != null) ? 1 : 0;
            if (action == EnumAction.View)
            {
                Count = Utility.GetParamValue(parameters, "Count", typeof(int));
            }
            Status = ((EnumResult)Utility.GetParamValue(parameters, "ResultId") == EnumResult.Success);
            SqlException = (EnumResult)Utility.GetParamValue(parameters, "ResultId");
        }

        public OperationResult(EnumAction action, int rowsAffected, object[] parameters)
        {
            Data = null;
            Count = rowsAffected;
            SqlException = (EnumResult)Utility.GetParamValue(parameters, "ResultId");
            Status = (EnumResult.Success == SqlException);
            Message = CreateMessage();
        }

        public OperationResult(Exception ex)
            : this(ex.Message, ex)
        {
        }

        public OperationResult(string message, Exception ex)
        {
            Data = null;
            Status = false;
            Message = message;
            SqlException = EnumResult.Failed;

            if (ex != null)
            {
                ExceptionMessage = ex.Message;
                ExceptionStackTrace = ex.StackTrace;
                ExceptionInnerMessage = (ex.InnerException == null) ? null : ex.InnerException.Message;
                ExceptionInnerStackTrace = (ex.InnerException == null) ? null : ex.InnerException.StackTrace;

                Logger.WriteError(ex);
            }

            throw ex;
        }

        #endregion

        #region Private methods

        protected string CreateMessage()
        {
            var message = string.Empty;

            if (SqlException != EnumResult.Success)
            {
                if (SqlException == EnumResult.AnotherUserModifiedRecord)
                {
                    message = "Another user modified the record.";
                }
                else
                {
                    message = "Some error occurred, contact administrator.";
                }
            }

            return message;
        }

        #endregion
    }
}
