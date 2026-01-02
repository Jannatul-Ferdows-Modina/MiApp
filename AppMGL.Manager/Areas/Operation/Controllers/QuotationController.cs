using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Operation;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Operation;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;
using AppMGL.Manager.Infrastructure.Helper;
using Newtonsoft.Json;
using AppMGL.DAL.Helper.Logging;
using System.Net.Http;
using AppMGL.Manager.Infrastructure.Results;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text;
//using OfficeOpenXml;
using System.Net;
using System.Net.Http.Headers;
using System.ComponentModel;
using AppMGL.Manager.Infrastructure.Report;

namespace AppMGL.Manager.Areas.Operation.Controllers
{
    public class QuotationController : BaseController<QuotationListDTO, QuotationListRepository, USP_GET_ENQUIRY_LIST_Result>
    {
        #region Constructor
        public QuotationController()
        {

        }

        public QuotationController(QuotationListRepository context)
        {
            _context = context;
            BaseModule = EnumModule.Quotation;
            KeyField = "EnquiryNo";
        }

        #endregion

        #region Public Method

        [System.Web.Http.HttpPost]
        public override ActionResult List(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<QuotationListDTO> result = _context.ExecuteQuery<QuotationListDTO>("EXEC dbo.USP_LG_QUOTATION_GET_LIST_New @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@DASHBOARD_FILTER,@SIT_ID,@isdraft",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("DASHBOARD_FILTER", searchCriteria["dashboardOption"]),
                        new SqlParameter("SIT_ID", listParams.SiteId),
                        new SqlParameter("isdraft", searchCriteria["isdraft"])

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
        public ActionResult GetApprovalList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<QuotationListDTO> result = _context.ExecuteQuery<QuotationListDTO>("EXEC dbo.USP_LG_QUOTATION_GET_APPROVAL_LIST @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@ISAPPROVED,@ISBOOKED,@DASHBOARD_FILTER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("ISAPPROVED", searchCriteria["isApproved"]),
                        new SqlParameter("ISBOOKED", searchCriteria["isBooked"]),
                        new SqlParameter("DASHBOARD_FILTER", searchCriteria["dashboardOption"]),
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
        public virtual ActionResult GetQuotaionDetail(QuotationDetailDTO objQuotation)
        {
            try
            {
                List<QuotationDetailDTO> result = _context.ExecuteQuery<QuotationDetailDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ENQUIRY_DETAIL @ENQUIRYID,@QUOTATIONID",
                                        new SqlParameter("ENQUIRYID", objQuotation.EnquiryID),
                                        new SqlParameter("QUOTATIONID", objQuotation.QuotationID)).ToList();

                QuotationDetailDTO objQuotationDetailDTO = result[0];

                if (objQuotationDetailDTO.QuotationID > 0)
                {
                    //get Quotation Carriers
                    List<QuotationCarrierDTO> objQuotationCarrierDTOList = _context.ExecuteQuery<QuotationCarrierDTO>("EXEC dbo.USP_LG_QUOTATION_GET_CARRIERS @QuotationID",
                                   new SqlParameter("QuotationID", objQuotationDetailDTO.QuotationID)).ToList();
                    if (objQuotationCarrierDTOList.Count > 0)
                    {
                        objQuotationDetailDTO.CarrierDTOList = objQuotationCarrierDTOList.ToArray();
                        foreach (QuotationCarrierDTO objQuotationCarrierDTO in objQuotationDetailDTO.CarrierDTOList)
                        {
                            //get Carrier Charges
                            List<CarrierChargesDTO> objCarrierChargesDTOList = _context.ExecuteQuery<CarrierChargesDTO>("EXEC dbo.USP_LG_QUOTATION_GET_CARRIER_RATES @QuotationID,@CarrierID",
                                   new SqlParameter("QuotationID", objQuotationCarrierDTO.QuotationID),
                                   new SqlParameter("CarrierID", objQuotationCarrierDTO.CarrierID)).ToList();
                            if (objCarrierChargesDTOList.Count > 0)
                            {
                                objQuotationCarrierDTO.CarrierChargesDTOList = objCarrierChargesDTOList.ToArray();
                            }
                            //get carrier Remarks
                            List<CarrierRemarksDTO> objCarrierRemarksDTOList = _context.ExecuteQuery<CarrierRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_CARRIER_REMARKS @QuotationID,@CarrierID",
                                  new SqlParameter("QuotationID", objQuotationCarrierDTO.QuotationID),
                                   new SqlParameter("CarrierID", objQuotationCarrierDTO.CarrierID)).ToList();

                            if (objCarrierRemarksDTOList.Count > 0)
                            {
                                objQuotationCarrierDTO.CarrierRemarksDTOList = objCarrierRemarksDTOList.ToArray();
                            }
                        }
                    }
                    //get Quotation Trucking Charges

                    List<TruckingCharges> TruckingChargesList = _context.ExecuteQuery<TruckingCharges>("EXEC dbo.USP_LG_QUOTATION_GET_TRUCKING_CHARGES @EnquiryId,@QuotationId",
                                    new SqlParameter("@EnquiryId", objQuotationDetailDTO.EnquiryID),
                                    new SqlParameter("@QuotationId", objQuotationDetailDTO.QuotationID)
                                   ).ToList();
                    if (TruckingChargesList.Count > 0)
                    {
                        objQuotationDetailDTO.TruckingChargesList = TruckingChargesList.ToArray();
                    }

                    //get commodity
                    IEnumerable<string> CommodityResult = _context.ExecuteQuery<string>("EXEC dbo.USP_LG_QUOTATION_EMAIL_GET_COMMODITY @QUOTATIONID",
                     new SqlParameter("QUOTATIONID", objQuotationDetailDTO.QuotationID)).ToList();
                    List<string> CommoditytList = CommodityResult.ToList();
                    var CommodityValue = "";
                    if (CommoditytList.Count > 0)
                    {
                        CommodityValue = string.Join(",", CommoditytList);
                    }
                    else
                    {
                        CommodityValue = "";
                    }
                    objQuotationDetailDTO.Commodity = CommodityValue;
                    if (objQuotationDetailDTO.IsDraft != null)
                    {
                        if (objQuotationDetailDTO.IsDraft == 1)
                        {
                            objQuotationDetailDTO.EmailMessageDTO = GetEmailData(objQuotationDetailDTO, objQuotation.EnquiryID);
                        }
                    }

                }
                else
                {
                    //get quotation No
                    IEnumerable<int> ResultList = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_GET_QUOTATION_NO").ToList();
                    List<int> QuotationNoResult = ResultList.ToList();
                    if (QuotationNoResult.Count > 0)
                    {
                        objQuotationDetailDTO.QuotationNo = QuotationNoResult[0].ToString();
                    }
                    //get Quotation Trucking Charges

                    List<TruckingCharges> TruckingChargesList = _context.ExecuteQuery<TruckingCharges>("EXEC dbo.USP_LG_QUOTATION_GET_TRUCKING_CHARGES @EnquiryId,@QuotationId",
                                    new SqlParameter("@EnquiryId", objQuotationDetailDTO.EnquiryID),
                                    new SqlParameter("@QuotationId", objQuotationDetailDTO.QuotationID)
                                   ).ToList();
                    if (TruckingChargesList.Count > 0)
                    {
                        objQuotationDetailDTO.TruckingChargesList = TruckingChargesList.ToArray();
                    }
                }
                ////get all Next Action remarks
                int bookingid = 0;
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                     new SqlParameter("QUOTATIONID", objQuotation.QuotationID),
                     new SqlParameter("ENQUIRYID", objQuotation.EnquiryID),
                     new SqlParameter("BookingID", bookingid),
                     new SqlParameter("RefType", "Quotation")).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                if (RemarksResultList.Count > 0)
                {
                    objQuotationDetailDTO.NextActionRemarksDTOList = RemarksResult.ToArray();
                    objQuotationDetailDTO.LastRemarks = RemarksResultList[0].Remarks;
                    objQuotationDetailDTO.LastRemarkDate = RemarksResultList[0].NextActionDate;
                }

                return AppResult(objQuotationDetailDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult saveQuotation(QuotationDetailDTO objQuotationDetail)
        {
            try
            {
                //if(objQuotationDetail.IsDraft==0)
                //{
                //    AppResult(null, 0, "You can not edit this.", EnumResult.Failed);
                //}

                IEnumerable<decimal> objResult = _context.ExecuteQuery<decimal>("EXEC dbo.USP_LG_QUOTATION_INSERT_UPDATE @QuotationID, @EnquiryID, @PaymentTerms, @OtherTerms, @CreatedBy, @UpdatedBy, @QuotationNo, @ShipmentMoveDate,@DestinationCountryID,@OriginCountryID, @Originportid, @Destinitionportid, @GalRepID,@ContactID, @ExpectedDate, @TruckingCharges, @BuyingTruckingCharge, @TruckingRemarks, @EmailRemark,@ValidTill, @CarriageCurrency,@IsConBooking",
                new SqlParameter("QuotationID", objQuotationDetail.QuotationID),
                new SqlParameter("EnquiryID", objQuotationDetail.EnquiryID),
                new SqlParameter("PaymentTerms", objQuotationDetail.PaymentTerms ?? Convert.DBNull),
                new SqlParameter("OtherTerms", objQuotationDetail.OtherTerms ?? Convert.DBNull),
                new SqlParameter("CreatedBy", objQuotationDetail.CreatedBy ?? Convert.DBNull),
                new SqlParameter("UpdatedBy", objQuotationDetail.UpdatedBy ?? Convert.DBNull),
                new SqlParameter("QuotationNo", objQuotationDetail.QuotationNo),
                new SqlParameter("ShipmentMoveDate", objQuotationDetail.ShipmentMoveDate ?? Convert.DBNull),
                new SqlParameter("DestinationCountryID", objQuotationDetail.DestinationCountryID ?? Convert.DBNull),
                new SqlParameter("OriginCountryID", objQuotationDetail.OriginCountryID ?? Convert.DBNull),
                new SqlParameter("Originportid", objQuotationDetail.OriginID),
                new SqlParameter("Destinitionportid", objQuotationDetail.DischargeID),
                new SqlParameter("GalRepID", objQuotationDetail.GalRepID ?? Convert.DBNull),
                new SqlParameter("ContactID", objQuotationDetail.ContactID),
                new SqlParameter("ExpectedDate", objQuotationDetail.ShipmentMoveDate ?? Convert.DBNull),
                new SqlParameter("TruckingCharges", objQuotationDetail.TruckSelling),
                new SqlParameter("BuyingTruckingCharge", objQuotationDetail.TruckBuying),
                new SqlParameter("TruckingRemarks", objQuotationDetail.TruckingRemarks ?? Convert.DBNull),
                new SqlParameter("EmailRemark", objQuotationDetail.EmailRemarks ?? Convert.DBNull),
                new SqlParameter("ValidTill", objQuotationDetail.ValidTill ?? Convert.DBNull),
                new SqlParameter("CarriageCurrency", objQuotationDetail.CarriageCurrency ?? Convert.DBNull),
                new SqlParameter("IsConBooking", objQuotationDetail.IsConBooking ?? Convert.DBNull)
                ).ToList();

                List<decimal> objList = objResult.ToList();
                decimal QuotationId = objList[0];
                if (QuotationId > 0)
                {
                    List<int> DeleteResult;
                    //delete existing CarrierRemarks --USP_LG_QUOTATION_REMARKS_DELETE
                    DeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_REMARKS_DELETE @QuotationID", new SqlParameter("QuotationID", QuotationId)).ToList();
                    //delete existing CarrierRates --USP_LG_QUOTATION_CARRIER_RATES_DELETE
                    DeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_CARRIER_RATES_DELETE @QuotationID", new SqlParameter("QuotationID", QuotationId)).ToList();
                    //delete existing carrier -- USP_LG_QUOTATION_CARRIER_DELETE
                    DeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_CARRIER_DELETE @QuotationID", new SqlParameter("QuotationID", QuotationId)).ToList();

                    if (objQuotationDetail.CarrierDTOList.Count() > 0)
                    {
                        SaveQuotationCarriers(objQuotationDetail.CarrierDTOList, QuotationId);
                    }
                    //save next action remarks
                    if (objQuotationDetail.NextActionRemarks != "" && objQuotationDetail.NextActionDueDate != null)
                    {
                        IEnumerable<int> objRemarksResult;
                        objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                        new SqlParameter("RefID", QuotationId),
                        new SqlParameter("NextActionDate", objQuotationDetail.NextActionDueDate),
                        new SqlParameter("Type", "Quotation"),
                        new SqlParameter("ActivityID", 2),
                        new SqlParameter("Remarks", objQuotationDetail.NextActionRemarks)).ToList();
                    }
                    //Delete exiting Quotation Trucking Charges

                    DeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_TRUCKING_CHARGES_DELETE @EnquiryId,@QuotationId",
                       new SqlParameter("EnquiryId", objQuotationDetail.EnquiryID),
                       new SqlParameter("QuotationId", QuotationId)).ToList();

                    if (objQuotationDetail.TruckingChargesList.Count() > 0)
                    {
                        SaveQuotationTruckingCharges(objQuotationDetail.TruckingChargesList, QuotationId, objQuotationDetail.EnquiryID, objQuotationDetail.CreatedBy);
                    }
                }

                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        public void SaveQuotationTruckingCharges(TruckingCharges[] TruckingChargesList, decimal QuotationId, decimal EnquiryId, int? CreatedBy)
        {
            IEnumerable<int> objResult;
            foreach (TruckingCharges objTruckingCharges in TruckingChargesList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_TRUCKING_CHARGES_INSERT @EnquiryId,@QuotationId,@BuyingRate,@SellingRate,@ContainerID,@CreatedBy",
                    new SqlParameter("EnquiryId", EnquiryId),
                    new SqlParameter("QuotationId", QuotationId),
                    new SqlParameter("BuyingRate", objTruckingCharges.TrkBuyingRate),
                    new SqlParameter("SellingRate", objTruckingCharges.TrkSellingRate),
                    new SqlParameter("ContainerID", objTruckingCharges.TrkContainerID),
                    new SqlParameter("CreatedBy", CreatedBy)
                    ).ToList();
            }
        }

        public void SaveQuotationCarriers(QuotationCarrierDTO[] QuotationCarrierDTOList, decimal QuotationId)
        {
            //insert enquiry Commodity details            
            IEnumerable<decimal> objResult;
            foreach (QuotationCarrierDTO objQuotationCarrierDTO in QuotationCarrierDTOList)
            {
                objResult = _context.ExecuteQuery<decimal>("EXEC dbo.USP_LG_QUOTATION_CARRIER_INSERT @QuotationID,@CarrierID,@StartDate,@EndDate,@Approved,@Remarks,@TransitTime,@Frequency",
                    new SqlParameter("@QuotationID", QuotationId),
                    new SqlParameter("@CarrierID", objQuotationCarrierDTO.CarrierID),
                    new SqlParameter("@StartDate", objQuotationCarrierDTO.StartDate),
                    new SqlParameter("@EndDate", objQuotationCarrierDTO.EndDate),
                    new SqlParameter("@Approved", objQuotationCarrierDTO.Approved),
                    new SqlParameter("@Remarks", objQuotationCarrierDTO.Remarks ?? Convert.DBNull),
                    new SqlParameter("@TransitTime", objQuotationCarrierDTO.TransitTime ?? Convert.DBNull),
                    new SqlParameter("@Frequency", objQuotationCarrierDTO.Frequency ?? Convert.DBNull)).ToList();
                List<decimal> objList = objResult.ToList();
                decimal QuotationCarrierId = objList[0];
                if (QuotationCarrierId > 0)
                {
                    if (objQuotationCarrierDTO.CarrierChargesDTOList != null)
                    {
                        if (objQuotationCarrierDTO.CarrierChargesDTOList.Count() > 0)
                        {
                            SaveCarrierCharges(objQuotationCarrierDTO.CarrierChargesDTOList, QuotationCarrierId, QuotationId);
                        }
                    }
                    if (objQuotationCarrierDTO.CarrierRemarksDTOList != null)
                    {
                        if (objQuotationCarrierDTO.CarrierRemarksDTOList.Count() > 0)
                        {
                            SaveCarrierRemarks(objQuotationCarrierDTO.CarrierRemarksDTOList, objQuotationCarrierDTO.CarrierID, QuotationId);
                        }
                    }
                }
            }
        }
        public void SaveCarrierCharges(CarrierChargesDTO[] objCarrierChargesDTOList, decimal QuotationCarrierId, decimal QuotationId)
        {
            //insert enquiry Commodity details
            IEnumerable<decimal> objResult;
            foreach (CarrierChargesDTO objCarrierChargesDTO in objCarrierChargesDTOList)
            {
                if (objCarrierChargesDTO.RefName == "Ocean Frieght" || objCarrierChargesDTO.RefName =="Ocean Freight")
                {
                    objResult = _context.ExecuteQuery<decimal>("EXEC dbo.USP_LG_QUOTATION_CARRIER_CONTAINER_RATE_INSERT @QuotationID,@QuotationCarrierID,@RefID,@Ref,@BuyingRate,@SellingRate,@fkContainerID,@IsApplicable,@remark",
                    new SqlParameter("@QuotationID", QuotationId),
                    new SqlParameter("@QuotationCarrierID", QuotationCarrierId),
                    new SqlParameter("@RefID", objCarrierChargesDTO.RefId),
                    new SqlParameter("@Ref", objCarrierChargesDTO.RefName),
                    new SqlParameter("@BuyingRate", objCarrierChargesDTO.BuyingRate),
                    new SqlParameter("@SellingRate", objCarrierChargesDTO.SellingRate),
                    new SqlParameter("@fkContainerID", objCarrierChargesDTO.fkContainerID),
                    new SqlParameter("@IsApplicable", objCarrierChargesDTO.IsApplicable),
                    new SqlParameter("@remark", objCarrierChargesDTO.itemremark ?? Convert.DBNull)
                    ).ToList();
                }
            }
            foreach (CarrierChargesDTO objCarrierChargesDTO in objCarrierChargesDTOList)
            {
                if (objCarrierChargesDTO.RefName != "Ocean Frieght")
                {
                    if (objCarrierChargesDTO.RefName != "Ocean Freight")
                    {
                        objResult = _context.ExecuteQuery<decimal>("EXEC dbo.USP_LG_QUOTATION_CARRIER_RATES_INSERT @QuotationID,@QuotationCarrierID,@RefID,@Ref,@BuyingRate,@SellingRate,@fkContainerID,@IsApplicable,@remark",
                    new SqlParameter("@QuotationID", QuotationId),
                    new SqlParameter("@QuotationCarrierID", QuotationCarrierId),
                    new SqlParameter("@RefID", objCarrierChargesDTO.RefId),
                    new SqlParameter("@Ref", objCarrierChargesDTO.RefName),
                    new SqlParameter("@BuyingRate", objCarrierChargesDTO.BuyingRate),
                    new SqlParameter("@SellingRate", objCarrierChargesDTO.SellingRate),
                    new SqlParameter("@fkContainerID", objCarrierChargesDTO.fkContainerID),
                    new SqlParameter("@IsApplicable", objCarrierChargesDTO.IsApplicable),
                    new SqlParameter("@remark", objCarrierChargesDTO.itemremark ?? Convert.DBNull)).ToList();
                    }
                }
            }

        }
        public void SaveCarrierRemarks(CarrierRemarksDTO[] objCarrierRemarksDTOList, int CarrierId, decimal QuotationId)
        {
            //insert enquiry Commodity details
            IEnumerable<decimal> objResult;
            foreach (CarrierRemarksDTO objCarrierRemarksDTO in objCarrierRemarksDTOList)
            {
                objResult = _context.ExecuteQuery<decimal>("EXEC dbo.USP_LG_QUOTATION_REMARKS_INSERT @QuotationID,@CarrierID,@RefID,@Remarks",
                    new SqlParameter("QuotationID", QuotationId),
                    new SqlParameter("CarrierID", CarrierId),
                    new SqlParameter("RefID", objCarrierRemarksDTO.RefId),
                    new SqlParameter("Remarks", objCarrierRemarksDTO.Remarks)).ToList();
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult DeleteQuotation(QuotationListDTO objQuotationListDTO)
        {
            try
            {
                //if (objQuotationListDTO.IsDraft == false)
                //{
                //    AppResult(null, 0, "You can not delete this", EnumResult.Failed);

                   
                //}


                IEnumerable<int> objResult = null;

                if (objQuotationListDTO.QuotationID > 0)
                {
                    objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_DELETE @ID, @IsCancelled,@Remarks",
                                        new SqlParameter("ID", objQuotationListDTO.QuotationID),
                                        new SqlParameter("IsCancelled", objQuotationListDTO.IsCancelled),
                                        new SqlParameter("Remarks", objQuotationListDTO.Remarks)).ToList();
                }
                else
                {
                    objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_DELETE @ID, @Remarks",
                                        new SqlParameter("ID", objQuotationListDTO.EnquiryID),
                                        new SqlParameter("Remarks", objQuotationListDTO.Remarks)).ToList();
                }

                List<int> objList = objResult.ToList();
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult ApproveQuotation(QuotationListDTO[] objQuotationList)
        {
            //if (objQuotationList[0].IsApproved == true)
            //{
            //    AppResult(null, 0, "Already Approved", EnumResult.Failed);
            //}

            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_APPROVE @QuotationID,@IsDraft,@IsApproved,@IsBooked",
                                   new SqlParameter("QuotationID", objQuotationList[0].QuotationID),
                                   new SqlParameter("IsDraft", objQuotationList[0].IsDraft),
                                   new SqlParameter("IsApproved", objQuotationList[0].IsApproved),
                                   new SqlParameter("IsBooked", objQuotationList[0].IsBooked)).ToList();
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult ApproveQuotationCarrier(QuotationCarrierDTO[] objQuotationCarrierDTOList)
        {
            try
            {
                IEnumerable<int> objResult = null;
                foreach (QuotationCarrierDTO objQuotationCarrierDTO in objQuotationCarrierDTOList)
                {
                    objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_CARRIER_APPROVE @QuotationID,@CarrierID,@IsApproved,@CustReqCutOffDate,@CustReqSailingDate",
                                       new SqlParameter("QuotationID", objQuotationCarrierDTO.QuotationID),
                                       new SqlParameter("CarrierID", objQuotationCarrierDTO.CarrierID),
                                       new SqlParameter("IsApproved", objQuotationCarrierDTO.ApprovedForBooking),
                                       new SqlParameter("CustReqCutOffDate", objQuotationCarrierDTO.custReqCutOffDate ?? Convert.DBNull),
                                       new SqlParameter("CustReqSailingDate", objQuotationCarrierDTO.custReqSailingDate ?? Convert.DBNull)).ToList();
                }
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetQuotationNo()
        {
            try
            {
                IEnumerable<decimal> objResult = _context.ExecuteQuery<decimal>("EXEC dbo.USP_LG_QUOTATION_GET_QUOTATION_NO").ToList();
                List<decimal> result = objResult.ToList();
                return AppResult(result, 1, "", EnumResult.Success);

            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetCurrencies()
        {
            try
            {
                var result = _context.ExecuteQuery<SIPLCurrencyDTO>("EXEC dbo.USP_LG_QUOTATION_GET_CURRENCIES").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetEnquiryContainers(long id)
        {
            try
            {
                decimal enquiryId;
                enquiryId = Convert.ToDecimal(id);

                var result = _context.ExecuteQuery<ContainerTypeDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ENQUIRY_CONTAINERS @ENQUIRY_ID",
                     new SqlParameter("ENQUIRY_ID", enquiryId)).ToList();
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
        public virtual ActionResult GetAllRemarks()
        {
            try
            {
                var result = _context.ExecuteQuery<CarrierRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_REMARKS").ToList();
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
        public virtual ActionResult GetCarrierAllRates(long id)
        {
            try
            {
                decimal enquiryId;
                enquiryId = Convert.ToDecimal(id);
                //int TotalCount;                
                var result = _context.ExecuteQuery<CarrierChargesDTO>("EXEC dbo.USP_LG_QUOTATION_GET_MANUAL_RATE_BY_ENQUIRY @EnquiryID",
                     new SqlParameter("EnquiryID", enquiryId)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpGet]
        public virtual ActionResult GetEmailIds(string id)
        {
            try
            {
                //IEnumerable<EmailDTO> objResult = null;
                var result = _context.ExecuteQuery<EmailDTO>("EXEC dbo.USP_LG_EMAIL_GET @SEARCH_VALUE",
                                        new SqlParameter("SEARCH_VALUE", id)).ToList();

                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        public EmailMessage GetEmailData(QuotationDetailDTO objQuotationDetailDTO, decimal enquiryId)
        {
            try
            {
                var result = _context.ExecuteQuery<EmailMessage>("EXEC dbo.USP_LG_QUOTATION_GET_EMAIL_DATA @QuotationID",
                     new SqlParameter("QuotationID", objQuotationDetailDTO.QuotationID)).ToList();

                EmailMessage objEmailMessage = result[0];
                //get Equipment details
                //IEnumerable<decimal> objResult
                IEnumerable<string> EquipmentResult = _context.ExecuteQuery<string>("EXEC dbo.USP_LG_QUOTATION_GET_EQUIPMENT @ENQUIRYID",
                     new SqlParameter("ENQUIRYID", enquiryId)).ToList();
                List<string> EquipmentList = EquipmentResult.ToList();
                var EquipmentValue = "";
                if (EquipmentList.Count > 0)
                {
                    EquipmentValue = string.Join(",", EquipmentResult);
                }
                else { EquipmentValue = ""; }
                ////get Commodity details
                //IEnumerable<string> CommodityResult = _context.ExecuteQuery<string>("EXEC dbo.USP_LG_QUOTATION_EMAIL_GET_COMMODITY @QUOTATIONID",
                //     new SqlParameter("QUOTATIONID", objQuotationDetailDTO.QuotationID)).ToList();
                //List<string> CommoditytList = CommodityResult.ToList();
                //var CommodityValue = "";
                //if (CommoditytList.Count > 0)
                //{
                //    CommodityValue = string.Join(",", CommoditytList);
                //}
                //else
                //{
                //    CommodityValue = "";
                //}
                //get Carrier Details
                //IEnumerable<object> CarrierChargesResult = _context.ExecuteQuery<object>("EXEC dbo.USP_LG_QUOTATION_EMAIL_GET_CARRIER_CHARGES @QuotationCarrierID,@QuotationID,@Remark",
                //     new SqlParameter("QuotationCarrierID", 8011),
                //     new SqlParameter("QuotationID", 6490),
                //     new SqlParameter("Remark", "ddd")).ToList();
                //List<string> EquipmentList = EquipmentResult.ToList();
                //IEnumerable<IDictionary<string, object>> results;
                //results = _context.ExecuteQuery<IDictionary<string, object>>("EXEC dbo.USP_LG_QUOTATION_EMAIL_GET_CARRIER_CHARGES @QuotationCarrierID,@QuotationID,@Remark",
                //     new SqlParameter("QuotationCarrierID", 8011),
                //     new SqlParameter("QuotationID", 6490),
                //     new SqlParameter("Remark", "ddd")).ToList();
                //foreach(var row in results)
                //{
                //    foreach(var col in row)
                //    {
                //        var i = col.Key;
                //    }
                //}

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/QuotationEmailTemplate.html")))
                {
                    body = reader.ReadToEnd();
                    body = body.Replace("{{quotationNo}}", "  " + result[0].QuotationNo);
                    body = body.Replace("{{validTill}}", "  " + result[0].ValidTill ?? "");
                    body = body.Replace("{{companyName}}", "  " + result[0].CompanyName ?? "");
                    body = body.Replace("{{customerInqNo}}", "  " + result[0].CustomerInqNo ?? "");
                    body = body.Replace("{{contactPerson}}", "  " + result[0].ContactPerson ?? "");
                    body = body.Replace("{{enquiryNo}}", "  " + result[0].EnquiryNo ?? "");
                    body = body.Replace("{{telNo}}", "  " + result[0].TelNo ?? "");
                    body = body.Replace("{{todayDate}}", "  " + result[0].TodayDate ?? "");
                    body = body.Replace("{{cityOfOrigin}}", "  " + result[0].CityOfOrigin ?? "");
                    body = body.Replace("{{railRamp}}", "  " + result[0].railRamp ?? "");
                    body = body.Replace("{{origin}}", "  " + result[0].Origin ?? "");
                    body = body.Replace("{{discharge}}", "  " + result[0].Discharge ?? "");
                    body = body.Replace("{{terminalName}}", "  " + result[0].terminalName ?? "");
                    body = body.Replace("{{Commodity}}", "  " + objQuotationDetailDTO.Commodity ?? "");
                    string evalue = string.Empty;
                    foreach (QuotationCarrierDTO objCarrierDTO in objQuotationDetailDTO.CarrierDTOList)
                    {
                        if (objCarrierDTO.CarrierChargesDTOList != null)
                        {
                            if (objCarrierDTO.CarrierChargesDTOList.Count() > 0)
                            {
                                for (int i = 0; i < objCarrierDTO.CarrierChargesDTOList.Count(); i++)
                                {
                                    if (!evalue.Contains(objCarrierDTO.CarrierChargesDTOList[i].ContainerTypeName.ToString()))
                                        evalue = evalue + ", " + objCarrierDTO.CarrierChargesDTOList[i].ContainerTypeName.ToString();
                                }
                            }
                        }
                    }
                    if(string.IsNullOrEmpty(evalue))
                    body = body.Replace("{{Equipment}}", "  " + EquipmentValue ?? "");
                    else
                        body = body.Replace("{{Equipment}}", "  " + evalue.Trim(',') ?? ""); 

                    body = body.Replace("{{noOfContainer}}", "  " + result[0].NoOfContainer.ToString());
                    body = body.Replace("{{currencyName}}", "  " + result[0].CurrencyName ?? "");


                }
                string CarrierTable = "";
                if (objQuotationDetailDTO.CarrierDTOList != null)
                {
                    if (objQuotationDetailDTO.CarrierDTOList.Count() > 0)
                    {
                        List<string> ContainerTypeList = new List<string>();
                        List<string> ChargesList = new List<string>();
                        List<decimal> TotalRates = new List<decimal>();
                        CarrierTable = CarrierTable + "<div style='width: 50%; margin: 0 auto;'><div style='text-align: center;width: 100%;'>";
                        foreach (QuotationCarrierDTO objCarrierDTO in objQuotationDetailDTO.CarrierDTOList)
                        {
                            ContainerTypeList.Clear();
                            TotalRates.Clear();
                            ChargesList.Clear();

                            CarrierTable = CarrierTable + "<br/><div><table class='borderTable' border='1' style='width:100%;'><tr>";
                            CarrierTable = CarrierTable + "<td style='white-space:nowrap;padding:5px 10px; font-size:16px; background-color:#39cccc; background-position: bottom;font-weight:bold;'>Carrier: " + objCarrierDTO.CarrierName + " </td>";
                            CarrierTable = CarrierTable + "<td style='padding:5px 10px; font-size:16px; background-color:#39cccc; background-position: bottom;font-weight:bold;'>Transit Time: " + objCarrierDTO.TransitTime + "</td>";
                            CarrierTable = CarrierTable + "<td style='white-space:nowrap;padding:5px 10px; font-size:16px; background-color:#39cccc; background-position: bottom;font-weight:bold;'>Frequency: " + objCarrierDTO.Frequency + "</td></tr></table></div>";
                            if (objCarrierDTO.CarrierChargesDTOList != null)
                            {
                                if (objCarrierDTO.CarrierChargesDTOList.Count() > 0)
                                {

                                    for (int i = 0; i < objCarrierDTO.CarrierChargesDTOList.Count(); i++)
                                    {
                                        if (!ContainerTypeList.Exists(e => e.Contains(objCarrierDTO.CarrierChargesDTOList[i].ContainerTypeName)))
                                        {
                                            ContainerTypeList.Add(objCarrierDTO.CarrierChargesDTOList[i].ContainerTypeName);
                                            if (i == 0)
                                            {
                                                CarrierTable = CarrierTable + "<table class='borderTable' border='1' style='width:100%;'>";
                                                CarrierTable = CarrierTable + "<tr><td style='border:#666666 1px solid; text-align:left; font-weight:bold;padding:5px 3px; vertical-align:top;'>&nbsp;</td>";
                                                CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; font-weight:bold;padding:5px 3px; vertical-align:top;'>&nbsp;" + objCarrierDTO.CarrierChargesDTOList[i].ContainerTypeName + "</td>";
                                            }
                                            else
                                            {
                                                CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; font-weight:bold;padding:5px 3px; vertical-align:top;'>&nbsp;" + objCarrierDTO.CarrierChargesDTOList[i].ContainerTypeName + "</td>";
                                            }
                                        }
                                    }
                                    CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; font-weight:bold;padding:5px 3px; vertical-align:top;'>&nbsp;" + "Remarks" + "</td>";
                                    CarrierTable = CarrierTable + "</tr>";
                                    int ColumsCount = 0;
                                    ColumsCount = ContainerTypeList.Count();
                                    //ChargesList.Clear();
                                    string chargesItem = "";
                                    int Tdcount = 0;
                                    bool IsColumnExits = false;
                                    string strremark = "";
                                    for (int i = 0; i < objCarrierDTO.CarrierChargesDTOList.Count(); i++)
                                    {
                                        chargesItem = objCarrierDTO.CarrierChargesDTOList[i].RefName;
                                        if (!ChargesList.Exists(e => e.Contains(chargesItem)))
                                        {
                                            ChargesList.Add(chargesItem);
                                            CarrierTable = CarrierTable + "<tr>";
                                            Tdcount = 0;
                                            IsColumnExits = false;
                                            foreach (string colName in ContainerTypeList)
                                            {
                                                IsColumnExits = false;
                                                for (int j = 0; j < objCarrierDTO.CarrierChargesDTOList.Count(); j++)
                                                {
                                                    if (Tdcount == 0)
                                                    {
                                                        CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;" + chargesItem + "</td>";
                                                        Tdcount = Tdcount + 1;
                                                    }
                                                    if (chargesItem == objCarrierDTO.CarrierChargesDTOList[j].RefName && colName == objCarrierDTO.CarrierChargesDTOList[j].ContainerTypeName)
                                                    {
                                                        CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;" + objCarrierDTO.CarrierChargesDTOList[j].SellingRate.ToString() + "</td>";
                                                        Tdcount = Tdcount + 1;
                                                        IsColumnExits = true;
                                                        if (strremark == "")
                                                            strremark = objCarrierDTO.CarrierChargesDTOList[j].itemremark;
                                                        else
                                                            strremark = strremark + ", " + objCarrierDTO.CarrierChargesDTOList[j].itemremark;
                                                    }
                                                    
                                                    if (IsColumnExits)
                                                    {
                                                        break;
                                                    }
                                                }
                                                if (!IsColumnExits)
                                                {
                                                    CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;" + "0" + "</td>";
                                                    Tdcount = Tdcount + 1;
                                                }
                                                
                                            }
                                            // vikas comment to show remark carrier
                                            //CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;" + objCarrierDTO.CarrierChargesDTOList[i].Remarks + "</td>";
                                            
                                            //if (!string.IsNullOrEmpty(objCarrierDTO.Remarks))
                                            //    strremark = strremark + objCarrierDTO.Remarks + ", ";

                                            //for (int j = 0; j < objCarrierDTO.CarrierChargesDTOList.Count(); j++)
                                            //{
                                            //    if(!string.IsNullOrEmpty( objCarrierDTO.CarrierChargesDTOList[j].itemremark))
                                            //    {
                                            //        strremark = strremark + objCarrierDTO.CarrierChargesDTOList[j].itemremark + ",";
                                            //    }
                                            //}
                                            //if(!string.IsNullOrEmpty(strremark))
                                            //{
                                            //    strremark = strremark.Substring(0, strremark.Length - 2);
                                            //}
                                            //if (!string.IsNullOrEmpty(objCarrierDTO.CarrierChargesDTOList[i].itemremark))
                                              //  strremark = objCarrierDTO.CarrierChargesDTOList[i].itemremark;

                                            CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;" + strremark + "</td>";
                                            CarrierTable = CarrierTable + "</tr>";
                                            strremark = "";
                                        }

                                    }

                                    //CarrierTable = CarrierTable + "<tr>";
                                    //CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;Trucking</td>";
                                    //CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;" + objQuotationDetailDTO.TruckSelling + "</td>";
                                    //CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;" + objQuotationDetailDTO.TruckingRemarks + "</td>";
                                    //CarrierTable = CarrierTable + "</tr>";

                                    // CarrierTable = CarrierTable + "<tr><td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;Trucking</td>";

                                    // chnaged by vikas to hidde trucking if rate is "0"
                                    string trucking = "<tr><td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;Trucking</td>";
                                    string truckingRate = "";
                                    string truckingRateHtml = "";
                                    foreach (string colName in ContainerTypeList)
                                    {
                                        if (objQuotationDetailDTO.TruckingChargesList != null)
                                        {
                                            for (int k = 0; k < objQuotationDetailDTO.TruckingChargesList.Count(); k++)
                                            {
                                                if (colName == objQuotationDetailDTO.TruckingChargesList[k].ContainerName)
                                                {
                                                    // chnaged by vikas to hidde trucking if rate is "0"   
                                                    // CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;" + objQuotationDetailDTO.TruckingChargesList[k].TrkSellingRate.ToString() + "</td>";
                                                    truckingRateHtml = truckingRateHtml + "<td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;" + objQuotationDetailDTO.TruckingChargesList[k].TrkSellingRate.ToString() + "</td>";
                                                    truckingRate = objQuotationDetailDTO.TruckingChargesList[k].TrkSellingRate.ToString();
                                                }
                                            }
                                        }
                                    }

                                    if (truckingRate != "0" && truckingRate != "")
                                    {
                                        CarrierTable = CarrierTable + trucking;
                                        CarrierTable = CarrierTable + truckingRateHtml;

                                        CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;" + objQuotationDetailDTO.TruckingRemarks + "</td>";
                                        //CarrierTable = CarrierTable + "</tr>";
                                    }

                                    if (truckingRate == "0" && truckingRate == "" && objQuotationDetailDTO.TruckingRemarks != "")
                                    {
                                        CarrierTable = CarrierTable + trucking;
                                        CarrierTable = CarrierTable + truckingRateHtml;

                                        CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;" + objQuotationDetailDTO.TruckingRemarks + "</td>";
                                        CarrierTable = CarrierTable + "</tr>";
                                    }


                                    decimal TotalSellingRate = 0;
                                    List<string> TruckingItemList = new List<string>();
                                    TruckingItemList.Clear();
                                    foreach (string colName in ContainerTypeList)
                                    {
                                        for (int k = 0; k < objCarrierDTO.CarrierChargesDTOList.Count(); k++)
                                        {
                                            if (objQuotationDetailDTO.TruckingChargesList != null)
                                            {
                                                for (int j = 0; j < objQuotationDetailDTO.TruckingChargesList.Count(); j++)
                                                {
                                                    if (colName == objCarrierDTO.CarrierChargesDTOList[k].ContainerTypeName && colName == objQuotationDetailDTO.TruckingChargesList[j].ContainerName)
                                                    {
                                                        TotalSellingRate += objCarrierDTO.CarrierChargesDTOList[k].SellingRate;
                                                        if (!TruckingItemList.Exists(e => e.Contains(objQuotationDetailDTO.TruckingChargesList[j].ContainerName)))
                                                        {
                                                            TruckingItemList.Add(objQuotationDetailDTO.TruckingChargesList[j].ContainerName);
                                                            TotalSellingRate += objQuotationDetailDTO.TruckingChargesList[j].TrkSellingRate;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        TotalRates.Add(TotalSellingRate);
                                        TotalSellingRate = 0;
                                    }


                                    CarrierTable = CarrierTable + "<tr><td style='border:#666666 1px solid; font-weight:bold; background-color:#39cccc; background-position: bottom;text-align:left; padding:5px 3px; vertical-align:top;'>Total</td>";

                                    foreach (decimal SellingRate in TotalRates)
                                    {
                                        CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; font-weight:bold; background-color:#39cccc; background-position: bottom;text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;" + SellingRate + "</td>";
                                    }
                                    CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; font-weight:bold; background-color:#39cccc; background-position: bottom;text-align:left; padding:5px 3px; vertical-align:top;'>" + result[0].PickupType + "</td>";
                                    CarrierTable = CarrierTable + "</tr></table><br/><br/>\n";
                                }
                            }
                            CarrierTable = CarrierTable + "<br/>\n";
                        }
                        CarrierTable = CarrierTable + "</div></div><br>\n\n";
                    }
                }

                result[0].EmailContent = body + "\n" + CarrierTable.Replace("Frieght","Freight");
                result[0].EmailSubject = "Quotation #" + result[0].QuotationNo + " for your shipment from " + result[0].Origin + " to " + result[0].Discharge;


                var EmailConfigList = _context.ExecuteQuery<QuotationEmailData>("EXEC dbo.USP_LG_QUOTATION_GET_EMAIL_CONFIG @QuotationID",
                     new SqlParameter("QuotationID", objQuotationDetailDTO.QuotationID)).ToList();
                foreach (QuotationEmailData objQuotationEmailData in EmailConfigList)
                {
                    if (objQuotationEmailData.ConfigName == "TO")
                    {
                        result[0].EmailTo = objQuotationEmailData.ConfigValue;
                    }
                    if (objQuotationEmailData.ConfigName == "CC")
                    {
                        result[0].Emailcc = objQuotationEmailData.ConfigValue;
                    }
                    if (objQuotationEmailData.ConfigName == "BCC")
                    {
                        result[0].EmailBcc = objQuotationEmailData.ConfigValue;
                    }
                }

                //return AppResult(result, result.Count);
                return result[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult SendEmail(QuotationEmailData emailData)
        {
            try
            {
                //update Quotatin emailmessage status
                List<string> EmailList = null;
                EmailList = new List<string>();
                //if (emailData.EmailTo.Contains(";"))
                //{
                //    Emailid = emailData.EmailTo.Split(';')[0];
                //}
                //else {
                //    Emailid = emailData.EmailTo;
                //}

               var userDetail = _context.ExecuteQuery<LG_USER>("EXEC dbo.GetUserDetailBYID @UserID",
                                 new SqlParameter("UserID", emailData.CreatedBy)).FirstOrDefault();
                userDetail.UsrSmtpPassword = SecurityHelper.DecryptString(userDetail.UsrSmtpPassword, "sblw-3hn8-sqoy19");

               // Logger.WriteWarning("pass " +  userDetail.UsrSmtpPassword);

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_EMAIL_UPDATE @QuotationID",
                                 new SqlParameter("QuotationID", emailData.QuotationID)).ToList();
                List<int> objList = objResult.ToList();
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new  MailAddress(userDetail.UsrSmtpUsername); // MailAddress(userDetail.UsrSmtpUsername);//        new MailAddress("Noreply@miamigloballines.com");
              //  mailMessage.ReplyToList.Add(new MailAddress(userDetail.UsrSmtpUsername, "reply-to"));
                //  mailMessage.From = new MailAddress(userDetail.UsrSmtpUsername);//
                //mailMessage.To.Add(new MailAddress(emailData.EmailTo));
                if (emailData.EmailTo.Contains(";"))
                {
                    string[] strUser = emailData.EmailTo.Split(';');
                    for (int i = 0; i < strUser.Length; i++)
                    {
                        if (strUser[i] != "")
                        {
                            mailMessage.To.Add(new MailAddress(strUser[i]));
                            EmailList.Add(strUser[i]);
                        }
                    }
                }
                else
                {
                    mailMessage.To.Add(new MailAddress(emailData.EmailTo));
                    EmailList.Add(emailData.EmailTo);
                }
                if (emailData.Emailcc != null)
                {
                    if (emailData.Emailcc.Contains(";"))
                    {
                        string[] strUser = emailData.Emailcc.Split(';');
                        for (int i = 0; i < strUser.Length; i++)
                        {
                            if (strUser[i] != "")
                            {
                                mailMessage.CC.Add(new MailAddress(strUser[i]));
                                EmailList.Add(strUser[i]);
                            }
                        }
                    }
                    else
                    {
                        mailMessage.CC.Add(new MailAddress(emailData.Emailcc));
                        EmailList.Add(emailData.Emailcc);
                    }
                }
                if (emailData.EmailBcc != null)
                {
                    if (emailData.EmailBcc.Contains(";"))
                    {
                        string[] strUser = emailData.EmailBcc.Split(';');
                        for (int i = 0; i < strUser.Length; i++)
                        {
                            if (strUser[i] != "")
                            {
                                mailMessage.Bcc.Add(new MailAddress(strUser[i]));
                                EmailList.Add(strUser[i]);
                            }
                        }
                    }
                    else
                    {
                        mailMessage.Bcc.Add(new MailAddress(emailData.EmailBcc));
                        EmailList.Add(emailData.EmailBcc);
                    }
                }
                IEnumerable<int> objEmailResult;
                foreach (string email in EmailList)
                {
                    objEmailResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_EMAIL_INSERT @EMAIL",
                                    new SqlParameter("EMAIL", email)).ToList();
                }

                mailMessage.Subject = emailData.EmailSubject;

                mailMessage.Body = emailData.EmailBody.Replace("src='Images/", "src='" + ConfigurationManager.AppSettings["WebPath"] + "Images/"); ;
                mailMessage.IsBodyHtml = true;
                string shtml = "<html><head><body> <table><tr><td>" + mailMessage.Body + "</td></tr></table></body></head></html>";              
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
               

                         List<QuotationListDTO> result = _context.ExecuteQuery<QuotationListDTO>("EXEC dbo.USP_LG_QUOTATION_EMAIL_Content_UPDATE @QuotationID, @EmailTo,@EmailBcc,@EmailCC,@EmailBody,@EmailSubject",
                        new SqlParameter("QuotationID", emailData.QuotationID),
                        new SqlParameter("EmailTo", emailData.EmailTo),
                        new SqlParameter("EmailBcc", emailData.EmailBcc==null?"" : emailData.EmailBcc),
                        new SqlParameter("EmailCC", emailData.Emailcc==null?"": emailData.Emailcc),
                        new SqlParameter("EmailBody", emailData.EmailBody)   ,
                         new SqlParameter("EmailSubject", emailData.EmailSubject==null?"": emailData.EmailSubject )
                        
                        ).ToList();

                EmailHelper.Send(principal, mailMessage);

                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        public async Task SendMailAsync(QuotationEmailData emailData)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("Noreply@miamigloballines.com");

            //if (emailData.EmailTo.Contains(";"))
            //{
            //    string[] strUser = emailData.EmailTo.Split(';');
            //    for (int i = 0; i < strUser.Length; i++)
            //    {
            //        if (strUser[i] != "")
            //        {                       
            //            mailMessage.To.Add(strUser[i]);
            //        }                   
            //    }
            //}
            //else {                
            //    mailMessage.To.Add(emailData.EmailTo);
            //}
            //if (emailData.Emailcc != null)
            //{
            //    if (emailData.Emailcc.Contains(";"))
            //    {
            //        string[] strUser = emailData.Emailcc.Split(';');
            //        for (int i = 0; i < strUser.Length; i++)
            //        {
            //            if (strUser[i] != "")
            //            {
            //                mailMessage.CC.Add(strUser[i]);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        mailMessage.CC.Add(emailData.Emailcc);
            //    }
            //}
            //if (emailData.EmailBcc != null)
            //{
            //    if (emailData.EmailBcc.Contains(";"))
            //    {
            //        string[] strUser = emailData.EmailBcc.Split(';');
            //        for (int i = 0; i < strUser.Length; i++)
            //        {
            //            if (strUser[i] != "")
            //            {
            //                mailMessage.Bcc.Add(strUser[i]);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        mailMessage.Bcc.Add(emailData.EmailBcc);
            //    }
            //}
            //mailMessage.CC.Add(emailData.Emailcc);
            //mailMessage.Bcc.Add(emailData.EmailBcc);
            mailMessage.Subject = emailData.EmailSubject;
            mailMessage.Body = "hello";//emailData.EmailBody;
            mailMessage.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient();
            await smtpClient.SendMailAsync(mailMessage);
        }
        #endregion

        //protected byte[] ConvertHtmlToPdf(string html, string header = null, string footer = null, bool isPageNumberInFooter = false)
        //{
        //    // Create ABCpdf Doc object
        //    var doc = new Doc();
        //    if (header == null && footer == null)
        //        doc.Rect.Inset(20, 20);
        //    else
        //        doc.Rect.String = "0 70 600 760"; /*padding from left, padding from bottom, width from left, height from bottom*/
        //    // Add html to Doc   
        //    //html = "<html><head></head><body></body></html>";
        //    int theId = doc.AddImageHtml(html);

        //    // Loop through document to create multi-page PDF
        //    while (true)
        //    {
        //        if (!doc.Chainable(theId))
        //            break;
        //        doc.Page = doc.AddPage();
        //        theId = doc.AddImageToChain(theId);
        //    }
        //    var count = doc.PageCount;

        //    /*****************Footer area******************/
        //    if (footer != null)
        //    {
        //        var newfooter = "";
        //        doc.Rect.String = "40 20 580 50";
        //        for (int i = 1; i <= count; i++)
        //        {

        //            doc.PageNumber = i;
        //            if (isPageNumberInFooter)
        //            {
        //                newfooter = footer.Replace("PageNumber", "Page " + i.ToString() + " of " + count.ToString());
        //                int id = doc.AddImageHtml(newfooter);

        //                while (true)
        //                {
        //                    if (!doc.Chainable(id))
        //                        break;
        //                    id = doc.AddImageToChain(id);
        //                }
        //            }
        //            else
        //                doc.AddText(footer);
        //        }
        //    }
        //    /*****************Footer area******************/


        //    // Flatten the PDF
        //    for (int i = 1; i <= doc.PageCount; i++)
        //    {
        //        doc.PageNumber = i;
        //        doc.Flatten();
        //    }

        //    var pdf = doc.GetData();
        //    doc.Clear();
        //    // Get PDF as byte array. Couls also use .Save() to save to disk
        //    return pdf;
        //}

        private void UploadFile(string fileName, string emailbody)
        {
            byte[] fileContent = null;

            StringReader sr = new StringReader(emailbody);

            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                htmlparser.Parse(sr);
                pdfDoc.Close();

                byte[] bytes = memoryStream.ToArray();
                fileContent = bytes;
                memoryStream.Close();
            }







            //byte[] filecontent = null;

            //byte[] fileContent = System.Text.Encoding.ASCII.GetBytes(emailbody);

            StandardJsonResult standardJsonResult = new StandardJsonResult
            {
                ResultId = Convert.ToInt32(EnumResult.Failed)
            };
            try
            {
                string path = ConfigurationManager.AppSettings["DocumentsPath"];
                string text = HttpContext.Current.Server.MapPath(path);
                string text2 = text + "\\DockReceipt";
                MultipartFormDataStreamProvider multipartFormDataStreamProvider = new MultipartFormDataStreamProvider(text);
                if (fileContent.Length != 0)
                {
                    if (Directory.Exists(text2))
                    {
                        File.WriteAllBytes(text2 + "\\" + fileName, fileContent);
                    }
                    else
                    {
                        Directory.CreateDirectory(text2);
                        File.WriteAllBytes(text2 + "\\" + fileName, fileContent);
                    }
                    standardJsonResult.AddMessage("File is uploaded successfully.", clearLastMessages: true);
                }
                else
                {
                    standardJsonResult.AddMessage("Some error found, please re generate the file.", clearLastMessages: true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                standardJsonResult.AddMessage(ex.Message, clearLastMessages: true);
            }
        }


        public QuotationDetailDTO GetQuationDetailOnly(QuotationDetailDTO objQuotation)
        {
            List<QuotationDetailDTO> result = _context.ExecuteQuery<QuotationDetailDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ENQUIRY_DETAIL @ENQUIRYID,@QUOTATIONID",
                                       new SqlParameter("ENQUIRYID", objQuotation.EnquiryID),
                                       new SqlParameter("QUOTATIONID", objQuotation.QuotationID)).ToList();

            QuotationDetailDTO objQuotationDetailDTO = result[0];

            if (objQuotationDetailDTO.QuotationID > 0)
            {
                //get Quotation Carriers
                List<QuotationCarrierDTO> objQuotationCarrierDTOList = _context.ExecuteQuery<QuotationCarrierDTO>("EXEC dbo.USP_LG_QUOTATION_GET_CARRIERS @QuotationID",
                               new SqlParameter("QuotationID", objQuotationDetailDTO.QuotationID)).ToList();
                if (objQuotationCarrierDTOList.Count > 0)
                {
                    objQuotationDetailDTO.CarrierDTOList = objQuotationCarrierDTOList.ToArray();
                    foreach (QuotationCarrierDTO objQuotationCarrierDTO in objQuotationDetailDTO.CarrierDTOList)
                    {
                        //get Carrier Charges
                        List<CarrierChargesDTO> objCarrierChargesDTOList = _context.ExecuteQuery<CarrierChargesDTO>("EXEC dbo.USP_LG_QUOTATION_GET_CARRIER_RATES @QuotationID,@CarrierID",
                               new SqlParameter("QuotationID", objQuotationCarrierDTO.QuotationID),
                               new SqlParameter("CarrierID", objQuotationCarrierDTO.CarrierID)).ToList();
                        if (objCarrierChargesDTOList.Count > 0)
                        {
                            objQuotationCarrierDTO.CarrierChargesDTOList = objCarrierChargesDTOList.ToArray();
                        }
                        //get carrier Remarks
                        List<CarrierRemarksDTO> objCarrierRemarksDTOList = _context.ExecuteQuery<CarrierRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_CARRIER_REMARKS @QuotationID,@CarrierID",
                              new SqlParameter("QuotationID", objQuotationCarrierDTO.QuotationID),
                               new SqlParameter("CarrierID", objQuotationCarrierDTO.CarrierID)).ToList();

                        if (objCarrierRemarksDTOList.Count > 0)
                        {
                            objQuotationCarrierDTO.CarrierRemarksDTOList = objCarrierRemarksDTOList.ToArray();
                        }
                    }
                }
                //get Quotation Trucking Charges

                List<TruckingCharges> TruckingChargesList = _context.ExecuteQuery<TruckingCharges>("EXEC dbo.USP_LG_QUOTATION_GET_TRUCKING_CHARGES @EnquiryId,@QuotationId",
                                new SqlParameter("@EnquiryId", objQuotationDetailDTO.EnquiryID),
                                new SqlParameter("@QuotationId", objQuotationDetailDTO.QuotationID)
                               ).ToList();
                if (TruckingChargesList.Count > 0)
                {
                    objQuotationDetailDTO.TruckingChargesList = TruckingChargesList.ToArray();
                }

                //get commodity
                IEnumerable<string> CommodityResult = _context.ExecuteQuery<string>("EXEC dbo.USP_LG_QUOTATION_EMAIL_GET_COMMODITY @QUOTATIONID",
                 new SqlParameter("QUOTATIONID", objQuotationDetailDTO.QuotationID)).ToList();
                List<string> CommoditytList = CommodityResult.ToList();
                var CommodityValue = "";
                if (CommoditytList.Count > 0)
                {
                    CommodityValue = string.Join(",", CommoditytList);
                }
                else
                {
                    CommodityValue = "";
                }
                objQuotationDetailDTO.Commodity = CommodityValue;
                if (objQuotationDetailDTO.IsDraft != null)
                {
                    if (objQuotationDetailDTO.IsDraft == 1)
                    {
                        objQuotationDetailDTO.EmailMessageDTO = GetEmailData(objQuotationDetailDTO, objQuotation.EnquiryID);
                    }
                }

            }
            else
            {
                //get quotation No
                IEnumerable<int> ResultList = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_GET_QUOTATION_NO").ToList();
                List<int> QuotationNoResult = ResultList.ToList();
                if (QuotationNoResult.Count > 0)
                {
                    objQuotationDetailDTO.QuotationNo = QuotationNoResult[0].ToString();
                }
                //get Quotation Trucking Charges

                List<TruckingCharges> TruckingChargesList = _context.ExecuteQuery<TruckingCharges>("EXEC dbo.USP_LG_QUOTATION_GET_TRUCKING_CHARGES @EnquiryId,@QuotationId",
                                new SqlParameter("@EnquiryId", objQuotationDetailDTO.EnquiryID),
                                new SqlParameter("@QuotationId", objQuotationDetailDTO.QuotationID)
                               ).ToList();
                if (TruckingChargesList.Count > 0)
                {
                    objQuotationDetailDTO.TruckingChargesList = TruckingChargesList.ToArray();
                }
            }
            ////get all Next Action remarks
            int bookingid = 0;
            IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                 new SqlParameter("QUOTATIONID", objQuotation.QuotationID),
                 new SqlParameter("ENQUIRYID", objQuotation.EnquiryID),
                 new SqlParameter("BookingID", bookingid),
                 new SqlParameter("RefType", "Quotation")).ToList();
            List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

            if (RemarksResultList.Count > 0)
            {
                objQuotationDetailDTO.NextActionRemarksDTOList = RemarksResult.ToArray();
                objQuotationDetailDTO.LastRemarks = RemarksResultList[0].Remarks;
                objQuotationDetailDTO.LastRemarkDate = RemarksResultList[0].NextActionDate;
            }
            return objQuotationDetailDTO;
        }



        public decimal SaveQuationDetail(QuotationDetailDTO objQuotationDetail)
        {
            try {
                IEnumerable<decimal> objResult = _context.ExecuteQuery<decimal>("EXEC dbo.USP_LG_QUOTATION_INSERT_UPDATE @QuotationID, @EnquiryID, @PaymentTerms, @OtherTerms, @CreatedBy, @UpdatedBy, @QuotationNo, @ShipmentMoveDate,@DestinationCountryID,@OriginCountryID, @Originportid, @Destinitionportid, @GalRepID,@ContactID, @ExpectedDate, @TruckingCharges, @BuyingTruckingCharge, @TruckingRemarks, @EmailRemark,@ValidTill, @CarriageCurrency",
                  new SqlParameter("QuotationID", objQuotationDetail.QuotationID),
                  new SqlParameter("EnquiryID", objQuotationDetail.EnquiryID),
                  new SqlParameter("PaymentTerms", objQuotationDetail.PaymentTerms ?? Convert.DBNull),
                  new SqlParameter("OtherTerms", objQuotationDetail.OtherTerms ?? Convert.DBNull),
                  new SqlParameter("CreatedBy", objQuotationDetail.CreatedBy ?? Convert.DBNull),
                  new SqlParameter("UpdatedBy", objQuotationDetail.UpdatedBy ?? Convert.DBNull),
                  new SqlParameter("QuotationNo", objQuotationDetail.QuotationNo),
                  new SqlParameter("ShipmentMoveDate", objQuotationDetail.ShipmentMoveDate ?? Convert.DBNull),
                  new SqlParameter("DestinationCountryID", objQuotationDetail.DestinationCountryID ?? Convert.DBNull),
                  new SqlParameter("OriginCountryID", objQuotationDetail.OriginCountryID ?? Convert.DBNull),
                  new SqlParameter("Originportid", objQuotationDetail.OriginID),
                  new SqlParameter("Destinitionportid", objQuotationDetail.DischargeID),
                  new SqlParameter("GalRepID", objQuotationDetail.GalRepID ?? Convert.DBNull),
                  new SqlParameter("ContactID", objQuotationDetail.ContactID),
                  new SqlParameter("ExpectedDate", objQuotationDetail.ShipmentMoveDate ?? Convert.DBNull),
                  new SqlParameter("TruckingCharges", objQuotationDetail.TruckSelling),
                  new SqlParameter("BuyingTruckingCharge", objQuotationDetail.TruckBuying),
                  new SqlParameter("TruckingRemarks", objQuotationDetail.TruckingRemarks ?? Convert.DBNull),
                  new SqlParameter("EmailRemark", objQuotationDetail.EmailRemarks ?? Convert.DBNull),
                  new SqlParameter("ValidTill", objQuotationDetail.ValidTill ?? Convert.DBNull),
                  new SqlParameter("CarriageCurrency", objQuotationDetail.CarriageCurrency ?? Convert.DBNull)).ToList();

                List<decimal> objList = objResult.ToList();
                decimal QuotationId = objList[0];
                if (QuotationId > 0)
                {
                    List<int> DeleteResult;
                    //delete existing CarrierRemarks --USP_LG_QUOTATION_REMARKS_DELETE
                    DeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_REMARKS_DELETE @QuotationID", new SqlParameter("QuotationID", QuotationId)).ToList();
                    //delete existing CarrierRates --USP_LG_QUOTATION_CARRIER_RATES_DELETE
                    DeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_CARRIER_RATES_DELETE @QuotationID", new SqlParameter("QuotationID", QuotationId)).ToList();
                    //delete existing carrier -- USP_LG_QUOTATION_CARRIER_DELETE
                    DeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_CARRIER_DELETE @QuotationID", new SqlParameter("QuotationID", QuotationId)).ToList();

                    if (objQuotationDetail.CarrierDTOList.Count() > 0)
                    {
                        SaveQuotationCarriers(objQuotationDetail.CarrierDTOList, QuotationId);
                    }
                    //save next action remarks
                    if (objQuotationDetail.NextActionRemarks != "" && objQuotationDetail.NextActionDueDate != null)
                    {
                        IEnumerable<int> objRemarksResult;
                        objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                        new SqlParameter("RefID", QuotationId),
                        new SqlParameter("NextActionDate", objQuotationDetail.NextActionDueDate),
                        new SqlParameter("Type", "Quotation"),
                        new SqlParameter("ActivityID", 2),
                        new SqlParameter("Remarks", objQuotationDetail.NextActionRemarks)).ToList();
                    }
                    //Delete exiting Quotation Trucking Charges

                    DeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_TRUCKING_CHARGES_DELETE @EnquiryId,@QuotationId",
                       new SqlParameter("EnquiryId", objQuotationDetail.EnquiryID),
                       new SqlParameter("QuotationId", QuotationId)).ToList();

                    if (objQuotationDetail.TruckingChargesList.Count() > 0)
                    {
                        SaveQuotationTruckingCharges(objQuotationDetail.TruckingChargesList, QuotationId, objQuotationDetail.EnquiryID, objQuotationDetail.CreatedBy);
                    }
                }

                return QuotationId;
            }
            catch (Exception ex)
            {
                return 0; 

            }

        }
        //[System.Web.Http.HttpPost]
        //public  ActionResult ExportQuotationData()
        //{
        //    byte[] stream = ExportQuotation();
        //    string excelName = string.Format("{0}.xlsx", "Income_Statement_Pivot_" + DateTime.Now.Ticks + "_" + DateTime.Now.Ticks + "_" + DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace("PM", "").Replace("AM", ""));
        //     return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        //   // return null;
        //}
        //public byte[] ExportQuotation()
        //{


        //        var stream = new System.IO.MemoryStream();
        //        using (ExcelPackage package = new ExcelPackage(stream))
        //        {
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
        //            worksheet.Cells.Style.Font.Name = "Open Sans";
        //            worksheet.Cells.Style.Font.Size = 8;
        //            worksheet.Row(1).Height = 25;

        //            worksheet.Column(1).Width = 30;
        //            worksheet.Column(2).Width = 25;
        //            for (int i = 3; i <= 15; i++)
        //            {
        //                worksheet.Column(i).Width = 15;
        //            }
        //            worksheet.View.FreezePanes(3, 3);
        //            package.Save();
        //            stream.Position = 0;
        //          byte[] buffer = null;
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            int count = 0;
        //            do
        //            {
        //                byte[] buf = new byte[1024];
        //                count = stream.Read(buf, 0, 1024);
        //                ms.Write(buf, 0, count);
        //            } while (stream.CanRead && count > 0);
        //            buffer = ms.ToArray();
        //        }
        //        return buffer;
        //        // return stream.ToByteArray();
        //    }


        //    //return null;

        //}
        //[System.Web.Http.HttpGet]
        //public ActionResult ExportQuotationData()
        //{
        //    // string excelName = string.Format("{0}.xlsx", "Income_Statement_Pivot_" + DateTime.Now.Ticks + "_" + DateTime.Now.Ticks + "_" + DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace("PM", "").Replace("AM", ""));
        //    // byte[] stream = getApplicantsStatistics();
        //    //return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        //    //return new FileContentResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
           

        //}


        //private ExcelPackage CreateDoc(string title, string subject, string keyword)
        //{
        //    var p = new ExcelPackage();
        //    p.Workbook.Properties.Title = title;
        //    p.Workbook.Properties.Author = "Application Name";
        //    p.Workbook.Properties.Subject = subject;
        //    p.Workbook.Properties.Keywords = keyword;
        //    return p;
        //}
        //public byte[] getApplicantsStatistics()
        //{
           // ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
           // ExcelPackage p = new ExcelPackage();
           

           // var worksheet = p.Workbook.Worksheets.Add("Applicant Statistics");

           // //Add Report Header
           //// worksheet.Cells[1, 1].Value = "Applicant Statistics";
           // //
           // //worksheet.Cells[1, 1, 1, 3].Merge = true;

           // //Get the data you want to send to the excel file
            

           // var appProg = _context.ExecuteQuery<ExportTempQuotation>("EXEC dbo.SIPL_Quotation_ExportSP @SiteId",
           //                                 new SqlParameter("SiteId", 454)).ToList();

            
           // //First add the headers
           // worksheet.Cells[2, 1].Value = "Enquiry No";
           // worksheet.Cells[2, 2].Value = "Quotation No";
           // worksheet.Cells[2, 3].Value = "Enquiry Date";
           // worksheet.Cells[2, 4].Value = "Company Name";
           // worksheet.Cells[2, 5].Value = "Expected Shipment Date";
           // worksheet.Cells[2, 6].Value = "Source Country";
           // worksheet.Cells[2, 7].Value = "Destination Country";
           // worksheet.Cells[2, 8].Value = "Port of Origin";
           // worksheet.Cells[2, 9].Value = "Port of Destination";

           // //Add values
           // var numberformat = "#,##0";
           // var dataCellStyleName = "TableNumber";
           // var numStyle = p.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
           // numStyle.Style.Numberformat.Format = numberformat;

           // for (int i = 0; i < appProg.Count; i++)
           // {
           //    // worksheet.Cells[i + 3, 1].Value = i + 1;
               
           //     worksheet.Cells[i + 3, 1].Value = appProg[i].EnquiryNo;
           //     worksheet.Cells[i + 3, 2].Value = appProg[i].QuotationNo;
           //     worksheet.Cells[i + 3, 3].Value = appProg[i].EnquiryDate;
           //     worksheet.Cells[i + 3, 4].Value = appProg[i].CompanyName;
           //     worksheet.Cells[i + 3, 5].Value = appProg[i].ExpectedShipmentDate;
           //     worksheet.Cells[i + 3, 6].Value = appProg[i].SourceCountry;
           //     worksheet.Cells[i + 3, 7].Value = appProg[i].DestinationCountry;
           //     worksheet.Cells[i + 3, 8].Value = appProg[i].PortOrigin;
           //     worksheet.Cells[i + 3, 9].Value = appProg[i].PortDestination;
           // }
           // // Add to table / Add summary row
           // //var rowEnd = appProg.Count + 2;
           // //var tbl = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 2, fromCol: 1, toRow: rowEnd, toColumn: 3), "Applicants");
           // //tbl.ShowHeader = true;
           // //tbl.TableStyle = TableStyles.Dark9;
           // //tbl.ShowTotal = true;
           // //tbl.Columns[2].DataCellStyleName = dataCellStyleName;
           // //tbl.Columns[2].TotalsRowFunction = RowFunctions.Sum;
           // //worksheet.Cells[rowEnd, 3].Style.Numberformat.Format = numberformat;

           // //// AutoFitColumns
           // //worksheet.Cells[2, 1, rowEnd, 3].AutoFitColumns();
           // return p.GetAsByteArray();
            
        //}

        // Tracking Module
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> uploadQuotationFinalFile()
        {
            StandardJsonResult output = new StandardJsonResult
            {
                ResultId = Convert.ToInt32(EnumResult.Failed)
            };
            //if (!Request.Content.IsMimeMultipartContent())
            //{
            //    output.AddMessage(HttpContext.GetGlobalResourceObject("CommonTerms", "ErrFileTypeNotSupported").ToString(), clearLastMessages: true);
            //    return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, new
            //    {
            //        output
            //    });
            //}
            try
            {
                string path = ConfigurationManager.AppSettings["QuotationTracking"];
                string root = HttpContext.Current.Server.MapPath(path);
                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(root);
                MultipartFormDataStreamProvider multipartFormDataStreamProvider = await Request.Content.ReadAsMultipartAsync(streamProvider);
                if (multipartFormDataStreamProvider.FormData.HasKeys())
                {
                    string FileName = GetUnescapeData(multipartFormDataStreamProvider, "FileName").ToString();
                    string SiteId = GetUnescapeData(multipartFormDataStreamProvider, "SiteId").ToString();
                    //FileInfo[] files = new DirectoryInfo(root).GetFiles(str2 + "_*");
                    FileName = FileName + "_" + SiteId+".xlsx";
                    FileInfo[] files = new DirectoryInfo(root).GetFiles(FileName);
                    for (int i = 0; i < files.Length; i++)
                    {
                        files[i].Delete();
                    }
                    DateTime now = DateTime.Now;
                    new FileInfo(multipartFormDataStreamProvider.FileData.First().LocalFileName).MoveTo(root + "\\" + FileName);
                    output.AddMessage("File is uploaded successfully.", clearLastMessages: true);
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        output
                    });
                }
                output.AddMessage("Some error found, please contact administrator.", clearLastMessages: true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    output
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                output.AddMessage(ex.Message, clearLastMessages: true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    output
                });
            }
        }
        [System.Web.Http.HttpPost]
        public HttpResponseMessage downloadQuotationFinal()
        {
            try
            {
                string FileName = "";
                FileName = base.Request.Headers.GetValues("FileName").ToList()[0];
                string SiteId = base.Request.Headers.GetValues("SiteId").ToList()[0];
                //text = text + "_" + num;
                FileName = FileName + "_" + SiteId + ".xlsx";
                HttpResponseMessage httpResponseMessage = null;
                if (!File.Exists(FileName))
                {
                    httpResponseMessage = base.Request.CreateResponse(HttpStatusCode.Gone);
                }
                if (!string.IsNullOrEmpty(FileName))
                {
                    string str = ConfigurationManager.AppSettings["QuotationTracking"];
                    string path = HttpContext.Current.Server.MapPath(str + "/") + FileName;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            byte[] array = new byte[fileStream.Length];
                            fileStream.Read(array, 0, (int)fileStream.Length);
                            memoryStream.Write(array, 0, (int)fileStream.Length);
                            HttpResponseMessage httpResponseMessage2 = new HttpResponseMessage();
                            httpResponseMessage2.Content = new ByteArrayContent(array.ToArray());
                            httpResponseMessage2.Content.Headers.Add("x-filename", FileName);
                            httpResponseMessage2.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                            httpResponseMessage2.StatusCode = HttpStatusCode.OK;
                            return httpResponseMessage2;
                        }
                    }
                }
                return base.Request.CreateResponse(HttpStatusCode.NotFound, "File not found.");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return base.Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }


        [System.Web.Http.HttpPost]
        public HttpResponseMessage TrackingExportReport(Dictionary<string, string> exportParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/QuatationTrack";
                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();
                reportParams.Add(new KeyValuePair<string, string>("SiteId", exportParams["SitId"]));
                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);
                if (reportBuffer.Length > 0)
                {
                    string fileName = "QuatationTrack_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";

                    HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new MemoryStream(reportBuffer));
                    result.Content.Headers.Add("X-FileName", fileName);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = fileName;
                    result.StatusCode = HttpStatusCode.OK;

                    return result;
                }

                output.AddMessage("System cannot generated report, please contact administrator.", true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { output });
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                output.AddMessage(ex.Message, true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { output });
            }
        }

        [System.Web.Http.HttpDelete]
        public ActionResult TempDeleteQuotation(int id)
        {
            try
            {
                IEnumerable<int> source = _context.ExecuteQuery<int>("EXEC dbo.SIPL_Quotation_ExportDeleteSP @SiteId", new object[1]
                {
                    new SqlParameter("SiteId",id)
                }).ToList();
                List<int> entity = source.ToList();
                return AppResult(entity, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpGet]
        public ActionResult GetQuotationRemark(int id)
        {
            ////get all Next Action remarks
            try
            {
                int bookingid = 0;
                decimal EnquiryID = Convert.ToDecimal(0);
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@EnquiryID,@BookingID,@RefType",
                     new SqlParameter("QUOTATIONID", id),
                     new SqlParameter("EnquiryID", EnquiryID),
                     new SqlParameter("BookingID", bookingid),
                     new SqlParameter("RefType", "Quotation")).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();
                return AppResult(RemarksResultList, 1);
            }
            catch(Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage ExportReportRemarkQuotation(ListParams listParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/QuotationReport_Remark";
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                // var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();
                reportParams.Add(new KeyValuePair<string, string>("OPTIONVALUE", searchCriteria["optionValue"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHVALUE", searchCriteria["seachValue"]));
                reportParams.Add(new KeyValuePair<string, string>("SORTCOLUMN", "EnquiryDate"));
                reportParams.Add(new KeyValuePair<string, string>("SORTORDER", "DESC"));
                reportParams.Add(new KeyValuePair<string, string>("DASHBOARD_FILTER", searchCriteria["dashboardOption"]));
                reportParams.Add(new KeyValuePair<string, string>("SIT_ID", listParams.SiteId.ToString()));
                reportParams.Add(new KeyValuePair<string, string>("isdraft", searchCriteria["isdraft"]));


                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = "QuotationRemark_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";

                    HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new MemoryStream(reportBuffer));
                    result.Content.Headers.Add("X-FileName", fileName);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = fileName;
                    result.StatusCode = HttpStatusCode.OK;

                    return result;
                }

                output.AddMessage("System cannot generated report, please contact administrator.", true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { output });
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                output.AddMessage(ex.Message, true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { output });
            }
        }
      
        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveQuationRemarks(QuotationRemarksDTO objQuotationDetail)
        {
            try
            {
                IEnumerable<int> objResult = null;
                if (objQuotationDetail.LastRemarks != "" && objQuotationDetail.NextActionDueDate != null)
                {

                    objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                    new SqlParameter("RefID", objQuotationDetail.QuotationID),
                    new SqlParameter("NextActionDate", objQuotationDetail.NextActionDueDate),
                    new SqlParameter("Type", "Quotation"),
                    new SqlParameter("ActivityID", 2),
                    new SqlParameter("Remarks", objQuotationDetail.LastRemarks)).ToList();

                }
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);

            }
        }
    }
}