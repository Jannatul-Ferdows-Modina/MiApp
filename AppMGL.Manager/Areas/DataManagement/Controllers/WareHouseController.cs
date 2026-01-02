using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class WareHouselocationController : BaseController<AppMGL.DTO.DataManagement.WarehouseLocationDTO, WarehouseLocationRepository, AppMGL.DAL.Models.sipl_warehouselocation>
    {
        public WareHouselocationController(WarehouseLocationRepository context)
        {
            _context = context;
            BaseModule = EnumModule.WarehouseLocation;
            KeyField = "LocationID";
        }


        [System.Web.Http.HttpPost]
        public override ActionResult Insert(WarehouseLocationDTO dto)
        {
            try
            {

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_WarehouseLocation_IUD @LocationID,@LocationName,@BlockNumber,@CreatedBy,@ModifiedBy,@IsActive,@ActionType,@BlockId",

                new SqlParameter("LocationID", dto.LocationId ?? Convert.DBNull),
                new SqlParameter("LocationName", dto.Storagelocation ?? Convert.DBNull),
                new SqlParameter("BlockNumber", dto.BlockNumber ?? Convert.DBNull),
                new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                new SqlParameter("IsActive", dto.IsActive),               
                new SqlParameter("ActionType", "insert"),
                new SqlParameter("BlockId", dto.BlockId?? Convert.DBNull)

                ).ToList();
                List<int> objList = objResult.ToList();
                long vcontactcategoryid = objList[0];
                //var result = _context.Detail(vcontactcategoryid);

                List<SqlParameter> param1 = new List<SqlParameter>
                {
                    new SqlParameter("@BlockId", vcontactcategoryid)
                };

                List<sipl_warehouselocation> CommodityDetail = _context.ExecuteQuery<sipl_warehouselocation>("GET_WAREHOUSELOCATION_DETAILBYBlockID @BlockId", param1.ToArray()).ToList();
                var result = CommodityDetail[0];

                return AppResult(result, 1, PrepareMessage(EnumAction.Insert), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPut]
        public override ActionResult Detail(long id)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@BlockId", id)
                };
                List<sipl_warehouselocation> CommodityDetail = _context.ExecuteQuery<sipl_warehouselocation>("GET_WAREHOUSELOCATION_DETAILBYBlockID @BlockId", param.ToArray()).ToList();
                var result = CommodityDetail[0];
                return AppResult(result, "");


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPut]
        public override ActionResult Update(long id, WarehouseLocationDTO dto)
        {
            try
            {

                List<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_WarehouseLocation_IUD @LocationID,@LocationName,@BlockNumber,@CreatedBy,@ModifiedBy,@IsActive,@ActionType,@BlockId",

                new SqlParameter("LocationID", dto.LocationId ?? Convert.DBNull),
                new SqlParameter("LocationName", dto.Storagelocation ?? Convert.DBNull),
                new SqlParameter("BlockNumber", dto.BlockNumber ?? Convert.DBNull),
                new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                new SqlParameter("IsActive", dto.IsActive),
                new SqlParameter("ActionType", "update"),
                new SqlParameter("BlockId", dto.BlockId ?? Convert.DBNull)
                ).ToList();
                List<int> objList = objResult.ToList();
                long blockid = objList[0];
                List<SqlParameter> param1 = new List<SqlParameter>
                {
                    //new SqlParameter("@LocationId", locationid),
                    new SqlParameter("@BlockId", blockid)
                };

                List<sipl_warehouselocation> CommodityDetail = _context.ExecuteQuery<sipl_warehouselocation>("GET_WAREHOUSELOCATION_DETAILBYBlockID @BlockId", param1.ToArray()).ToList();
                var result = CommodityDetail[0];
                return AppResult(result, 1, PrepareMessage(EnumAction.Update), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Delete(WarehouseLocationDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_WarehouseLocation_IUD @LocationID,@LocationName,@BlockNumber,@CreatedBy,@ModifiedBy,@IsActive,@ActionType,@BlockId",

                 new SqlParameter("LocationID", dto.LocationId ?? Convert.DBNull),
                 new SqlParameter("LocationName", dto.Storagelocation ?? Convert.DBNull),
                 new SqlParameter("BlockNumber", dto.BlockNumber ?? Convert.DBNull),
                 new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                 new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                 new SqlParameter("IsActive", dto.IsActive),
                 new SqlParameter("ActionType", "delete"),
                 new SqlParameter("BlockId", dto.BlockId ?? Convert.DBNull)).ToList();
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1, PrepareMessage(EnumAction.Delete), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult List(AppMGL.DAL.Helper.ListParams listParams)
        {
           try
            {
                string SEARCHVALUE = "";
                Dictionary<string, string> dictionary = null;
                var ss = listParams.Filter;
                if(ss.Length>0)
                {

                    var jsonArray = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(ss);
                    foreach (var item in jsonArray)
                    {
                        if (item.ContainsKey("name") && item.ContainsKey("value"))
                        {
                            SEARCHVALUE = item["value"];
                        }
                    }


                   

                }


                //dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int count = 0;
                List<sipl_warehouselocation> list = _context.ExecuteQuery<sipl_warehouselocation>("EXEC dbo.USP_LG_Warehouselocation_GET_LIST @PAGENO, @PAGESIZE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@SIT_ID", new object[6]
                {
                    new SqlParameter("PAGENO", listParams.PageIndex),
                    new SqlParameter("PAGESIZE", listParams.PageSize),
                    new SqlParameter("SEARCHVALUE", SEARCHVALUE),
                    new SqlParameter("SIT_ID", listParams.SiteId),
                    new SqlParameter("SORTCOLUMN", source.First().Key),
                    new SqlParameter("SORTORDER", source.First().Value)
                }).ToList();
                if (list.Count > 0)
                {
                    count = list[0].TotalCount.Value;
                }
                return AppResult(list, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }



        }

        [System.Web.Http.HttpGet]
        public  ActionResult WarehoseMappingDetail(long id)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@Id", id),
                     new SqlParameter("@GetFromType","Detail")
                };
                List<sipl_warehousemappinglist> MappingDetail = _context.ExecuteQuery<sipl_warehousemappinglist>("GET_WAREHOUSEMAPPING_DETAILBYID @Id,@GetFromType", param.ToArray()).ToList();
                var result = MappingDetail[0];

                IEnumerable<CargoMaineFest> CargoMaineFestResult = _context.ExecuteQuery<CargoMaineFest>("EXEC dbo.GET_WarehouseCargoMainefest @WareHouseId",
                     new SqlParameter("WareHouseId", id)).ToList();
                List<CargoMaineFest> CargoMaineFest = CargoMaineFestResult.ToList();
                if (CargoMaineFest.Count > 0)
                {
                    result.CargoMaineFest = CargoMaineFest.ToArray();
                }
                List<SqlParameter> param1 = new List<SqlParameter>
                {
                    new SqlParameter("@locationid", result.WareHouseId)

                };
                List<blocknumberlist> Detail = _context.ExecuteQuery<blocknumberlist>("GET_BlockNumer @locationid", param1.ToArray()).ToList();
                result.blocknumberlist = Detail;
                return AppResult(result, "");


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public  ActionResult WarehouseinwardNo()
        {
            try
            {
                
                List<string> CommodityDetail = _context.ExecuteQuery<string>("GET_WarehouseinwardNo").ToList();
                var result = CommodityDetail[0];

               
                return AppResult(result, "");


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult SearchQuotation(AppMGL.DAL.Helper.ListParams listParams)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@searchkey", listParams.Filter),
                    new SqlParameter("@SIT_ID", listParams.SiteId)
                };
                List<quotationsearch> CommodityDetail = _context.ExecuteQuery<quotationsearch>("GET_QuotationSearch @searchkey,@SIT_ID", param.ToArray()).ToList();                
                return AppResult(CommodityDetail.ToList(), CommodityDetail.ToList().Count);


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


        [System.Web.Http.HttpPost]
        public ActionResult SearchWarehouselocation(AppMGL.DAL.Helper.ListParams listParams)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@searchkey", listParams.Filter),
                    new SqlParameter("@SIT_ID", listParams.SiteId)
                };
                List<warehoselocationsearch> Detail = _context.ExecuteQuery<warehoselocationsearch>("GET_WarehouseLocationSearch @searchkey,@SIT_ID", param.ToArray()).ToList();
                return AppResult(Detail.ToList(), Detail.ToList().Count);


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpGet]
        public ActionResult Searchblocknumberlist(int id)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@locationid", id)
                    
                };
                List<blocknumberlist> Detail = _context.ExecuteQuery<blocknumberlist>("GET_BlockNumer @locationid", param.ToArray()).ToList();
                return AppResult(Detail.ToList(), Detail.ToList().Count);


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


        [System.Web.Http.HttpPost]
        public  ActionResult WareHouseList(AppMGL.DAL.Helper.ListParams listParams)
        {
            try
            {
                string SEARCHVALUE = "";
                string challanno = "";
                string challandate = "";
                string challanenddate = "";
                string quotationno = "";
                string shippername = "";
                Dictionary<string, string> dictionary = null;
                var ss = listParams.Filter;
                if (ss.Length > 0)
                {
                    Dictionary<string, string> myDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(ss);



                    //var jsonArray = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(ss);
                    foreach (var item in myDict)
                    {
                        if (item.Key=="name" )
                        {
                            SEARCHVALUE = item.Value;
                        }
                        else if (item.Key=="challanno" )
                        {
                            challanno = item.Value;
                        }
                       else if (item.Key=="challandate")
                        {
                            challandate = item.Value;
                        }
                        else if(item.Key == "challanenddate")
                        {
                            challanenddate = item.Value;

                        }
                        else if (item.Key == "quotationno")
                        {
                            quotationno = item.Value;

                        }
                        else if(item.Key == "shippername")
                        {
                            shippername = item.Value;
                        }

                    }
                }


                //dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int count = 0;
                List<sipl_warehousemappinglist> list = _context.ExecuteQuery<sipl_warehousemappinglist>("EXEC dbo.USP_LG_Warehousemapping_GET_LIST @PAGENO, @PAGESIZE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@SIT_ID,@CHALLAN_NO,@CHALLAN_DATE,@CHALLAN_ENDDATE,@QUOTATION_NO,@SHIPPER_NAME", new object[11]
                {
                    new SqlParameter("PAGENO", listParams.PageIndex),
                    new SqlParameter("PAGESIZE", listParams.PageSize),
                    new SqlParameter("SEARCHVALUE", string.IsNullOrWhiteSpace(SEARCHVALUE)?(object)DBNull.Value:SEARCHVALUE),
                    new SqlParameter("SIT_ID", listParams.SiteId),
                    new SqlParameter("SORTCOLUMN", source.First().Key),
                    new SqlParameter("SORTORDER", source.First().Value),
                    new SqlParameter("CHALLAN_NO",string.IsNullOrWhiteSpace(challanno) ? (object)DBNull.Value :challanno) ,
                    new SqlParameter("CHALLAN_DATE" ,string.IsNullOrWhiteSpace(challandate) ? (object)DBNull.Value : DateTime.Parse(challandate)),
                    new SqlParameter("CHALLAN_ENDDATE" ,string.IsNullOrWhiteSpace(challanenddate) ? (object)DBNull.Value : DateTime.Parse(challanenddate)),
                    new SqlParameter("QUOTATION_NO" ,string.IsNullOrWhiteSpace(quotationno) ? (object)DBNull.Value : (quotationno)),
                    new SqlParameter("SHIPPER_NAME" ,string.IsNullOrWhiteSpace(shippername) ? (object)DBNull.Value : (shippername))

                    
                }).ToList();
                if (list.Count > 0)
                {
                    count = list[0].TotalCount.Value;
                }
                return AppResult(list, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }



        }

        [System.Web.Http.HttpPost]
        public  ActionResult InsertUpdateWareHouse(WarehouseMappingDTO dto)
        {
            try
            {
                string actionType = "insert";
                if (dto.ID > 0)
                    actionType = "update";
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_WarehouseMapping_IUD @ID,@QuatationId,@QuatationNo,@WareHouseName,@WareHouseId,@BlockNumber,@Description,@CreatedBy,@ModifiedBy,@IsActive,@ActionType,@BlockId,@SIT_ID,@ChallanNo,@ChallanDate,@Warehouseinwardno,@ShipperName,@consigneeName, @bLNo, @marks, @packagesDescription, @grosswt, @cBM, @fobAmount, @freightStatus",


                new SqlParameter("ID", dto.ID ?? Convert.DBNull),
                new SqlParameter("QuatationId", dto.QuatationId ?? Convert.DBNull),
                new SqlParameter("QuatationNo", dto.QuatationNo ?? Convert.DBNull),
                new SqlParameter("WareHouseName", dto.WareHouseName ?? Convert.DBNull),
                new SqlParameter("WareHouseId", dto.WareHouseId ?? Convert.DBNull),
                new SqlParameter("BlockNumber", dto.BlockNumber ?? Convert.DBNull),
                new SqlParameter("Description", dto.Description ?? Convert.DBNull),
                new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                new SqlParameter("IsActive", dto.IsActive),
                new SqlParameter("ActionType", actionType),
                new SqlParameter("BlockId", dto.BlockId??Convert.DBNull),
                new SqlParameter("SIT_ID", dto.SIT_ID),
                new SqlParameter("ChallanNo", dto.ChallanNo ?? Convert.DBNull),
                new SqlParameter("ChallanDate", dto.ChallanDate ?? Convert.DBNull),
                new SqlParameter("Warehouseinwardno", dto.Warehouseinwardno ?? Convert.DBNull),
                new SqlParameter("ShipperName", dto.ShipperName ?? Convert.DBNull),
                new SqlParameter("@consigneeName", dto.ConsigneeName ?? (object)DBNull.Value),
                new SqlParameter("@bLNo", dto.BLNo ?? (object)DBNull.Value),
                new SqlParameter("@marks", dto.Marks ?? (object)DBNull.Value),
                new SqlParameter("@packagesDescription", dto.PackagesDescription ?? (object)DBNull.Value),
                new SqlParameter("@grosswt", dto.GrossWt ?? (object)DBNull.Value),
                new SqlParameter("@cBM", dto.CBM ?? (object)DBNull.Value),
                new SqlParameter("@fobAmount", dto.FOBAmount),
                new SqlParameter("@freightStatus", dto.FreightStatus ?? (object)DBNull.Value)

                ).ToList();
                List<int> objList = objResult.ToList();
                long vcontactcategoryid = objList[0];
                object result =null;
                if (vcontactcategoryid > 0)
                {
                    List<SqlParameter> param1 = new List<SqlParameter>
                     {
                      new SqlParameter("@Id", vcontactcategoryid),
                      new SqlParameter("@GetFromType","Save")
                     };

                      List<WarehouseMappingDTO> CommodityDetail = _context.ExecuteQuery<WarehouseMappingDTO>("GET_WAREHOUSEMAPPING_DETAILBYID @Id,@GetFromType", param1.ToArray()).ToList();
                        result = CommodityDetail[0];

                    if (dto.CargoMaineFest != null)
                    {
                        if (dto.CargoMaineFest.Length > 0)
                        {
                            for (int i = 0; i < dto.CargoMaineFest.Length; i++)
                            {

                                object[] objLine = new object[14]
                                     {
                                 new SqlParameter("@WareHouseId", vcontactcategoryid),
                                 new SqlParameter("@CargoMaineId", dto.CargoMaineFest[i].CargoMaineId?? Convert.DBNull),
                                 new SqlParameter("@ConsigneeName", dto.CargoMaineFest[i].ConsigneeName?? Convert.DBNull),
                                 new SqlParameter("@BLNo", dto.CargoMaineFest[i].BLNo?? Convert.DBNull),
                                 new SqlParameter("@Marks", dto.CargoMaineFest[i].Marks?? Convert.DBNull),
                                 new SqlParameter("@PackagesDescription", dto.CargoMaineFest[i].PackagesDescription?? Convert.DBNull),
                                 new SqlParameter("@GrossWt", dto.CargoMaineFest[i].GrossWt?? Convert.DBNull),
                                 new SqlParameter("@CBM", dto.CargoMaineFest[i].CBM?? Convert.DBNull),
                                 new SqlParameter("@FOBAmount", dto.CargoMaineFest[i].FOBAmount?? Convert.DBNull),
                                 new SqlParameter("@FreightStatus", dto.CargoMaineFest[i].FreightStatus?? Convert.DBNull),
                                 new SqlParameter("@FirstQuantity", dto.CargoMaineFest[i].FirstQuantity?? Convert.DBNull),
                                 new SqlParameter("@FirstUOM", dto.CargoMaineFest[i].FirstUOM?? Convert.DBNull),
                                 new SqlParameter("@SecondQuantity", dto.CargoMaineFest[i].SecondQuantity?? Convert.DBNull),
                                 new SqlParameter("@SecondUOM", dto.CargoMaineFest[i].SecondUOM?? Convert.DBNull)
                             };
                                _context.ExecuteQuery("EXEC dbo.Save_WarehouseCargoMainefest @WareHouseId,@CargoMaineId,@ConsigneeName,@BLNo,@Marks,@PackagesDescription,@GrossWt,@CBM,@FOBAmount,@FreightStatus,@FirstQuantity,@FirstUOM,@SecondQuantity,@SecondUOM", objLine).ToList();

                            }
                        }
                    }

                }
                string msg = dto.ID > 0 ? "Successfully updated data." : "Successfully added data.";

                return AppResult(result, 1, msg, EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


        [System.Web.Http.HttpPost]
        public ActionResult DeleteWareHouse(int id)
        {
            try
            {
                WarehouseMappingDTO dto = new WarehouseMappingDTO();
                dto.ID = id;
                string actionType = "delete";
                if (dto.ID > 0)
                    actionType = "delete";
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_WarehouseMapping_IUD @ID,@QuatationId,@QuatationNo,@WareHouseName,@WareHouseId,@BlockNumber,@Description,@CreatedBy,@ModifiedBy,@IsActive,@ActionType,@BlockId,@SIT_ID,@ChallanNo,@ChallanDate,@Warehouseinwardno,@ShipperName,@consigneeName, @bLNo, @marks, @packagesDescription, @grosswt, @cBM, @fobAmount, @freightStatus",


                new SqlParameter("ID", dto.ID ?? Convert.DBNull),
                new SqlParameter("QuatationId", dto.QuatationId ?? Convert.DBNull),
                new SqlParameter("QuatationNo", dto.QuatationNo ?? Convert.DBNull),
                new SqlParameter("WareHouseName", dto.WareHouseName ?? Convert.DBNull),
                new SqlParameter("WareHouseId", dto.WareHouseId ?? Convert.DBNull),
                new SqlParameter("BlockNumber", dto.BlockNumber ?? Convert.DBNull),
                new SqlParameter("Description", dto.Description ?? Convert.DBNull),
                new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                new SqlParameter("IsActive", dto.IsActive ),
                new SqlParameter("ActionType", actionType),
                new SqlParameter("BlockId", dto.BlockId ?? Convert.DBNull),
                new SqlParameter("SIT_ID", dto.SIT_ID?? Convert.DBNull),
                new SqlParameter("ChallanNo", dto.ChallanNo ?? Convert.DBNull),
                new SqlParameter("ChallanDate", dto.ChallanDate ?? Convert.DBNull),
                new SqlParameter("Warehouseinwardno", dto.Warehouseinwardno ?? Convert.DBNull),
                new SqlParameter("ShipperName", dto.ShipperName ?? Convert.DBNull),
                new SqlParameter("@consigneeName", dto.ConsigneeName ?? (object)DBNull.Value),
                new SqlParameter("@bLNo", dto.BLNo ?? (object)DBNull.Value),
                new SqlParameter("@marks", dto.Marks ?? (object)DBNull.Value),
                new SqlParameter("@packagesDescription", dto.PackagesDescription ?? (object)DBNull.Value),
                new SqlParameter("@grosswt", dto.GrossWt ?? (object)DBNull.Value),
                new SqlParameter("@cBM", dto.CBM ?? (object)DBNull.Value),
                new SqlParameter("@fobAmount", dto.FOBAmount),
                new SqlParameter("@freightStatus", dto.FreightStatus ?? (object)DBNull.Value)

                ).ToList();              
                
                

                return AppResult(null, 1, "Successfully deleted data.", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

    }
}