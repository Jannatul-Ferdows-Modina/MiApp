using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using AppMGL.Manager.Infrastructure;
using AppMGL.DAL.Repository.Operation;
using AppMGL.DTO.Operation;
using AppMGL.DTO.DataManagement;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System;

namespace AppMGL.Manager.Areas.Operation.Controllers
{
    public class DisplayRateController : BaseController<LGVWDisplayRateDTO, DisplayRateRepository, LG_VW_DisplayRate>
    {


        public DisplayRateController(DisplayRateRepository context)
        {
            _context = context;
            BaseModule = EnumModule.DisplayRate;
            KeyField = "CCID";


        }

        [System.Web.Http.HttpPost]
        public ActionResult GetDisplayRateList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                //int GALBookingStatusID = Convert.ToInt32(searchCriteria["galBookingStatusID"]);
                List<LGVWDisplayRateDTO> result = _context.ExecuteQuery<LGVWDisplayRateDTO>("EXEC dbo.USP_LG_GET_DISPLAY_RATE @PAGENO, @PAGESIZE,@OPTIONSELECTVALUE,@SEARCHBOXVALUE,@STARTDATE,@ENDDATE,@OPTIONORIGIN,@ORIGINVALUE,@OPTIONDISCHARGE,@DISCHARGEVALUE,@OPTIONCOMMODITY,@COMMODITYVALUE,@ROUTE,@VIA1,@VIA2,@SORTCOLUMN,@SORTORDER",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONSELECTVALUE", searchCriteria["selectOption"]),
                        new SqlParameter("SEARCHBOXVALUE", searchCriteria["searchBox"]),
                        new SqlParameter("STARTDATE", searchCriteria["startDate"]),
                        // new SqlParameter("STARTDATEVALUE", ""),
                        new SqlParameter("ENDDATE", searchCriteria["endDate"]),
                        // new SqlParameter("ENDDATEVALUE",""),
                        new SqlParameter("OPTIONORIGIN", searchCriteria["selectOptionOrigin"]),
                        new SqlParameter("ORIGINVALUE", searchCriteria["searchBoxorigin"]),
                        new SqlParameter("OPTIONDISCHARGE", searchCriteria["selectOptionDisch"]),
                        new SqlParameter("DISCHARGEVALUE", searchCriteria["searchBoxdisch"]),
                        new SqlParameter("OPTIONCOMMODITY", searchCriteria["selectOptionComm"]),
                        new SqlParameter("COMMODITYVALUE", searchCriteria["searchBoxcomm"]),
                        new SqlParameter("ROUTE", searchCriteria["route"]),
                        // new SqlParameter("ROUTEVALUE", ""),
                        new SqlParameter("VIA1", searchCriteria["via1"]),
                        // new SqlParameter("VIA1VALUE", ""),
                        new SqlParameter("VIA2", searchCriteria["via2"]),
                        //  new SqlParameter("VIA2VALUE", ""),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value)
                        ).ToList();
                if (result.Count > 0)
                {
                    TotalRows = result[0].TotalCount.Value;
                }
                return AppResult(result, TotalRows);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult GetCaptureRouteList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<RouteDTO> result = _context.ExecuteQuery<RouteDTO>("EXEC dbo.USP_LG_CAPTURE_ROUTE_LIST @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("SIT_ID", listParams.SiteId)
                        ).ToList();
                if (result.Count > 0)
                {
                    TotalRows = result[0].TotalCount.Value;
                }
                return AppResult(result, TotalRows);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveCaptureRoute(RouteDTO objRouteDTO)
        {
            try
            {
                IEnumerable<int> objResult;
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CAPTURE_ROUTE_INSERT_UPDATE  @RouteId,@RouteName,@fkOriginID,@fkDestinationID,@OriginType,@DestinationType,@Via1,@Via2,@ViaType1,@ViaType2",
                        new SqlParameter("RouteId", objRouteDTO.RouteId),
                        new SqlParameter("RouteName", objRouteDTO.RouteName ?? Convert.DBNull),
                        new SqlParameter("fkOriginID", objRouteDTO.fkOriginID ?? Convert.DBNull),
                        new SqlParameter("fkDestinationID", objRouteDTO.fkDestinationID ?? Convert.DBNull),
                        new SqlParameter("OriginType", objRouteDTO.OriginType ?? Convert.DBNull),
                        new SqlParameter("DestinationType", objRouteDTO.DestinationType ?? Convert.DBNull),
                        new SqlParameter("Via1", objRouteDTO.Via1 ?? Convert.DBNull),
                        new SqlParameter("Via2", objRouteDTO.Via2 ?? Convert.DBNull),
                        new SqlParameter("ViaType1", objRouteDTO.ViaType1 ?? Convert.DBNull),
                        new SqlParameter("ViaType2", objRouteDTO.ViaType2 ?? Convert.DBNull)
                        ).ToList();
                List<int> objList = objResult.ToList();

                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }

        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult GetCaptureRouteDetails(RouteDTO objRouteDTO)
        {
            try
            {

                IEnumerable<RouteDTO> RouteDTOResult = _context.ExecuteQuery<RouteDTO>("EXEC dbo.USP_LG_CAPTURE_ROUTE_GET_DETAILS @RouteId",
                     new SqlParameter("RouteId", objRouteDTO.RouteId)).ToList();
                List<RouteDTO> RouteDTOList = RouteDTOResult.ToList();

                RouteDTO objRouteListDTO = new RouteDTO();

                if (RouteDTOList.Count > 1)
                {
                    objRouteListDTO.RouteId = RouteDTOList[0].RouteId;
                    objRouteListDTO.RouteName = RouteDTOList[0].RouteName;
                    objRouteListDTO.Origin = RouteDTOList[0].Origin;
                    objRouteListDTO.Destination = RouteDTOList[0].Destination;
                    objRouteListDTO.fkOriginID = RouteDTOList[0].fkOriginID;
                    objRouteListDTO.fkDestinationID = RouteDTOList[0].fkDestinationID;
                    objRouteListDTO.OriginType = RouteDTOList[0].OriginType;
                    objRouteListDTO.DestinationType = RouteDTOList[0].DestinationType;
                    objRouteListDTO.Via1 = RouteDTOList[0].Via1;
                    objRouteListDTO.Via1Name = RouteDTOList[0].Via1Name;
                    objRouteListDTO.ViaType1 = RouteDTOList[0].ViaType1;
                    objRouteListDTO.Via2 = RouteDTOList[1].Via2;
                    objRouteListDTO.Via2Name = RouteDTOList[1].Via2Name;
                    objRouteListDTO.ViaType2 = RouteDTOList[1].ViaType2;
                }
                else
                {
                    objRouteListDTO.RouteId = RouteDTOList[0].RouteId;
                    objRouteListDTO.RouteName = RouteDTOList[0].RouteName;
                    objRouteListDTO.Origin = RouteDTOList[0].Origin;
                    objRouteListDTO.Destination = RouteDTOList[0].Destination;
                    objRouteListDTO.fkOriginID = RouteDTOList[0].fkOriginID;
                    objRouteListDTO.fkDestinationID = RouteDTOList[0].fkDestinationID;
                    objRouteListDTO.OriginType = RouteDTOList[0].OriginType;
                    objRouteListDTO.DestinationType = RouteDTOList[0].DestinationType;
                    //if (!string.IsNullOrEmpty(RouteDTOList[0].Via1))
                    //{
                    objRouteListDTO.Via1 = RouteDTOList[0].Via1;
                    //}
                    objRouteListDTO.Via1Name = RouteDTOList[0].Via1Name;
                    objRouteListDTO.ViaType1 = RouteDTOList[0].ViaType1;
                }

                return AppResult(objRouteListDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult DeleteRoute(RouteDTO objRouteDTO)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CAPTURE_ROUTE_DELETE @RouteId",
                                    new SqlParameter("RouteId", objRouteDTO.RouteId)
                                    ).ToList();
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


        [System.Web.Http.HttpGet]
        public virtual ActionResult GetRouteNames(string id)
        {
            try
            {
                var result = _context.ExecuteQuery<RouteDTO>("EXEC dbo.USP_LG_CAPTURE_ROUTE_GET_ROUTE_NAMES @SEARCH_VALUE",
                                         new SqlParameter("SEARCH_VALUE", id)).ToList();

                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        public virtual ActionResult GetAllCarriers()
        {
            try
            {
                //int TotalCount;                
                var result = _context.ExecuteQuery<QuotationCarrierDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_CARRIERS").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetCarrierAllCharges()
        {
            try
            {
                var result = _context.ExecuteQuery<TradeServiceDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_CHARGES").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpGet]
        public virtual ActionResult GetContainerCategories()
        {
            try
            {
                var result = _context.ExecuteQuery<ContainerCategory>("EXEC dbo.USP_LG_ENQUIRY_GET_CONTAINER_CATEGORIES").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetContainerSizes()
        {
            try
            {
                var result = _context.ExecuteQuery<ContainerTypeDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_CONTAINER_SIZES").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult GetRateCaptureList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<RateCaptureDTO> result = _context.ExecuteQuery<RateCaptureDTO>("EXEC dbo.USP_LG_CONTRACT_RATE_LIST @PAGENO, @PAGESIZE,@CARRIERNAME,@ORIGIN,@DESTINATION,@ORIGINCOUNTRY,@DESTINATIONCOUNTRY,@ORIGINSTATE,@DESTINATIONSTATE,@ORIGINCITY,@DESTINATIONCITY,@COMMODITY,@SORTCOLUMN,@SORTORDER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("CARRIERNAME", searchCriteria["carrierName"]),
                        new SqlParameter("ORIGIN", searchCriteria["origin"]),
                        new SqlParameter("DESTINATION", searchCriteria["destination"]),
                        new SqlParameter("ORIGINCOUNTRY", searchCriteria["originCountry"]),
                        new SqlParameter("DESTINATIONCOUNTRY", searchCriteria["destinationCountry"]),
                        new SqlParameter("ORIGINSTATE", searchCriteria["originState"]),
                        new SqlParameter("DESTINATIONSTATE", searchCriteria["destinationState"]),
                        new SqlParameter("ORIGINCITY", searchCriteria["originCity"]),
                        new SqlParameter("DESTINATIONCITY", searchCriteria["destinationCity"]),

                        new SqlParameter("COMMODITY", searchCriteria["commodity"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("SIT_ID", listParams.SiteId)
                        ).ToList();
                if (result.Count > 0)
                {
                    TotalRows = result[0].TotalCount.Value;
                }
                return AppResult(result, TotalRows);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveRateCapture(RateCaptureDTO objRateCaptureDTO)
        {
            try
            {
                IEnumerable<decimal> objResultValidate;
                string CommodityIds = "";
                string ContainerTypeIds = "";
                string ContainerCategoryIds = "";
                foreach (CommodityDTO objCommodityDTO in objRateCaptureDTO.CommodityDTOList)
                {
                    CommodityIds = CommodityIds + objCommodityDTO.CommodityId + ",";
                        
                }
                if(!string.IsNullOrEmpty(CommodityIds))
                {
                    CommodityIds = CommodityIds.Substring(0, CommodityIds.Length - 1);
                }    

                foreach(ContainerCharges objContainerCharges in objRateCaptureDTO.ContainerChargesList)
                {
                    ContainerTypeIds = ContainerTypeIds + objContainerCharges.containerTypeId + ",";
                    ContainerCategoryIds = ContainerCategoryIds + objContainerCharges.CategoryID + ",";
                }
                if (!string.IsNullOrEmpty(ContainerTypeIds))
                {
                    ContainerTypeIds = ContainerTypeIds.Substring(0, ContainerTypeIds.Length - 1);
                }
                if (!string.IsNullOrEmpty(ContainerCategoryIds))
                {
                    ContainerCategoryIds = ContainerCategoryIds.Substring(0, ContainerCategoryIds.Length - 1);
                }


                objResultValidate = _context.ExecuteQuery<decimal>("EXEC dbo.USP_LG_CONTRACT_RATE_INSERT_UPDATE_Validate @ContractRateID,@CarrierID,@PortOfOriginID,@PortOfDestinationID,@RouteBy,@RouteBy2,@CommoditiesIDs,@ContainerType,@ContainerCategory",
                       new SqlParameter("ContractRateID", (Convert.ToString(objRateCaptureDTO.ContractRateID)==null?0:Convert.ToInt32(objRateCaptureDTO.ContractRateID))),
                       new SqlParameter("CarrierID", (Convert.ToString( objRateCaptureDTO.CarrierID)==null?0: Convert.ToInt32( objRateCaptureDTO.CarrierID))),
                       new SqlParameter("PortOfOriginID", (objRateCaptureDTO.originID ==null?0:objRateCaptureDTO.originID)),
                       new SqlParameter("PortOfDestinationID", objRateCaptureDTO.dischargeID ?? Convert.DBNull),
                       new SqlParameter("RouteBy",  (objRateCaptureDTO.RouteBy!=null ? objRateCaptureDTO.RouteBy:"")),
                       new SqlParameter("RouteBy2", objRateCaptureDTO.RouteBy2 ?? Convert.DBNull),
                       new SqlParameter("CommoditiesIDs", CommodityIds ?? Convert.DBNull),
                       new SqlParameter("ContainerType", ContainerTypeIds  ?? Convert.DBNull),
                        new SqlParameter("ContainerCategory", ContainerCategoryIds ?? Convert.DBNull)
                       

                       ).ToList();
                List<decimal> objListValidate = objResultValidate.ToList();
                Int64 Status = Convert.ToInt64( objListValidate[0]);
                if(Status==1)
                {
                    return AppResult(null, 0, "Already Exists", EnumResult.ValidationFailed);
                }

                /////////////////////////
                IEnumerable<decimal> objResult;
                objResult = _context.ExecuteQuery<decimal>("EXEC dbo.USP_LG_CONTRACT_RATE_INSERT_UPDATE @ContractRateID,@ContractID,@CarrierID,@EffectiveDate, @ExpiryDate, @ApplicableToService, @PortOfOriginID, @PortOfDestinationID, @OriginCountryID, @DestinationCountryID, @OrignStateID, @DestinationStateID, @OriginCityID, @DestinationCityID,@OriginModeID, @DestinationnModeID, @AMDNo, @Notes, @Remarks, @CreatedBy, @UpdatedBy,@RouteBy,@RouteBy2,@TranssitTime",
                        new SqlParameter("ContractRateID", objRateCaptureDTO.ContractRateID),
                        new SqlParameter("ContractID", objRateCaptureDTO.ContractID),
                        new SqlParameter("CarrierID", objRateCaptureDTO.CarrierID),
                        new SqlParameter("EffectiveDate", objRateCaptureDTO.EffectiveDate ?? Convert.DBNull),
                        new SqlParameter("ExpiryDate", objRateCaptureDTO.ExpiryDate ?? Convert.DBNull),
                        new SqlParameter("ApplicableToService", objRateCaptureDTO.ApplicableToService ?? Convert.DBNull),
                        new SqlParameter("PortOfOriginID", objRateCaptureDTO.originID ?? Convert.DBNull),
                        new SqlParameter("PortOfDestinationID", objRateCaptureDTO.dischargeID ?? Convert.DBNull),
                        new SqlParameter("OriginCountryID", objRateCaptureDTO.OriginCountryID ?? Convert.DBNull),
                        new SqlParameter("DestinationCountryID", objRateCaptureDTO.DestinationCountryID ?? Convert.DBNull),
                        new SqlParameter("OrignStateID", objRateCaptureDTO.OrignStateID ?? Convert.DBNull),
                        new SqlParameter("DestinationStateID", objRateCaptureDTO.DestinationStateID ?? Convert.DBNull),
                        new SqlParameter("OriginCityID", objRateCaptureDTO.OriginCityID ?? Convert.DBNull),
                        new SqlParameter("DestinationCityID", objRateCaptureDTO.DestinationCityID ?? Convert.DBNull),
                        new SqlParameter("OriginModeID", objRateCaptureDTO.OriginModeID ?? Convert.DBNull),
                        new SqlParameter("DestinationnModeID", objRateCaptureDTO.DestinationModeID ?? Convert.DBNull),
                        new SqlParameter("AMDNo", objRateCaptureDTO.AMDNO ?? Convert.DBNull),
                        new SqlParameter("Notes", objRateCaptureDTO.Notes ?? Convert.DBNull),
                        new SqlParameter("Remarks", objRateCaptureDTO.Remarks ?? Convert.DBNull),
                        new SqlParameter("CreatedBy", objRateCaptureDTO.CreatedBy ?? Convert.DBNull),
                        new SqlParameter("UpdatedBy", objRateCaptureDTO.UpdatedBy ?? Convert.DBNull),
                        new SqlParameter("RouteBy", objRateCaptureDTO.RouteBy ?? Convert.DBNull),
                        new SqlParameter("RouteBy2", objRateCaptureDTO.RouteBy2 ?? Convert.DBNull),
                        new SqlParameter("TranssitTime", objRateCaptureDTO.TranssitTime ?? Convert.DBNull)
                        ).ToList();
                List<decimal> objList = objResult.ToList();
                decimal ContractRateID = objList[0];

                if (ContractRateID > 0)
                {
                    int contRateID = Convert.ToInt32(ContractRateID);
                    IEnumerable<int> objDeleteResult;
                    //delete existing Commdity details
                    objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_COMMODITY_DELETE @RefId,@RefType",
                        new SqlParameter("RefId", contRateID),
                        new SqlParameter("RefType", "ContractRate")).ToList();
                    if (objRateCaptureDTO.CommodityDTOList.Count() > 0)
                    {
                        SaveCommodityDetails(objRateCaptureDTO.CommodityDTOList, contRateID);
                    }

                    objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTRACT_RATE_CHARGES_DELETE @ContractRateID", new SqlParameter("ContractRateID", ContractRateID)).ToList();
                    //Container charges
                    if (objRateCaptureDTO.ContainerChargesList.Count() > 0)
                    {
                        SaveRateCharges(objRateCaptureDTO.ContainerChargesList, ContractRateID, objRateCaptureDTO.CreatedBy);
                    }
                }

                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }

        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult GetRateCaptureDetails(RateCaptureDTO objRateCaptureDTO)
        {
            try
            {

                List<RateCaptureDTO> result = _context.ExecuteQuery<RateCaptureDTO>("EXEC dbo.USP_LG_CONTRACT_RATE_DETAILS @ContractRateID",
                     new SqlParameter("ContractRateID", objRateCaptureDTO.ContractRateID)).ToList();

                RateCaptureDTO objRateCaptureDetailDTO = result[0];
                if (result[0].ContractRateID > 0)
                {
                    //get commodity
                    List<CommodityDTO> CommodityDTOList = _context.ExecuteQuery<CommodityDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_COMMODITY @RefId,@RefType",
                                    new SqlParameter("RefId", result[0].ContractRateID),
                                    new SqlParameter("RefType", "ContractRate")).ToList();
                    if (CommodityDTOList.Count > 0)
                    {
                        objRateCaptureDetailDTO.CommodityDTOList = CommodityDTOList.ToArray();
                    }

                    //get Container Charges                                     
                    List<ContainerCharges> ContainerChargesList = _context.ExecuteQuery<ContainerCharges>("EXEC dbo.USP_LG_CONTRACT_GET_RATE_CHARGES @ContractRateID",
                                    new SqlParameter("ContractRateID", result[0].ContractRateID)).ToList();
                    if (ContainerChargesList.Count > 0)
                    {
                        objRateCaptureDetailDTO.ContainerChargesList = ContainerChargesList.ToArray();
                    }

                }

                return AppResult(objRateCaptureDetailDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        public void SaveCommodityDetails(CommodityDTO[] objCommodityDTOList, int contractRateID)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (CommodityDTO objCommodityDTO in objCommodityDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_COMMODITY_INSERT @fkCommodityId,@RefId,@RefType",
                    new SqlParameter("fkCommodityId", objCommodityDTO.CommodityId),
                    new SqlParameter("RefId", contractRateID),
                    new SqlParameter("RefType", "ContractRate")).ToList();
            }
        }

        public void SaveRateCharges(ContainerCharges[] ContainerChargesList, decimal contractRateID, int? CreatedBy)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (ContainerCharges objContainerCharges in ContainerChargesList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTRACT_RATE_CHARGES_INSERT @ContractRateID,@CCID,@ContainerTypeId, @TradeServiceId, @ExpiryDate, @BuyingAmt, @CreatedBy",
                    new SqlParameter("ContractRateID", contractRateID),
                    new SqlParameter("CCID", objContainerCharges.CategoryID),
                    new SqlParameter("ContainerTypeId", objContainerCharges.containerTypeId),
                    new SqlParameter("TradeServiceId", objContainerCharges.ChargeId),
                    new SqlParameter("ExpiryDate", objContainerCharges.ChargeExpiryDate),
                    new SqlParameter("BuyingAmt", objContainerCharges.BuyingAmt),
                    new SqlParameter("CreatedBy", CreatedBy)).ToList();
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetCarrierContracts(long id)
        {

            try
            {
                decimal carrierId;
                carrierId = Convert.ToDecimal(id);

                var result = _context.ExecuteQuery<ContractRateDTO>("EXEC dbo.USP_LG_CONTRACT_GET_CONTRACTNOS @CarrierID",
                     new SqlParameter("CarrierID", carrierId)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult DeleteContractRate(RateCaptureDTO objRateCaptureDTO)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTRACT_RATE_DELETE @ContractRateID",
                                    new SqlParameter("ContractRateID", objRateCaptureDTO.ContractRateID)
                                    ).ToList();
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public ActionResult GetRateCapturebreakupDetails( )
        {
            //get Container Charges
            //
            string typ = Request.Headers.GetValues("typ").ToList()[0];
            string ContractRateID = Request.Headers.GetValues("ContractRateID").ToList()[0];

            RateCaptureDTO objRateCaptureDetailDTO = new RateCaptureDTO() ;
            List<ContainerCharges> ContainerChargesList = _context.ExecuteQuery<ContainerCharges>("EXEC dbo.USP_LG_CONTRACT_GET_RATE_CHARGES_TYPE @ContractRateID,@ContainerType",
                            new SqlParameter("ContractRateID", ContractRateID),
                            new SqlParameter("ContainerType", typ)

                            ).ToList();
            if (ContainerChargesList.Count > 0)
            {
                objRateCaptureDetailDTO.ContainerChargesList = ContainerChargesList.ToArray();
            }

            return AppResult(objRateCaptureDetailDTO, 1, "", EnumResult.Success);
        }
    }
}