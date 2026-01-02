using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using AppMGL.DAL.Helper.Helper;

namespace AppMGL.DAL.Helper
{
    public static class Utility
    {
        #region Public Methods

        public static string GetSort(string sorts)
        {
            var sort = JsonConvert.DeserializeObject<Dictionary<string, string>>(sorts);
            var sortClause = sort.Select(f => f.Key + " " + f.Value).ToList();
            return string.Join<string>(", ", sortClause);
        }

        public static string GetFilter(string filters)
        {
            var filter = JsonConvert.DeserializeObject<List<Filter>>(filters);
            return string.Join<string>(" AND ", GetFilterClause(filter));
        }

        public static string GetWhere(string filters)
        {
            var filter = JsonConvert.DeserializeObject<List<Filter>>(filters);
            if (filter.Count == 0)
            {
                return "1=1";
            }
            return string.Join<string>(" AND ", GetWhereClause(filter));
        }
        public static string GetWhere1(string filters)
        {
            var filter = JsonConvert.DeserializeObject<List<Filter>>(filters);
            if (filter.Count == 0)
            {
                return "1=1";
            }
            return string.Join<string>(" AND ", GetWhereClause1(filter));
        }
        public static string GetWhere2(string filters)
        {
            var filter = JsonConvert.DeserializeObject<List<Filter>>(filters);
            if (filter.Count == 0)
            {
                return "1=1";
            }
            return string.Join<string>(" AND ", GetWhereClause2(filter));
        }
        public static void SetParamValue(object[] parameters, string parameterName, object parameterValue)
        {
            parameters.Cast<SqlParameter>().ToArray().Single(p => p.ParameterName == parameterName).Value = parameterValue;
        }

        public static void SetParamValue(List<SqlParameter> parameters, string parameterName, object parameterValue)
        {
            parameters.Single(p => p.ParameterName == parameterName).Value = parameterValue;
        }

        public static long GetParamValue(object[] parameters, string parameterName)
        {
            return Convert.ToInt64(parameters.Cast<SqlParameter>().ToArray().Single(p => p.ParameterName == parameterName).Value);
        }

        public static dynamic GetParamValue(object[] parameters, string parameterName, Type valueType)
        {
            return Convert.ChangeType(parameters.Cast<SqlParameter>().ToArray().Single(p => p.ParameterName == parameterName).Value, valueType);
        }

        public static dynamic GetParamValue(List<SqlParameter> parameters, string parameterName, Type valueType)
        {
            return Convert.ChangeType(parameters.Single(p => p.ParameterName == parameterName).Value, valueType);
        }

        public static string XmlSerialize<TEntity>(List<TEntity> listEntity)
        {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                NewLineHandling = NewLineHandling.Entitize
            };
            XmlSerializer serializer = new XmlSerializer(typeof(List<TEntity>));
            StringWriter writer = new StringWriter();
            using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
            {
                serializer.Serialize(xmlWriter, listEntity);
            }
            return (writer.ToString());
        }

        public static List<TEntity> XmlDeserialize<TEntity>(string serializedXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<TEntity>));
            StringReader reader = new StringReader(serializedXml);
            return ((List<TEntity>)serializer.Deserialize(reader));
        }

        public static string CreateHtmlTable(List<KeyValuePair<string, string>> data)
        {
            List<string> rows = new List<string>();
            foreach (KeyValuePair<string, string> d in data)
            {
                if (d.Value == "#SUB#HEADING#")
                {
                    rows.Add(string.Format("<tr><td colspan='2' style='border: 1px Solid Black;vertical-align: top;background-color: #cee3f6;'><b>{0}</b></td></tr>", d.Key));
                }
                else
                {
                    rows.Add(string.Format("<tr><td style='border: 1px Solid Black;vertical-align: top;background-color: #f4f4f4;'><b>{0}</b></td><td style='border: 1px Solid Black;vertical-align: top;'>{1}</td></tr>", d.Key, d.Value));
                }
            }
            return CreateHtmlTable(rows);
        }

        public static string CreateHtmlTable(List<string> rows)
        {
            return "<table style='width: 100%; padding: 2px; border: 1px Solid Black; border-collapse: collapse;'>" + string.Join(string.Empty, rows) + "</table>";
        }

        public static string ReplacePlaceHolder(string text, Dictionary<string, string> placeHolder)
        {
            foreach (KeyValuePair<string, string> ph in placeHolder)
            {
                if (ph.Key == "EmailAddresses" || ph.Key == "EmailAddressesCC" || ph.Key == "EmailAddressesBCC")
                {
                    continue;
                }
                string key = string.Format("~{0}~", ph.Key);
                if (text.Contains(key))
                {
                    text = text.Replace(key, ph.Value);
                }
            }
            return text;
        }

        #endregion

        #region Private Methods

        private static List<string> GetFilterClause(List<Filter> filter)
        {
            var filterClause = new List<string>();

            foreach (var f in filter)
            {
                if (f.Operator == "contains")
                {
                    filterClause.Add(string.Format("{0} LIKE '%{1}%'", f.FieldName, f.Value));
                }
                else if (f.Operator == "containsIn")
                {
                    filterClause.Add(string.Format("{0} IN ({1})", f.FieldName, f.Value));
                }
                else if (f.Operator == "startWith")
                {
                    filterClause.Add(string.Format("{0} LIKE '{1}%'", f.FieldName, f.Value));
                }
                else if (f.Operator == "equalTo")
                {
                    if (f.Type == "datetime")
                    {
                        filterClause.Add(string.Format("{0} = '{1}'", f.FieldName, f.Value));
                    }
                    else
                    {
                        filterClause.Add(string.Format("{0} = {1}", f.FieldName, f.Value));
                    }
                }
                else if (f.Operator == "notEqualTo")
                {
                    if (f.Type == "datetime")
                    {
                        filterClause.Add(string.Format("{0} <> '{1}'", f.FieldName, f.Value));
                    }
                    else
                    {
                        filterClause.Add(string.Format("{0} <> {1}", f.FieldName, f.Value));
                    }
                }
                else if (f.Operator == "greaterThan")
                {
                    if (f.Type == "datetime")
                    {
                        filterClause.Add(string.Format("{0} > '{1}'", f.FieldName, f.Value));
                    }
                    else
                    {
                        filterClause.Add(string.Format("{0} > {1}", f.FieldName, f.Value));
                    }
                }
                else if (f.Operator == "lessThan")
                {
                    if (f.Type == "datetime")
                    {
                        filterClause.Add(string.Format("{0} < '{1}'", f.FieldName, f.Value));
                    }
                    else
                    {
                        filterClause.Add(string.Format("{0} < {1}", f.FieldName, f.Value));
                    }
                }
                else if (f.Operator == "between")
                {
                    if (f.Type == "datetime")
                    {
                        filterClause.Add(string.Format("{0} BETWEEN '{1}' AND '{2}'", f.FieldName, f.Value, f.ValueT));
                    }
                    else
                    {
                        filterClause.Add(string.Format("{0} BETWEEN {1} AND {2}", f.FieldName, f.Value, f.ValueT));
                    }
                }
            }

            return filterClause;
        }

        private static List<string> GetWhereClause(List<Filter> filter)
        {
            var filterClause = new List<string>();

            foreach (var f in filter)
            {
                if (f.Operator == "contains")
                {
                    if (f.Type == "decimal")
                    {
                        filterClause.Add(string.Format("{0}.ToString().Contains(\"{1}\")", f.FieldName, f.Value));
                    }
                    else if (f.Type == "numeric")
                    {
                        filterClause.Add(string.Format("{0}.Value.ToString().Contains(\"{1}\")", f.FieldName, f.Value));
                    }
                    else
                    {
                        filterClause.Add(string.Format("{0}.Contains(\"{1}\")", f.FieldName, f.Value));
                    }
                }
                else if (f.Operator == "startWith")
                {
                    if (f.Type == "decimal")
                    {
                        filterClause.Add(string.Format("{0}.ToString().StartsWith(\"{1}\")", f.FieldName, f.Value));
                    }
                    else if (f.Type == "numeric")
                    {
                        filterClause.Add(string.Format("{0}.Value.ToString().StartsWith(\"{1}\")", f.FieldName, f.Value));
                    }
                    else
                    {
                        filterClause.Add(string.Format("{0}.StartsWith(\"{1}\")", f.FieldName, f.Value));
                    }
                }
                else if (f.Operator == "equalTo")
                {
                    if (f.Type == "decimal")
                    {
                        filterClause.Add(string.Format("{0} = {1}", f.FieldName, f.Value));
                    }
                    else if (f.Type == "numeric")
                    {
                        filterClause.Add(string.Format("{0} = {1}", f.FieldName, f.Value));
                    }
                    else
                    {
                        filterClause.Add(string.Format("{0} = {1}", f.FieldName, f.Value));
                    }
                }
            }

            return filterClause;
        }
        private static List<string> GetWhereClause1(List<Filter> filter)
        {
            var filterClause = new List<string>();

            foreach (var f in filter)
            {
                if (f.Operator == "contains")
                {
                    if (f.Type == "decimal")
                    {
                        filterClause.Add(string.Format("{0}.ToString().Contains(\"{1}\")", f.FieldName, f.Value));
                    }
                    else if (f.Type == "numeric")
                    {
                        filterClause.Add(string.Format("{0}.Value.ToString().Contains(\"{1}\")", f.FieldName, f.Value));
                    }
                    if (filter.Count > 1)
                    {
                        filterClause.Add(string.Format("{0} like '%{1}'", f.FieldName, f.Value));
                    }
                    else
                    {
                        filterClause.Add(string.Format("{0}", f.Value));
                    }
                }
                else if (f.Operator == "startWith")
                {
                    if (f.Type == "decimal")
                    {
                        filterClause.Add(string.Format("{0}.ToString().StartsWith(\"{1}\")", f.FieldName, f.Value));
                    }
                    else if (f.Type == "numeric")
                    {
                        filterClause.Add(string.Format("{0}.Value.ToString().StartsWith(\"{1}\")", f.FieldName, f.Value));
                    }
                    else
                    {
                        filterClause.Add(string.Format("{0}.StartsWith(\"{1}\")", f.FieldName, f.Value));
                    }
                }
                else if (f.Operator == "equalTo")
                {
                    if (f.Type == "decimal")
                    {
                        filterClause.Add(string.Format("{0} = {1}", f.FieldName, f.Value));
                    }
                    else if (f.Type == "numeric")
                    {
                        filterClause.Add(string.Format("{0} = {1}", f.FieldName, f.Value));
                    }
                    else
                    {
                        filterClause.Add(string.Format("{0} = {1}", f.FieldName, f.Value));
                    }
                }
            }

            return filterClause;
        }
        private static List<string> GetWhereClause2(List<Filter> filter)
        {
            var filterClause = new List<string>();

            foreach (var f in filter)
            {
                if (f.Operator == "contains")
                {
                    if (f.Type == "decimal")
                    {
                        filterClause.Add(string.Format("{0}.ToString().Contains(\"{1}\")", f.FieldName, f.Value));
                    }
                    else if (f.Type == "numeric")
                    {
                        filterClause.Add(string.Format("{0}.Value.ToString().Contains(\"{1}\")", f.FieldName, f.Value));
                    }
                    
                    else
                    {
                        filterClause.Add(string.Format("{0} like '{1}%'", f.FieldName, f.Value));
                    }
                }
                else if (f.Operator == "startWith")
                {
                    if (f.Type == "decimal")
                    {
                        filterClause.Add(string.Format("{0}.ToString().StartsWith(\"{1}\")", f.FieldName, f.Value));
                    }
                    else if (f.Type == "numeric")
                    {
                        filterClause.Add(string.Format("{0}.Value.ToString().StartsWith(\"{1}\")", f.FieldName, f.Value));
                    }
                    else
                    {
                        filterClause.Add(string.Format("{0}.StartsWith(\"{1}\")", f.FieldName, f.Value));
                    }
                }
                else if (f.Operator == "equalTo")
                {
                    if (f.Type == "decimal")
                    {
                        filterClause.Add(string.Format("{0} = {1}", f.FieldName, f.Value));
                    }
                    else if (f.Type == "numeric")
                    {
                        filterClause.Add(string.Format("{0} = {1}", f.FieldName, f.Value));
                    }
                    else
                    {
                        filterClause.Add(string.Format("{0} = {1}", f.FieldName, f.Value));
                    }
                }
            }

            return filterClause;
        }
        public static string GetWhere3(string filters)
        {
            var filter = JsonConvert.DeserializeObject<List<Filter>>(filters);
            if (filter.Count == 0)
            {
                return "1=1";
            }
            return string.Join<string>(" AND ", GetWhereClause3(filter));
        }
        private static List<string> GetWhereClause3(List<Filter> filter)
        {
            var filterClause = new List<string>();

            foreach (var f in filter)
            {
                filterClause.Add(string.Format("{0} like '{1}%'", f.FieldName, f.Value));
            }

            return filterClause;
        }
        #endregion
    }
}