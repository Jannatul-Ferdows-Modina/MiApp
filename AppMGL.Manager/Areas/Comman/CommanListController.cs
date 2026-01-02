using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Repository.Operation;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.DTO.Operation;
using AppMGL.Manager.Infrastructure;
using Newtonsoft.Json;

namespace AppMGL.Manager.Areas.Comman.Controllers
{

    public class CommanListController : BaseController<BookingListDTO, BookingListRepository, USP_GET_ENQUIRY_LIST_Result>
    {
        public CommanListController(BookingListRepository context, EnquiryListRepository enquiryListRepository)
        {
            _context = context;
            BaseModule = EnumModule.Booking;
            //KeyField = "EnquiryNo";
        }
        public CommanListController()
        { }
        [System.Web.Http.HttpPost]
        public ActionResult FilingOption()
        {
            try
            {
                var result = _context.ExecuteQuery<FillingOption>("EXEC dbo.USP_FilingOption").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult ModeOfTransport()
        {
            try
            {
                var result = _context.ExecuteQuery<FillingOption>("EXEC dbo.SP_ModeOfTransport").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult ExportInformationCode()
        {
            try
            {
                var result = _context.ExecuteQuery<FillingOptionEntity>("EXEC dbo.SP_ExportInformationCode").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult SaveExportInformationCode()
        {
            try
            {
                var result = _context.ExecuteQuery<FillingOption>("EXEC dbo.SP_ExportInformationCode").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult uomList()
        {
            try
            {
                var result = _context.ExecuteQuery<ItemValue>("EXEC dbo.SP_UnitOfMeasurement").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult htsCode()
        {
            try
            {
                var result = _context.ExecuteQuery<FillingOption>("EXEC dbo.SP_HTSCode").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
       
        [System.Web.Http.HttpPost]
        public ActionResult portofExport()
        {
            try
            {
                var result = _context.ExecuteQuery<ItemValue>("EXEC dbo.USP_Port").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult lienseExemptionCode()
        {
            try
            {
                var result = _context.ExecuteQuery<LienseExemptionCode>("EXEC dbo.SP_LienseExemptionCode").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult ddtcitar()
        {
            try
            {
                var result = _context.ExecuteQuery<ItemValue>("EXEC dbo.LG_ddtcitar_SP").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult ddtcusml()
        {
            try
            {
                var result = _context.ExecuteQuery<ItemValue>("EXEC dbo.LG_ddtcusml_SP").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult htsCodeSearch(ListParams listParams)
        {
            try
            {

                // int count;
                var result = _context.ExecuteQuery<HtcCodeItem>("EXEC dbo.SP_HTSCode_Search @SEARCHVALUE",
                    new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult PortofExportSearch(ListParams listParams)
        {
            try
            {

                //int count;
                var result = _context.ExecuteQuery<ItemValue1>("EXEC dbo.SP_PortofExport_Search @SEARCHVALUE",
                    new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult CountrySearch(ListParams listParams)
        {
            try
            {

                //int count;
                var result = _context.ExecuteQuery<ItemValue>("EXEC dbo.SP_Country_Search @SEARCHVALUE",
                    new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult OriginState(ListParams listParams)
        {
            try
            {

                //int count;
                var result = _context.ExecuteQuery<ItemValue>("EXEC dbo.SP_State_Search @SEARCHVALUE",
                    new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult CompanySearch(ListParams listParams)
        {
            try
            {

                string json = @"" + listParams.Filter;
                List<Filter2> filters = JsonConvert.DeserializeObject<List<Filter2>>(json);
                var result = _context.ExecuteQuery<CompanyDetail>("EXEC dbo.SP_ContactDetail_Search @SEARCHVALUE",
                   new SqlParameter("SEARCHVALUE", filters[0].Value)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        public ActionResult Country(ListParams listParams)
        {
            try
            {
                string json = @"" + listParams.Filter;
                List<Filter2> filters = JsonConvert.DeserializeObject<List<Filter2>>(json);
                var result = _context.ExecuteQuery<CommonSearchData>("EXEC dbo.Country_Search_SP @SEARCHVALUE",
                   new SqlParameter("SEARCHVALUE", filters[0].Value)).ToList();
                return AppResult(result, result.Count);
               
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult State(ListParams listParams)
        {
            try
            {
                string json = @""+ listParams.Filter;
                List<Filter2> filters = JsonConvert.DeserializeObject<List<Filter2>>(json);
                var result = _context.ExecuteQuery<CommonSearchData>("EXEC dbo.State_Search_SP @SEARCHVALUE",
                   new SqlParameter("SEARCHVALUE", filters[0].Value)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpGet]
        public virtual ActionResult GetCities(int id)
        {
            try
            {
                List<SIPLCityDTO> list = _context.ExecuteQuery<SIPLCityDTO>("EXEC dbo.USP_LG_CONTACT_GET_CITIES @StateId", new object[1]
                {
                    new SqlParameter("StateId", id)
                }).ToList();
                return AppResult(list, list.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
    }
    
}

public class Filter2
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string FieldName { get; set; }
    public string Value { get; set; }
    public string Operator { get; set; }
    public string ValueT { get; set; }
}