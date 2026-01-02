using System;
using System.Collections.Generic;
using System.Data;

namespace AppMGL.DTO.Operation
{
    public class CustomerUnitDTO
    {
        public int ContactID { get; set; }
        public string CompanyName { get; set; }
        public string CustomerCode { get; set; }
        public string ContactCategoryID { get; set; }
        public decimal? SitId { get; set; }
        public decimal? ReferredBySitId { get; set; }
        public string SitName { get; set; }
        public string ReferredBySitName { get; set; }
        public int? SfcID { get; set; }
        public string SfcName { get; set; }
        public long? ROWNUM { get; set; }
        public int? TotalCount { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
    public class SiteList
    {
        public decimal? SitId { get; set; }
        public string SitName { get; set; }
    }

    public class ContactListAngular
    {     
        public int ContactID { get; set; }
        public string CompanyName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string TelNo { get; set; }
        public string Address { get; set; }
        public string MobNo { get; set; }
        public string Fax { get; set; }

        public int TotalCount { get; set; }
    }

    public class gridResultDdata
    {
       public List<Dictionary<string, string>> Data { get; set; }
        public string RecordsCount { get; set; }
        public string Begin { get; set; }
        public string End { get; set; }
        public string PageNumber { get; set; }        

    }


    public class C2M_API_McompGetGridViews
    {
        public string config { get; set; }
        public string viewname { get; set; }
        public string defaultview { get; set; }
        public string CONFIGJSON { get; set; }
        public string userid { get; set; }
        public string MasterConfig { get; set; }
        public string Views { get; set; }

        public bool IsDefaultview { get; set; }


    }



    public class C2M_API_McompGetGridViews_New
    {
        public string Config { get; set; }
        public string Viewname { get; set; }
        public string defaultview { get; set; }
        public string CONFIGJSON { get; set; }
        public string userid { get; set; }
        public string MasterConfig { get; set; }
        public string Views { get; set; }
        public string Viewnames { get; set; }
        public bool IsDefaultview { get; set; }


    }




    //  public enum SortOrder { desc,asc }
    public class MGridConfig
    {
        public string ContainerID { get; set; }
        public string FinalJson { get; set; }
        public string ViewName { get; set; }
        public bool IsDefaultView { get; set; }
        public string OldViewName { get; set; }
        public string ProcessName { get; set; }
    }
    //public class GridBaseConfiguration
    //{

    //    public string SortColumn { get; set; }
    //    public SortOrder SortOrder { get; set; }
    //    public int? TimeZone { get; set; }
    //    public List<GridFilter> GridFilters { get; set; }
    //}

    //public enum FilterOperator
    //{
    //    CONTAINS,
    //    CONTAINS_CASE_SENSITIVE,
    //    DOES_NOT_CONTAIN,
    //    DOES_NOT_CONTAIN_CASE_SENSITIVE,
    //    EMPTY,
    //    ENDS_WITH,
    //    ENDS_WITH_CASE_SENSITIVE,
    //    EQUAL,
    //    EQUAL_CASE_SENSITIVE,
    //    GREATER_THAN,
    //    GREATER_THAN_OR_EQUAL,
    //    LESS_THAN,
    //    LESS_THAN_OR_EQUAL,
    //    NOT_EMPTY,
    //    NOT_EQUAL,
    //    NOT_NULL,
    //    NULL,
    //    STARTS_WITH,
    //    STARTS_WITH_CASE_SENSITIVE,
    //    IN,
    //    RANGE
    //}
    //public class GridManagerData
    //{
    //    public string Condition { get; set; }
    //    public string SortColumn { get; set; }
    //    public string SortDirection { get; set; }
    //    public int PageSize { get; set; }
    //    public int PageNumber { get; set; }
    //    public int? TimeZone { get; set; }
    //    public int GroupID { get; set; }
    //    public string Apikey { get; set; }
    //    public int UserType { get; set; }

    //}
    //public class GridFilterCondition
    //{
    //    public FilterOperator Condition { get; set; }
    //    public string ConditionValue { get; set; }
    //}
    //public enum LogicalOperator
    //{
    //    Or, And
    //}
    //public enum FilterType
    //{
    //    Alph_Filter,
    //    Column_Filter,
    //    Custom_Filter,
    //    Date_Filter,
    //    DMO_Filter,
    //    Global_Search,
    //    MyRecord,
    //    State_Filter,
    //    Stage_Filter,
    //    Transaction_Filter
    //}

    //public class GridFilter
    //{
    //    public List<GridFilterCondition> GridConditions { get; set; }

    //    public string DataField { get; set; }
    //    public LogicalOperator? LogicalOperator { get; set; }
    //    public FilterType FilterType { get; set; }
    //}
    //public class GridConfiguration : GridBaseConfiguration
    //{
    //    public int PageSize { get; set; }
    //    public int PageNumber { get; set; }
    //    public string Apikey { get; set; }

    //    public string GridView { get; set; }
    //    public string EncryptedGroupId { get; set; }

    //    public int UserType { get; set; }
    //}


    public static class GridHelper
    {
        public static string MapOperator(string dataField, FilterOperator operators, string conditionValue, DataTable dtMap = null)
        {
            var dateTyps = new List<string> { "DateTimeZone", "DateEditBox", "DateWithCalendar", "DateTimeBox" };
            var dateFieldList = new List<string> { "createddate", "modifieddate", "orderdate", "lastrefresh" };
            bool isDateField = false;

            if (dtMap != null)
            {
                DataView dv = dtMap.DefaultView;
                dv.RowFilter = "DmoGuid='" + dataField + "'";
                isDateField = dv.ToTable().Rows.Count == 1 && dateTyps.Contains(dv[0]["TYP"].ToString());
            }

            var dateTime = new DateTime();
            dataField = dataField.ToLower();
            switch (operators)
            {

                case FilterOperator.CONTAINS:
                    return $" Like (''%{conditionValue}%'')";
                case FilterOperator.CONTAINS_CASE_SENSITIVE:
                    return $" COLLATE utf8_bin Like (''%{conditionValue}%'')";
                case FilterOperator.DOES_NOT_CONTAIN:
                    return $" NOT Like (''%{conditionValue}%'')";
                case FilterOperator.DOES_NOT_CONTAIN_CASE_SENSITIVE:
                    return $" COLLATE utf8_bin NOT Like (''%{conditionValue}%'')";
                case FilterOperator.EMPTY:
                    return " = ''''";
                case FilterOperator.ENDS_WITH:
                    return $" Like (''%{conditionValue}'')";
                case FilterOperator.ENDS_WITH_CASE_SENSITIVE:
                    return $" COLLATE utf8_bin Like (''{conditionValue}%'')";
                case FilterOperator.EQUAL:
                    {
                        if (dataField == "crtdon" || dataField == "modfon" || isDateField)
                        {
                            return $" = (STR_TO_DATE('{conditionValue}','%m/%d/%Y'))";
                        }
                        else if (dataField.Contains("datecreated") || dataField.Contains("datemodified"))
                        {
                            DateTime startdt = DateTime.ParseExact(conditionValue, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                            DateTime enddt = startdt.AddDays(1);

                            string startDateTime = startdt.ToString("yyyy-MM-dd HH:mm:ss");
                            string endDateTime = enddt.ToString("yyyy-MM-dd HH:mm:ss");

                            return $" BETWEEN ''{startDateTime}'' AND ''{endDateTime}'' ";
                        }
                        return $" = (''{conditionValue}'')";
                    }
                case FilterOperator.EQUAL_CASE_SENSITIVE:
                    return $" COLLATE utf8_bin = (''{conditionValue}'')";
                case FilterOperator.GREATER_THAN:
                    {
                        if (dataField == "crtdon" || dataField == "modfon" || isDateField)
                        {
                            if (DateTime.TryParse(conditionValue, out dateTime))
                            {
                                conditionValue = dateTime.ToString("MM/dd/yyyy HH:mm:ss");
                            }
                            return $" > (STR_TO_DATE(''{conditionValue}'',''%m/%d/%Y %H:%i:%s''))";
                        }
                        return $" > (''{conditionValue}'')";
                    }
                case FilterOperator.GREATER_THAN_OR_EQUAL:
                    {
                        if (dataField == "crtdon" || dataField == "modfon" || isDateField)
                        {
                            if (DateTime.TryParse(conditionValue, out dateTime))
                            {
                                conditionValue = dateTime.ToString("MM/dd/yyyy HH:mm:ss");
                            }

                            return $" >= (STR_TO_DATE(''{conditionValue}'',''%m/%d/%Y %H:%i:%s''))";
                        }
                        return $" >= (''{conditionValue}'')";
                    }
                case FilterOperator.LESS_THAN:
                    {
                        if (dataField == "crtdon" || dataField == "modfon" || isDateField)
                        {
                            if (DateTime.TryParse(conditionValue, out dateTime))
                            {
                                if (dateTime == dateTime.Date)
                                {
                                    dateTime = dateTime.AddDays(1).AddSeconds(-1);
                                }

                                conditionValue = dateTime.ToString("MM/dd/yyyy HH:mm:ss");
                            }

                            return $" < (STR_TO_DATE(''{conditionValue}'',''%m/%d/%Y %H:%i:%s''))";
                        }
                        return $" < (''{conditionValue}'')";
                    }
                case FilterOperator.LESS_THAN_OR_EQUAL:
                    {
                        if (dataField == "crtdon" || dataField == "modfon" || isDateField)
                        {
                            if (DateTime.TryParse(conditionValue, out dateTime))
                            {
                                if (dateTime == dateTime.Date)
                                {
                                    dateTime = dateTime.AddDays(1).AddSeconds(-1);
                                }

                                conditionValue = dateTime.ToString("MM/dd/yyyy HH:mm:ss");
                            }

                            return $" <= (STR_TO_DATE(''{conditionValue}'',''%m/%d/%Y %H:%i:%s''))";
                        }

                        return $" <= (''{conditionValue}'')";
                    }
                case FilterOperator.NOT_EMPTY:
                    return $" <> ''''";
                case FilterOperator.NOT_EQUAL:
                    {
                        return $" <> ''{conditionValue}''";
                    }
                case FilterOperator.NOT_NULL:
                    return $" IS NOT NULL";
                case FilterOperator.NULL:
                    return $" IS NULL";
                case FilterOperator.STARTS_WITH:
                    return $" Like (''{conditionValue}%'')";
                case FilterOperator.STARTS_WITH_CASE_SENSITIVE:
                    return $" COLLATE utf8_bin Like (''%{conditionValue}'')";
                case FilterOperator.IN:
                    var values = conditionValue.Split(',');
                    string condition = "";
                    for (int i = 0; i < values.Length; i++)
                        condition += $"''{values[i]}'',";
                    return $" IN ({condition.Trim(',')})";

                case FilterOperator.RANGE:
                    //Saleem - to apply filtering on RangeBox type of DMO Mar 30 2020
                    string lowRange = "";
                    string highRange = "";

                    string[] arrCondVal = conditionValue.Split('-');
                    if (arrCondVal.Length == 2)
                    {
                        lowRange = Convert.ToString(arrCondVal[0]).Trim();
                        highRange = Convert.ToString(arrCondVal[1]).Trim();
                    }
                    else if (arrCondVal.Length == 1)
                    {
                        lowRange = "0";
                        highRange = Convert.ToString(arrCondVal[0]).Trim();
                    }
                    else
                    {
                        throw new Exception("Values for range filter are not correct, please send low and high range separated by hyphen (-).");
                    }

                    string response = $"({lowRange} <= SUBSTRING({dataField}, 1, INSTR({dataField}, '-') - 1) AND {highRange} >= SUBSTRING({dataField}, 1, INSTR({dataField}, '-') - 1))";
                    response += $"OR ({lowRange} >= SUBSTRING({dataField}, 1, INSTR({dataField}, '-') - 1) AND {highRange} <= SUBSTRING({dataField}, INSTR({dataField}, '-') + 1))";
                    response += $"OR ({lowRange} <= SUBSTRING({dataField}, INSTR({dataField}, '-') + 1) AND {highRange} >= SUBSTRING({dataField}, INSTR({dataField}, '-') + 1))";
                    response += $"OR ({lowRange} <= SUBSTRING({dataField}, 1, INSTR({dataField}, '-') - 1) AND {highRange} >= SUBSTRING({dataField}, INSTR({dataField}, '-') + 1))";
                    return $" ({response})";

                default:
                    return $" Like (''{conditionValue}'')";
            }
        }
    }

    public enum FilterOperator
    {
        CONTAINS,
        CONTAINS_CASE_SENSITIVE,
        DOES_NOT_CONTAIN,
        DOES_NOT_CONTAIN_CASE_SENSITIVE,
        EMPTY,
        ENDS_WITH,
        ENDS_WITH_CASE_SENSITIVE,
        EQUAL,
        EQUAL_CASE_SENSITIVE,
        GREATER_THAN,
        GREATER_THAN_OR_EQUAL,
        LESS_THAN,
        LESS_THAN_OR_EQUAL,
        NOT_EMPTY,
        NOT_EQUAL,
        NOT_NULL,
        NULL,
        STARTS_WITH,
        STARTS_WITH_CASE_SENSITIVE,
        IN,
        RANGE
    }

    public enum FilterType
    {
        Alph_Filter,
        Column_Filter,
        Custom_Filter,
        Date_Filter,
        DMO_Filter,
        Global_Search,
        MyRecord,
        State_Filter,
        Stage_Filter,
        Transaction_Filter
    }

    public enum LogicalOperator
    {
        Or, And
    }

    public enum SortOrder
    {
        desc, asc
    }

    public class GridManagerData
    {
        public string Condition { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int? TimeZone { get; set; }
        public int GroupID { get; set; }
        public string Apikey { get; set; }
        public int UserType { get; set; }

    }

    public class GridBaseConfiguration
    {
        
        public string SortColumn { get; set; }
        public SortOrder SortOrder { get; set; }
        public int? TimeZone { get; set; }
        public List<GridFilter> GridFilters { get; set; }
    }

    public class GridConfiguration : GridBaseConfiguration
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string Apikey { get; set; }

        public string GridView { get; set; }
        public string EncryptedGroupId { get; set; }

        public int UserType { get; set; }
    }

    public class GridFilter
    {
        public List<GridFilterCondition> GridConditions { get; set; }
        
        public string DataField { get; set; }
        public LogicalOperator? LogicalOperator { get; set; }
        public FilterType FilterType { get; set; }
    }

    public class GridFilterCondition
    {
        public FilterOperator Condition { get; set; }

      
        public string ConditionValue { get; set; }
    }

}
