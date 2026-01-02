using System.Collections.Generic;
using System.Configuration;
using System.Net;
using AppMGL.Manager.ReportExecutionProvider;

namespace AppMGL.Manager.Infrastructure.Report
{
    public class ReportServerProxy
    {
        #region Public Methods

        public byte[] Render(string reportPath, List<KeyValuePair<string, string>> reportParams, ReportFormat reportFormat)
        {
            ReportExecutionService rs = new ReportExecutionService();
            string username = ConfigurationManager.AppSettings["RSWUsername"];
            string password = ConfigurationManager.AppSettings["RSWPassword"];
            string domain = ConfigurationManager.AppSettings["RSWDomain"];

            rs.Credentials = new NetworkCredential(username, password);

            string historyId = null;

            rs.LoadReport(reportPath, historyId);

            ParameterValue[] parameters = new ParameterValue[reportParams.Count];
            for (int k = 0; k < reportParams.Count; k++)
            {
                parameters[k] = new ParameterValue();
                parameters[k].Name = reportParams[k].Key;
                parameters[k].Value = reportParams[k].Value;
            }

            rs.SetExecutionParameters(parameters, "en-us");

            string format = GetFormat(reportFormat);
            string deviceInfo = GetDeviceInfo(reportFormat);
            byte[] result = null;
            string extension = string.Empty;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            Warning[] warnings = null;
            string[] streamIds = null;

            result = rs.Render(format, deviceInfo, out extension, out mimeType, out encoding, out warnings, out streamIds);

            return result;
        }

        #endregion

        #region Private Methods

        private string GetFormat(ReportFormat reportFormat)
        {
            string format = string.Empty;
            switch (reportFormat)
            {
                case ReportFormat.XLSX:
                    format = "EXCELOPENXML";
                    break;
                case ReportFormat.DOCX:
                    format = "WORDOPENXML";
                    break;
                case ReportFormat.XLS:
                    format = "EXCEL";
                    break;
                case ReportFormat.DOC:
                    format = "WORD";
                    break;
                case ReportFormat.HTML:
                    format = "HTML4.0";
                    break;
                default:
                    format = reportFormat.ToString();
                    break;
            }
            return format;
        }

        private string GetDeviceInfo(ReportFormat reportFormat)
        {
            string deviceInfo = string.Empty;
            if (reportFormat == ReportFormat.HTML)
            {
                deviceInfo = "<DeviceInfo><HTMLFragment>True</HTMLFragment></DeviceInfo>";
            }
            return deviceInfo;
        }

        #endregion
    }

    public enum ReportFormat
    {
        PDF,
        XLSX,
        DOCX,
        XLS,
        DOC,
        CSV,
        HTML
    };
}