using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Operation;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Operation;
using AppMGL.Manager.Infrastructure;
using AppMGL.Manager.Infrastructure.Report;
using Newtonsoft.Json;

using System.Net;
using System.Net.Http;
using AppMGL.DAL.Helper.Logging;
using System.Data.Entity.Infrastructure;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure.Results;

using System.Net.Http.Headers;
using System.Security.Claims;

using AppMGL.Manager.Infrastructure.Helper;
using AppMGL.DTO.Document;
using AppMGL.DTO.Report;
using System.Text;
using Renci.SshNet;
using System.Text.RegularExpressions;
using EAGetMail;
using System.Data;
using ClosedXML.Excel;
using OfficeOpenXml;
using System.Web.Http;

namespace AppMGL.Manager.Areas.Operation.Controllers
{

    public class BookingController : BaseController<BookingListDTO, BookingListRepository, USP_GET_ENQUIRY_LIST_Result>
    {

        string _newFileNo = "";
        #region Constructor


        public BookingController(BookingListRepository context, EnquiryListRepository enquiryListRepository)
        {
            _context = context;
            BaseModule = EnumModule.Booking;
            KeyField = "EnquiryNo";
        }
        public BookingController()
        { }


        #endregion

        #region Public Method

        [System.Web.Http.HttpPost]
        public override ActionResult List(ListParams listParams)
        {
            try
            {
                //EnquiryDetailDTO objEnquiryDetailDTO = new EnquiryDetailDTO();

                //return  enquiryController.GetEnquiryDetail(objEnquiryDetailDTO);
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_APPROVED_QUOT_LIST @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@DASHBOARD_FILTER,  @SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
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
        public ActionResult List_Site(ListParams listParams)
        {
            try
            {
                //EnquiryDetailDTO objEnquiryDetailDTO = new EnquiryDetailDTO();

                //return  enquiryController.GetEnquiryDetail(objEnquiryDetailDTO);
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_APPROVED_QUOT_LIST_Site @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@DASHBOARD_FILTER,  @SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
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
        public virtual ActionResult getBookingDetail(BookingDetailDTO objBooking)
        {
            try
            {
                List<BookingDetailDTO> result = _context.ExecuteQuery<BookingDetailDTO>("EXEC dbo.USP_LG_BOOKING_GET_DETAILS @DOCUMENTCOMMONID,@ENQUIRYID,@QUOTATIONID,@SIT_ID",
                                        new SqlParameter("DOCUMENTCOMMONID", objBooking.DocumentCommonID),
                                        new SqlParameter("ENQUIRYID", objBooking.EnquiryID),
                                        new SqlParameter("QUOTATIONID", objBooking.QuotationID),
                                        new SqlParameter("SIT_ID", objBooking.SiteId)).ToList();
                BookingDetailDTO objBookingDetailDTO = result[0];
                //objBookingDetailDTO.EnquiryID = objBooking.EnquiryID;
                //objBookingDetailDTO.QuotationID = objBooking.QuotationID;

                ////get all Next Action remarks
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                     new SqlParameter("QUOTATIONID", objBookingDetailDTO.QuotationID),
                     new SqlParameter("ENQUIRYID", objBookingDetailDTO.EnquiryID),
                     new SqlParameter("BookingID", objBookingDetailDTO.DocumentCommonID),
                     new SqlParameter("RefType", "Booking")).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                if (RemarksResultList.Count > 0)
                {
                    objBookingDetailDTO.NextActionRemarksDTOList = RemarksResult.ToArray();
                    objBookingDetailDTO.LastRemarks = RemarksResultList[0].Remarks;
                    objBookingDetailDTO.LastRemarkDate = RemarksResultList[0].NextActionDate;
                }
                int RefId = 0;
                string RefType = "";
                if (result[0].DocumentCommonID > 0)
                {
                    RefId = result[0].DocumentCommonID;
                    RefType = "Booking";
                }
                else
                {
                    RefId = Convert.ToInt32(objBookingDetailDTO.EnquiryID);
                    RefType = "Enquiry";
                }
                List<CommodityDTO> CommodityDTOList = _context.ExecuteQuery<CommodityDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_COMMODITY @RefId,@RefType",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType)).ToList();
                if (CommodityDTOList.Count > 0)
                {
                    objBookingDetailDTO.CommodityDTOList = CommodityDTOList.ToArray();
                }
                //get Container details / FCL                                     
                List<EnquiryContainerServiceDTO> ContainerServiceDTOList = _context.ExecuteQuery<EnquiryContainerServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_CONTAINER_LIST @RefId,@RefType",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType)).ToList();
                if (ContainerServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryContainerServiceDTOList = ContainerServiceDTOList.ToArray();
                }
                //get Air
                List<EnquiryAIRServiceDTO> AirServiceDTOList = _context.ExecuteQuery<EnquiryAIRServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_AIR @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 7)).ToList();
                if (AirServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryAIRServiceDTOList = AirServiceDTOList.ToArray();
                }
                //get Break Bulk
                List<EnquiryBreakBulkServiceDTO> BreakBulkServiceDTOList = _context.ExecuteQuery<EnquiryBreakBulkServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_BREAK_BULK @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 5)).ToList();
                if (BreakBulkServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryBreakBulkServiceDTOList = BreakBulkServiceDTOList.ToArray();
                }
                //get LCL
                List<EnquiryLCLServiceDTO> LCLServiceDTOList = _context.ExecuteQuery<EnquiryLCLServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_LCL @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 2)).ToList();
                if (LCLServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryLCLServiceDTOList = LCLServiceDTOList.ToArray();
                }
                //get RORO
                List<EnquiryROROServiceDTO> ROROServiceDTOList = _context.ExecuteQuery<EnquiryROROServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_RORO @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 3)).ToList();
                if (ROROServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryROROServiceDTOList = ROROServiceDTOList.ToArray();
                }

                // Add CarrierCharges by vikas solanki  on 21 june 2020
                //  List<CarrierChargesDTO> carrir = _context.ExecuteQuery<CarrierChargesDTO>("EXEC dbo.USP_LG_BOOKING_GET_CARRIER_RATES @DOCUMENTCOMMONID",
                //                 new SqlParameter("DOCUMENTCOMMONID", objBooking.DocumentCommonID)).ToList();
                List<CarrierChargesDTO> carrir = _context.ExecuteQuery<CarrierChargesDTO>("EXEC dbo.USP_LG_QUOTATION_GET_CARRIER_RATES @QuotationID,@CarrierID",
                                new SqlParameter("QuotationID", objBookingDetailDTO.QuotationID),
                                new SqlParameter("CarrierID", objBookingDetailDTO.ContactID)
                                ).ToList();

                if (carrir.Count > 0)
                {
                    objBookingDetailDTO.CarrierChargesDTOList = carrir.ToArray();
                }
                // end Code 



                return AppResult(objBookingDetailDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


        [System.Web.Http.HttpPost]
        public virtual ActionResult getBookingDetail_CRM(BookingDetailDTO objBooking)
        {
            try
            {
                List<BookingDetailDTO> result = _context.ExecuteQuery<BookingDetailDTO>("EXEC dbo.USP_LG_BOOKING_GET_DETAILS_CRM @DOCUMENTCOMMONID,@ENQUIRYID,@QUOTATIONID,@SIT_ID",
                                        new SqlParameter("DOCUMENTCOMMONID", objBooking.DocumentCommonID),
                                        new SqlParameter("ENQUIRYID", objBooking.EnquiryID),
                                        new SqlParameter("QUOTATIONID", objBooking.QuotationID),
                                        new SqlParameter("SIT_ID", objBooking.SiteId)).ToList();
                BookingDetailDTO objBookingDetailDTO = result[0];
                //objBookingDetailDTO.EnquiryID = objBooking.EnquiryID;
                //objBookingDetailDTO.QuotationID = objBooking.QuotationID;

                ////get all Next Action remarks
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                     new SqlParameter("QUOTATIONID", objBookingDetailDTO.QuotationID),
                     new SqlParameter("ENQUIRYID", objBookingDetailDTO.EnquiryID),
                     new SqlParameter("BookingID", objBookingDetailDTO.DocumentCommonID),
                     new SqlParameter("RefType", "Booking")).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                if (RemarksResultList.Count > 0)
                {
                    objBookingDetailDTO.NextActionRemarksDTOList = RemarksResult.ToArray();
                    objBookingDetailDTO.LastRemarks = RemarksResultList[0].Remarks;
                    objBookingDetailDTO.LastRemarkDate = RemarksResultList[0].NextActionDate;
                }
                int RefId = 0;
                string RefType = "";
                if (result[0].DocumentCommonID > 0)
                {
                    RefId = result[0].DocumentCommonID;
                    RefType = "Booking";
                }
                else
                {
                    RefId = Convert.ToInt32(objBookingDetailDTO.EnquiryID);
                    RefType = "Enquiry";
                }
                List<CommodityDTO> CommodityDTOList = _context.ExecuteQuery<CommodityDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_COMMODITY @RefId,@RefType",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType)).ToList();
                if (CommodityDTOList.Count > 0)
                {
                    objBookingDetailDTO.CommodityDTOList = CommodityDTOList.ToArray();
                }
                //get Container details / FCL                                     
                List<EnquiryContainerServiceDTO> ContainerServiceDTOList = _context.ExecuteQuery<EnquiryContainerServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_CONTAINER_LIST @RefId,@RefType",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType)).ToList();
                if (ContainerServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryContainerServiceDTOList = ContainerServiceDTOList.ToArray();
                }
                //get Air
                List<EnquiryAIRServiceDTO> AirServiceDTOList = _context.ExecuteQuery<EnquiryAIRServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_AIR @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 7)).ToList();
                if (AirServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryAIRServiceDTOList = AirServiceDTOList.ToArray();
                }
                //get Break Bulk
                List<EnquiryBreakBulkServiceDTO> BreakBulkServiceDTOList = _context.ExecuteQuery<EnquiryBreakBulkServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_BREAK_BULK @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 5)).ToList();
                if (BreakBulkServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryBreakBulkServiceDTOList = BreakBulkServiceDTOList.ToArray();
                }
                //get LCL
                List<EnquiryLCLServiceDTO> LCLServiceDTOList = _context.ExecuteQuery<EnquiryLCLServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_LCL @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 2)).ToList();
                if (LCLServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryLCLServiceDTOList = LCLServiceDTOList.ToArray();
                }
                //get RORO
                List<EnquiryROROServiceDTO> ROROServiceDTOList = _context.ExecuteQuery<EnquiryROROServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_RORO @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 3)).ToList();
                if (ROROServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryROROServiceDTOList = ROROServiceDTOList.ToArray();
                }

                // Add CarrierCharges by vikas solanki  on 21 june 2020
                //  List<CarrierChargesDTO> carrir = _context.ExecuteQuery<CarrierChargesDTO>("EXEC dbo.USP_LG_BOOKING_GET_CARRIER_RATES @DOCUMENTCOMMONID",
                //                 new SqlParameter("DOCUMENTCOMMONID", objBooking.DocumentCommonID)).ToList();
                List<CarrierChargesDTO> carrir = _context.ExecuteQuery<CarrierChargesDTO>("EXEC dbo.USP_LG_QUOTATION_GET_CARRIER_RATES @QuotationID,@CarrierID",
                                new SqlParameter("QuotationID", objBookingDetailDTO.QuotationID),
                                new SqlParameter("CarrierID", objBookingDetailDTO.ContactID)
                                ).ToList();

                if (carrir.Count > 0)
                {
                    objBookingDetailDTO.CarrierChargesDTOList = carrir.ToArray();
                }
                // end Code 



                return AppResult(objBookingDetailDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveBookingDetail(BookingDetailDTO objBooking)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_INSERT_UPDATE @DocumentCommonID,@FileNo,@BookingCategory,@QuotationID,@EnquiryID,@DocumentNumber,@ConsigneePONo,@ShipperCustomerName,@BookingDate,@BookingRequestID,@GALBookingType,@DepartmentID,@OriginCountry, @DestinationCountry, @Originport, @DestinationPort, @OriginRailRamp, @DestinationTerminal, @ContractBookingID, @ExpecteddateOfMove,@CustomerID, @OriginAddId, @DestinationAddId, @IsHaz, @Class, @UNNo, @Hazweight, @HazVolume,@PackingType, @hazDescription, @HazRemarks,@ShipperID, @ReqdSailingRemarks, @PickupCategory,@PortcutoffDate,@RampCutOff,@ETA,@ETS,@CutOffDate,@EarliestPickUpDate,@ERD,@AutoCutOff,@BookingReceivedBy,@CommunicationRecdDate,@ModeID,@BookedBy,@Remarks,@ExporterID,@ExportRef,@Vessel,@Voyage,@InvoiceStatusID,@ConsignedTo,@BookingStatus,@CarrierBooking,@GALBookingStatusID,@ETSailing,@CNTNO,@VIN,@ETAFinalplaceid,@Finalplaceid,@SpaceBookingRemarks,@SiteId,@OrignStateID,@DestinationStateID,@OriginCityID,@DestinationCityID, @QuotationSentDate,@NoOfContainer,@TruckCompanyName,@Telno,@FaxNo,@LoadingLocation,@PickupDate,@PickupLocation,@ReturnLocation,@CreatedBy,@UpdatedBy",
                new SqlParameter("DocumentCommonID", objBooking.DocumentCommonID),
                new SqlParameter("FileNo", objBooking.FileNo ?? Convert.DBNull),
                new SqlParameter("BookingCategory", objBooking.BookingCategory ?? Convert.DBNull),
                new SqlParameter("QuotationID", objBooking.QuotationID ?? Convert.DBNull),
                new SqlParameter("EnquiryID", objBooking.EnquiryID ?? Convert.DBNull),
                new SqlParameter("DocumentNumber", objBooking.BookingNo ?? Convert.DBNull),
                new SqlParameter("ConsigneePONo", objBooking.ConsigneePONo ?? Convert.DBNull),
                new SqlParameter("ShipperCustomerName", objBooking.ShipperCustomerName ?? Convert.DBNull),
                new SqlParameter("BookingDate", objBooking.BookingDate ?? Convert.DBNull),
                new SqlParameter("BookingRequestID", objBooking.BookingRequestID ?? Convert.DBNull),
                new SqlParameter("GALBookingType", objBooking.GALBookingType ?? Convert.DBNull),
                new SqlParameter("DepartmentID", objBooking.DepartmentID ?? Convert.DBNull),
                new SqlParameter("OriginCountry", Convert.ToString(objBooking.OriginCountryID) ?? Convert.DBNull),
                new SqlParameter("DestinationCountry", Convert.ToString(objBooking.DestinationCountryID) ?? Convert.DBNull),
                new SqlParameter("Originport", Convert.ToString(objBooking.OriginID) ?? Convert.DBNull),
                new SqlParameter("DestinationPort", Convert.ToString(objBooking.DischargeID) ?? Convert.DBNull),
                new SqlParameter("OriginRailRamp", Convert.ToString(objBooking.OrgnRailRampId) ?? Convert.DBNull),
                new SqlParameter("DestinationTerminal", Convert.ToString(objBooking.DestnTerminalId) ?? Convert.DBNull),
                new SqlParameter("ContractBookingID", objBooking.ContractBookingID ?? Convert.DBNull),
                new SqlParameter("ExpecteddateOfMove", objBooking.ExpecteddateOfMove ?? Convert.DBNull),
                new SqlParameter("CustomerID", objBooking.CustomerID ?? Convert.DBNull),
                new SqlParameter("OriginAddId", objBooking.OriginAddId ?? Convert.DBNull),
                new SqlParameter("DestinationAddId", objBooking.DestinationAddId ?? Convert.DBNull),
                new SqlParameter("IsHaz", objBooking.IsHaz ?? Convert.DBNull),
                new SqlParameter("Class", objBooking.Class ?? Convert.DBNull),
                new SqlParameter("UNNo", objBooking.UNNo ?? Convert.DBNull),
                new SqlParameter("Hazweight", objBooking.Hazweight ?? Convert.DBNull),
                new SqlParameter("HazVolume", objBooking.HazVolume ?? Convert.DBNull),
                new SqlParameter("PackingType", objBooking.PackingType ?? Convert.DBNull),
                new SqlParameter("hazDescription", objBooking.HazDescription ?? Convert.DBNull),
                new SqlParameter("HazRemarks", objBooking.HazRemarks ?? Convert.DBNull),
                new SqlParameter("ShipperID", objBooking.ShipperID ?? Convert.DBNull),
                new SqlParameter("ReqdSailingRemarks", objBooking.ReqdSailingRemarks ?? Convert.DBNull),
                new SqlParameter("PickupCategory", objBooking.PickupCategory ?? Convert.DBNull),
                new SqlParameter("PortcutoffDate", objBooking.PortcutoffDate ?? Convert.DBNull),
                new SqlParameter("RampCutOff", objBooking.RampCutOff ?? Convert.DBNull),
                new SqlParameter("ETA", objBooking.ETA ?? Convert.DBNull),
                new SqlParameter("ETS", objBooking.ETS ?? Convert.DBNull),
                new SqlParameter("CutOffDate", objBooking.CutOffDate ?? Convert.DBNull),
                new SqlParameter("EarliestPickUpDate", objBooking.EarliestPickUpDate ?? Convert.DBNull),
                new SqlParameter("ERD", objBooking.ERD ?? Convert.DBNull),
                new SqlParameter("AutoCutOff", objBooking.AutoCutOff ?? Convert.DBNull),
                 new SqlParameter("BookingReceivedBy", objBooking.BookingReceivedBy ?? Convert.DBNull),
                new SqlParameter("CommunicationRecdDate", objBooking.CommunicationRecdDate ?? Convert.DBNull),
                new SqlParameter("ModeID", objBooking.ModeID ?? Convert.DBNull),
                new SqlParameter("BookedBy", objBooking.BookedBy ?? Convert.DBNull),
                new SqlParameter("Remarks", objBooking.Remarks ?? Convert.DBNull),
                new SqlParameter("ExporterID", objBooking.ExporterID ?? Convert.DBNull),
                new SqlParameter("ExportRef", objBooking.ExportRef ?? Convert.DBNull),
                new SqlParameter("Vessel", objBooking.Vessel ?? Convert.DBNull),
                new SqlParameter("Voyage", objBooking.Voyage ?? Convert.DBNull),
                new SqlParameter("InvoiceStatusID", objBooking.InvoiceStatusID ?? Convert.DBNull),
                new SqlParameter("ConsignedTo", objBooking.ConsignedTo ?? Convert.DBNull),
                new SqlParameter("BookingStatus", objBooking.BookingStatus ?? Convert.DBNull),
                new SqlParameter("CarrierBooking", objBooking.CarrierBooking ?? Convert.DBNull),
                new SqlParameter("GALBookingStatusID", objBooking.GALBookingStatusID ?? Convert.DBNull),
                new SqlParameter("ETSailing", objBooking.ETSailing ?? Convert.DBNull),
                new SqlParameter("CNTNO", objBooking.CNTNO ?? Convert.DBNull),
                new SqlParameter("VIN", objBooking.VIN ?? Convert.DBNull),
                new SqlParameter("ETAFinalplaceid", objBooking.ETAFinalplaceid ?? Convert.DBNull),
                new SqlParameter("Finalplaceid", objBooking.Finalplaceid ?? Convert.DBNull),
                new SqlParameter("SpaceBookingRemarks", objBooking.SpaceBookingRemarks ?? Convert.DBNull),
                new SqlParameter("SiteId", objBooking.SiteId),
                new SqlParameter("OrignStateID", objBooking.OrignStateID ?? Convert.DBNull),
                new SqlParameter("DestinationStateID", objBooking.DestinationStateID ?? Convert.DBNull),
                new SqlParameter("OriginCityID", objBooking.OriginCityID ?? Convert.DBNull),
                new SqlParameter("DestinationCityID", objBooking.DestinationCityID ?? Convert.DBNull),
                new SqlParameter("QuotationSentDate", objBooking.QuotationSentDate ?? Convert.DBNull),
                new SqlParameter("NoOfContainer", objBooking.NoOfContainer ?? Convert.DBNull),
                new SqlParameter("TruckCompanyName", objBooking.TruckCompanyName ?? Convert.DBNull),
                new SqlParameter("Telno", objBooking.Telno ?? Convert.DBNull),
                new SqlParameter("FaxNo", objBooking.FaxNo ?? Convert.DBNull),
                new SqlParameter("LoadingLocation", objBooking.LoadingLocation ?? Convert.DBNull),
                new SqlParameter("PickupDate", objBooking.PickupDate ?? Convert.DBNull),
                new SqlParameter("PickupLocation", objBooking.PickupLocationId ?? Convert.DBNull),
                new SqlParameter("ReturnLocation", objBooking.ReturnLocationId ?? Convert.DBNull),
                new SqlParameter("CreatedBy", objBooking.CreatedBy ?? Convert.DBNull),
                new SqlParameter("UpdatedBy", objBooking.UpdatedBy ?? Convert.DBNull)
                ).ToList();
                List<int> objList = objResult.ToList();
                int DocumentCommonID = objList[0];

                if (objBooking.NextActionRemarks != "" && objBooking.NextActionDueDate != null)
                {
                    IEnumerable<int> objRemarksResult;
                    objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                    new SqlParameter("RefID", DocumentCommonID),
                    new SqlParameter("NextActionDate", objBooking.NextActionDueDate),
                    new SqlParameter("Type", "Booking"),
                    new SqlParameter("ActivityID", 3),
                    new SqlParameter("Remarks", objBooking.NextActionRemarks)).ToList();
                }
                //int EnquiryId = Convert.ToInt32(objBooking.EnquiryID);
                IEnumerable<int> objDeleteResult;
                //delete existing enquiry Commdity details
                objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_COMMODITY_DELETE @RefId,@RefType",
                    new SqlParameter("RefId", DocumentCommonID),
                    new SqlParameter("RefType", "Booking")).ToList();
                if (objBooking.CommodityDTOList.Count() > 0)
                {
                    SaveEnquiryCommodityDetails(objBooking.CommodityDTOList, DocumentCommonID);
                }
                int modeOfService = Convert.ToInt32(objBooking.ModeOfService);
                //delete existing Container Service details
                objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_SERVICE_REF_DELETE @RefId,@RefType",
                    new SqlParameter("RefId", DocumentCommonID),
                    new SqlParameter("RefType", "Booking")).ToList();
                //For containers & FCL = 1
                if (objBooking.EnquiryContainerServiceDTOList.Count() > 0)
                {
                    SaveEnquiryContainerService(objBooking.EnquiryContainerServiceDTOList, DocumentCommonID, modeOfService, objBooking.NoOfContainer);

                    if (modeOfService == 7) //AIR
                    {
                        if (objBooking.EnquiryAIRServiceDTOList.Count() > 0)
                        {
                            SaveEnquiryAIRService(objBooking.EnquiryAIRServiceDTOList, DocumentCommonID, objBooking.NoOfContainer);
                        }
                    }
                    if (modeOfService == 5) //Break Bulk
                    {
                        if (objBooking.EnquiryBreakBulkServiceDTOList.Count() > 0)
                        {
                            SaveEnquiryBreakBulkService(objBooking.EnquiryBreakBulkServiceDTOList, DocumentCommonID, objBooking.NoOfContainer);
                        }
                    }
                    if (modeOfService == 2) //LCL
                    {
                        if (objBooking.EnquiryLCLServiceDTOList.Count() > 0)
                        {
                            SaveEnquiryLCLService(objBooking.EnquiryLCLServiceDTOList, DocumentCommonID, objBooking.NoOfContainer);
                        }
                    }

                    if (modeOfService == 3)  //RORO Service details
                    {
                        if (objBooking.EnquiryROROServiceDTOList.Count() > 0)
                        {
                            SaveEnquiryROROService(objBooking.EnquiryROROServiceDTOList, DocumentCommonID, objBooking.NoOfContainer);
                        }
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
        public ActionResult GetDocumentList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                int GALBookingStatusID = Convert.ToInt32(searchCriteria["galBookingStatusID"]);
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_DOCUMENT_LIST @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@DEPARTMENTID,@GALBookingStatusID,@SORTCOLUMN,@SORTORDER,@DASHBOARD_FILTER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("DEPARTMENTID", searchCriteria["departmentID"]),
                        new SqlParameter("GALBookingStatusID", GALBookingStatusID),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
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
        public virtual ActionResult getDocumentDetail(BookingDetailDTO objBooking)
        {
            try
            {
                List<BookingDetailDTO> result = _context.ExecuteQuery<BookingDetailDTO>("EXEC dbo.USP_LG_BOOKING_GET_DOCUMENT_DETAILS @DOCUMENTCOMMONID,@SIT_ID",
                                        new SqlParameter("DOCUMENTCOMMONID", objBooking.DocumentCommonID),
                                        new SqlParameter("SIT_ID", objBooking.SiteId)).ToList();
                BookingDetailDTO objBookingDetailDTO = result[0];

                //get carrier charges
                List<CarrierChargesDTO> objCarrierChargesDTOList = _context.ExecuteQuery<CarrierChargesDTO>("EXEC dbo.USP_LG_BOOKING_GET_CARRIER_RATES @DOCUMENTCOMMONID",
                                  new SqlParameter("DOCUMENTCOMMONID", objBookingDetailDTO.DocumentCommonID)).ToList();

                if (objCarrierChargesDTOList.Count > 0)
                {
                    objBookingDetailDTO.CarrierChargesDTOList = objCarrierChargesDTOList.ToArray();
                }


                ////get all Next Action remarks
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                     new SqlParameter("QUOTATIONID", objBookingDetailDTO.QuotationID),
                     new SqlParameter("ENQUIRYID", objBookingDetailDTO.EnquiryID),
                     new SqlParameter("BookingID", objBookingDetailDTO.DocumentCommonID),
                     new SqlParameter("RefType", "Booking")).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                if (RemarksResultList.Count > 0)
                {
                    objBookingDetailDTO.NextActionRemarksDTOList = RemarksResult.ToArray();
                    objBookingDetailDTO.LastRemarks = RemarksResultList[0].Remarks;
                    objBookingDetailDTO.LastRemarkDate = RemarksResultList[0].NextActionDate;
                }

                if (objBookingDetailDTO.DocumentCommonID > 0)
                {
                    //get commodity
                    List<CommodityDTO> CommodityDTOList = _context.ExecuteQuery<CommodityDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_COMMODITY @RefId,@RefType",
                                    new SqlParameter("RefId", objBookingDetailDTO.DocumentCommonID),
                                    new SqlParameter("RefType", "Booking")).ToList();
                    if (CommodityDTOList.Count > 0)
                    {
                        objBookingDetailDTO.CommodityDTOList = CommodityDTOList.ToArray();
                    }
                    //get Container details / FCL                                     
                    List<EnquiryContainerServiceDTO> ContainerServiceDTOList = _context.ExecuteQuery<EnquiryContainerServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_CONTAINER_LIST @RefId,@RefType",
                                     new SqlParameter("RefId", objBookingDetailDTO.DocumentCommonID),
                                     new SqlParameter("RefType", "Booking")).ToList();
                    if (ContainerServiceDTOList.Count > 0)
                    {
                        objBookingDetailDTO.EnquiryContainerServiceDTOList = ContainerServiceDTOList.ToArray();
                    }
                    //get Air
                    List<EnquiryAIRServiceDTO> AirServiceDTOList = _context.ExecuteQuery<EnquiryAIRServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_AIR @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", objBookingDetailDTO.DocumentCommonID),
                                    new SqlParameter("RefType", "Booking"),
                                    new SqlParameter("fkServiceRequiredID", 7)).ToList();
                    if (AirServiceDTOList.Count > 0)
                    {
                        objBookingDetailDTO.EnquiryAIRServiceDTOList = AirServiceDTOList.ToArray();
                    }
                    //get Break Bulk
                    List<EnquiryBreakBulkServiceDTO> BreakBulkServiceDTOList = _context.ExecuteQuery<EnquiryBreakBulkServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_BREAK_BULK @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", objBookingDetailDTO.DocumentCommonID),
                                    new SqlParameter("RefType", "Booking"),
                                    new SqlParameter("fkServiceRequiredID", 5)).ToList();
                    if (BreakBulkServiceDTOList.Count > 0)
                    {
                        objBookingDetailDTO.EnquiryBreakBulkServiceDTOList = BreakBulkServiceDTOList.ToArray();
                    }
                    //get LCL
                    List<EnquiryLCLServiceDTO> LCLServiceDTOList = _context.ExecuteQuery<EnquiryLCLServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_LCL @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", objBookingDetailDTO.DocumentCommonID),
                                    new SqlParameter("RefType", "Booking"),
                                    new SqlParameter("fkServiceRequiredID", 2)).ToList();
                    if (LCLServiceDTOList.Count > 0)
                    {
                        objBookingDetailDTO.EnquiryLCLServiceDTOList = LCLServiceDTOList.ToArray();
                    }
                    //get RORO
                    List<EnquiryROROServiceDTO> ROROServiceDTOList = _context.ExecuteQuery<EnquiryROROServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_RORO @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", objBookingDetailDTO.DocumentCommonID),
                                    new SqlParameter("RefType", "Booking"),
                                    new SqlParameter("fkServiceRequiredID", 3)).ToList();
                    if (ROROServiceDTOList.Count > 0)
                    {
                        objBookingDetailDTO.EnquiryROROServiceDTOList = ROROServiceDTOList.ToArray();
                    }
                }
                if (_newFileNo != "") //rollover
                {
                    objBookingDetailDTO.OldBookingID = objBookingDetailDTO.DocumentCommonID;
                    objBookingDetailDTO.DocumentCommonID = 0;
                    objBookingDetailDTO.FileNo = _newFileNo;
                    objBookingDetailDTO.QuotationID = 0;
                    objBookingDetailDTO.QuotationNo = "Direct";
                    objBookingDetailDTO.EnquiryID = 0;
                    objBookingDetailDTO.EnquiryNo = "Direct";
                    objBookingDetailDTO.BookingCategory = 1;

                }
                return AppResult(objBookingDetailDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveDocumentDetail(BookingDetailDTO objBooking)
        {
            try
            {
                //if (!string.IsNullOrEmpty(Convert.ToString( objBooking.DocumentCommonID)))
                //{
                //    List<EnquiryContainerServiceDTO> ContainerServiceDTOList = _context.ExecuteQuery<EnquiryContainerServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_CONTAINER_LIST @RefId,@RefType",
                //                        new SqlParameter("RefId", objBooking.DocumentCommonID),
                //                        new SqlParameter("RefType", "Booking")).ToList();

                //    //List<CarrierChargesDTO> objCarrierChargesDTOList = _context.ExecuteQuery<CarrierChargesDTO>("EXEC dbo.USP_LG_QUOTATION_GET_CARRIER_RATES @QuotationID,@CarrierID",
                //    //              new SqlParameter("QuotationID", objBooking.QuotationID),
                //    //              new SqlParameter("CarrierID", objBooking.CarrierChargesDTOList[0].CarrierID)).ToList();
                //    if (ContainerServiceDTOList.Count > 0)
                //    {
                //        if (objBooking.EnquiryContainerServiceDTOList.Count() > 0)
                //        {
                //            foreach (var item in objBooking.EnquiryContainerServiceDTOList)
                //            {

                //                var objf = ContainerServiceDTOList.Where(x => x.ContainerName == item.ContainerName && x.ContainerTypeID == item.ContainerTypeID).ToList();
                //                if (objf.Count == 0)
                //                {
                //                    string s = item.ContainerName + "Container is mismatch with Quotation";

                //                    return AppResult(null, 0, s, EnumResult.Failed);

                //                }
                //            }

                //            //foreach (var item in objCarrierChargesDTOList)
                //            //{
                //            //    var itm = objCarrierChargesDTOList.Where(x => x.fkContainerID == item.fkContainerID && x.ContainerTypeName == item.ContainerTypeName).ToList();
                //            //    if(itm.Count>0)
                //            //    {
                //            //        item.IsApproved = "1";
                //            //    }
                //            //    else
                //            //    {
                //            //        item.IsApproved = "0";
                //            //    }

                //            //}

                //        }



                       
                //    }
                //}

              

                IEnumerable<int> objResult = null;
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_VALIDATE_FILE_NO @FILE_NO",
                                        new SqlParameter("FILE_NO", objBooking.FileNo)).ToList();
                List<int> objList = objResult.ToList();
                //return AppResult(objList, 1);
                if (objList.Count > 0)
                {
                    if (objList[0] != objBooking.DocumentCommonID)
                    {
                        throw new Exception("This system no is already exsits");
                    }
                }
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_INSERT_UPDATE @DocumentCommonID,@OldBookingID,@FileNo,@BookingCategory,@QuotationID,@EnquiryID,@DocumentNumber,@ConsigneePONo,@ShipperCustomerName,@BookingDate,@BookingRequestID,@GALBookingType,@DepartmentID,@OriginCountry, @DestinationCountry, @Originport, @DestinationPort, @OriginRailRamp, @DestinationTerminal, @ContractBookingID, @ExpecteddateOfMove,@CustomerID, @OriginAddId, @DestinationAddId, @IsHaz, @Class, @UNNo, @Hazweight, @HazVolume,@PackingType, @hazDescription, @HazRemarks,@ShipperID, @ReqdSailingRemarks, @PickupCategory,@PortcutoffDate,@RampCutOff,@ETA,@ETS,@CutOffDate,@EarliestPickUpDate,@ERD,@AutoCutOff,@BookingReceivedBy,@CommunicationRecdDate,@ModeID,@BookedBy,@Remarks,@ExporterID,@ExportRef,@Vessel,@Voyage,@InvoiceStatusID,@ConsignedTo,@BookingStatus,@CarrierBooking,@GALBookingStatusID,@ETSailing,@CNTNO,@VIN,@ETAFinalplaceid,@Finalplaceid,@SpaceBookingRemarks,@SiteId,@OrignStateID,@DestinationStateID,@OriginCityID,@DestinationCityID, @QuotationSentDate,@NoOfContainer,@TruckCompanyName,@Telno,@FaxNo,@LoadingLocation,@PickupDate,@PickupLocation,@ReturnLocation,@CreatedBy,@UpdatedBy,@CustReqCutOffDate,@CustReqSailingDate,@BookingReqtRecivedDate,@bookingRemarks,@Dos_ID,@DocRequired",
                new SqlParameter("DocumentCommonID", objBooking.DocumentCommonID),
                new SqlParameter("OldBookingID", objBooking.OldBookingID ?? Convert.DBNull),
                new SqlParameter("FileNo", objBooking.FileNo ?? Convert.DBNull),
                new SqlParameter("BookingCategory", objBooking.BookingCategory ?? Convert.DBNull),
                new SqlParameter("QuotationID", objBooking.QuotationID ?? Convert.DBNull),
                new SqlParameter("EnquiryID", objBooking.EnquiryID ?? Convert.DBNull),
                new SqlParameter("DocumentNumber", objBooking.BookingNo ?? Convert.DBNull),
                new SqlParameter("ConsigneePONo", objBooking.ConsigneePONo ?? Convert.DBNull),
                new SqlParameter("ShipperCustomerName", objBooking.ShipperCustomerName ?? Convert.DBNull),
                new SqlParameter("BookingDate", objBooking.BookingDate ?? Convert.DBNull),
                new SqlParameter("BookingRequestID", objBooking.BookingRequestID ?? Convert.DBNull),
                new SqlParameter("GALBookingType", objBooking.GALBookingType ?? Convert.DBNull),
                new SqlParameter("DepartmentID", objBooking.DepartmentID ?? Convert.DBNull),
                new SqlParameter("OriginCountry", Convert.ToString(objBooking.OriginCountryID) ?? Convert.DBNull),
                new SqlParameter("DestinationCountry", Convert.ToString(objBooking.DestinationCountryID) ?? Convert.DBNull),
                new SqlParameter("Originport", Convert.ToString(objBooking.OriginID) ?? Convert.DBNull),
                new SqlParameter("DestinationPort", Convert.ToString(objBooking.DischargeID) ?? Convert.DBNull),
                new SqlParameter("OriginRailRamp", Convert.ToString(objBooking.OrgnRailRampId) ?? Convert.DBNull),
                new SqlParameter("DestinationTerminal", Convert.ToString(objBooking.DestnTerminalId) ?? Convert.DBNull),
                new SqlParameter("ContractBookingID", objBooking.ContractBookingID ?? Convert.DBNull),
                new SqlParameter("ExpecteddateOfMove", objBooking.ExpecteddateOfMove ?? Convert.DBNull),
                new SqlParameter("CustomerID", objBooking.CustomerID ?? Convert.DBNull),
                new SqlParameter("OriginAddId", objBooking.OriginAddId ?? Convert.DBNull),
                new SqlParameter("DestinationAddId", objBooking.DestinationAddId ?? Convert.DBNull),
                new SqlParameter("IsHaz", objBooking.IsHaz ?? Convert.DBNull),
                new SqlParameter("Class", objBooking.Class ?? Convert.DBNull),
                new SqlParameter("UNNo", objBooking.UNNo ?? Convert.DBNull),
                new SqlParameter("Hazweight", objBooking.Hazweight ?? Convert.DBNull),
                new SqlParameter("HazVolume", objBooking.HazVolume ?? Convert.DBNull),
                new SqlParameter("PackingType", objBooking.PackingType ?? Convert.DBNull),
                new SqlParameter("hazDescription", objBooking.HazDescription ?? Convert.DBNull),
                new SqlParameter("HazRemarks", objBooking.HazRemarks ?? Convert.DBNull),
                new SqlParameter("ShipperID", objBooking.ContactID),
                new SqlParameter("ReqdSailingRemarks", objBooking.ReqdSailingRemarks ?? Convert.DBNull),
                new SqlParameter("PickupCategory", objBooking.PickupCategory ?? Convert.DBNull),
                new SqlParameter("PortcutoffDate", objBooking.PortcutoffDate ?? Convert.DBNull),
                new SqlParameter("RampCutOff", objBooking.RampCutOff ?? Convert.DBNull),
                new SqlParameter("ETA", objBooking.ETA ?? Convert.DBNull),
                new SqlParameter("ETS", objBooking.ETS ?? Convert.DBNull),
                new SqlParameter("CutOffDate", objBooking.CutOffDate ?? Convert.DBNull),
                new SqlParameter("EarliestPickUpDate", objBooking.EarliestPickUpDate ?? Convert.DBNull),
                new SqlParameter("ERD", objBooking.ERD ?? Convert.DBNull),
                new SqlParameter("AutoCutOff", objBooking.AutoCutOff ?? Convert.DBNull),
                new SqlParameter("BookingReceivedBy", objBooking.BookingReceivedBy ?? Convert.DBNull),
                new SqlParameter("CommunicationRecdDate", objBooking.CommunicationRecdDate ?? Convert.DBNull),
                new SqlParameter("ModeID", objBooking.ModeID ?? Convert.DBNull),
                new SqlParameter("BookedBy", objBooking.BookedBy ?? Convert.DBNull),
                new SqlParameter("Remarks", objBooking.Remarks ?? Convert.DBNull),
                new SqlParameter("ExporterID", objBooking.ExporterID ?? Convert.DBNull),
                new SqlParameter("ExportRef", objBooking.ExportRef ?? Convert.DBNull),
                new SqlParameter("Vessel", objBooking.Vessel ?? Convert.DBNull),
                new SqlParameter("Voyage", objBooking.Voyage ?? Convert.DBNull),
                new SqlParameter("InvoiceStatusID", objBooking.InvoiceStatusID ?? Convert.DBNull),
                new SqlParameter("ConsignedTo", objBooking.ConsignedTo ?? Convert.DBNull),
                new SqlParameter("BookingStatus", objBooking.BookingStatus ?? Convert.DBNull),
                new SqlParameter("CarrierBooking", objBooking.CarrierBooking ?? Convert.DBNull),
                new SqlParameter("GALBookingStatusID", objBooking.GALBookingStatusID ?? Convert.DBNull),
                new SqlParameter("ETSailing", objBooking.ETSailing ?? Convert.DBNull),
                new SqlParameter("CNTNO", objBooking.CNTNO ?? Convert.DBNull),
                new SqlParameter("VIN", objBooking.VIN ?? Convert.DBNull),
                new SqlParameter("ETAFinalplaceid", objBooking.ETAFinalplaceid ?? Convert.DBNull),
                new SqlParameter("Finalplaceid", objBooking.Finalplaceid ?? Convert.DBNull),
                new SqlParameter("SpaceBookingRemarks", objBooking.SpaceBookingRemarks ?? Convert.DBNull),
                new SqlParameter("SiteId", objBooking.SiteId),
                new SqlParameter("OrignStateID", objBooking.OrignStateID ?? Convert.DBNull),
                new SqlParameter("DestinationStateID", objBooking.DestinationStateID ?? Convert.DBNull),
                new SqlParameter("OriginCityID", objBooking.OriginCityID ?? Convert.DBNull),
                new SqlParameter("DestinationCityID", objBooking.DestinationCityID ?? Convert.DBNull),
                new SqlParameter("QuotationSentDate", objBooking.QuotationSentDate ?? Convert.DBNull),
                new SqlParameter("NoOfContainer", objBooking.NoOfContainer ?? Convert.DBNull),
                new SqlParameter("TruckCompanyName", objBooking.TruckCompanyName ?? Convert.DBNull),
                new SqlParameter("Telno", objBooking.Telno ?? Convert.DBNull),
                new SqlParameter("FaxNo", objBooking.FaxNo ?? Convert.DBNull),
                new SqlParameter("LoadingLocation", objBooking.LoadingLocation ?? Convert.DBNull),
                new SqlParameter("PickupDate", objBooking.PickupDate ?? Convert.DBNull),
                new SqlParameter("PickupLocation", objBooking.PickupLocationId ?? Convert.DBNull),
                new SqlParameter("ReturnLocation", objBooking.ReturnLocationId ?? Convert.DBNull),
                new SqlParameter("CreatedBy", objBooking.CreatedBy ?? Convert.DBNull),
                new SqlParameter("UpdatedBy", objBooking.UpdatedBy ?? Convert.DBNull),
                new SqlParameter("CustReqCutOffDate", objBooking.custReqCutOffDate ?? Convert.DBNull),
                new SqlParameter("CustReqSailingDate", objBooking.custReqSailingDate ?? Convert.DBNull),
                new SqlParameter("BookingReqtRecivedDate", objBooking.BookingReqtRecivedDate ?? Convert.DBNull),
                new SqlParameter("bookingRemarks", objBooking.BookingRemarks ?? Convert.DBNull),
                new SqlParameter("Dos_ID", objBooking.Dos_Id ?? Convert.DBNull),
                new SqlParameter("DocRequired", objBooking.DocRequired ?? Convert.DBNull)
                
                ).ToList();
                objList = objResult.ToList();
                int DocumentCommonID = objList[0];

                if (objBooking.NextActionRemarks != "" && objBooking.NextActionDueDate != null)
                {
                    IEnumerable<int> objRemarksResult;
                    objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                    new SqlParameter("RefID", DocumentCommonID),
                    new SqlParameter("NextActionDate", objBooking.NextActionDueDate),
                    new SqlParameter("Type", "Booking"),
                    new SqlParameter("ActivityID", 3),
                    new SqlParameter("Remarks", objBooking.NextActionRemarks)).ToList();
                }
                //int EnquiryId = Convert.ToInt32(objBooking.EnquiryID);
                IEnumerable<int> objDeleteResult;
                //delete existing enquiry Commdity details
                objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_COMMODITY_DELETE @RefId,@RefType",
                    new SqlParameter("RefId", DocumentCommonID),
                    new SqlParameter("RefType", "Booking")).ToList();
                if (objBooking.CommodityDTOList.Count() > 0)
                {
                    SaveEnquiryCommodityDetails(objBooking.CommodityDTOList, DocumentCommonID);
                }
                int modeOfService = Convert.ToInt32(objBooking.ModeOfService);
                //delete existing Container Service details
                objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_SERVICE_REF_DELETE @RefId,@RefType",
                    new SqlParameter("RefId", DocumentCommonID),
                    new SqlParameter("RefType", "Booking")).ToList();
                //For containers & FCL = 1
                if (objBooking.EnquiryContainerServiceDTOList.Count() > 0)
                {
                    SaveEnquiryContainerService(objBooking.EnquiryContainerServiceDTOList, DocumentCommonID, modeOfService, objBooking.NoOfContainer);

                    if (modeOfService == 7) //AIR
                    {
                        if (objBooking.EnquiryAIRServiceDTOList.Count() > 0)
                        {
                            SaveEnquiryAIRService(objBooking.EnquiryAIRServiceDTOList, DocumentCommonID, objBooking.NoOfContainer);
                        }
                    }
                    if (modeOfService == 5) //Break Bulk
                    {
                        if (objBooking.EnquiryBreakBulkServiceDTOList.Count() > 0)
                        {
                            SaveEnquiryBreakBulkService(objBooking.EnquiryBreakBulkServiceDTOList, DocumentCommonID, objBooking.NoOfContainer);
                        }
                    }
                    if (modeOfService == 2) //LCL
                    {
                        if (objBooking.EnquiryLCLServiceDTOList.Count() > 0)
                        {
                            SaveEnquiryLCLService(objBooking.EnquiryLCLServiceDTOList, DocumentCommonID, objBooking.NoOfContainer);
                        }
                    }

                    if (modeOfService == 3)  //RORO Service details
                    {
                        if (objBooking.EnquiryROROServiceDTOList.Count() > 0)
                        {
                            SaveEnquiryROROService(objBooking.EnquiryROROServiceDTOList, DocumentCommonID, objBooking.NoOfContainer);
                        }
                    }
                }
                //Create Shipper Report
                //AsyncHelper.RunSync(() => this.createShipperReports();
                //var user = AsyncHelper.RunSync<bool>(() => this.CreateShipperReport(DocumentCommonID));

                var uploadFolder = ConfigurationManager.AppSettings["ShipperDocPath"];
                string root = HttpContext.Current.Server.MapPath(uploadFolder);

                Task<bool> task = Task.Run<bool>(async () => await this.CreateShipperReport(DocumentCommonID, root));
                //CreateShipperReport(DocumentCommonID);
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult GetLatestBookingNo(BookingDetailDTO objBookingDetailDTO)
        {
            try
            {
                //get Booking No
                BookingListDTO objBookingDTO = new BookingListDTO();
                IEnumerable<string> ResultList = _context.ExecuteQuery<string>("EXEC dbo.USP_LG_BOOKING_GET_FILE_NO @SITE_ID,@DepartmentID",
                    new SqlParameter("SITE_ID", objBookingDetailDTO.SiteId),
                    new SqlParameter("DepartmentID", objBookingDetailDTO.DepartmentID)).ToList();
                List<string> bookingNoResult = ResultList.ToList();
                if (bookingNoResult.Count > 0)
                {
                    objBookingDTO.FileNo = bookingNoResult[0].ToString();
                }
                return AppResult(objBookingDTO, 1, "", EnumResult.Success);

            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult ValidateBookingFileNo(string id)
        {
            try
            {
                IEnumerable<int> objResult = null;
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_VALIDATE_FILE_NO @FILE_NO",
                                        new SqlParameter("FILE_NO", id)).ToList();
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult DeleteQuotation(QuotationListDTO objQuotationListDTO)
        {
            try
            {
                IEnumerable<int> objResult = null;
                if (objQuotationListDTO.QuotationID > 0)
                {
                    //objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_DELETE @ID, @Remarks",
                    //                    new SqlParameter("ID", objQuotationListDTO.QuotationID),
                    //                new SqlParameter("Remarks", objQuotationListDTO.Remarks)).ToList();
                    objQuotationListDTO.IsCancelled = "Yes";
                    objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_DELETE @ID, @IsCancelled,@Remarks",
                                        new SqlParameter("ID", objQuotationListDTO.QuotationID),
                                        new SqlParameter("IsCancelled", objQuotationListDTO.IsCancelled),
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
        public virtual ActionResult DeleteBooking(BookingListDTO objBookingListDTO)
        {
            try
            {
                IEnumerable<int> objResult = null;
                if (objBookingListDTO.DocumentCommonID > 0)
                {
                    objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_DELETE @ID",
                                        new SqlParameter("ID", objBookingListDTO.DocumentCommonID)).ToList();
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
        public virtual ActionResult CancelBooking(BookingListDTO objBookingListDTO)
        {
            try
            {
                IEnumerable<int> objResult = null;
                if (objBookingListDTO.DocumentCommonID > 0)
                {
                    objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_CANCEL @ID, @Remarks,@NextActionDueDate",
                                        new SqlParameter("ID", objBookingListDTO.DocumentCommonID),
                                        new SqlParameter("Remarks", objBookingListDTO.Remarks),
                                        new SqlParameter("NextActionDueDate", objBookingListDTO.NextActionDueDate)).ToList();
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
        public virtual ActionResult RollOverBooking(BookingListDTO objBookingListDTO)
        {
            try
            {
                IEnumerable<int> objResult = null;
                if (objBookingListDTO.DocumentCommonID > 0)
                {
                    objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_ROLLED_OVER @DOCUMENTCOMMONID, @Remarks,@NextActionDueDate",
                                        new SqlParameter("DOCUMENTCOMMONID", objBookingListDTO.DocumentCommonID),
                                        new SqlParameter("Remarks", objBookingListDTO.Remarks),
                                        new SqlParameter("NextActionDueDate", objBookingListDTO.NextActionDueDate)).ToList();
                }
                List<int> objList = objResult.ToList();
                if (objList[0] > 0)
                {
                    _newFileNo = objList[0].ToString();

                    BookingDetailDTO objBooking = new BookingDetailDTO();
                    objBooking.DocumentCommonID = objBookingListDTO.DocumentCommonID;
                    objBooking.SiteId = 0;
                    return getDocumentDetail(objBooking);
                }
                else
                {
                    return AppResult(objList, 1);
                }
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        public void SaveEnquiryCommodityDetails(CommodityDTO[] objCommodityDTOList, int DocumentCommonID)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (CommodityDTO objCommodityDTO in objCommodityDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_COMMODITY_INSERT @fkCommodityId,@RefId,@RefType",
                    new SqlParameter("fkCommodityId", objCommodityDTO.CommodityId),
                    new SqlParameter("RefId", DocumentCommonID),
                    new SqlParameter("RefType", "Booking")).ToList();
            }
        }

        public void SaveEnquiryContainerService(EnquiryContainerServiceDTO[] EnquiryContainerServiceDTOList, int DocumentCommonID, int modeOfService, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryContainerServiceDTO objContainerServiceDTO in EnquiryContainerServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_FCL_INSERT @ServiceRequiredID,@NoofContainer,@ContainerSizeID,@QTY,@RefId,@RefType",
                    new SqlParameter("ServiceRequiredID", modeOfService),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("ContainerSizeID", objContainerServiceDTO.ContainerTypeID),
                    new SqlParameter("QTY", objContainerServiceDTO.Quantity),
                    new SqlParameter("RefId", DocumentCommonID),
                    new SqlParameter("RefType", "Booking")).ToList();
            }
        }

        public void SaveEnquiryAIRService(EnquiryAIRServiceDTO[] EnquiryAIRServiceDTOList, int DocumentCommonID, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryAIRServiceDTO objAIRService in EnquiryAIRServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_AIR_INSERT @ServiceRequiredID,@NoofContainer,@PieceCount,@Weight,@TotalWt,@Length,@Width,@Height,@Volume,@RefId,@RefType",
                    new SqlParameter("ServiceRequiredID", 7),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("PieceCount", objAIRService.PieceCount),
                    new SqlParameter("Weight", objAIRService.Weight),
                    new SqlParameter("TotalWt", objAIRService.TotalWt),
                    new SqlParameter("Length", objAIRService.Length),
                    new SqlParameter("Width", objAIRService.Width),
                    new SqlParameter("Height", objAIRService.Height),
                    new SqlParameter("Volume", objAIRService.Volume),
                    new SqlParameter("RefId", DocumentCommonID),
                    new SqlParameter("RefType", "Booking")).ToList();
            }

        }

        public void SaveEnquiryBreakBulkService(EnquiryBreakBulkServiceDTO[] EnquiryBreakBulkServiceDTOList, int DocumentCommonID, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryBreakBulkServiceDTO objBBulkService in EnquiryBreakBulkServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_BREAK_BULK_INSERT @ServiceRequiredID,@NoofContainer,@RefId,@RefType,@Weight,@Length,@Width,@Height,@Volume,@Description,@Qty,@ChkInland,@InlandRemarks,@MafiCharges",
                    new SqlParameter("ServiceRequiredID", 5),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("RefId", DocumentCommonID),
                    new SqlParameter("RefType", "Booking"),
                    new SqlParameter("Weight", objBBulkService.Weight),
                    new SqlParameter("Length", objBBulkService.Length),
                    new SqlParameter("Width", objBBulkService.Width),
                    new SqlParameter("Height", objBBulkService.Height),
                    new SqlParameter("Volume", objBBulkService.Volume ?? Convert.DBNull),
                    new SqlParameter("Description", objBBulkService.Description ?? Convert.DBNull),
                    new SqlParameter("Qty", objBBulkService.Qty),
                    new SqlParameter("ChkInland", objBBulkService.ChkInland ?? Convert.DBNull),
                    new SqlParameter("InlandRemarks", objBBulkService.InlandRemarks ?? Convert.DBNull),
                    new SqlParameter("MafiCharges", objBBulkService.MafiCharges ?? Convert.DBNull)).ToList();
            }
        }
        public void SaveEnquiryLCLService(EnquiryLCLServiceDTO[] EnquiryLCLServiceDTOList, int DocumentCommonID, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryLCLServiceDTO objLCLService in EnquiryLCLServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_LCL_INSERT @ServiceRequiredID,@NoofContainer,@PieceCount,@Weight,@TotalWt,@Length,@Width,@Height,@Volume,@RefId,@RefType",
                    new SqlParameter("ServiceRequiredID", 2),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("PieceCount", objLCLService.PieceCount),
                    new SqlParameter("Weight", objLCLService.Weight),
                    new SqlParameter("TotalWt", objLCLService.TotalWt),
                    new SqlParameter("Length", objLCLService.Length),
                    new SqlParameter("Width", objLCLService.Width),
                    new SqlParameter("Height", objLCLService.Height),
                    new SqlParameter("Volume", objLCLService.Volume ?? Convert.DBNull),
                    new SqlParameter("RefId", DocumentCommonID),
                    new SqlParameter("RefType", "Booking")).ToList();
            }

        }
        public void SaveEnquiryROROService(EnquiryROROServiceDTO[] EnquiryROROServiceDTOList, int DocumentCommonID, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult1;
            foreach (EnquiryROROServiceDTO objROROService in EnquiryROROServiceDTOList)
            {
                objResult1 = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_RORO_INSERT @ServiceRequiredID,@NoofContainer,@RefId,@RefType,@NoofCrain,@Weight,@Length,@Width,@Volume,@Height,@ChkInland,@InlandRemarks,@MafiCharges",
                    new SqlParameter("ServiceRequiredID", 3),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("RefId", DocumentCommonID),
                    new SqlParameter("RefType", "Booking"),
                    new SqlParameter("NoofCrain", objROROService.NoofCrain ?? Convert.DBNull),
                    new SqlParameter("Weight", objROROService.Weight),
                    new SqlParameter("Length", objROROService.Length),
                    new SqlParameter("Width", objROROService.Width),
                    new SqlParameter("Volume", objROROService.Volume),
                    new SqlParameter("Height", objROROService.Height),
                    new SqlParameter("ChkInland", objROROService.ChkInland ?? Convert.DBNull),
                    new SqlParameter("InlandRemarks", objROROService.InlandRemarks ?? Convert.DBNull),
                    new SqlParameter("MafiCharges", objROROService.MafiCharges ?? Convert.DBNull)).ToList();
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult showDocumnetExpenseDetail(BookingListDTO objBooking)
        {
            try
            {
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_EXPENSE_DATA @OPTIONVALUE,@SEARCHVALUE,@SIT_ID",
                                        new SqlParameter("OPTIONVALUE", objBooking.OptionVale),
                                        new SqlParameter("SEARCHVALUE", objBooking.SearchVale),
                                        new SqlParameter("SIT_ID", objBooking.SiteId)).ToList();
                BookingListDTO objBookingListDTO = result[0];

                ////get Booking Expense details
                IEnumerable<ExpenseDetailDTO> ExpenseResult = _context.ExecuteQuery<ExpenseDetailDTO>("EXEC dbo.USP_LG_BOOKING_GET_EXPENSE_DETAIL @DocumentCommonID",
                     new SqlParameter("DocumentCommonID", objBookingListDTO.DocumentCommonID)).ToList();
                List<ExpenseDetailDTO> ExpenseDetailDTOList = ExpenseResult.ToList();

                if (ExpenseDetailDTOList.Count > 0)
                {
                    objBookingListDTO.ExpenseDetailDTOList = ExpenseResult.ToArray();
                }
                return AppResult(objBookingListDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetExpenseHeads()
        {
            try
            {
                var result = _context.ExecuteQuery<ExpenseHeadDTO>("EXEC dbo.USP_LG_BOOKING_GET_ALL_EXPENSE_HEAD").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveBookingExpenses(BookingListDTO objBookingListDTO)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_UPDATE_INVOICE_STATUS @FileNo",
                new SqlParameter("FileNo", objBookingListDTO.FileNo)).ToList();
                List<int> objList = objResult.ToList();
                int DocumentCommonID = objList[0];
                IEnumerable<int> objDeleteResult;
                //delete existing Booking Expense details
                objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_DELETE_EXPENSE @DocumentCommonID",
                    new SqlParameter("DocumentCommonID", DocumentCommonID)).ToList();
                //insert Booking Expense details
                IEnumerable<int> objExpenseResult;
                if (objBookingListDTO.ExpenseDetailDTOList != null)
                {
                    foreach (ExpenseDetailDTO objExpenseDetailDTO in objBookingListDTO.ExpenseDetailDTOList)
                    {
                        objExpenseResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_EXPENSE_INSERT @DocumentCommonID,@VendorName,@DocNo,@DocDate,@SellingAmount,@BuyingAmount,@ExpenseHeadID,@Narration,@Remarks,@IsActive",
                            new SqlParameter("DocumentCommonID", DocumentCommonID),
                            new SqlParameter("VendorName", objExpenseDetailDTO.VendorName ?? Convert.DBNull),
                            new SqlParameter("DocNo", objExpenseDetailDTO.DocNo ?? Convert.DBNull),
                            new SqlParameter("DocDate", objExpenseDetailDTO.DocDate ?? Convert.DBNull),
                            new SqlParameter("SellingAmount", objExpenseDetailDTO.SellingAmount ?? Convert.DBNull),
                            new SqlParameter("BuyingAmount", objExpenseDetailDTO.BuyingAmount ?? Convert.DBNull),
                            new SqlParameter("ExpenseHeadID", objExpenseDetailDTO.ExpenseHeadID ?? Convert.DBNull),
                            new SqlParameter("Narration", objExpenseDetailDTO.Narration ?? Convert.DBNull),
                            new SqlParameter("Remarks", objExpenseDetailDTO.Remarks ?? Convert.DBNull),
                            new SqlParameter("IsActive", objExpenseDetailDTO.IsActive)).ToList();
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
        public ActionResult GetExpensesReportList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<ExpenseDetailDTO> result = _context.ExecuteQuery<ExpenseDetailDTO>("EXEC dbo.USP_LG_BOOKING_GET_EXPENSES_REPORT_LIST @PAGENO, @PAGESIZE,@OPTIONDATEVALUE,@FROMDATE,@TODATE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONDATEVALUE", searchCriteria["optionDateValue"]),
                        new SqlParameter("FROMDATE", searchCriteria["fromDate"]),
                        new SqlParameter("TODATE", searchCriteria["toDate"]),
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
        public virtual ActionResult GetExpensesReportDetails(BookingListDTO objBooking)
        {
            try
            {
                int TotalRows = 0;
                List<ExpenseDetailDTO> result = _context.ExecuteQuery<ExpenseDetailDTO>("EXEC dbo.USP_LG_BOOKING_GET_EXPENSES_REPORT_DETAILS @DOCUMENTCOMMONID,@SIT_ID",
                                        new SqlParameter("DOCUMENTCOMMONID", objBooking.DocumentCommonID),
                                        new SqlParameter("SIT_ID", objBooking.SiteId)).ToList();
                return AppResult(result, TotalRows);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage ExportExpensesReport(Dictionary<string, string> exportParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/ExpensesReport";

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();

                reportParams.Add(new KeyValuePair<string, string>("OPTIONDATEVALUE", exportParams["optionDateValue"]));
                reportParams.Add(new KeyValuePair<string, string>("FROMDATE", exportParams["fromDate"]));
                reportParams.Add(new KeyValuePair<string, string>("TODATE", exportParams["toDate"]));
                reportParams.Add(new KeyValuePair<string, string>("OPTIONVALUE", exportParams["optionValue"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHVALUE", exportParams["seachValue"]));
                reportParams.Add(new KeyValuePair<string, string>("SIT_ID", exportParams["SitId"]));

                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = exportParams["SitId"].ToString() + "_ExpensesReport.xlsx";

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
        public HttpResponseMessage ExportActivityDueReport(Dictionary<string, string> exportParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/ActivityDueReport";

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();

                reportParams.Add(new KeyValuePair<string, string>("REPORTTYPE", exportParams["reportType"]));
                reportParams.Add(new KeyValuePair<string, string>("FROMDATE", exportParams["fromDate"]));
                reportParams.Add(new KeyValuePair<string, string>("TODATE", exportParams["toDate"]));
                reportParams.Add(new KeyValuePair<string, string>("SIT_ID", exportParams["SitId"]));

                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = exportParams["SitId"].ToString() + "_ActivityDueReport.xlsx";

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
        public ActionResult GetActivityDueReportList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<NextActionRemarksDTO> result = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_BOOKING_ACTIVITY_DUE_REPORT_LIST @PAGENO, @PAGESIZE,@REPORTTYPE,@FROMDATE,@TODATE,@SORTCOLUMN,@SORTORDER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("REPORTTYPE", searchCriteria["reportType"]),
                        new SqlParameter("FROMDATE", searchCriteria["fromDate"]),
                        new SqlParameter("TODATE", searchCriteria["toDate"]),
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
        public ActionResult GetDispatchContainerList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_CAPTURE_CONTAINER_LIST @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@DEPARTMENTID,@SORTCOLUMN,@SORTORDER,@DASHBOARD_FILTER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("DEPARTMENTID", searchCriteria["departmentID"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
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
        public virtual ActionResult GetDispatchContainerDetail(BookingListDTO objBooking)
        {
            try
            {
                BookingListDTO objBookingListDTO = new BookingListDTO();
                objBookingListDTO.DocumentCommonID = objBooking.DocumentCommonID;
                objBookingListDTO.FileNo = objBooking.FileNo;
                objBookingListDTO.ExportRef = objBooking.ExportRef;
                objBookingListDTO.BLNumber = objBooking.BLNumber;

                IEnumerable<DispatchContainerDTO> ContainerResult = _context.ExecuteQuery<DispatchContainerDTO>("EXEC dbo.USP_LG_BOOKING_GET_CONTAINER_DISPATCH_TABLE @CommonID",
                                        new SqlParameter("CommonID", objBooking.DocumentCommonID)).ToList();
                List<DispatchContainerDTO> DispatchContainerDTOList = ContainerResult.ToList();
                if (DispatchContainerDTOList.Count > 0)
                {
                    objBookingListDTO.DispatchContainerDTOList = DispatchContainerDTOList.ToArray();
                }
                IEnumerable<DispatchContainerDTO> DispatchTableResult = _context.ExecuteQuery<DispatchContainerDTO>("EXEC dbo.USP_LG_BOOKING_GET_DISPATCH_TABLE_DATA @CommonID",
                                        new SqlParameter("CommonID", objBooking.DocumentCommonID)).ToList();
                List<DispatchContainerDTO> DispatchTableList = DispatchTableResult.ToList();
                if (DispatchContainerDTOList.Count > 0 && DispatchTableList.Count > 0)
                {
                    foreach (DispatchContainerDTO ContainerItem in objBookingListDTO.DispatchContainerDTOList)
                    {
                        foreach (DispatchContainerDTO DispatchItem in DispatchTableList)
                        {
                            if (DispatchItem.ContainerID == ContainerItem.ContainerID && DispatchItem.SeqNo == ContainerItem.SeqNo)
                            {
                                ContainerItem.CNTNo = DispatchItem.CNTNo;
                                ContainerItem.SealNo = DispatchItem.SealNo;
                                ContainerItem.SeqNo = DispatchItem.SeqNo;
                                ContainerItem.DispatchRemarks = DispatchItem.DispatchRemarks;
                                ContainerItem.IsSelected = true;
                            }
                        }
                    }
                }

                if (DispatchContainerDTOList.Count > 0)
                {
                    foreach (var item in DispatchContainerDTOList)
                    {
                        item.ContainerOwnerName = "LINE";
                    }
                    objBookingListDTO.DispatchContainerDTOList = DispatchContainerDTOList.ToArray();
                }
                ////get all Next Action remarks
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                     new SqlParameter("QUOTATIONID", objBookingListDTO.QuotationID),
                     new SqlParameter("ENQUIRYID", objBookingListDTO.EnquiryID),
                     new SqlParameter("BookingID", objBookingListDTO.DocumentCommonID),
                     new SqlParameter("RefType", "Booking")).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                if (RemarksResultList.Count > 0)
                {
                    objBookingListDTO.NextActionRemarksDTOList = RemarksResult.ToArray();
                    objBookingListDTO.LastRemarks = RemarksResultList[0].Remarks;
                    objBookingListDTO.LastRemarkDate = RemarksResultList[0].NextActionDate;
                }

                return AppResult(objBookingListDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveDispatchContainerData(BookingListDTO objBookingListDTO)
        {
            try
            {
                //delete existing dispatch table & update BLNo
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_DISPATCH_CONTAINER_DELETE @CommonID,@BLNO",
                new SqlParameter("CommonID", objBookingListDTO.DocumentCommonID),
                new SqlParameter("BLNO", objBookingListDTO.BLNumber ?? Convert.DBNull)).ToList();
                List<int> objList = objResult.ToList();

                //insert Dispatch Container details
                IEnumerable<int> objDispatchResult;
                bool status = true;
                foreach (DispatchContainerDTO objDispatchContainerDTO in objBookingListDTO.DispatchContainerDTOList)
                {
                    if (string.IsNullOrEmpty(objDispatchContainerDTO.CNTNo) || string.IsNullOrEmpty(objDispatchContainerDTO.SealNo))
                    {
                        status = false;
                    }
                    objDispatchResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_DISPATCH_CONTAINER_INSERT @CommonID,@SealNo, @CNTNo, @fkServiceRefID, @fkContainerTypeID, @DispatchRemarks, @SeqNo,@ContainerOwner",
                        new SqlParameter("CommonID", objBookingListDTO.DocumentCommonID),
                        new SqlParameter("SealNo", objDispatchContainerDTO.SealNo ?? Convert.DBNull),
                        new SqlParameter("CNTNo", objDispatchContainerDTO.CNTNo ?? Convert.DBNull),
                        new SqlParameter("fkServiceRefID", objDispatchContainerDTO.ServiceReqdID ?? Convert.DBNull),
                        new SqlParameter("fkContainerTypeID", objDispatchContainerDTO.ContainerID ?? Convert.DBNull),
                        new SqlParameter("DispatchRemarks", objDispatchContainerDTO.DispatchRemarks ?? Convert.DBNull),
                        new SqlParameter("SeqNo", objDispatchContainerDTO.SeqNo ?? Convert.DBNull),
                        new SqlParameter("ContainerOwner", objDispatchContainerDTO.ContainerOwnerName ?? Convert.DBNull)).ToList();
                }//update status
                if (status == true)
                {
                    IEnumerable<int> objResult1 = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_CONTAINER_INSERT_STATUS @CommonID",
                    new SqlParameter("CommonID", objBookingListDTO.DocumentCommonID)).ToList();
                    List<int> objList1 = objResult.ToList();
                }
                //insert Next Action remarks
                if (objBookingListDTO.NextActionRemarks != "" && objBookingListDTO.NextActionDueDate != null)
                {
                    IEnumerable<int> objRemarksResult;
                    objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                    new SqlParameter("RefID", objBookingListDTO.DocumentCommonID),
                    new SqlParameter("NextActionDate", objBookingListDTO.NextActionDueDate),
                    new SqlParameter("Type", "Trucking"),
                    new SqlParameter("ActivityID", 11),
                    new SqlParameter("Remarks", objBookingListDTO.NextActionRemarks)).ToList();
                }
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }

        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveDispatchContainerDataAbandonement(BookingListDTO objBookingListDTO)
        {
            try
            {
                ////delete existing dispatch table & update BLNo
                //IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_DISPATCH_CONTAINER_DELETE @CommonID,@BLNO",
                //new SqlParameter("CommonID", objBookingListDTO.DocumentCommonID),
                //new SqlParameter("BLNO", objBookingListDTO.BLNumber ?? Convert.DBNull)).ToList();
                List<int> objList = new List<int>();

                ////insert Dispatch Container details
                IEnumerable<int> objDispatchResult;
                bool status = true;
                foreach (DispatchContainerDTO objDispatchContainerDTO in objBookingListDTO.DispatchContainerDTOList)
                {
                    if (string.IsNullOrEmpty(objDispatchContainerDTO.CNTNo) || string.IsNullOrEmpty(objDispatchContainerDTO.SealNo))
                    {
                        status = false;
                    }
                    objDispatchResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_DISPATCH_CONTAINER_INSERT_UDATE_Abandonement @CommonID,@SealNo, @CNTNo, @fkServiceRefID, @fkContainerTypeID, @DispatchRemarks, @SeqNo,@ContainerOwner,@Destuffed,@DateofDestuffed",
                        new SqlParameter("CommonID", objBookingListDTO.DocumentCommonID),
                        new SqlParameter("SealNo", objDispatchContainerDTO.SealNo ?? Convert.DBNull),
                        new SqlParameter("CNTNo", objDispatchContainerDTO.CNTNo ?? Convert.DBNull),
                        new SqlParameter("fkServiceRefID", objDispatchContainerDTO.ServiceReqdID ?? Convert.DBNull),
                        new SqlParameter("fkContainerTypeID", objDispatchContainerDTO.ContainerID ?? Convert.DBNull),
                        new SqlParameter("DispatchRemarks", objDispatchContainerDTO.DispatchRemarks ?? Convert.DBNull),
                        new SqlParameter("SeqNo", objDispatchContainerDTO.SeqNo ?? Convert.DBNull),
                        new SqlParameter("ContainerOwner", objDispatchContainerDTO.ContainerOwnerName ?? Convert.DBNull),
                         new SqlParameter("Destuffed", objDispatchContainerDTO.Destuffed ?? Convert.DBNull),
                          new SqlParameter("DateofDestuffed", objDispatchContainerDTO.DateofDestuffed ?? Convert.DBNull)).ToList();
                }//update status
                if (status == true)
                {
                  //  IEnumerable<int> objResult1 = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_CONTAINER_INSERT_STATUS @CommonID",
                  //  new SqlParameter("CommonID", objBookingListDTO.DocumentCommonID)).ToList();
                  //  List<int> objList1 = objResult.ToList();
                }
                //insert Next Action remarks
                //if (objBookingListDTO.NextActionRemarks != "" && objBookingListDTO.NextActionDueDate != null)
                //{
                //    IEnumerable<int> objRemarksResult;
                //    objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                //    new SqlParameter("RefID", objBookingListDTO.DocumentCommonID),
                //    new SqlParameter("NextActionDate", objBookingListDTO.NextActionDueDate),
                //    new SqlParameter("Type", "Trucking"),
                //    new SqlParameter("ActivityID", 11),
                //    new SqlParameter("Remarks", objBookingListDTO.NextActionRemarks)).ToList();
                //}
                return AppResult(1, "Sucessfuly saved.");
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }

        }





        [System.Web.Http.HttpPost]
        public virtual ActionResult GetDocumentAttachmentDetail(BookingListDTO objBooking)
        {
            try
            {
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_DOCUMENT_ATTACHMENT_DETAIL @CommonID",
                                        new SqlParameter("CommonID", objBooking.DocumentCommonID)).ToList();
                BookingListDTO objBookingListDTO = new BookingListDTO();
                objBookingListDTO = result[0];

                //get all Shipment Docs details
                IEnumerable<ShipmentDocsDTO> DocsResult = _context.ExecuteQuery<ShipmentDocsDTO>("EXEC dbo.USP_LG_BOOKING_GET_SHIPMENT_DOCS @DocumentCommonID",
                     new SqlParameter("DocumentCommonID", objBooking.DocumentCommonID)).ToList();
                List<ShipmentDocsDTO> ShipmentDocsResultList = DocsResult.ToList();

                if (ShipmentDocsResultList.Count > 0)
                {
                    objBookingListDTO.ShipmentDocsDTOList = ShipmentDocsResultList.ToArray();
                }

                ////get all Next Action remarks
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                     new SqlParameter("QUOTATIONID", objBooking.QuotationID),
                     new SqlParameter("ENQUIRYID", objBooking.EnquiryID),
                     new SqlParameter("BookingID", objBooking.DocumentCommonID),
                     new SqlParameter("RefType", "Booking")).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                if (RemarksResultList.Count > 0)
                {
                    objBookingListDTO.NextActionRemarksDTOList = RemarksResult.ToArray();
                    objBookingListDTO.LastRemarks = RemarksResultList[0].Remarks;
                    objBookingListDTO.LastRemarkDate = RemarksResultList[0].NextActionDate;
                }

                return AppResult(objBookingListDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult GetCustomerDocumentDetails(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                //var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);

                BookingListDTO objBookingListDTO = new BookingListDTO();
                //get all Shipment Docs details
                IEnumerable<ShipmentDocsDTO> DocsResult = _context.ExecuteQuery<ShipmentDocsDTO>("EXEC dbo.USP_LG_BOOKING_GET_CUSTOMER_DOCS @DocumentCommonID,@DocumentType",
                     new SqlParameter("DocumentCommonID", searchCriteria["documentCommonID"]),
                     new SqlParameter("DocumentType", searchCriteria["documentType"])
                     ).ToList();
                List<ShipmentDocsDTO> ShipmentDocsResultList = DocsResult.ToList();

                if (ShipmentDocsResultList.Count > 0)
                {
                    objBookingListDTO.ShipmentDocsDTOList = ShipmentDocsResultList.ToArray();
                }


                return AppResult(objBookingListDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult GetAllActionRemarks(BookingListDTO objBooking)
        {
            try
            {
                BookingListDTO objBookingListDTO = new BookingListDTO();

                ////get all Next Action remarks
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_BOOKING_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID",
                     new SqlParameter("QUOTATIONID", objBooking.QuotationID),
                     new SqlParameter("ENQUIRYID", objBooking.EnquiryID),
                     new SqlParameter("BookingID", objBooking.DocumentCommonID)
                     ).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                if (RemarksResultList.Count > 0)
                {
                    objBookingListDTO.NextActionRemarksDTOList = RemarksResult.ToArray();
                    objBookingListDTO.LastRemarks = RemarksResultList[0].Remarks;
                    objBookingListDTO.LastRemarkDate = RemarksResultList[0].NextActionDate;
                }

                return AppResult(objBookingListDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveCustomerDocAttachements(BookingListDTO objBookingListDTO)
        {
            try
            {
                //insert shipment doc file names
                IEnumerable<int> objDcosResult = new List<int>();
                if (objBookingListDTO.ShipmentDocsDTOList != null)
                {
                    foreach (ShipmentDocsDTO objShipmentDocsDTO in objBookingListDTO.ShipmentDocsDTOList)
                    {
                        objDcosResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_SHIPMENT_DOC_INSERT @DocumentCommonID,@DocName,@DocType,@CreatedBy,@IsSysGenerated",
                            new SqlParameter("DocumentCommonID", objBookingListDTO.DocumentCommonID),
                            new SqlParameter("DocName", objShipmentDocsDTO.DocName),
                            new SqlParameter("DocType", objBookingListDTO.DocumentType),
                            new SqlParameter("CreatedBy", objBookingListDTO.CreatedBy),
                            new SqlParameter("IsSysGenerated", false)).ToList();
                    }
                }
                List<int> objList = objDcosResult.ToList();
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }

        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveDocumentAttachement(BookingListDTO objBookingListDTO)
        {
            try
            {
                //Update Document details
                IEnumerable<int> objResult;
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_DOCUMENT_ATTACHMENT_UPDATE @CommonID,@ExportRef, @Vessel, @Voyage,@IsPublic,@CutOffDate",
                        new SqlParameter("CommonID", objBookingListDTO.DocumentCommonID),
                        new SqlParameter("ExportRef", objBookingListDTO.ExportRef ?? Convert.DBNull),
                        new SqlParameter("Vessel", objBookingListDTO.Vessel ?? Convert.DBNull),
                        new SqlParameter("Voyage", objBookingListDTO.Voyage ?? Convert.DBNull),
                        new SqlParameter("IsPublic", objBookingListDTO.IsPublic),
                        new SqlParameter("CutOffDate", objBookingListDTO.CutOffDate ?? Convert.DBNull)
                        ).ToList();
                List<int> objList = objResult.ToList();
                //insert shipment doc file names
                IEnumerable<int> objDcosResult;
                if (objBookingListDTO.ShipmentDocsDTOList != null)
                {
                    foreach (ShipmentDocsDTO objShipmentDocsDTO in objBookingListDTO.ShipmentDocsDTOList)
                    {
                        objDcosResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_SHIPMENT_DOC_INSERT @DocumentCommonID,@DocName,@DocType,@CreatedBy,@IsSysGenerated",
                            new SqlParameter("DocumentCommonID", objBookingListDTO.DocumentCommonID),
                            new SqlParameter("DocName", objShipmentDocsDTO.DocName),
                            new SqlParameter("DocType", "Booking"),
                            new SqlParameter("CreatedBy", objBookingListDTO.CreatedBy),
                            new SqlParameter("IsSysGenerated", false)).ToList();
                    }
                }
                //insert Next Action remarks
                if (objBookingListDTO.NextActionRemarks != "" && objBookingListDTO.NextActionDueDate != null)
                {
                    IEnumerable<int> objRemarksResult;
                    objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                    new SqlParameter("RefID", objBookingListDTO.DocumentCommonID),
                    new SqlParameter("NextActionDate", objBookingListDTO.NextActionDueDate),
                    new SqlParameter("Type", "Booking"),
                    new SqlParameter("ActivityID", 5),
                    new SqlParameter("Remarks", objBookingListDTO.NextActionRemarks)).ToList();
                }
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }

        }



        [System.Web.Http.HttpPost]
        public virtual ActionResult saveFinalizedTransport(BookingListDTO objBookingListDTO)
        {
            try
            {
                //Update Document details
                IEnumerable<int> objResult;
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_FINALIZED_TRANSPORT_UPDATE @DocumentCommonID,@Trucking_ContactID, @Loading_ContactID,@LoadingDate",
                        new SqlParameter("DocumentCommonID", objBookingListDTO.DocumentCommonID),
                        new SqlParameter("Trucking_ContactID", objBookingListDTO.Trucking_ContactID ?? Convert.DBNull),
                        new SqlParameter("Loading_ContactID", objBookingListDTO.Loading_ContactID ?? Convert.DBNull),
                        new SqlParameter("LoadingDate", objBookingListDTO.LoadingDate ?? Convert.DBNull)).ToList();
                List<int> objList = objResult.ToList();

                //insert Next Action remarks
                if (objBookingListDTO.NextActionRemarks != "" && objBookingListDTO.NextActionDueDate != null)
                {
                    IEnumerable<int> objRemarksResult;
                    objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                    new SqlParameter("RefID", objBookingListDTO.DocumentCommonID),
                    new SqlParameter("NextActionDate", objBookingListDTO.NextActionDueDate),
                    new SqlParameter("Type", "Trucking"),
                    new SqlParameter("ActivityID", 6),
                    new SqlParameter("Remarks", objBookingListDTO.NextActionRemarks)).ToList();
                }
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }

        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult GetFinalizedTransportDetails(BookingListDTO objBooking)
        {
            try
            {
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_FINALIZED_TRANSPORT_DETAILS @DocumentCommonID",
                                        new SqlParameter("DocumentCommonID", objBooking.DocumentCommonID)).ToList();
                BookingListDTO objBookingListDTO = result[0];

                ////get all Next Action remarks
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                     new SqlParameter("QUOTATIONID", objBookingListDTO.QuotationID),
                     new SqlParameter("ENQUIRYID", objBookingListDTO.EnquiryID),
                     new SqlParameter("BookingID", objBookingListDTO.DocumentCommonID),
                     new SqlParameter("RefType", "Trucking")).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                if (RemarksResultList.Count > 0)
                {
                    objBookingListDTO.NextActionRemarksDTOList = RemarksResult.ToArray();
                    objBookingListDTO.LastRemarks = RemarksResultList[0].Remarks;
                    objBookingListDTO.LastRemarkDate = RemarksResultList[0].NextActionDate;
                }

                return AppResult(objBookingListDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult GetPendingMovementList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                //int GALBookingStatusID = Convert.ToInt32(searchCriteria["galBookingStatusID"]);
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_PENDING_MOVEMENT_LIST @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@DEPARTMENTID,@SORTCOLUMN,@SORTORDER,@DASHBOARD_FILTER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("DEPARTMENTID", searchCriteria["departmentID"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
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
        public virtual ActionResult SavePendingMovement(BookingListDTO objBookingListDTO)
        {
            try
            {
                //Update Document details
                IEnumerable<int> objResult;
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_PENDING_MOVEMENT_UPDATE @DocumentCommonID,@ConfStuffedDate",
                        new SqlParameter("DocumentCommonID", objBookingListDTO.DocumentCommonID),
                        new SqlParameter("ConfStuffedDate", objBookingListDTO.ConfStuffedDate ?? Convert.DBNull)
                        ).ToList();
                List<int> objList = objResult.ToList();

                //insert Next Action remarks
                if (objBookingListDTO.NextActionRemarks != "" && objBookingListDTO.NextActionDueDate != null)
                {
                    IEnumerable<int> objRemarksResult;
                    objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                    new SqlParameter("RefID", objBookingListDTO.DocumentCommonID),
                    new SqlParameter("NextActionDate", objBookingListDTO.NextActionDueDate),
                    new SqlParameter("Type", "Trucking"),
                    new SqlParameter("ActivityID", 6),
                    new SqlParameter("Remarks", objBookingListDTO.NextActionRemarks)).ToList();
                }
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }

        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult GetPendingMovementDetails(BookingListDTO objBooking)
        {
            try
            {
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_PENDING_MOVEMENT_DETAILS @DocumentCommonID",
                                        new SqlParameter("DocumentCommonID", objBooking.DocumentCommonID)).ToList();
                BookingListDTO objBookingListDTO = result[0];

                ////get all Next Action remarks
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                     new SqlParameter("QUOTATIONID", objBookingListDTO.QuotationID),
                     new SqlParameter("ENQUIRYID", objBookingListDTO.EnquiryID),
                     new SqlParameter("BookingID", objBookingListDTO.DocumentCommonID),
                     new SqlParameter("RefType", "Trucking")).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                if (RemarksResultList.Count > 0)
                {
                    objBookingListDTO.NextActionRemarksDTOList = RemarksResult.ToArray();
                    objBookingListDTO.LastRemarks = RemarksResultList[0].Remarks;
                    objBookingListDTO.LastRemarkDate = RemarksResultList[0].NextActionDate;
                }

                return AppResult(objBookingListDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult GetConfReadyToSendList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                //int GALBookingStatusID = Convert.ToInt32(searchCriteria["galBookingStatusID"]);
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_CONF_READY_TO_SEND_LIST @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@DEPARTMENTID,@SORTCOLUMN,@SORTORDER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("DEPARTMENTID", searchCriteria["departmentID"]),
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
        public virtual ActionResult DeleteShipmentDoc(ShipmentDocsDTO objShipmentDocsDTO)
        {
            try
            {
                IEnumerable<int> objResult = null;
                if (objShipmentDocsDTO.DocumentCommonID > 0)
                {
                    objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_SHIPMENT_DOC_DELETE @DocumentCommonID,@DocName,@DocType",
                                        new SqlParameter("DocumentCommonID", objShipmentDocsDTO.DocumentCommonID),
                                        new SqlParameter("DocName", objShipmentDocsDTO.DocName),
                                        new SqlParameter("DocType", objShipmentDocsDTO.DocumentType)).ToList();
                }
                var uploadFolder = "";
                string fileName = objShipmentDocsDTO.DocName;
                if (objShipmentDocsDTO.DocumentType == "Booking")
                {
                    fileName = objShipmentDocsDTO.DocumentCommonID.ToString() + "_" + objShipmentDocsDTO.DocName;
                    uploadFolder = ConfigurationManager.AppSettings["BookingDocPath"];
                }
                if (objShipmentDocsDTO.DocumentType == "Other")
                {
                    fileName = objShipmentDocsDTO.DocumentCommonID.ToString() + "_" + objShipmentDocsDTO.DocName;
                    uploadFolder = ConfigurationManager.AppSettings["OtherDocPath"];
                }
                if (objShipmentDocsDTO.DocumentType == "Shipper")
                {
                    if (!objShipmentDocsDTO.IsSysGenerated)
                    {
                        fileName = objShipmentDocsDTO.DocumentCommonID.ToString() + "_" + objShipmentDocsDTO.DocName;
                    }
                    uploadFolder = ConfigurationManager.AppSettings["ShipperDocPath"];
                }
                if (objShipmentDocsDTO.DocumentType == "DockReceipt")
                {
                    if (!objShipmentDocsDTO.IsSysGenerated)
                    {
                        fileName = objShipmentDocsDTO.DocumentCommonID.ToString() + "_" + objShipmentDocsDTO.DocName;
                    }
                    uploadFolder = ConfigurationManager.AppSettings["DockReceiptPath"];
                }
                if (objShipmentDocsDTO.DocumentType == "HBL")
                {
                    if (!objShipmentDocsDTO.IsSysGenerated)
                    {
                        fileName = objShipmentDocsDTO.DocumentCommonID.ToString() + "_" + objShipmentDocsDTO.DocName;
                    }
                    uploadFolder = ConfigurationManager.AppSettings["HBLDocPath"];
                }
                if (objShipmentDocsDTO.DocumentType == "MBL")
                {
                    if (!objShipmentDocsDTO.IsSysGenerated)
                    {
                        fileName = objShipmentDocsDTO.DocumentCommonID.ToString() + "_" + objShipmentDocsDTO.DocName;
                    }
                    uploadFolder = ConfigurationManager.AppSettings["MBLDocPath"];
                }
                if (objShipmentDocsDTO.DocumentType == "CO")
                {
                    if (!objShipmentDocsDTO.IsSysGenerated)
                    {
                        fileName = objShipmentDocsDTO.DocumentCommonID.ToString() + "_" + objShipmentDocsDTO.DocName;
                    }
                    uploadFolder = ConfigurationManager.AppSettings["CODocPath"];
                }

                var root = HttpContext.Current.Server.MapPath(uploadFolder);
                DirectoryInfo dir = new DirectoryInfo(root);
                if (System.IO.File.Exists(dir.FullName + "\\" + fileName))
                {
                    System.IO.File.Delete(dir.FullName + "\\" + fileName);
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
        public virtual ActionResult DeleteBookingConfDocs(QuotationEmailData emailData)
        {
            try
            {
                IEnumerable<int> objResult = null;

                var uploadFolder = ConfigurationManager.AppSettings["BookingDocPath"];
                var root = HttpContext.Current.Server.MapPath(uploadFolder);
                DirectoryInfo dir = new DirectoryInfo(root);
                foreach (FileAttachementDTO objFileAttachement in emailData.FileAttachementDTOList)
                {
                    if (System.IO.File.Exists(dir.FullName + "\\" + "BkgConf" + emailData.DocumentCommonID.ToString() + "_" + objFileAttachement.FileName))
                    {
                        System.IO.File.Delete(dir.FullName + "\\" + "BkgConf" + emailData.DocumentCommonID.ToString() + "_" + objFileAttachement.FileName);
                    }
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
        public async Task<HttpResponseMessage> Upload()
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            if (!Request.Content.IsMimeMultipartContent())
            {
                output.AddMessage(HttpContext.GetGlobalResourceObject("CommonTerms", "ErrFileTypeNotSupported").ToString(), true);
                return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, new { output });
            }

            try
            {
                var uploadFolder = ConfigurationManager.AppSettings["BookingDocPath"];

                var root = HttpContext.Current.Server.MapPath(uploadFolder);
                var provider = new MultipartFormDataStreamProvider(root);

                var result = await Request.Content.ReadAsMultipartAsync(provider);

                if (result.FormData.HasKeys())
                {
                    var fileName = GetUnescapeData(result, "DisplayName").ToString();
                    var DocumentId = GetUnescapeData(result, "DocumentCommonID").ToString();
                    var DocumentType = GetUnescapeData(result, "DocumentType").ToString();


                    if (DocumentType == "Booking")
                    {
                        root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["BookingDocPath"]);
                    }
                    if (DocumentType == "Other" || DocumentType == "ExtDoc")
                    {
                        root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["OtherDocPath"]);
                    }
                    if (DocumentType == "Shipper")
                    {
                        root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ShipperDocPath"]);
                    }
                    if (DocumentType == "DockReceipt")
                    {
                        root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DockReceiptPath"]);
                    }
                    if (DocumentType == "HBL")
                    {
                        root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["HBLDocPath"]);
                    }
                    if (DocumentType == "MBL")
                    {
                        root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MBLDocPath"]);
                    }
                    if (DocumentType == "CO")
                    {
                        root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CODocPath"]);
                    }
                    if (DocumentType == "AES")
                    {
                        root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AESDocPath"]);
                    }
                    if (DocumentType == "LINEMBL")
                    {
                        root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["LineMBLPath"]);
                    }

                    DirectoryInfo dir = new DirectoryInfo(root);
                    
                    if (System.IO.File.Exists(dir.FullName + "\\" + fileName))
                    {
                        System.IO.File.Delete(dir.FullName + "\\" + fileName);
                    }
                    var attachedDate = DateTime.Now;
                    var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);

                    uploadedFileInfo.MoveTo(root + "\\" + fileName);
                    //uploadedFileInfo.MoveTo(dir.FullName + "\\" + DocumentId.ToString() + "_" + fileName);

                    output.AddMessage("File is uploaded successfully.", true);
                    return Request.CreateResponse(HttpStatusCode.OK, new { output });
                }

                output.AddMessage("Some error found, please contact administrator.", true);
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
        public HttpResponseMessage downloadAttachment()
        {
            try
            {
                string fileName = "";
                string DocumentType = "";
                int DocumentCommonID;
                int IsSysGenerated;
                fileName = Request.Headers.GetValues("fileName").ToList()[0];
                DocumentCommonID = Int32.Parse(Request.Headers.GetValues("documentCommonID").ToList()[0]);
                DocumentType = Request.Headers.GetValues("documentType").ToList()[0];
                IsSysGenerated = Int32.Parse(Request.Headers.GetValues("isSystemGenerated").ToList()[0]);

                HttpResponseMessage result = null;
                if (!System.IO.File.Exists(fileName))
                {
                    result = Request.CreateResponse(HttpStatusCode.Gone);
                }
                if (!string.IsNullOrEmpty(fileName))
                {
                    var ShipmentFolder = "";
                    if (DocumentType == "Shipper")
                    {
                        if (IsSysGenerated == 0)
                        {
                            fileName = DocumentCommonID + "_" + fileName;
                        }
                        ShipmentFolder = ConfigurationManager.AppSettings["ShipperDocPath"];
                    }
                    if (DocumentType == "Booking")
                    {
                        fileName = DocumentCommonID + "_" + fileName;
                        ShipmentFolder = ConfigurationManager.AppSettings["BookingDocPath"];
                    }
                    if (DocumentType == "Other" || DocumentType == "ExtDoc")
                    {
                        fileName = DocumentCommonID + "_" + fileName;
                        ShipmentFolder = ConfigurationManager.AppSettings["OtherDocPath"];
                    }
                    if (DocumentType == "DockReceipt")
                    {
                        if (IsSysGenerated == 0)
                        {
                            fileName = DocumentCommonID + "_" + fileName;
                        }
                        ShipmentFolder = ConfigurationManager.AppSettings["DockReceiptPath"];
                    }
                    if (DocumentType == "HBL" || DocumentType == "HBLWithoutLogo")
                    {
                        if (IsSysGenerated == 0)
                        {
                            fileName = DocumentCommonID + "_" + fileName;
                        }
                        ShipmentFolder = ConfigurationManager.AppSettings["HBLDocPath"];
                    }
                    if (DocumentType == "MBL")
                    {
                        if (IsSysGenerated == 0)
                        {
                            fileName = DocumentCommonID + "_" + fileName;
                        }
                        ShipmentFolder = ConfigurationManager.AppSettings["MBLDocPath"];
                    }
                    if (DocumentType == "CO")
                    {
                        if (IsSysGenerated == 0)
                        {
                            fileName = DocumentCommonID + "_" + fileName;
                        }
                        ShipmentFolder = ConfigurationManager.AppSettings["CODocPath"];
                    }
                    if (DocumentType == "AES")
                    {
                        if (IsSysGenerated == 0)
                        {
                            fileName = DocumentCommonID + "_" + fileName;
                        }
                        ShipmentFolder = ConfigurationManager.AppSettings["AESDocPath"];
                    }
                    if (DocumentType == "LINEMBL")
                    {
                        if (IsSysGenerated == 0)
                        {
                            fileName = DocumentCommonID + "_" + fileName;
                        }
                        ShipmentFolder = ConfigurationManager.AppSettings["LineMBLPath"];
                    }
                    string filePath = HttpContext.Current.Server.MapPath(ShipmentFolder + "/") + fileName;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            byte[] bytes = new byte[file.Length];
                            file.Read(bytes, 0, (int)file.Length);
                            ms.Write(bytes, 0, (int)file.Length);

                            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                            httpResponseMessage.Content = new ByteArrayContent(bytes.ToArray());
                            httpResponseMessage.Content.Headers.Add("x-filename", fileName);
                            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                            //httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                            //httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
                            httpResponseMessage.StatusCode = HttpStatusCode.OK;
                            return httpResponseMessage;
                        }
                    }
                }
                return this.Request.CreateResponse(HttpStatusCode.NotFound, "File not found.");

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage DownloadBookingConfDoc()
        {
            try
            {
                string fileName = "";
                //int DocumentCommonID;
                fileName = Request.Headers.GetValues("fileName").ToList()[0];
                //DocumentCommonID = Int32.Parse(Request.Headers.GetValues("documentCommonID").ToList()[0]);
                //fileName = DocumentCommonID + "_" + fileName;

                var ShipmentFolder = ConfigurationManager.AppSettings["ShipperDocPath"];
                string filePath = HttpContext.Current.Server.MapPath(ShipmentFolder + "/") + fileName;


                HttpResponseMessage result = null;
                if (!System.IO.File.Exists(fileName))
                {
                    result = Request.CreateResponse(HttpStatusCode.Gone);
                }
                if (!string.IsNullOrEmpty(fileName))
                {


                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            byte[] bytes = new byte[file.Length];
                            file.Read(bytes, 0, (int)file.Length);
                            ms.Write(bytes, 0, (int)file.Length);

                            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                            httpResponseMessage.Content = new ByteArrayContent(bytes.ToArray());
                            httpResponseMessage.Content.Headers.Add("x-filename", fileName);
                            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                            //httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                            //httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
                            httpResponseMessage.StatusCode = HttpStatusCode.OK;
                            return httpResponseMessage;
                        }
                    }
                }
                return this.Request.CreateResponse(HttpStatusCode.NotFound, "File not found.");

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult SendEmail(QuotationEmailData emailData)
        {
            try
            {

                //var ShipmentFolder = ConfigurationManager.AppSettings["ShipmentPath"];
                //string filePath = HttpContext.Current.Server.MapPath(ShipmentFolder + "/") + "Shipper_9015.pdf";

                List<int> objList = null;
                IEnumerable<int> objResult = null;
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_UPDATE_GAL_BOOKING_STATUS @DocumentCommonID,@GALBookingStatusID",
                                        new SqlParameter("DocumentCommonID", emailData.DocumentCommonID),
                                        new SqlParameter("GALBookingStatusID", emailData.GALBookingStatusID)).ToList();
                objList = objResult.ToList();

                MailMessage mailMessage = new MailMessage();

                var userDetail = _context.ExecuteQuery<LG_USER>("EXEC dbo.GetUserDetailBYID @UserID",
                                 new SqlParameter("UserID", emailData.CreatedBy)).FirstOrDefault();
                userDetail.UsrSmtpPassword = SecurityHelper.DecryptString(userDetail.UsrSmtpPassword, "sblw-3hn8-sqoy19");

                mailMessage.From = new System.Net.Mail.MailAddress(userDetail.UsrSmtpUsername); // MailAddress("Noreply@miamigloballines.com");

                var ShipmentFolder = ConfigurationManager.AppSettings["ShipperDocPath"];
                var BookingFolder = ConfigurationManager.AppSettings["BookingDocPath"];
                string root1 = HttpContext.Current.Server.MapPath(ShipmentFolder);
                string root2 = HttpContext.Current.Server.MapPath(BookingFolder);
                System.Net.Mail.Attachment attachment;
                DirectoryInfo dir1 = new DirectoryInfo(root1);
                DirectoryInfo dir2 = new DirectoryInfo(root2);


                var Count = 0;

                if (emailData.FileAttachementDTOList != null)
                {
                    foreach (FileAttachementDTO objFileAttachement in emailData.FileAttachementDTOList)
                    {
                        if (Count == 0)
                        {
                            if (System.IO.File.Exists(dir1.FullName + "\\" + "Shipper_" + emailData.DocumentCommonID.ToString() + ".pdf"))
                            {
                                attachment = new System.Net.Mail.Attachment(dir1.FullName + "\\" + "Shipper_" + emailData.DocumentCommonID.ToString() + ".pdf");
                                mailMessage.Attachments.Add(attachment);
                            }
                        }
                        Count = Count + 1;
                        if (System.IO.File.Exists(dir2.FullName + "\\" + "BkgConf" + emailData.DocumentCommonID.ToString() + "_" + objFileAttachement.FileName))
                        {
                            attachment = new System.Net.Mail.Attachment(dir2.FullName + "\\" + "BkgConf" + emailData.DocumentCommonID.ToString() + "_" + objFileAttachement.FileName);
                            mailMessage.Attachments.Add(attachment);
                        }
                    }
                }
                else
                {
                    if (System.IO.File.Exists(dir1.FullName + "\\" + "Shipper_" + emailData.DocumentCommonID.ToString() + ".pdf"))
                    {
                        attachment = new System.Net.Mail.Attachment(dir1.FullName + "\\" + "Shipper_" + emailData.DocumentCommonID.ToString() + ".pdf");
                        mailMessage.Attachments.Add(attachment);
                    }
                }
                List<string> EmailList = null;
                EmailList = new List<string>();

                //mailMessage.To.Add(new MailAddress(emailData.EmailTo));
                if (emailData.EmailTo.Contains(";"))
                {
                    string[] strUser = emailData.EmailTo.Split(';');
                    for (int i = 0; i < strUser.Length; i++)
                    {
                        if (strUser[i] != "")
                        {
                            mailMessage.To.Add(new System.Net.Mail.MailAddress(strUser[i]));
                            EmailList.Add(strUser[i]);
                        }
                    }
                }
                else
                {
                    mailMessage.To.Add(new System.Net.Mail.MailAddress(emailData.EmailTo));
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
                                mailMessage.CC.Add(new System.Net.Mail.MailAddress(strUser[i]));
                                EmailList.Add(strUser[i]);
                            }
                        }
                    }
                    else
                    {
                        mailMessage.CC.Add(new System.Net.Mail.MailAddress(emailData.Emailcc));
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
                                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(strUser[i]));
                                EmailList.Add(strUser[i]);
                            }
                        }
                    }
                    else
                    {
                        mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(emailData.EmailBcc));
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
                mailMessage.Body = emailData.EmailBody;
                mailMessage.IsBodyHtml = true;

                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                EmailHelper.Send(principal, mailMessage);
                mailMessage.Attachments.Dispose();

                //foreach (FileAttachementDTO objFileAttachement in emailData.FileAttachementDTOList)
                //{                    
                //    if (System.IO.File.Exists(dir.FullName + "\\" + emailData.DocumentCommonID.ToString() + "_" + objFileAttachement.FileName))
                //    {
                //        System.IO.File.Delete(dir.FullName + "\\" + emailData.DocumentCommonID.ToString() + "_" + objFileAttachement.FileName);
                //    }
                //}
                return AppResult(objList, 1);
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

        [System.Web.Http.HttpPost]
        public ActionResult GetRolledOverList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_ROLLED_OVER_LIST @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@DEPARTMENTID,@SORTCOLUMN,@SORTORDER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("DEPARTMENTID", searchCriteria["departmentID"]),
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
        public virtual ActionResult CancelRollOver(BookingListDTO objBookingListDTO)
        {
            try
            {
                IEnumerable<int> objResult = null;
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_CANCEL_ROLLED_OVER @DocumentCommonID",
                                    new SqlParameter("DocumentCommonID", objBookingListDTO.DocumentCommonID)).ToList();
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult GetShippingConfirmationList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_SHIPPER_CONFIRMATION_LIST @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@DEPARTMENTID,@SORTCOLUMN,@SORTORDER,@DASHBOARD_FILTER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("DEPARTMENTID", searchCriteria["departmentID"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
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
        public ActionResult GetFinalizedTransportationList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                //int GALBookingStatusID = Convert.ToInt32(searchCriteria["galBookingStatusID"]);
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_FINALIZED_TRANSPORTATION_LIST @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@DEPARTMENTID,@SORTCOLUMN,@SORTORDER,@DASHBOARD_FILTER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("DEPARTMENTID", searchCriteria["departmentID"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
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
        public virtual ActionResult SaveShipperConfirmation(BookingListDTO objBookingListDTO)
        {
            try
            {
                //Update Document shipper details
                IEnumerable<int> objResult;
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_SHIPPER_CONFIRM_INSERT @DocumentCommonID,@PONO, @ConsigneeID,@ShipperCustomerName, @ShipperConfirmDate,@MailMode,@ShipperRepName,@ShipperAttachFile,@NextActionDate,@Remarks",
                        new SqlParameter("DocumentCommonID", objBookingListDTO.DocumentCommonID),
                        new SqlParameter("PONO", objBookingListDTO.ConsigneePONo ?? Convert.DBNull),
                        new SqlParameter("ConsigneeID", objBookingListDTO.ConsigneeID ?? Convert.DBNull),
                        new SqlParameter("ShipperCustomerName", objBookingListDTO.ShipperCustomerName ?? Convert.DBNull),
                        new SqlParameter("ShipperConfirmDate", objBookingListDTO.ShipperConfirmDate ?? Convert.DBNull),
                        new SqlParameter("MailMode", objBookingListDTO.MailMode ?? Convert.DBNull),
                        new SqlParameter("ShipperRepName", objBookingListDTO.BookingRequest ?? Convert.DBNull),
                        new SqlParameter("ShipperAttachFile", objBookingListDTO.ShipperAttachFile ?? Convert.DBNull),
                        new SqlParameter("NextActionDate", objBookingListDTO.NextActionDueDate ?? Convert.DBNull),
                        new SqlParameter("Remarks", objBookingListDTO.Remarks ?? Convert.DBNull)
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
        public virtual ActionResult GetShipperConfirmationDetail(BookingListDTO objBooking)
        {
            try
            {
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_SHIPPER_CONFIRM_DETAILS @DocumentCommonID",
                                        new SqlParameter("DocumentCommonID", objBooking.DocumentCommonID)).ToList();
                BookingListDTO objBookingListDTO = result[0];

                ////get all Next Action remarks
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                     new SqlParameter("QUOTATIONID", objBookingListDTO.QuotationID),
                     new SqlParameter("ENQUIRYID", objBookingListDTO.EnquiryID),
                     new SqlParameter("BookingID", objBookingListDTO.DocumentCommonID),
                     new SqlParameter("RefType", "Booking")).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                if (RemarksResultList.Count > 0)
                {
                    objBookingListDTO.NextActionRemarksDTOList = RemarksResult.ToArray();
                    objBookingListDTO.LastRemarks = RemarksResultList[0].Remarks;
                    objBookingListDTO.LastRemarkDate = RemarksResultList[0].NextActionDate;
                }

                return AppResult(objBookingListDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage downloadShipperAttachment()
        {
            try
            {
                string fileName = "";
                int DocumentCommonID;
                fileName = Request.Headers.GetValues("fileName").ToList()[0];
                DocumentCommonID = Int32.Parse(Request.Headers.GetValues("documentCommonID").ToList()[0]);
                fileName = DocumentCommonID + "_" + fileName;
                HttpResponseMessage result = null;
                if (!System.IO.File.Exists(fileName))
                {
                    result = Request.CreateResponse(HttpStatusCode.Gone);
                }
                if (!string.IsNullOrEmpty(fileName))
                {
                    var ShipperDocPathFolder = ConfigurationManager.AppSettings["ShipperDocPath"];
                    string filePath = HttpContext.Current.Server.MapPath(ShipperDocPathFolder + "/") + fileName;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            byte[] bytes = new byte[file.Length];
                            file.Read(bytes, 0, (int)file.Length);
                            ms.Write(bytes, 0, (int)file.Length);

                            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                            httpResponseMessage.Content = new ByteArrayContent(bytes.ToArray());
                            httpResponseMessage.Content.Headers.Add("x-filename", fileName);
                            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                            //httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                            //httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
                            httpResponseMessage.StatusCode = HttpStatusCode.OK;
                            return httpResponseMessage;
                        }
                    }
                }
                return this.Request.CreateResponse(HttpStatusCode.NotFound, "File not found.");

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> UploadShipperAttachment()
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            if (!Request.Content.IsMimeMultipartContent())
            {
                output.AddMessage(HttpContext.GetGlobalResourceObject("CommonTerms", "ErrFileTypeNotSupported").ToString(), true);
                return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, new { output });
            }

            try
            {
                var uploadFolder = ConfigurationManager.AppSettings["ShipperDocPath"];
                var root = HttpContext.Current.Server.MapPath(uploadFolder);
                var provider = new MultipartFormDataStreamProvider(root);

                var result = await Request.Content.ReadAsMultipartAsync(provider);

                if (result.FormData.HasKeys())
                {
                    var fileName = GetUnescapeData(result, "DisplayName").ToString();
                    var DocumentId = GetUnescapeData(result, "DocumentCommonID").ToString();
                    DirectoryInfo dir = new DirectoryInfo(root);
                    //FileInfo[] files = dir.GetFiles(DocumentId + "_*");
                    //foreach (FileInfo file in files)
                    //{
                    //    file.Delete();
                    //}                   
                    if (System.IO.File.Exists(dir.FullName + "\\" + DocumentId.ToString() + "_" + fileName))
                    {
                        System.IO.File.Delete(dir.FullName + "\\" + DocumentId.ToString() + "_" + fileName);
                    }
                    var attachedDate = DateTime.Now;
                    var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);

                    uploadedFileInfo.MoveTo(root + "\\" + fileName);

                    output.AddMessage("File is uploaded successfully.", true);
                    return Request.CreateResponse(HttpStatusCode.OK, new { output });
                }

                output.AddMessage("Some error found, please contact administrator.", true);
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
        public virtual ActionResult SaveShippingRemarks(BookingListDTO objBookingListDTO)
        {
            try
            {
                //delete existing Shipper Remarks
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_SHIPPING_REMARKS_DELETE @ShipperId",
                new SqlParameter("ShipperId", objBookingListDTO.contactID)).ToList();
                List<int> objList = objResult.ToList();

                //insert Shipper Remarks
                IEnumerable<int> objRemarksResult;
                foreach (ShipperRemarksDTO objShipperRemarksDTO in objBookingListDTO.ShipperRemarksDTOList)
                {
                    objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_SHIPPING_REMARKS_INSERT @ShipperId,@ShipperRemarks, @ShipperCommonRemarks",
                        new SqlParameter("@ShipperId", objBookingListDTO.contactID),
                        new SqlParameter("@ShipperRemarks", objShipperRemarksDTO.Remarks ?? Convert.DBNull),
                        new SqlParameter("@ShipperCommonRemarks", objBookingListDTO.ShipperCommonRemarks ?? Convert.DBNull)).ToList();
                }

                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetShipperRemarks(int id)
        {
            try
            {

                List<ShipperRemarksDTO> result = _context.ExecuteQuery<ShipperRemarksDTO>("EXEC dbo.USP_LG_BOOKING_SHIPPING_REMARKS_GET @ShipperId",
                                        new SqlParameter("ShipperId", id.ToString())).ToList();

                BookingListDTO objBookingDTO = new BookingListDTO();
                if (result.Count > 0)
                {
                    objBookingDTO.contactID = result[0].contactID;
                    objBookingDTO.ShipperCommonRemarks = result[0].ShipperCommonRemarks;
                    objBookingDTO.ShipperRemarksDTOList = result.ToArray();
                }

                return AppResult(objBookingDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult deleteShippingRemarks(ShipperRemarksDTO objShipperRemarksDTO)
        {
            try
            {
                IEnumerable<int> objResult = null;
                if (objShipperRemarksDTO.contactID > 0)
                {
                    objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_SHIPPING_REMARKS_DELETE @ShipperId",
                                        new SqlParameter("ShipperId", objShipperRemarksDTO.contactID)).ToList();
                }


                List<int> objList = objResult.ToList();
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        public Task<bool> GetData()
        {
            return Task.Run<bool>(() => { return true; });
        }
        public async Task<bool> CreateShipperReport(int DocumentId, string ShipperRootPath)
        {
            try
            {

                var result = await GetData();
                //var root1 = HttpContext.Current.Server.MapPath(uploadFolder);
                string reportPath = "/AppMGL.Report/ShipperConfReport";

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();
                reportParams.Add(new KeyValuePair<string, string>("DocumentCommonID", Convert.ToString(DocumentId)));

                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.PDF);

                if (reportBuffer.Length > 0)
                {
                    string fileName = "Shipper_" + Convert.ToString(DocumentId) + ".pdf";
                    //var uploadFolder = ConfigurationManager.AppSettings["ShipmentPath"];
                    //var root = HttpContext.Current.Server.MapPath(uploadFolder);
                    DirectoryInfo dir = new DirectoryInfo(ShipperRootPath);
                    if (System.IO.File.Exists(dir.FullName + "\\" + fileName))
                    {
                        System.IO.File.Delete(dir.FullName + "\\" + fileName);
                    }
                    System.IO.File.WriteAllBytes(dir.FullName + "\\" + fileName, reportBuffer);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return true;
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage ExportReport(Dictionary<string, string> exportParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/DocAttachmentReport";

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();

                reportParams.Add(new KeyValuePair<string, string>("SIT_ID", exportParams["SitId"]));

                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = exportParams["SitId"].ToString() + "_DocAttachmentReport.xlsx";

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
        public HttpResponseMessage ExportRegisterReport(Dictionary<string, string> exportParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/DocExportRegisterReport";

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();

                reportParams.Add(new KeyValuePair<string, string>("OPTIONDATEVALUE", exportParams["optionDateValue"]));
                reportParams.Add(new KeyValuePair<string, string>("FROMDATE", exportParams["fromDate"]));
                reportParams.Add(new KeyValuePair<string, string>("TODATE", exportParams["toDate"]));
                reportParams.Add(new KeyValuePair<string, string>("GALBOOKING_STATUS_ID", exportParams["galBookingStatusID"]));
                reportParams.Add(new KeyValuePair<string, string>("DEPARTMENTID", exportParams["departmentID"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHOPTION1VALUE", exportParams["searchOption1Value"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHBOX1VALUE", exportParams["searchBox1Value"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHOPTION2VALUE", exportParams["searchOption2Value"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHBOX2VALUE", exportParams["searchBox2Value"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHOPTION3VALUE", exportParams["searchOption3Value"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHBOX3VALUE", exportParams["searchBox3Value"]));
                reportParams.Add(new KeyValuePair<string, string>("ISCURRENTYEAR", exportParams["isCurrentYear"]));
                reportParams.Add(new KeyValuePair<string, string>("ISEXCLUDECANCELLROLLOVER", exportParams["isExcludeCancellRollover"]));
                reportParams.Add(new KeyValuePair<string, string>("SIT_ID", exportParams["SitId"]));

                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = exportParams["SitId"].ToString() + "_DocExportRegisterReport.xlsx";

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
        public ActionResult GetExportRegisterList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                //int GALBookingStatusID = Convert.ToInt32(searchCriteria["galBookingStatusID"]);
                // List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_EXPORT_REGISTER_LIST @PAGENO, @PAGESIZE,@OPTIONDATEVALUE,@FROMDATE,@TODATE,@GALBOOKING_STATUS_ID,@DEPARTMENTID,@SEARCHOPTION1VALUE,@SEARCHBOX1VALUE,@SEARCHOPTION2VALUE,@SEARCHBOX2VALUE,@SEARCHOPTION3VALUE,@SEARCHBOX3VALUE,@ISCURRENTYEAR,@ISEXCLUDECANCELLROLLOVER,@SORTCOLUMN,@SORTORDER,@SIT_ID",
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_EXPORT_REGISTER_LIST_SiteId @PAGENO, @PAGESIZE,@OPTIONDATEVALUE,@FROMDATE,@TODATE,@GALBOOKING_STATUS_ID,@DEPARTMENTID,@SEARCHOPTION1VALUE,@SEARCHBOX1VALUE,@SEARCHOPTION2VALUE,@SEARCHBOX2VALUE,@SEARCHOPTION3VALUE,@SEARCHBOX3VALUE,@ISCURRENTYEAR,@ISEXCLUDECANCELLROLLOVER,@SORTCOLUMN,@SORTORDER,@SIT_ID",
                         new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONDATEVALUE", searchCriteria["optionDateValue"]),
                        new SqlParameter("FROMDATE", searchCriteria["fromDate"]),
                        new SqlParameter("TODATE", searchCriteria["toDate"]),
                        new SqlParameter("GALBOOKING_STATUS_ID", searchCriteria["galBookingStatusID"]),
                        new SqlParameter("DEPARTMENTID", searchCriteria["departmentID"]),
                        new SqlParameter("SEARCHOPTION1VALUE", searchCriteria["searchOption1Value"]),
                        new SqlParameter("SEARCHBOX1VALUE", searchCriteria["searchBox1Value"]),
                        new SqlParameter("SEARCHOPTION2VALUE", searchCriteria["searchOption2Value"]),
                        new SqlParameter("SEARCHBOX2VALUE", searchCriteria["searchBox2Value"]),
                        new SqlParameter("SEARCHOPTION3VALUE", searchCriteria["searchOption3Value"]),
                        new SqlParameter("SEARCHBOX3VALUE", searchCriteria["searchBox3Value"]),
                        new SqlParameter("ISCURRENTYEAR", searchCriteria["isCurrentYear"]),
                        new SqlParameter("ISEXCLUDECANCELLROLLOVER", searchCriteria["isExcludeCancellRollover"]),
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
        public virtual ActionResult GetExportRegisterDetail(BookingListDTO objBooking)
        {
            try
            {
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_EXPORT_REGISTER_DETAILS @DOCUMENTCOMMONID,@SIT_ID",
                                        new SqlParameter("DOCUMENTCOMMONID", objBooking.DocumentCommonID),
                                        new SqlParameter("SIT_ID", objBooking.SiteId)).ToList();
                BookingListDTO objBookingListDTO = result[0];

                return AppResult(objBookingListDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetDocumentationStatus()
        {
            try
            {
                var result = _context.ExecuteQuery<DocumentationStatus>("EXEC dbo.USP_LG_BOOKING_GET_DOCUMENTATION_STATUS").ToList();
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
        public virtual ActionResult GetEmailDocumentDetail()
        {
            try
            {


                string quotationID = Request.Headers.GetValues("EnqId").ToList()[0];
                var result = _context.ExecuteQuery<QuotationEmailData>("EXEC dbo.USP_Get_Quatation_Email_Deatil @QuotationID",
                    new SqlParameter("QuotationID", quotationID)
                    ).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


        [System.Web.Http.HttpPost]
        public ActionResult GetExportRegisterListEES(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                //int GALBookingStatusID = Convert.ToInt32(searchCriteria["galBookingStatusID"]);
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_EXPORT_REGISTER_LIST @PAGENO, @PAGESIZE,@OPTIONDATEVALUE,@FROMDATE,@TODATE,@GALBOOKING_STATUS_ID,@DEPARTMENTID,@SEARCHOPTION1VALUE,@SEARCHBOX1VALUE,@SEARCHOPTION2VALUE,@SEARCHBOX2VALUE,@SEARCHOPTION3VALUE,@SEARCHBOX3VALUE,@ISCURRENTYEAR,@ISEXCLUDECANCELLROLLOVER,@SORTCOLUMN,@SORTORDER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONDATEVALUE", searchCriteria["optionDateValue"]),
                        new SqlParameter("FROMDATE", searchCriteria["fromDate"]),
                        new SqlParameter("TODATE", searchCriteria["toDate"]),
                        new SqlParameter("GALBOOKING_STATUS_ID", searchCriteria["galBookingStatusID"]),
                        new SqlParameter("DEPARTMENTID", searchCriteria["departmentID"]),
                        new SqlParameter("SEARCHOPTION1VALUE", searchCriteria["searchOption1Value"]),
                        new SqlParameter("SEARCHBOX1VALUE", searchCriteria["searchBox1Value"]),
                        new SqlParameter("SEARCHOPTION2VALUE", searchCriteria["searchOption2Value"]),
                        new SqlParameter("SEARCHBOX2VALUE", searchCriteria["searchBox2Value"]),
                        new SqlParameter("SEARCHOPTION3VALUE", searchCriteria["searchOption3Value"]),
                        new SqlParameter("SEARCHBOX3VALUE", searchCriteria["searchBox3Value"]),
                        new SqlParameter("ISCURRENTYEAR", searchCriteria["isCurrentYear"]),
                        new SqlParameter("ISEXCLUDECANCELLROLLOVER", searchCriteria["isExcludeCancellRollover"]),
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
        public HttpResponseMessage ExportRegisterReport_AES(Dictionary<string, string> exportParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/AESTest";

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();

                reportParams.Add(new KeyValuePair<string, string>("OPTIONDATEVALUE", exportParams["optionDateValue"]));
                reportParams.Add(new KeyValuePair<string, string>("FROMDATE", exportParams["fromDate"]));
                reportParams.Add(new KeyValuePair<string, string>("TODATE", exportParams["toDate"]));
                reportParams.Add(new KeyValuePair<string, string>("GALBOOKING_STATUS_ID", exportParams["galBookingStatusID"]));
                reportParams.Add(new KeyValuePair<string, string>("DEPARTMENTID", exportParams["departmentID"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHOPTION1VALUE", exportParams["searchOption1Value"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHBOX1VALUE", exportParams["searchBox1Value"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHOPTION2VALUE", exportParams["searchOption2Value"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHBOX2VALUE", exportParams["searchBox2Value"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHOPTION3VALUE", exportParams["searchOption3Value"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHBOX3VALUE", exportParams["searchBox3Value"]));
                reportParams.Add(new KeyValuePair<string, string>("ISCURRENTYEAR", exportParams["isCurrentYear"]));
                reportParams.Add(new KeyValuePair<string, string>("ISEXCLUDECANCELLROLLOVER", exportParams["isExcludeCancellRollover"]));
                reportParams.Add(new KeyValuePair<string, string>("SIT_ID", exportParams["SitId"]));

                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = exportParams["SitId"].ToString() + "_AESExportRegisterReport.xlsx";

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
        public ActionResult Save_EES_Submit(AppMGL.DTO.Report.EESSubmission objees)
        {
            try
            {
                string fileseq = "01";
                int seq;
                string fileName = string.Empty;
                string refreshfileName = string.Empty;

                List<EESLIST> result = _context.ExecuteQuery<EESLIST>("EXEC dbo.SP_CheckStatusAesFile @AESId",
                                      new SqlParameter("AESId", objees.Id == null ? "0" : objees.Id)).ToList();
                if (result.Count > 0)
                {

                    if (string.IsNullOrEmpty(objees.AesFileName))
                    {
                        fileName = objees.shipmentNumber + "aes_input_" + fileseq + ".txt";

                    }
                    else if (!string.IsNullOrEmpty(objees.AesFileName) && result[0].isuploaded == "0")
                    {
                        fileName = objees.AesFileName;
                    }
                    else
                    {
                        string[] t = objees.AesFileName.Split('_');
                        seq = Convert.ToInt32(t[2].ToString().Split('.')[0].ToString());
                        seq = seq + 1;
                        if (seq < 10)
                            fileseq = "0" + seq.ToString();
                        else
                            fileseq = seq.ToString();
                        fileName = objees.shipmentNumber + "aes_input_" + fileseq + ".txt";
                    }
                }
                else
                {

                    fileName = objees.shipmentNumber + "aes_input_" + fileseq + ".txt";
                }

                object[] obj = new object[113]
                 {
                     new SqlParameter("Id", objees.Id),
                     new SqlParameter("DOCUMENTCOMMONID", objees.DocumentCommonID ?? Convert.DBNull),
                     new SqlParameter("SIT_ID", objees.siteId ?? Convert.DBNull),
                     new SqlParameter("QuotationID", objees.QuotationID ?? Convert.DBNull),
                     new SqlParameter("Email", objees.EmailResponseAddress ?? Convert.DBNull),
                     new SqlParameter("EnquiryNo", objees.enquiryNo),
                     new SqlParameter("QuotationNo", objees.quotationNo ?? Convert.DBNull),
                     new SqlParameter("BookingNo", objees.bookingNo ?? Convert.DBNull),
                     new SqlParameter("CompanyName", objees.companyName ?? Convert.DBNull),
                     new SqlParameter("ShipmentReferenceNumber", objees.shipmentNumber ?? Convert.DBNull),

                     new SqlParameter("FilingOption", objees.FilingOption ?? Convert.DBNull),
                     new SqlParameter("ModeofTransport", objees.ModeofTransport ?? Convert.DBNull),
                     new SqlParameter("PortofExport", objees.PortofExport ?? Convert.DBNull),
                     new SqlParameter("PortofExportCode", objees.PortofExportCode ?? Convert.DBNull),
                     new SqlParameter("PortofUnloading", objees.PortofUnloading ?? Convert.DBNull),
                     new SqlParameter("PortofUnloadingCode", objees.PortofUnloadingCode ?? Convert.DBNull),
                     new SqlParameter("DepartureDate", objees.DepartureDate ?? Convert.DBNull),
                     new SqlParameter("OriginState", objees.OriginState ?? Convert.DBNull),
                     new SqlParameter("OriginStateCode", objees.OriginStateCode ?? Convert.DBNull),
                     new SqlParameter("CountryOfDestination", objees.CountryOfDestination ?? Convert.DBNull),

                     new SqlParameter("CountryCode", objees.CountryCode ?? Convert.DBNull),
                     new SqlParameter("InbondType", objees.InbondType ?? Convert.DBNull),
                     new SqlParameter("ForeginTradeZone", objees.ForeginTradeZone ?? Convert.DBNull),
                     new SqlParameter("ImportEntry", objees.ImportEntry ?? Convert.DBNull),
                     new SqlParameter("OriginalITN", objees.OriginalITN ?? Convert.DBNull),
                     new SqlParameter("IsRoutedTransaction", objees.IsRoutedTransaction ?? Convert.DBNull),
                     new SqlParameter("IsUltimateConsigneeCompanies", objees.IsUltimateConsigneeCompanies ?? Convert.DBNull),
                     new SqlParameter("IsHazardousMaterial", objees.IsHazardousMaterial ?? Convert.DBNull),
                     new SqlParameter("USPPIIDNumberTypeId", objees.USPPIIDNumberTypeId ?? Convert.DBNull),
                     new SqlParameter("USPPIIDNumber", objees.USPPIIDNumber ?? Convert.DBNull),

                     new SqlParameter("USPPICompanyName", objees.USPPICompanyName ?? Convert.DBNull),
                     new SqlParameter("IRSNumber", objees.IRSNumber ?? Convert.DBNull),
                     new SqlParameter("USPPIFirstName", objees.USPPIFirstName ?? Convert.DBNull),
                     new SqlParameter("USPPILastName", objees.USPPILastName ?? Convert.DBNull),
                     new SqlParameter("USPPIPhoneNumber", objees.USPPIPhoneNumber ?? Convert.DBNull),
                     new SqlParameter("USPPIAddressLine1", objees.USPPIAddressLine1 ?? Convert.DBNull),
                     new SqlParameter("USPPIAddressLine2", objees.USPPIAddressLine2 ?? Convert.DBNull),
                     new SqlParameter("USPPIPostalCodeId", objees.USPPIPostalCodeId ?? Convert.DBNull),
                     new SqlParameter("USPPICity", objees.USPPICity ?? Convert.DBNull),
                     new SqlParameter("USPPIStateId", objees.USPPIStateId ?? Convert.DBNull),

                     new SqlParameter("USPPIStateCode", objees.USPPIStateCode ?? Convert.DBNull),
                     new SqlParameter("USPPIEmail", objees.USPPIEmail ?? Convert.DBNull),

                     new SqlParameter("IsSoldEnRoute", objees.IsSoldEnRoute ?? Convert.DBNull),
                     new SqlParameter("ConsigneeTypeId", objees.ConsigneeTypeId ?? Convert.DBNull),
                     new SqlParameter("UltimateIdNumberTypeId", objees.UltimateIdNumberTypeId ?? Convert.DBNull),
                     new SqlParameter("UltimateIdNumber", objees.UltimateIdNumber ?? Convert.DBNull),
                     new SqlParameter("UltimateCompanyName", objees.UltimateCompanyName ?? Convert.DBNull),
                     new SqlParameter("UltimateFirstName", objees.UltimateFirstName ?? Convert.DBNull),
                     new SqlParameter("UltimateLastName", objees.UltimateLastName ?? Convert.DBNull),
                     new SqlParameter("UltimatePhoneNumber", objees.UltimatePhoneNumber ?? Convert.DBNull),
                     new SqlParameter("UltimateAddressLine1", objees.UltimateAddressLine1 ?? Convert.DBNull),

                     new SqlParameter("UltimateAddressLine2", objees.UltimateAddressLine2 ?? Convert.DBNull),
                     new SqlParameter("UltimateCountryId", objees.UltimateCountryId ?? Convert.DBNull),
                     new SqlParameter("UltimateCountryCode", objees.UltimateCountryCode ?? Convert.DBNull),
                     new SqlParameter("UltimatePostalCodeId", objees.UltimatePostalCodeId ?? Convert.DBNull),
                     new SqlParameter("UltimateCity", objees.UltimateCity ?? Convert.DBNull),
                     new SqlParameter("UltimateStateId", objees.UltimateStateId ?? Convert.DBNull),
                     new SqlParameter("UltimateStateCode", objees.UltimateStateCode ?? Convert.DBNull),
                      new SqlParameter("UltimateEmail", objees.UltimateEmail ?? Convert.DBNull),

                     new SqlParameter("InterIdNumberTypeId", objees.InterIdNumberTypeId ?? Convert.DBNull),
                     new SqlParameter("InterIdNumber", objees.InterIdNumber ?? Convert.DBNull),
                     new SqlParameter("InterCompanyName", objees.InterCompanyName ?? Convert.DBNull),
                     new SqlParameter("InterFirstName", objees.InterFirstName ?? Convert.DBNull),
                     new SqlParameter("InterLastName", objees.InterLastName ?? Convert.DBNull),
                     new SqlParameter("InterPhoneNumber", objees.InterPhoneNumber ?? Convert.DBNull),
                     new SqlParameter("InterAddressLine1", objees.InterAddressLine1 ?? Convert.DBNull),
                     new SqlParameter("InterAddressLine2", objees.InterAddressLine2 ?? Convert.DBNull),
                     new SqlParameter("InterCountryId", objees.InterCountryId ?? Convert.DBNull),
                      new SqlParameter("InterCountryCode", objees.InterCountryCode ?? Convert.DBNull),
                     new SqlParameter("InterPostalCodeId", objees.InterPostalCodeId ?? Convert.DBNull),
                     new SqlParameter("InterCity", objees.InterCity ?? Convert.DBNull),
                     new SqlParameter("InterStateId", objees.InterStateId ?? Convert.DBNull),
                     new SqlParameter("InterStateCode", objees.InterStateCode ?? Convert.DBNull),
                     new SqlParameter("InterEmail", objees.InterEmail ?? Convert.DBNull),

                    new SqlParameter("FreightIdNumberTypeId", objees.FreightIdNumberTypeId ?? Convert.DBNull),
                    new SqlParameter("FreightIdNumber", objees.FreightIdNumber ?? Convert.DBNull),
                    new SqlParameter("FreightCompanyName", objees.FreightCompanyName ?? Convert.DBNull),
                    new SqlParameter("FreightFirstName", objees.FreightFirstName ?? Convert.DBNull),
                    new SqlParameter("FreightLastName", objees.FreightLastName ?? Convert.DBNull),
                    new SqlParameter("FreightPhoneNumber", objees.FreightPhoneNumber ?? Convert.DBNull),
                    new SqlParameter("FreightAddressLine1", objees.FreightAddressLine1 ?? Convert.DBNull),
                    new SqlParameter("FreightAddressLine2", objees.FreightAddressLine2 ?? Convert.DBNull),
                    new SqlParameter("FreightPostalCodeId", objees.FreightPostalCodeId ?? Convert.DBNull),
                    new SqlParameter("FreightCity", objees.FreightCity ?? Convert.DBNull),
                    new SqlParameter("FreightStateId", objees.FreightStateId ?? Convert.DBNull),
                    new SqlParameter("FreightStateCode", objees.FreightStateCode ?? Convert.DBNull),
                    new SqlParameter("FreightEmail", objees.FreightEmail ?? Convert.DBNull),

                    new SqlParameter("CarrierSCAC", objees.CarrierSCAC ?? Convert.DBNull),
                    new SqlParameter("ConveyanceName", objees.ConveyanceName ?? Convert.DBNull),
                    new SqlParameter("TransportationReferenceNumber", objees.TransportationReferenceNumber ?? Convert.DBNull),
                    new SqlParameter("AES_FILENAME", fileName),
                    new SqlParameter("AESTYPE", objees.AESTYPE==null?"A":objees.AESTYPE ),
                    new SqlParameter("CreatedBy", objees.CreatedBy ?? Convert.DBNull),
                    new SqlParameter("ModifiedBy", objees.ModifiedBy ?? Convert.DBNull),
                    new SqlParameter("ExporterId", objees.ExporterId ?? Convert.DBNull),
                    new SqlParameter("ExporterFirstName", objees.ExporterFirstName ?? Convert.DBNull),
                    new SqlParameter("ExporterLastName", objees.ExporterLastName ?? Convert.DBNull),
                    new SqlParameter("ExporterPhoneNumber", objees.ExporterPhoneNumber ?? Convert.DBNull),
                    new SqlParameter("ExporterCountryId", objees.ExporterCountryId ?? Convert.DBNull),
                    new SqlParameter("ExporterCountryCode", objees.ExporterCountryCode ?? Convert.DBNull),
                    new SqlParameter("ExporterStateId", objees.ExporterStateId ?? Convert.DBNull),
                    new SqlParameter("ExporterStateCode", objees.ExporterStateCode ?? Convert.DBNull),
                    new SqlParameter("ExporterCity", objees.ExporterCity ?? Convert.DBNull),
                    new SqlParameter("ExporterPostalCodeId", objees.ExporterPostalCodeId ?? Convert.DBNull),
                    new SqlParameter("ExporterAddress", objees.ExporterAddress ?? Convert.DBNull),
                    new SqlParameter("ExporterAddress2", objees.ExporterAddress2 ?? Convert.DBNull),
                    new SqlParameter("ExporterEmail", objees.ExporterEmail ?? Convert.DBNull),
                    new SqlParameter("etype", objees.etype ?? Convert.DBNull),
                    new SqlParameter("isuploaded", objees.isuploaded ?? Convert.DBNull),
                    new SqlParameter("isDraft", objees.isDraft ),
                    new SqlParameter("isFreeze", objees.isFreeze?? Convert.DBNull),
                    new SqlParameter("FreezeRemarks", objees.FreezeRemarks?? Convert.DBNull),
                    new SqlParameter("IsTemplate", objees.IsTemplate?? Convert.DBNull),
    };

                IEnumerable<int> source = _context.ExecuteQuery<int>("EXEC dbo.USP_INSERT_UPDATE_AES_Submit @Id,@DOCUMENTCOMMONID,@SIT_ID,@QuotationID,@Email,@EnquiryNo,@QuotationNo,@BookingNo,@CompanyName,@ShipmentReferenceNumber,@FilingOption,@ModeofTransport,@PortofExport,@PortofExportCode,@PortofUnloading,@PortofUnloadingCode,@DepartureDate, @OriginState,@OriginStateCode, @CountryofDestination,@CountryCode,@InbondType,@ForeginTradeZone,@ImportEntry,@OriginalITN,@IsRoutedTransaction,@IsUltimateConsigneeCompanies,@IsHazardousMaterial, @USPPIIDNumberTypeId,@USPPIIDNumber,@USPPICompanyName,@IRSNumber,@USPPIFirstName,@USPPILastName,@USPPIPhoneNumber,@USPPIAddressLine1,@USPPIAddressLine2,@USPPIPostalCodeId,@USPPICity,@USPPIStateId,@USPPIStateCode,@USPPIEmail,@IsSoldEnRoute,@ConsigneeTypeId,@UltimateIdNumberTypeId,@UltimateIdNumber,@UltimateCompanyName,@UltimateFirstName,@UltimateLastName,@UltimatePhoneNumber,@UltimateAddressLine1,@UltimateAddressLine2, @UltimateCountryId,@UltimateCountryCode, @UltimatePostalCodeId,@UltimateCity, @UltimateStateId,@UltimateStateCode,@UltimateEmail,@InterIdNumberTypeId,@InterIdNumber,@InterCompanyName,@InterFirstName,@InterLastName,@InterPhoneNumber,@InterAddressLine1,@InterAddressLine2,@InterCountryId,@InterCountryCode,@InterPostalCodeId,@InterCity,@InterStateId,@InterStateCode,@InterEmail,@FreightIdNumberTypeId,@FreightIdNumber,@FreightCompanyName,@FreightFirstName,@FreightLastName,@FreightPhoneNumber,@FreightAddressLine1,@FreightAddressLine2,@FreightPostalCodeId,@FreightCity,@FreightStateId,@FreightStateCode,@FreightEmail,@CarrierSCAC,@ConveyanceName,@TransportationReferenceNumber,@AES_FILENAME,@AESTYPE,@CreatedBy,@ModifiedBy,@ExporterId,@ExporterFirstName,@ExporterLastName,@ExporterPhoneNumber,@ExporterCountryId,@ExporterCountryCode,@ExporterStateId,@ExporterStateCode,@ExporterCity,@ExporterPostalCodeId,@ExporterAddress,@ExporterAddress2,@ExporterEmail,@etype,@isuploaded,@isDraft,@isFreeze,@FreezeRemarks,@IsTemplate", obj).ToList();
                List<int> list = source.ToList();
                int num = list[0];
                if (num > 0)
                {
                    if (objees.EquipmentLineDetail.Length > 0)
                    {
                        for (int i = 0; i < objees.EquipmentLineDetail.Length; i++)
                        {
                            if (objees.EquipmentLineDetail[i].EquipmentNumber != "")
                            {
                                object[] objLine = new object[7]
                                     {
                                 new SqlParameter("@LineNo", objees.EquipmentLineDetail[i].LineNo?? Convert.DBNull),
                                 new SqlParameter("@EquipmentNumber", objees.EquipmentLineDetail[i].EquipmentNumber?? Convert.DBNull),
                                 new SqlParameter("@SealNumber", objees.EquipmentLineDetail[i].SealNumber?? Convert.DBNull),
                                 new SqlParameter("@DocumentCommonId", objees.DocumentCommonID ?? Convert.DBNull),
                                 new SqlParameter("@SiteId", objees.EquipmentLineDetail[i].SiteId?? Convert.DBNull),
                                 new SqlParameter("@AesId", num.ToString()?? Convert.DBNull),
                                 new SqlParameter("@CreatedBY", objees.CreatedBy?? Convert.DBNull ),

                                      };

                                _context.ExecuteQuery("EXEC dbo.AES_EquipmentLineDetail_SP @LineNo,@EquipmentNumber,@SealNumber,@DocumentCommonId,@SiteId,@AesId,@CreatedBY", objLine).ToList();
                            }
                        }

                    }
                    if (objees.CommodityDetail.Length > 0)
                    {
                        for (int i = 0; i < objees.CommodityDetail.Length; i++)
                        {
                            // if (objees.CommodityDetail[i].ExportCode != "" && objees.CommodityDetail[i].ScheduleB != "" && objees.CommodityDetail[i].IsType != "D")
                            if (objees.CommodityDetail[i].ExportCode != "" && objees.CommodityDetail[i].ExportCode != "-Select-")
                            {
                                object[] objLine = new object[29]
                                     {
                                 new SqlParameter("@idd", objees.CommodityDetail[i].idd?? Convert.DBNull),
                                 new SqlParameter("@htsCodeId", objees.CommodityDetail[i].htsCodeId?? Convert.DBNull),
                                 new SqlParameter("@ExportCode", objees.CommodityDetail[i].ExportCode?? Convert.DBNull),
                                 new SqlParameter("@ScheduleB", objees.CommodityDetail[i].ScheduleB?? Convert.DBNull),
                                 new SqlParameter("@FirstQuantity", objees.CommodityDetail[i].FirstQuantity?? Convert.DBNull),
                                 new SqlParameter("@FirstUOM", objees.CommodityDetail[i].FirstUOM?? Convert.DBNull),
                                 new SqlParameter("@SecondQuantity", objees.CommodityDetail[i].SecondQuantity?? Convert.DBNull),
                                 new SqlParameter("@SecondUOM", objees.CommodityDetail[i].SecondUOM?? Convert.DBNull),
                                 new SqlParameter("@OriginofGoods", objees.CommodityDetail[i].OriginofGoods?? Convert.DBNull),
                                 new SqlParameter("@ValueofGoods", objees.CommodityDetail[i].ValueofGoods?? Convert.DBNull),
                                 new SqlParameter("@ShippingWeight", objees.CommodityDetail[i].ShippingWeight?? Convert.DBNull),
                                 new SqlParameter("@Eccn", objees.CommodityDetail[i].Eccn?? Convert.DBNull),
                                 new SqlParameter("@LicenseTypeCode", objees.CommodityDetail[i].LicenseTypeCode?? Convert.DBNull),
                                 new SqlParameter("@ExpLic", objees.CommodityDetail[i].ExpLic?? Convert.DBNull),
                                 new SqlParameter("@IsGovermentAgency", objees.CommodityDetail[i].IsGovermentAgency?? Convert.DBNull),
                                 new SqlParameter("@CommodityDescription", objees.CommodityDetail[i].CommodityDescription?? Convert.DBNull),
                                 new SqlParameter("@DocumentCommonId", objees.DocumentCommonID ?? Convert.DBNull),
                                 new SqlParameter("@SiteId", objees.CommodityDetail[i].SiteId?? Convert.DBNull),
                                 new SqlParameter("@AesId", num.ToString()?? Convert.DBNull),
                                new SqlParameter("@DDTCITAR", objees.CommodityDetail[i].dDTCITAR==null?"": objees.CommodityDetail[i].dDTCITAR),
                                new SqlParameter("@DDTCReg", objees.CommodityDetail[i].dDTCReg==null?"": objees.CommodityDetail[i].dDTCReg),
                                new SqlParameter("@DDTCSignificant", objees.CommodityDetail[i].dDTCSignificant==null?"": objees.CommodityDetail[i].dDTCSignificant),
                                new SqlParameter("@DDTCEligible", objees.CommodityDetail[i].dDTCEligible==null?"": objees.CommodityDetail[i].dDTCEligible),
                                new SqlParameter("@DDTCUSML", objees.CommodityDetail[i].dDTCUSML==null?"": objees.CommodityDetail[i].dDTCUSML),
                                new SqlParameter("@DDTCUnit", objees.CommodityDetail[i].dDTCUnit==null?"": objees.CommodityDetail[i].dDTCUnit),
                                new SqlParameter("@DDTCQuantity", objees.CommodityDetail[i].dDTCQuantity==null?"": objees.CommodityDetail[i].dDTCQuantity),
                                new SqlParameter("@DDTCLicense", objees.CommodityDetail[i].dDTCLicense==null?"": objees.CommodityDetail[i].dDTCLicense),
                                new SqlParameter("@ctype", objees.CommodityDetail[i].ctype?? Convert.DBNull),
                                new SqlParameter("@LicValueAmount", objees.CommodityDetail[i].LicValueAmount?? Convert.DBNull),

                                  };

                                _context.ExecuteQuery("EXEC dbo.AES_CommodityLineDetail_SP @idd,@htsCodeId,@ExportCode,@ScheduleB,@FirstQuantity,@FirstUOM,@SecondQuantity,@SecondUOM,@OriginofGoods,@ValueofGoods,@ShippingWeight,@Eccn,@LicenseTypeCode,@ExpLic,@IsGovermentAgency,@CommodityDescription,@DocumentCommonId,@SiteId,@AesId,@DDTCITAR,@DDTCReg,@DDTCSignificant,@DDTCEligible,@DDTCUSML,@DDTCUnit,@DDTCQuantity,@DDTCLicense,@ctype,@LicValueAmount", objLine).ToList();

                            }

                        }
                    }
                    if (objees.VinDetail.Length > 0)
                    {
                        for (int i = 0; i < objees.VinDetail.Length; i++)
                        {
                            if (objees.VinDetail[i].VINNumber != "")
                            {
                                object[] objvin = new object[9]
                                     {
                                 new SqlParameter("@htsCodeId", objees.VinDetail[i].HtsCodeId?? Convert.DBNull),
                                 new SqlParameter("@VIN", objees.VinDetail[i].VIN?? Convert.DBNull),
                                 new SqlParameter("@VINNumber", objees.VinDetail[i].VINNumber?? Convert.DBNull),
                                 new SqlParameter("@VehicleTitleNum", objees.VinDetail[i].VehicleTitleNum?? Convert.DBNull),
                                 new SqlParameter("@VehicleTitleState", objees.VinDetail[i].VehicleTitleState?? Convert.DBNull),
                                 new SqlParameter("@SiteId", objees.VinDetail[i].SiteId?? Convert.DBNull),
                                 new SqlParameter("@DocumentCommonId", objees.DocumentCommonID ?? Convert.DBNull),
                                 new SqlParameter("@AesId", num.ToString()?? Convert.DBNull),
                                 new SqlParameter("@Line_No", objees.VinDetail[i].Line_No?? Convert.DBNull)

                                  };

                                _context.ExecuteQuery("EXEC dbo.AES_VINDetail_SP @htsCodeId,@VIN,@VINNumber,@VehicleTitleNum,@VehicleTitleState,@SiteId,@DocumentCommonId,@AesId,@Line_No", objvin).ToList();

                            }

                        }
                    }

                    GenerateFile_EES(objees, fileName);


                }
                return AppResult(list, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        private void GenerateFile_EES(AppMGL.DTO.Report.EESSubmission objees, string fileName)
        {


            string partytype = "u";
            string FinalStr = "";
            if (objees.AESTYPE.ToUpper()!="X")
             FinalStr = GenerateAStringAndZString(objees, "A") + GenerateBStringANDYString(objees, "B") + GenerateClassSC1(objees, objees.AESTYPE) + GenerateClassSC2(objees) + GenerateClassSC3(objees) + GenerateClassN01(objees, "u") + GenerateClassN02(objees, "u") + GenerateClassN03(objees, "u") + GenerateClassN01(objees, "uc") + GenerateClassN02(objees, "uc") + GenerateClassN03(objees, "uc") + GenerateClassN01(objees, "ic") + GenerateClassN02(objees, "ic") + GenerateClassN03(objees, "ic") + GenerateClassN01(objees, "ff") + GenerateClassN02(objees, "ff") + GenerateClassN03(objees, "ff") + GenerateClassC01C02EV(objees) + GenerateBStringANDYString(objees, "Y") + GenerateAStringAndZString(objees, "Z");
             else
                FinalStr = GenerateAStringAndZString(objees, "A") + GenerateBStringANDYString(objees, "B") + GenerateClassSC1(objees, objees.AESTYPE) + GenerateBStringANDYString(objees, "Y") + GenerateAStringAndZString(objees, "Z");
            //   string fileName = objees.shipmentNumber + objees.Id + DateTime.Now.ToString("yyyymmdd") + ".txt";
            string path = ConfigurationManager.AppSettings["DocumentsPath"] + "\\AES\\";
            string phypath = HttpContext.Current.Server.MapPath(path) + fileName;
            File.WriteAllText(phypath, FinalStr);

        }
        // string generation
        private string GenerateAStringAndZString(AppMGL.DTO.Report.EESSubmission objees, string type)
        {
            StringBuilder sba = new StringBuilder();

            sba.Append(type);
            sba.Append(' ', 4);
            string transmitterid = "";




            if (!string.IsNullOrEmpty(objees.FreightIdNumber))
            {
                objees.FreightIdNumber= RemoveAllSpecialCharacters(objees.FreightIdNumber);
                if (objees.FreightIdNumber.Length > 9)
                {
                    sba.Append(objees.FreightIdNumber.Substring(0, 9).ToUpper());
                    //  transmitterid = objees.USPPIIDNumber.Substring(0, 9);
                }
                else
                {
                    sba.Append(objees.FreightIdNumber.ToUpper());
                    sba.Append(' ', 9 - objees.FreightIdNumber.Length);
                    //transmitterid = objees.USPPIIDNumber.PadRight((9), ' ');
                }
            }
            else
            {
                sba.Append(' ', 9);
            }
            sba.Append(' ', 6);

            if (!string.IsNullOrEmpty(objees.USPPIIDNumberTypeId))
            {
                objees.USPPIIDNumberTypeId= RemoveAllSpecialCharacters(objees.USPPIIDNumberTypeId);
                if (objees.USPPIIDNumberTypeId.ToLower().IndexOf("d") > -1)
                {
                    sba.Append("D");
                }
                else if (objees.USPPIIDNumberTypeId.ToLower().IndexOf("s") > -1)
                {
                    sba.Append("S");

                }
                else if (objees.USPPIIDNumberTypeId.ToLower().IndexOf("e") > -1)
                {
                    sba.Append("E");

                }
                else
                {
                    sba.Append(objees.USPPIIDNumberTypeId.ToUpper());
                }
            }
            else
            {
                sba.Append(" ");
            }
            sba.Append("XP");
            sba.Append(DateTime.Now.ToString("yyyyMMdd"));
            sba.Append(' ', 6); // Batch controll numer
            sba.Append(' ', 1);
            if (!string.IsNullOrEmpty(objees.FreightIdNumber))
            {
                objees.FreightIdNumber=RemoveAllSpecialCharacters(objees.FreightIdNumber);
                if (objees.FreightIdNumber.Length > 9)
                {
                    sba.Append(objees.FreightIdNumber.Substring(0, 9).ToUpper());
                    //transmitterid = objees.FreightIdNumber.Substring(0, 9);
                }
                else
                {
                    sba.Append(objees.FreightIdNumber.ToUpper());
                    sba.Append(' ', 9 - objees.FreightIdNumber.Length);
                    //  transmitterid = objees.FreightIdNumber.PadRight((9), ' ');
                }
            }
            else
            {
                sba.Append(' ', 9);
            }


            //sba.Append(transmitterid);
            sba.Append(' ', 33);
            return sba.ToString();
        }
        private string GenerateBStringANDYString(AppMGL.DTO.Report.EESSubmission objees, string type)
        {
            StringBuilder sbb = new StringBuilder();
            sbb.Append(type);
            sbb.Append(' ', 2);
            string USPPIIDNumber = "";
            if (!string.IsNullOrEmpty(objees.USPPIIDNumber))
            {
                objees.USPPIIDNumber=RemoveAllSpecialCharacters(objees.USPPIIDNumber);

                //  if (objees.USPPIIDNumber.Length == 9)
                //  {
                //      sbb.Append(objees.USPPIIDNumber);
                //      sbb.Append(' ',2);
                //      USPPIIDNumber = objees.USPPIIDNumber;
                //  }
                // else if (objees.USPPIIDNumber.Length == 10)
                //  {
                //      sbb.Append(objees.USPPIIDNumber);
                //      sbb.Append(' ', 1);
                //      USPPIIDNumber = objees.USPPIIDNumber ;
                //  }



                //else  
                if (objees.USPPIIDNumber.Length > 11)
                {
                    sbb.Append(objees.USPPIIDNumber.Substring(0, 11).ToUpper());
                    USPPIIDNumber = objees.USPPIIDNumber.Substring(0, 11).ToUpper();
                }
                else if (objees.USPPIIDNumber.Length == 11)
                {
                    sbb.Append(objees.USPPIIDNumber.ToUpper());
                    USPPIIDNumber = objees.USPPIIDNumber.ToUpper();
                }
                else
                {
                    sbb.Append(objees.USPPIIDNumber.ToUpper());
                    sbb.Append('0', 11 - objees.USPPIIDNumber.Length);
                    USPPIIDNumber = objees.USPPIIDNumber.PadRight((11), '0').ToUpper();
                }
            }
            else
            {
                sbb.Append('0', 11);
            }
            // sbb.Append('0', 2);

            if (!string.IsNullOrEmpty(objees.USPPIIDNumberTypeId))
            {
                objees.USPPIIDNumberTypeId=RemoveAllSpecialCharacters(objees.USPPIIDNumberTypeId);
                if (objees.USPPIIDNumberTypeId.ToLower().IndexOf("d") > -1)
                {
                    sbb.Append("D");
                }
                else if (objees.USPPIIDNumberTypeId.ToLower().IndexOf("s") > -1)
                {
                    sbb.Append("S");
                }
                else if (objees.USPPIIDNumberTypeId.ToLower().IndexOf("e") > -1)
                {
                    sbb.Append("E");
                }
                else if (objees.USPPIIDNumberTypeId.ToLower().IndexOf("t") > -1)
                {
                    sbb.Append("T");
                }

            }
            else
            {
                sbb.Append(' ');
            }
            sbb.Append(' ', 10);
            if (!string.IsNullOrEmpty(objees.USPPICompanyName))
            {
                objees.USPPICompanyName=RemoveAllSpecialCharacters(objees.USPPICompanyName);
                if (objees.USPPICompanyName.Length > 30)
                {
                    sbb.Append(objees.USPPICompanyName.Substring(0, 30).ToUpper());
                }
                else if (objees.USPPICompanyName.Length == 30)
                {
                    sbb.Append(objees.USPPICompanyName.ToUpper());
                }
                else
                {
                    sbb.Append(objees.USPPICompanyName.ToUpper());
                    sbb.Append(' ', 30 - objees.USPPICompanyName.Length);
                }
            }
            else
            {
                sbb.Append(' ', 30);
            }

            sbb.Append(' ', 25);
            return sbb.ToString();
        }
        private string GenerateClassSC1(AppMGL.DTO.Report.EESSubmission objees, string shipmentFillingAction)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SC1");
            if (objees.IsUltimateConsigneeCompanies.ToLower() == "yes")
            {
                sb.Append("Y");
            }
            else
            {
                sb.Append("N");
            }

            if (!string.IsNullOrEmpty(objees.ModeofTransport)) // 2AN
            {
                sb.Append(objees.ModeofTransport.ToUpper());
            }

            // country code /   // Country of Destination


            var resultCountry = _context.ExecuteQuery<SIPL_Country>("EXEC dbo.AES_GETCountryStateCode_SP @name,@type",
                    new SqlParameter("name", objees.CountryOfDestination),
                    new SqlParameter("type", 1)
                    ).FirstOrDefault();
            if (resultCountry != null)
            {
                sb.Append(resultCountry.CountryCode.ToUpper());
            }
            else
            {
                sb.Append(' ', 2);
            }

            // state code  //    // Origin State
            if (!string.IsNullOrEmpty(objees.OriginState))
            {
                var resultState = _context.ExecuteQuery<SIPL_State>("EXEC dbo.AES_GETCountryStateCode_SP @name,@type",
                   new SqlParameter("name", objees.OriginState),
                   new SqlParameter("type", 2)
                   ).FirstOrDefault();
                if (resultState != null)
                {
                    sb.Append(resultState.StateCode.ToUpper());
                }
                else
                {
                    sb.Append(' ', 2);
                }
            }
            else
            {
                sb.Append(' ', 2);
            }

            if (!string.IsNullOrEmpty(objees.CarrierSCAC))
            {
                objees.CarrierSCAC=RemoveAllSpecialCharacters(objees.CarrierSCAC);
                if (objees.CarrierSCAC.Length > 4)
                {
                    sb.Append(objees.CarrierSCAC.Substring(0, 4).ToUpper());
                }
                else if (objees.CarrierSCAC.Length < 4)
                {
                    sb.Append(objees.CarrierSCAC.ToUpper());
                    sb.Append(' ', 4 - objees.CarrierSCAC.Length);
                }
                else
                {
                    sb.Append(objees.CarrierSCAC.ToUpper());
                }
            }
            else
            {
                sb.Append(' ', 4);
            }
            if (!string.IsNullOrEmpty(objees.shipmentNumber))
            {
                objees.shipmentNumber=RemoveAllSpecialCharacters(objees.shipmentNumber);
                if (objees.shipmentNumber.Length > 17)
                {
                    sb.Append(objees.shipmentNumber.Substring(0, 17).ToUpper());
                }
                else if (objees.shipmentNumber.Length < 17)
                {
                    sb.Append(objees.shipmentNumber.ToUpper());
                    sb.Append(' ', 17 - objees.shipmentNumber.Length);
                }
                else
                {
                    sb.Append(objees.shipmentNumber.ToUpper());
                }
            }
            else
            {
                sb.Append(' ', 17);
            }

            sb.Append(shipmentFillingAction.ToUpper());

            // C - Change , R - Replace , X- Cancel ;

            if (!string.IsNullOrEmpty(objees.ConveyanceName))
            {
                objees.ConveyanceName = RemoveAllSpecialCharacters(objees.ConveyanceName);
                if (objees.ConveyanceName.Length > 23)
                {
                    sb.Append(objees.ConveyanceName.Substring(0, 23).ToUpper());
                }
                else if (objees.ConveyanceName.Length < 23)
                {
                    sb.Append(objees.ConveyanceName.ToUpper());
                    sb.Append(' ', 23 - objees.ConveyanceName.Length);
                }
                else
                {
                    sb.Append(objees.ConveyanceName.ToUpper());
                }
            }
            else
            {
                sb.Append(' ', 23);
            }

            if (!string.IsNullOrEmpty(objees.FilingOption))
            {
                if (objees.FilingOption.ToLower().IndexOf("1") > -1)
                {
                    sb.Append("2");
                }
                else if (objees.FilingOption.ToLower().IndexOf("2") > -1)
                {
                    sb.Append("3");
                }
                else if (objees.FilingOption.ToLower().IndexOf("3") > -1)
                {
                    sb.Append("4");
                }
            }
            else
            {
                sb.Append(' ', 1);
            }
            if (!string.IsNullOrEmpty(objees.FilingOption) && objees.FilingOption.ToLower().IndexOf("1") > -1)
            {

                sb.Append(' ');  // AEI Filling Type
            }
            else if (!string.IsNullOrEmpty(objees.FilingOption) && objees.FilingOption.ToLower().IndexOf("2") > -1)
            {

                sb.Append('P');  // AEI Filling Type
            }
            else if (!string.IsNullOrEmpty(objees.FilingOption) && objees.FilingOption.ToLower().IndexOf("3") > -1)
            {

                sb.Append(' ');  // AEI Filling Type
            }
            else
            {
                sb.Append('F');
            }

            if (!string.IsNullOrEmpty(objees.PortofUnloadingCode))
            {
                //var resultportUnloading = _context.ExecuteQuery<SIPL_Port>("EXEC dbo.AES_GETCountryStateCode_SP @name,@type",
                //     new SqlParameter("name", objees.PortofUnloading),
                //     new SqlParameter("type", 3)
                //     ).FirstOrDefault();
                if (objees.PortofUnloadingCode != null)
                {
                    string p = objees.PortofUnloadingCode.ToUpper();
                    if (objees.PortofUnloadingCode.Length == 2)
                    {
                        p = p + "  ";
                    }
                    else if (objees.PortofUnloadingCode.Length == 3)
                    {
                        p = p + "   ";
                    }
                    else if (objees.PortofUnloadingCode.Length == 4)
                    {
                        p = p + " ";
                    }

                    sb.Append(p.ToUpper());
                }
                else
                {
                    sb.Append(' ', 5);
                }
            }
            else
            {
                sb.Append(' ', 5);
            }

            if (!string.IsNullOrEmpty(objees.PortofExportCode))
            {

                //var resultportExportg = _context.ExecuteQuery<SIPL_Port>("EXEC dbo.AES_GETCountryStateCode_SP @name,@type",
                //    new SqlParameter("name", objees.PortofExport),
                //    new SqlParameter("type", 3)
                //    ).FirstOrDefault();
                if (objees.PortofExportCode != null)
                {
                    string p = objees.PortofExportCode.ToUpper();
                    if (objees.PortofExportCode.Length == 2)
                    {
                        p = p + "  ";
                    }
                    else if (objees.PortofExportCode.Length == 3)
                    {
                        p = p + "   ";
                    }
                    else if (objees.PortofExportCode.Length == 4)
                    {

                    }


                    sb.Append(p.ToUpper());
                }
                else
                {
                    sb.Append(' ', 4);
                }
            }
            else
            {
                sb.Append(' ', 4);
            }



            if (!string.IsNullOrEmpty(objees.DepartureDate))
            {
                if (objees.DepartureDate.IndexOf('-') > 0)
                {

                    sb.Append(objees.DepartureDate.Split('-')[2]);
                    sb.Append(objees.DepartureDate.Split('-')[0]);
                    sb.Append(objees.DepartureDate.Split('-')[1]);
                }
                else if (objees.DepartureDate.IndexOf('/') > 0)
                {
                    sb.Append(objees.DepartureDate.Split('/')[2]);
                    sb.Append(objees.DepartureDate.Split('/')[0]);
                    sb.Append(objees.DepartureDate.Split('/')[1]);
                }
            }
            else
            {
                sb.Append(' ', 8);
            }
            sb.Append(' ', 1); // Filler
            if (!string.IsNullOrEmpty(objees.IsHazardousMaterial))
            {
                if (objees.IsHazardousMaterial.ToLower() == "y")
                {
                    sb.Append("Y");
                }
                else
                {
                    sb.Append("N");
                }
            }
            else { sb.Append("N"); }
            sb.Append(' ', 4);


            return sb.ToString();
        }
        private string GenerateClassSC2(AppMGL.DTO.Report.EESSubmission objees)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SC2");
            sb.Append("70");// Inbond code
            sb.Append(' ', 15); // entry number
            sb.Append(' ', 9); // f trade zone
            sb.Append(' ', 11); // Filler
            if (objees.IsRoutedTransaction.ToLower() == "y")
            {
                sb.Append("Y");
            }
            else
            {
                sb.Append("N");

            }

            if (!string.IsNullOrEmpty(objees.OriginalITN))
            {
                objees.OriginalITN = RemoveAllSpecialCharacters(objees.OriginalITN);
                if (objees.OriginalITN.Length > 15)
                {
                    sb.Append(objees.OriginalITN.Substring(0, 15).ToUpper());
                }
                else if (objees.OriginalITN.Length < 15)
                {
                    sb.Append(objees.OriginalITN.ToUpper());
                    sb.Append(' ', 15 - objees.OriginalITN.Length);
                }
                else
                {
                    sb.Append(objees.OriginalITN.ToUpper());
                }
            }
            else
            {
                sb.Append(' ', 15);
            }
            sb.Append(' ', 24);
            return sb.ToString();

        }
        private string GenerateClassSC3(AppMGL.DTO.Report.EESSubmission objees)
        {



            StringBuilder sb = new StringBuilder();

            Logger.WriteInfo("GenerateClassSC3");
            if (objees.EquipmentLineDetail != null)
            {
                Logger.WriteInfo("GenerateClassSC3 inside if");

                foreach (var item in objees.EquipmentLineDetail)
                {
                    Logger.WriteInfo("GenerateClassSC3 inside loop");

                    sb.Append("SC3");
                    string eqnum = "";
                    string sealnum = "";
                    if (!string.IsNullOrEmpty(item.EquipmentNumber))
                    {
                        item.EquipmentNumber = RemoveAllSpecialCharacters(item.EquipmentNumber);
                        eqnum = item.EquipmentNumber.ToString().ToUpper();
                    }
                    if (!string.IsNullOrEmpty(item.SealNumber))
                    {
                        item.SealNumber = RemoveAllSpecialCharacters(item.SealNumber);
                        sealnum = item.SealNumber.ToString().ToUpper();
                    }


                    Logger.WriteInfo("GenerateClassSC3 eqnum " + eqnum);

                    Logger.WriteInfo("GenerateClassSC3 sealnum  " + sealnum);

                    if (!string.IsNullOrEmpty(eqnum))
                    {
                        if (eqnum.Length > 14)
                        {
                            sb.Append(eqnum.Substring(0, 14).ToUpper());
                        }
                        else if (eqnum.Length < 14)
                        {
                            sb.Append(eqnum.ToUpper());
                            sb.Append(' ', 14 - eqnum.Length);
                        }
                        else
                        {
                            sb.Append(eqnum.ToUpper());
                        }
                    }
                    else
                    {
                        sb.Append(' ', 14);
                    }

                    if (!string.IsNullOrEmpty(sealnum))
                    {
                        if (sealnum.Length > 15)
                        {
                            sb.Append(sealnum.Substring(0, 15).ToUpper());
                        }
                        else if (sealnum.Length < 15)
                        {
                            sb.Append(sealnum.ToUpper());
                            sb.Append(' ', 15 - sealnum.Length);
                        }
                        else
                        {
                            sb.Append(sealnum.ToUpper());
                        }
                    }
                    else
                    {
                        sb.Append(' ', 15);
                    }


                    if (!string.IsNullOrEmpty(objees.TransportationReferenceNumber))
                    {
                        objees.TransportationReferenceNumber = RemoveAllSpecialCharacters(objees.TransportationReferenceNumber);
                        if (objees.TransportationReferenceNumber.Length > 30)
                        {
                            sb.Append(objees.TransportationReferenceNumber.Substring(0, 30).ToUpper());
                        }
                        else if (objees.TransportationReferenceNumber.Length < 30)
                        {
                            sb.Append(objees.TransportationReferenceNumber.ToUpper());
                            sb.Append(' ', 30 - objees.TransportationReferenceNumber.Length);
                        }
                        else
                        {
                            sb.Append(objees.TransportationReferenceNumber.ToUpper());
                        }
                    }
                    else
                    {
                        sb.Append(' ', 30);
                    }
                    sb.Append(' ', 18); // Filler
                }

            }
            Logger.WriteInfo("GenerateClassSC3 end");
            return sb.ToString();

        }
        private string GenerateClassN01(AppMGL.DTO.Report.EESSubmission objees, string partytype)
        {
            StringBuilder sb = new StringBuilder(); // usppi

            if (partytype == "u")
            {
                sb.Append("N01");
                if (!string.IsNullOrEmpty(objees.USPPIIDNumber))
                {
                    objees.USPPIIDNumber = RemoveAllSpecialCharacters(objees.USPPIIDNumber);
                    objees.USPPIIDNumber = objees.USPPIIDNumber.TrimStart().TrimEnd();
                    if (objees.USPPIIDNumber.Length > 11)
                    {
                        sb.Append(objees.USPPIIDNumber.Substring(0, 11).ToUpper());
                    }
                    else if (objees.USPPIIDNumber.Length < 11)
                    {
                        sb.Append(objees.USPPIIDNumber.ToUpper());
                        sb.Append(' ', 11 - objees.USPPIIDNumber.Length);
                    }
                    else
                    {
                        sb.Append(objees.USPPIIDNumber.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 11);
                }

                if (!string.IsNullOrEmpty(objees.USPPIIDNumberTypeId))
                {
                    if (objees.USPPIIDNumberTypeId.ToLower().IndexOf("d") > -1)
                    {
                        sb.Append("D");
                    }
                    else if (objees.USPPIIDNumberTypeId.ToLower().IndexOf("s") > -1)
                    {
                        sb.Append("S");
                    }
                    else if (objees.USPPIIDNumberTypeId.ToLower().IndexOf("e") > -1)
                    {
                        sb.Append("E");
                    }
                    else if (objees.USPPIIDNumberTypeId.ToLower().IndexOf("t") > -1)
                    {
                        sb.Append("T");
                    }

                }
                else
                {
                    sb.Append(' ');
                }

                sb.Append("E");
                if (!string.IsNullOrEmpty(objees.USPPICompanyName))
                {
                    objees.USPPICompanyName = RemoveAllSpecialCharacters(objees.USPPICompanyName);
                    objees.USPPICompanyName = objees.USPPICompanyName.TrimEnd().TrimStart();
                    if (objees.USPPICompanyName.Length > 30)
                    {
                        sb.Append(objees.USPPICompanyName.Substring(0, 30).ToUpper());
                    }
                    else if (objees.USPPICompanyName.Length < 30)
                    {
                        sb.Append(objees.USPPICompanyName.ToUpper());
                        sb.Append(' ', 30 - objees.USPPICompanyName.Length);
                    }
                    else
                    {
                        sb.Append(objees.USPPICompanyName.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 30);
                }

                if (!string.IsNullOrEmpty(objees.USPPIFirstName))
                {
                    objees.USPPIFirstName = RemoveAllSpecialCharacters(objees.USPPIFirstName);
                    objees.USPPIFirstName = objees.USPPIFirstName.TrimStart().TrimEnd();

                    if (objees.USPPIFirstName.Length > 13)
                    {
                        sb.Append(objees.USPPIFirstName.Substring(0, 13).ToUpper());
                    }
                    else if (objees.USPPIFirstName.Length < 13)
                    {
                        sb.Append(objees.USPPIFirstName.ToUpper());
                        sb.Append(' ', 13 - objees.USPPIFirstName.Length);
                    }
                    else
                    {
                        sb.Append(objees.USPPIFirstName.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 13);
                }

                if (!string.IsNullOrEmpty(objees.USPPILastName))
                {
                    objees.USPPILastName = RemoveAllSpecialCharacters(objees.USPPILastName);
                    objees.USPPILastName = objees.USPPILastName.TrimStart().TrimEnd();

                    if (objees.USPPILastName.Length > 20)
                    {
                        sb.Append(objees.USPPILastName.Substring(0, 20).ToUpper());
                    }
                    else if (objees.USPPILastName.Length < 20)
                    {
                        sb.Append(objees.USPPILastName.ToUpper());
                        sb.Append(' ', 20 - objees.USPPILastName.Length);
                    }
                    else
                    {
                        sb.Append(objees.USPPILastName.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 20);
                }
                if (!string.IsNullOrEmpty(objees.IsRoutedTransaction))
                {
                    sb.Append(" ");


                }
                else
                {
                    sb.Append(' ');
                }
            }
            // ultimate c
            else if (partytype == "uc")
            {
                sb.Append("N01");
                if (!string.IsNullOrEmpty(objees.UltimateIdNumber))
                {
                    objees.UltimateIdNumber = RemoveAllSpecialCharacters(objees.UltimateIdNumber);
                    objees.UltimateIdNumber = objees.UltimateIdNumber.TrimEnd().TrimStart();

                    if (objees.UltimateIdNumber.Length > 11)
                    {
                        sb.Append(objees.UltimateIdNumber.Substring(0, 11).ToUpper());
                    }
                    else if (objees.UltimateIdNumber.Length < 11)
                    {
                        sb.Append(objees.UltimateIdNumber.ToUpper());
                        sb.Append(' ', 11 - objees.UltimateIdNumber.Length);
                    }
                    else
                    {
                        sb.Append(objees.UltimateIdNumber.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 11);
                }

                if (!string.IsNullOrEmpty(objees.UltimateIdNumberTypeId))
                {

                    if (!string.IsNullOrEmpty(objees.UltimateIdNumber))
                    {
                        if (objees.UltimateIdNumberTypeId.ToLower().IndexOf("d") > -1)
                        {
                            sb.Append("D");
                        }
                        else if (objees.UltimateIdNumberTypeId.ToLower().IndexOf("s") > -1)
                        {
                            sb.Append("S");
                        }
                        else if (objees.UltimateIdNumberTypeId.ToLower().IndexOf("e") > -1)
                        {
                            sb.Append("E");
                        }
                        else if (objees.UltimateIdNumberTypeId.ToLower().IndexOf("t") > -1)
                        {
                            sb.Append("T");
                        }
                    }
                    else
                    {
                        sb.Append(' ');
                    }

                }
                else
                {
                    sb.Append(' ');
                }

                sb.Append("C");
                if (!string.IsNullOrEmpty(objees.UltimateCompanyName))
                {
                    objees.UltimateCompanyName=RemoveAllSpecialCharacters(objees.UltimateCompanyName);
                    objees.UltimateCompanyName = objees.UltimateCompanyName.Trim().TrimStart().TrimEnd();
                    if (objees.UltimateCompanyName.Length > 30)
                    {
                        sb.Append(objees.UltimateCompanyName.Substring(0, 30).ToUpper());
                    }
                    else if (objees.UltimateCompanyName.Length < 30)
                    {
                        sb.Append(objees.UltimateCompanyName.ToUpper());
                        sb.Append(' ', 30 - objees.UltimateCompanyName.Length);
                    }
                    else
                    {
                        sb.Append(objees.UltimateCompanyName.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 30);
                }

                if (!string.IsNullOrEmpty(objees.UltimateFirstName))
                {
                    objees.UltimateFirstName = RemoveAllSpecialCharacters(objees.UltimateFirstName);
                    objees.UltimateFirstName = objees.UltimateFirstName.TrimStart().TrimEnd();

                    if (objees.UltimateFirstName.Length > 13)
                    {
                        sb.Append(objees.UltimateFirstName.Substring(0, 13).ToUpper());
                    }
                    else if (objees.UltimateFirstName.Length < 13)
                    {
                        sb.Append(objees.UltimateFirstName.ToUpper());
                        sb.Append(' ', 13 - objees.UltimateFirstName.Length);
                    }
                    else
                    {
                        sb.Append(objees.UltimateFirstName.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 13);
                }

                if (!string.IsNullOrEmpty(objees.UltimateLastName))
                {
                    objees.UltimateLastName=RemoveAllSpecialCharacters(objees.UltimateLastName);
                    objees.UltimateLastName = objees.UltimateLastName.TrimStart().TrimEnd();
                    if (objees.UltimateLastName.Length > 20)
                    {
                        sb.Append(objees.UltimateLastName.Substring(0, 20).ToUpper());
                    }
                    else if (objees.UltimateLastName.Length < 20)
                    {
                        sb.Append(objees.UltimateLastName.ToUpper());
                        sb.Append(' ', 20 - objees.UltimateLastName.Length);
                    }
                    else
                    {
                        sb.Append(objees.UltimateLastName.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 20);
                }
                if (!string.IsNullOrEmpty(objees.IsSoldEnRoute))
                {
                    if (objees.IsSoldEnRoute.ToLower() == "y")
                    {
                        sb.Append("Y");
                    }

                    else
                    {
                        sb.Append("N");
                    }

                }
                else
                {
                    sb.Append(' ');
                }
            }

            // IC
            else if (partytype == "ic" && !string.IsNullOrEmpty(objees.InterCompanyName))
            {
                
                sb.Append("N01");

                if (!string.IsNullOrEmpty(objees.InterIdNumber))
                {
                    objees.InterIdNumber=RemoveAllSpecialCharacters(objees.InterIdNumber);
                    objees.InterIdNumber = objees.InterIdNumber.TrimStart().TrimEnd();

                    if (objees.InterIdNumber.Length > 11)
                    {
                        sb.Append(objees.InterIdNumber.Substring(0, 11).ToUpper());
                    }
                    else if (objees.InterIdNumber.Length < 11)
                    {
                        sb.Append(objees.InterIdNumber.ToUpper());
                        sb.Append(' ', 11 - objees.InterIdNumber.Length);
                    }
                    else
                    {
                        sb.Append(objees.InterIdNumber.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 11);
                }

                if (!string.IsNullOrEmpty(objees.InterIdNumberTypeId))
                {
                    if (objees.InterIdNumberTypeId.ToLower().IndexOf("d") > -1)
                    {
                        sb.Append("D");
                    }
                    else if (objees.InterIdNumberTypeId.ToLower().IndexOf("s") > -1)
                    {
                        sb.Append("S");
                    }
                    else if (objees.InterIdNumberTypeId.ToLower().IndexOf("e") > -1)
                    {
                        sb.Append("E");
                    }
                    else if (objees.InterIdNumberTypeId.ToLower().IndexOf("t") > -1)
                    {
                        sb.Append("T");
                    }

                }
                else
                {
                    sb.Append(' ');
                }

                sb.Append("I");
                if (!string.IsNullOrEmpty(objees.InterCompanyName))
                {
                    objees.InterCompanyName= RemoveAllSpecialCharacters(objees.InterCompanyName);
                    objees.InterCompanyName = objees.InterCompanyName.TrimStart().TrimEnd();

                    if (objees.InterCompanyName.Length > 30)
                    {
                        sb.Append(objees.InterCompanyName.Substring(0, 30).ToUpper());
                    }
                    else if (objees.InterCompanyName.Length < 30)
                    {
                        sb.Append(objees.InterCompanyName.ToUpper());
                        sb.Append(' ', 30 - objees.InterCompanyName.Length);
                    }
                    else
                    {
                        sb.Append(objees.InterCompanyName.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 30);
                }

                if (!string.IsNullOrEmpty(objees.InterFirstName))
                {
                    objees.InterFirstName=RemoveAllSpecialCharacters(objees.InterFirstName);
                    objees.InterFirstName = objees.InterFirstName.TrimStart().TrimEnd();
                    if (objees.InterFirstName.Length > 13)
                    {
                        sb.Append(objees.InterFirstName.Substring(0, 13).ToUpper());
                    }
                    else if (objees.InterFirstName.Length < 13)
                    {
                        sb.Append(objees.InterFirstName.ToUpper());
                        sb.Append(' ', 13 - objees.InterFirstName.Length);
                    }
                    else
                    {
                        sb.Append(objees.InterFirstName.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 13);
                }

                if (!string.IsNullOrEmpty(objees.InterLastName))
                {
                    objees.InterLastName=RemoveAllSpecialCharacters(objees.InterLastName);
                    objees.InterLastName = objees.InterLastName.TrimStart().TrimEnd();
                    if (objees.InterLastName.Length > 20)
                    {
                        sb.Append(objees.InterLastName.Substring(0, 20).ToUpper());
                    }
                    else if (objees.InterLastName.Length < 20)
                    {
                        sb.Append(objees.InterLastName.ToUpper());
                        sb.Append(' ', 20 - objees.InterLastName.Length);
                    }
                    else
                    {
                        sb.Append(objees.InterLastName.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 20);
                }
                if (!string.IsNullOrEmpty(objees.IsRoutedTransaction))
                {
                    sb.Append(" ");

                    //if (objees.IsRoutedTransaction.ToLower() == "y")
                    //{
                    //    sb.Append("Y");
                    //}

                    //else
                    //{
                    //    sb.Append("N");
                    //}

                }
                else
                {
                    sb.Append(' ');
                }
            }
            // ff
            else if (partytype == "ff")
            {
                sb.Append("N01");
                if (!string.IsNullOrEmpty(objees.FreightIdNumber))
                {
                    objees.FreightIdNumber=RemoveAllSpecialCharacters(objees.FreightIdNumber);
                    objees.FreightIdNumber = objees.FreightIdNumber.TrimStart().TrimEnd();
                    if (objees.FreightIdNumber.Length > 11)
                    {
                        sb.Append(objees.FreightIdNumber.Substring(0, 11).ToUpper());
                    }
                    else if (objees.FreightIdNumber.Length < 11)
                    {
                        sb.Append(objees.FreightIdNumber.ToUpper());
                        sb.Append('0', 11 - objees.FreightIdNumber.Length);
                    }
                    else
                    {
                        sb.Append(objees.FreightIdNumber.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 11);
                }

                if (!string.IsNullOrEmpty(objees.FreightIdNumberTypeId))
                {
                    if (objees.FreightIdNumberTypeId.ToLower().IndexOf("d") > -1)
                    {
                        sb.Append("D");
                    }
                    else if (objees.FreightIdNumberTypeId.ToLower().IndexOf("s") > -1)
                    {
                        sb.Append("S");
                    }
                    else if (objees.FreightIdNumberTypeId.ToLower().IndexOf("e") > -1)
                    {
                        sb.Append("E");
                    }
                    else if (objees.FreightIdNumberTypeId.ToLower().IndexOf("t") > -1)
                    {
                        sb.Append("T");
                    }

                }
                else
                {
                    sb.Append(' ');
                }

                sb.Append("F");
                if (!string.IsNullOrEmpty(objees.FreightCompanyName))
                {
                    objees.FreightCompanyName=RemoveAllSpecialCharacters(objees.FreightCompanyName);
                    objees.FreightCompanyName = objees.FreightCompanyName.TrimStart().TrimEnd();
                    if (objees.FreightCompanyName.Length > 30)
                    {
                        sb.Append(objees.FreightCompanyName.Substring(0, 30).ToUpper());
                    }
                    else if (objees.FreightCompanyName.Length < 30)
                    {
                        sb.Append(objees.FreightCompanyName.ToUpper());
                        sb.Append(' ', 30 - objees.FreightCompanyName.Length);
                    }
                    else
                    {
                        sb.Append(objees.FreightCompanyName.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 30);
                }

                if (!string.IsNullOrEmpty(objees.FreightFirstName))
                {
                    objees.FreightFirstName=RemoveAllSpecialCharacters(objees.FreightFirstName);
                    objees.FreightFirstName = objees.FreightFirstName.TrimStart().TrimEnd();
                    if (objees.FreightFirstName.Length > 13)
                    {
                        sb.Append(objees.FreightFirstName.Substring(0, 13).ToUpper());
                    }
                    else if (objees.FreightFirstName.Length < 13)
                    {
                        sb.Append(objees.FreightFirstName.ToUpper());
                        sb.Append(' ', 13 - objees.FreightFirstName.Length);
                    }
                    else
                    {
                        sb.Append(objees.FreightFirstName.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 13);
                }

                if (!string.IsNullOrEmpty(objees.FreightLastName))
                {
                    objees.FreightLastName=RemoveAllSpecialCharacters(objees.FreightLastName);
                    objees.FreightLastName = objees.FreightLastName.TrimStart().TrimEnd();
                    if (objees.FreightLastName.Length > 20)
                    {
                        sb.Append(objees.FreightLastName.Substring(0, 20).ToUpper());
                    }
                    else if (objees.FreightLastName.Length < 20)
                    {
                        sb.Append(objees.FreightLastName.ToUpper());
                        sb.Append(' ', 20 - objees.FreightLastName.Length);
                    }
                    else
                    {
                        sb.Append(objees.FreightLastName.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 20);
                }
                if (!string.IsNullOrEmpty(objees.IsRoutedTransaction))
                {
                    sb.Append(" ");

                    //if (objees.IsRoutedTransaction.ToLower() == "y")
                    //{
                    //    sb.Append("Y");
                    //}

                    //else
                    //{
                    //    sb.Append("N");
                    //}

                }
                else
                {
                    sb.Append(' ');
                }
            }
            Logger.WriteInfo("End no1");
            return sb.ToString();

        }


        private string GenerateClassN02(AppMGL.DTO.Report.EESSubmission objees, string partytype)
        {
            StringBuilder sb = new StringBuilder(); // usppi

            if (partytype == "u")
            {
                sb.Append("N02");
                if (!string.IsNullOrEmpty(objees.USPPIAddressLine1))
                {
                    objees.USPPIAddressLine1 = RemoveAllSpecialCharacters(objees.USPPIAddressLine1);
                    objees.USPPIAddressLine1 = Regex.Replace(objees.USPPIAddressLine1.TrimStart().TrimEnd(), @"\t|\n|\r", "");

                    if (objees.USPPIAddressLine1.Length > 32)
                    {
                        sb.Append(objees.USPPIAddressLine1.Substring(0, 32).ToUpper());
                    }
                    else if (objees.USPPIAddressLine1.Length < 32)
                    {
                        sb.Append(objees.USPPIAddressLine1.ToUpper());
                        sb.Append(' ', 32 - objees.USPPIAddressLine1.Length);
                    }
                    else
                    {
                        sb.Append(objees.USPPIAddressLine1.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 32);
                }

                if (!string.IsNullOrEmpty(objees.USPPIAddressLine2))
                {
                    objees.USPPIAddressLine2 = RemoveAllSpecialCharacters(objees.USPPIAddressLine2);
                    objees.USPPIAddressLine2 = objees.USPPIAddressLine2.TrimStart().TrimEnd();

                    objees.USPPIAddressLine2 = Regex.Replace(objees.USPPIAddressLine2, @"\t|\n|\r", "");
                    if (objees.USPPIAddressLine2.Length > 32)
                    {
                        sb.Append(objees.USPPIAddressLine2.Substring(0, 32).ToUpper());
                    }
                    else if (objees.USPPIAddressLine2.Length < 32)
                    {
                        sb.Append(objees.USPPIAddressLine2.ToUpper());
                        sb.Append(' ', 32 - objees.USPPIAddressLine2.Length);
                    }
                    else
                    {
                        sb.Append(objees.USPPIAddressLine2.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 32);
                }

                if (!string.IsNullOrEmpty(objees.USPPIPhoneNumber))
                {
                    objees.USPPIPhoneNumber=RemoveAllSpecialCharacters(objees.USPPIPhoneNumber);
                    objees.USPPIPhoneNumber = objees.USPPIPhoneNumber.TrimStart().TrimEnd();
                    if (objees.USPPIPhoneNumber.Length > 13)
                    {
                        sb.Append(objees.USPPIPhoneNumber.Substring(0, 13).Replace("|", "").ToUpper());
                    }
                    else if (objees.USPPIPhoneNumber.Length < 13)
                    {
                        sb.Append(objees.USPPIPhoneNumber.Replace("|", "").ToUpper());
                        sb.Append(' ', 13 - objees.USPPIPhoneNumber.Length);
                    }
                    else
                    {
                        sb.Append(objees.USPPIPhoneNumber.Replace("|", "").ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 13);
                }
            }


            if (partytype == "uc")
            {
                sb.Append("N02");
                if (!string.IsNullOrEmpty(objees.UltimateAddressLine1))
                {
                    objees.UltimateAddressLine1=RemoveAllSpecialCharacters(objees.UltimateAddressLine1);
                    objees.UltimateAddressLine1 = objees.UltimateAddressLine1.TrimStart().TrimEnd();
                    objees.UltimateAddressLine1 = Regex.Replace(objees.UltimateAddressLine1, @"\t|\n|\r", "");


                    if (objees.UltimateAddressLine1.Length > 32)
                    {
                        sb.Append(objees.UltimateAddressLine1.Substring(0, 32).ToUpper());
                    }
                    else if (objees.UltimateAddressLine1.Length < 32)
                    {
                        sb.Append(objees.UltimateAddressLine1.ToUpper());
                        sb.Append(' ', 32 - objees.UltimateAddressLine1.Length);
                    }
                    else
                    {
                        sb.Append(objees.UltimateAddressLine1.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 32);
                }

                if (!string.IsNullOrEmpty(objees.UltimateAddressLine2))
                {
                    objees.UltimateAddressLine2=RemoveAllSpecialCharacters(objees.UltimateAddressLine2);
                    objees.UltimateAddressLine2 = objees.UltimateAddressLine2.TrimStart().TrimEnd();

                    objees.UltimateAddressLine2 = Regex.Replace(objees.UltimateAddressLine2, @"\t|\n|\r", "");
                    if (objees.UltimateAddressLine2.Length > 32)
                    {
                        sb.Append(objees.UltimateAddressLine2.Substring(0, 32).ToUpper());
                    }
                    else if (objees.UltimateAddressLine2.Length < 32)
                    {
                        sb.Append(objees.UltimateAddressLine2.ToUpper());
                        sb.Append(' ', 32 - objees.UltimateAddressLine2.Length);
                    }
                    else
                    {
                        sb.Append(objees.UltimateAddressLine2.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 32);
                }

                if (!string.IsNullOrEmpty(objees.UltimatePhoneNumber))
                {
                    objees.UltimatePhoneNumber=RemoveAllSpecialCharacters(objees.UltimatePhoneNumber);
                    objees.UltimatePhoneNumber = objees.UltimatePhoneNumber.TrimStart().TrimEnd();
                    objees.UltimatePhoneNumber = objees.UltimatePhoneNumber.Replace("|", "").ToUpper();
                    if (objees.UltimatePhoneNumber.Length > 13)
                    {
                        sb.Append(objees.UltimatePhoneNumber.Substring(0, 13).ToUpper());
                    }
                    else if (objees.UltimatePhoneNumber.Length < 13)
                    {
                        sb.Append(objees.UltimatePhoneNumber.ToUpper());
                        sb.Append(' ', 13 - objees.UltimatePhoneNumber.Length);
                    }
                    else
                    {
                        sb.Append(objees.UltimatePhoneNumber.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 13);
                }
            }

            if (partytype == "ic" && !string.IsNullOrEmpty(objees.InterCompanyName))
            {
                sb.Append("N02");
                if (!string.IsNullOrEmpty(objees.InterAddressLine1))
                {
                    objees.InterAddressLine1=RemoveAllSpecialCharacters(objees.InterAddressLine1);
                    objees.InterAddressLine1 = objees.InterAddressLine1.TrimStart().TrimEnd();
                    if (objees.InterAddressLine1.Length > 32)
                    {
                        sb.Append(objees.InterAddressLine1.Substring(0, 32).ToUpper());
                    }
                    else if (objees.InterAddressLine1.Length < 32)
                    {
                        sb.Append(objees.InterAddressLine1.ToUpper());
                        sb.Append(' ', 32 - objees.InterAddressLine1.Length);
                    }
                    else
                    {
                        sb.Append(objees.InterAddressLine1.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 32);
                }

                if (!string.IsNullOrEmpty(objees.InterAddressLine2))
                {
                    objees.InterAddressLine2=RemoveAllSpecialCharacters(objees.InterAddressLine2);
                    objees.InterAddressLine2 = objees.InterAddressLine2.TrimStart().TrimEnd();
                    objees.InterAddressLine2 = Regex.Replace(objees.InterAddressLine2, @"\t|\n|\r", "");

                    if (objees.InterAddressLine2.Length > 32)
                    {
                        sb.Append(objees.InterAddressLine2.Substring(0, 32).ToUpper());
                    }
                    else if (objees.InterAddressLine2.Length < 32)
                    {
                        sb.Append(objees.InterAddressLine2.ToUpper());
                        sb.Append(' ', 32 - objees.InterAddressLine2.Length);
                    }
                    else
                    {
                        sb.Append(objees.InterAddressLine2.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 32);
                }

                if (!string.IsNullOrEmpty(objees.InterPhoneNumber))
                {
                    objees.InterPhoneNumber = RemoveAllSpecialCharacters(objees.InterPhoneNumber);
                    objees.InterPhoneNumber = objees.InterPhoneNumber.TrimStart().TrimEnd();
                    objees.InterPhoneNumber = objees.InterPhoneNumber.Replace("|", "").ToUpper();
                    objees.InterPhoneNumber = objees.InterPhoneNumber.Replace("+91", "");
                    objees.InterPhoneNumber = objees.InterPhoneNumber.Replace("+", "");
                    if (objees.InterPhoneNumber.Length > 13)
                    {
                        sb.Append(objees.InterPhoneNumber.Substring(0, 13).ToUpper());
                    }
                    else if (objees.InterPhoneNumber.Length < 13)
                    {
                        sb.Append(objees.InterPhoneNumber.ToUpper());
                        sb.Append(' ', 13 - objees.InterPhoneNumber.Length);
                    }
                    else
                    {
                        sb.Append(objees.InterPhoneNumber.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 13);
                }
            }

            if (partytype == "ff")
            {
                sb.Append("N02");
                if (!string.IsNullOrEmpty(objees.FreightAddressLine1))
                {
                    objees.FreightAddressLine1=RemoveAllSpecialCharacters(objees.FreightAddressLine1);
                    objees.FreightAddressLine1 = objees.FreightAddressLine1.TrimStart().TrimEnd();
                    objees.FreightAddressLine1 = Regex.Replace(objees.FreightAddressLine1, @"\t|\n|\r", "");

                    if (objees.FreightAddressLine1.Length > 32)
                    {
                        sb.Append(objees.FreightAddressLine1.Substring(0, 32).ToUpper());
                    }
                    else if (objees.FreightAddressLine1.Length < 32)
                    {
                        sb.Append(objees.FreightAddressLine1.ToUpper());
                        sb.Append(' ', 32 - objees.FreightAddressLine1.Length);
                    }
                    else
                    {
                        sb.Append(objees.FreightAddressLine1.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 32);
                }

                if (!string.IsNullOrEmpty(objees.FreightAddressLine2))
                {
                    objees.FreightAddressLine2=RemoveAllSpecialCharacters(objees.FreightAddressLine2);
                    objees.FreightAddressLine2 = objees.FreightAddressLine2.TrimStart().TrimEnd();
                    objees.FreightAddressLine2 = Regex.Replace(objees.FreightAddressLine2, @"\t|\n|\r", "");

                    if (objees.FreightAddressLine2.Length > 32)
                    {
                        sb.Append(objees.FreightAddressLine2.Substring(0, 32).ToUpper());
                    }
                    else if (objees.FreightAddressLine2.Length < 32)
                    {
                        sb.Append(objees.FreightAddressLine2.ToUpper());
                        sb.Append(' ', 32 - objees.FreightAddressLine2.Length);
                    }
                    else
                    {
                        sb.Append(objees.FreightAddressLine2.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 32);
                }

                if (!string.IsNullOrEmpty(objees.FreightPhoneNumber))
                {
                    objees.FreightPhoneNumber = RemoveAllSpecialCharacters(objees.FreightPhoneNumber);
                    objees.FreightPhoneNumber = objees.FreightPhoneNumber.TrimStart().TrimEnd();
                    objees.FreightPhoneNumber = objees.FreightPhoneNumber.Replace("|", "").ToUpper();
                    objees.FreightPhoneNumber = objees.FreightPhoneNumber.Replace("+91", "");
                    objees.FreightPhoneNumber = objees.FreightPhoneNumber.Replace("+", "");

                    if (objees.FreightPhoneNumber.Length > 13)
                    {
                        sb.Append(objees.FreightPhoneNumber.Substring(0, 13).ToUpper());
                    }
                    else if (objees.FreightPhoneNumber.Length < 13)
                    {
                        sb.Append(objees.FreightPhoneNumber.ToUpper());
                        sb.Append(' ', 13 - objees.FreightPhoneNumber.Length);
                    }
                    else
                    {
                        sb.Append(objees.FreightPhoneNumber.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 13);
                }
            }

            Logger.WriteInfo("End no2");
            return sb.ToString();

        }

        private string GenerateClassN03(AppMGL.DTO.Report.EESSubmission objees, string partytype)
        {
            StringBuilder sb = new StringBuilder(); // usppi


            if (partytype == "u")
            {
                sb.Append("N03");
                if (!string.IsNullOrEmpty(objees.USPPICity))
                {
                    objees.USPPICity=RemoveAllSpecialCharacters(objees.USPPICity);
                    objees.USPPICity = objees.USPPICity.TrimStart().TrimEnd();
                    if (objees.USPPICity.Length > 25)
                    {
                        sb.Append(objees.USPPICity.Substring(0, 25).ToUpper());
                    }
                    else if (objees.USPPICity.Length < 25)
                    {
                        sb.Append(objees.USPPICity.ToUpper());
                        sb.Append(' ', 25 - objees.USPPICity.Length);
                    }
                    else
                    {
                        sb.Append(objees.USPPICity.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 25);
                }

                if (!string.IsNullOrEmpty(objees.USPPIStateId))
                {
                    objees.USPPIStateId = RemoveAllSpecialCharacters(objees.USPPIStateId);

                    objees.USPPIStateId = objees.USPPIStateId.TrimStart().TrimEnd();
                    string usppistatecode = "";

                    var resultState = _context.ExecuteQuery<SIPL_State>("EXEC dbo.AES_GETCountryStateCode_SP @name,@type",
                   new SqlParameter("name", objees.USPPIStateId),
                   new SqlParameter("type", 2)
                   ).FirstOrDefault();
                    if (resultState != null)
                    {
                        if (!string.IsNullOrEmpty(resultState.StateCode))
                        {
                            usppistatecode = resultState.StateCode.ToUpper();
                        }
                        else
                        {
                            // sb.Append(' ', 2);
                        }
                    }
                    else
                    {
                        // sb.Append(' ', 2);
                    }


                    if (!string.IsNullOrEmpty(usppistatecode))
                    {

                        if (usppistatecode.Length > 2)
                        {
                            sb.Append(usppistatecode.Substring(0, 2).ToUpper());
                        }
                        else if (usppistatecode.Length < 2)
                        {
                            sb.Append(usppistatecode.ToUpper());
                            sb.Append(' ', 2 - usppistatecode.Length);
                        }
                        else
                        {
                            sb.Append(usppistatecode.ToUpper());
                        }
                    }
                    else
                    {
                        sb.Append(' ', 2);
                    }

                }
                else
                {
                    sb.Append(' ', 2);
                }

                string usppicountrycode = "US";
                var resultCountry = _context.ExecuteQuery<SIPL_Country>("EXEC dbo.AES_GETCountryStateCode_SP @name,@type",
                     new SqlParameter("name", "USA"),
                     new SqlParameter("type", 1)
                     ).FirstOrDefault();
                if (resultCountry != null)
                {
                    if (!string.IsNullOrEmpty(resultCountry.CountryCode))
                    {
                        usppicountrycode = (resultCountry.CountryCode).ToUpper();
                    }

                    else
                        sb.Append(' ', 2);
                }


                if (!string.IsNullOrEmpty(usppicountrycode))
                {


                    if (usppicountrycode.Length > 2)
                    {
                        sb.Append(usppicountrycode.Substring(0, 2).ToUpper());
                    }
                    else if (usppicountrycode.Length < 2)
                    {
                        sb.Append(usppicountrycode.ToUpper());
                        sb.Append(' ', 2 - usppicountrycode.Length);
                    }
                    else
                    {
                        sb.Append(usppicountrycode.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 2);
                }
                string postelcode = "";
                if (!string.IsNullOrEmpty(objees.USPPIPostalCodeId))
                {
                    objees.USPPIPostalCodeId = RemoveAllSpecialCharacters(objees.USPPIPostalCodeId);
                    postelcode = objees.USPPIPostalCodeId.TrimStart().TrimEnd();
                }
                if (!string.IsNullOrEmpty(postelcode))
                {


                    if (postelcode.Length > 9)
                    {
                        sb.Append(postelcode.Substring(0, 9).ToUpper());
                    }
                    else if (postelcode.Length < 9)
                    {
                        sb.Append(postelcode.ToUpper());
                        sb.Append(' ', 9 - postelcode.Length);
                    }
                    else
                    {
                        sb.Append(postelcode.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 9);
                }
                string irsnumber = "";
                if (!string.IsNullOrEmpty(objees.IRSNumber))
                {
                    objees.IRSNumber = RemoveAllSpecialCharacters(objees.IRSNumber);
                    irsnumber = objees.IRSNumber.TrimStart().TrimEnd();
                }
                if (!string.IsNullOrEmpty(irsnumber))
                {


                    if (irsnumber.Length > 9)
                    {
                        sb.Append(irsnumber.Substring(0, 9).ToUpper());
                    }
                    else if (irsnumber.Length < 9)
                    {
                        sb.Append(irsnumber.ToUpper());
                        sb.Append(' ', 9 - irsnumber.Length);
                    }
                    else
                    {
                        sb.Append(irsnumber.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 9);
                }
                string irsidtype = "";
                if(!string.IsNullOrEmpty(objees.USPPIIDNumberTypeId))
                    irsidtype = objees.USPPIIDNumberTypeId.ToUpper().TrimStart().TrimEnd();
                if (!string.IsNullOrEmpty(irsidtype) && !string.IsNullOrEmpty(irsnumber))
                {
                    // if(irsidtype.ToLower().IndexOf("ssn")>-1)
                    // {
                    //    sb.Append("S");
                    //  }
                    // else if(irsidtype.ToLower().IndexOf("ein") > -1)
                    // {
                    //     sb.Append("E");
                    // }  


                    if (irsidtype.Length > 1)
                    {
                        sb.Append(irsidtype.Substring(0, 1).ToUpper());
                    }
                    else
                    {
                        sb.Append(irsidtype.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ');
                }

                string UCType = "";
                if (!string.IsNullOrEmpty(UCType))
                {

                }
                else
                {
                    sb.Append(' ');
                }
                sb.Append(' ', 28);

            }
            ////
            else if (partytype == "uc")
            {
               
                sb.Append("N03");
                if (!string.IsNullOrEmpty(objees.UltimateCity))
                {
                    objees.UltimateCity = RemoveAllSpecialCharacters(objees.UltimateCity);
                    objees.UltimateCity = objees.UltimateCity.TrimStart().TrimEnd();
                    if (objees.UltimateCity.Length > 25)
                    {
                        sb.Append(objees.UltimateCity.Substring(0, 25).ToUpper());
                    }
                    else if (objees.UltimateCity.Length < 25)
                    {
                        sb.Append(objees.UltimateCity.ToUpper());
                        sb.Append(' ', 25 - objees.UltimateCity.Length);
                    }
                    else
                    {
                        sb.Append(objees.UltimateCity.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 25);
                }

                if (!string.IsNullOrEmpty(objees.UltimateStateId))
                {
                    objees.UltimateStateId = RemoveAllSpecialCharacters(objees.UltimateStateId);
                    objees.UltimateStateId = objees.UltimateStateId.TrimStart().TrimEnd();
                    string usppistatecode = "";

                    var resultState = _context.ExecuteQuery<SIPL_State>("EXEC dbo.AES_GETCountryStateCode_SP @name,@type",
                  new SqlParameter("name", objees.UltimateStateId),
                  new SqlParameter("type", 2)
                  ).FirstOrDefault();
                    if (resultState != null)
                    {
                        if (!string.IsNullOrEmpty(resultState.StateCode))
                            usppistatecode = resultState.StateCode.ToUpper();

                    }
                    if (!string.IsNullOrEmpty(usppistatecode))
                    {
                        if (usppistatecode.Length > 2)
                        {
                            sb.Append(usppistatecode.Substring(0, 2));
                        }
                        else if (usppistatecode.Length < 2)
                        {
                            sb.Append(usppistatecode.ToUpper());
                            sb.Append(' ', 2 - usppistatecode.Length);
                        }
                        else
                        {
                            sb.Append(usppistatecode.ToUpper());
                        }
                    }
                    else
                    {
                        sb.Append(' ', 2);
                    }

                }
                else
                {
                    sb.Append(' ', 2);
                }
                string usppicountrycode = "";
                if (!string.IsNullOrEmpty(objees.UltimateCountryId))
                {
                    objees.UltimateCountryId = RemoveAllSpecialCharacters(objees.UltimateCountryId);
                    var resultCountry = _context.ExecuteQuery<SIPL_Country>("EXEC dbo.AES_GETCountryStateCode_SP @name,@type",
                 new SqlParameter("name", objees.UltimateCountryId),
                 new SqlParameter("type", 1)
                 ).FirstOrDefault();
                    if (resultCountry != null)
                    {
                        if (!string.IsNullOrEmpty(resultCountry.CountryCode))
                            usppicountrycode = resultCountry.CountryCode.ToUpper();

                    }
                }

                if (!string.IsNullOrEmpty(usppicountrycode))
                {

                    usppicountrycode = RemoveAllSpecialCharacters(usppicountrycode);
                    if (usppicountrycode.Length > 2)
                    {
                        sb.Append(usppicountrycode.Substring(0, 2).ToUpper());
                    }
                    else if (usppicountrycode.Length < 2)
                    {
                        sb.Append(usppicountrycode.ToUpper());
                        sb.Append(' ', 2 - usppicountrycode.Length);
                    }
                    else
                    {
                        sb.Append(usppicountrycode.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 2);
                }

                string postelcode = "";
                if(!string.IsNullOrEmpty(objees.UltimatePostalCodeId))
                {
                    objees.UltimatePostalCodeId = RemoveAllSpecialCharacters(objees.UltimatePostalCodeId);
                
                   postelcode = objees.UltimatePostalCodeId.ToUpper();
                }
                if (!string.IsNullOrEmpty(postelcode))
                {


                    if (postelcode.Length > 9)
                    {
                        sb.Append(postelcode.Substring(0, 9).ToUpper());
                    }
                    else if (postelcode.Length < 9)
                    {
                        sb.Append(postelcode.ToUpper());
                        sb.Append(' ', 9 - postelcode.Length);
                    }
                    else
                    {
                        sb.Append(postelcode.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 9);
                }
                string irsnumber = "";// objees.UltimateIdNumber;
                if (!string.IsNullOrEmpty(irsnumber))
                {
                    irsnumber = RemoveAllSpecialCharacters(irsnumber);

                    if (irsnumber.Length > 9)
                    {
                        sb.Append(irsnumber.Substring(0, 9).ToUpper());
                    }
                    else if (irsnumber.Length < 9)
                    {
                        sb.Append(irsnumber.ToUpper());
                        sb.Append(' ', 9 - irsnumber.Length);
                    }
                    else
                    {
                        sb.Append(irsnumber.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 9);
                }

                string irsidtype = "";// objees.UltimateIdNumberTypeId;
                if (!string.IsNullOrEmpty(irsidtype))
                {

                    sb.Append(irsidtype.Substring(0, 1).ToUpper());
                }
                else
                {
                    sb.Append(' ');
                }

                string UCType = objees.ConsigneeTypeId;
                if (!string.IsNullOrEmpty(UCType))
                {
                    sb.Append(UCType.Substring(0, 1).ToUpper());


                }
                else
                {
                    sb.Append(' ');
                }
                sb.Append(' ', 28);
            }
            ///// ic

            else if (partytype == "ic" && !string.IsNullOrEmpty(objees.InterCompanyName))
            {
                sb.Append("N03");
                if (!string.IsNullOrEmpty(objees.InterCity))
                {
                    objees.InterCity = RemoveAllSpecialCharacters(objees.InterCity);
                    objees.InterCity = objees.InterCity.TrimStart().TrimEnd();
                    if (objees.InterCity.Length > 25)
                    {
                        sb.Append(objees.InterCity.Substring(0, 25).ToUpper());
                    }
                    else if (objees.InterCity.Length < 25)
                    {
                        sb.Append(objees.InterCity.ToUpper());
                        sb.Append(' ', 25 - objees.InterCity.Length);
                    }
                    else
                    {
                        sb.Append(objees.InterCity.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 25);
                }

                if (!string.IsNullOrEmpty(objees.InterStateId))
                {
                    objees.InterStateId = RemoveAllSpecialCharacters(objees.InterStateId);
                    objees.InterStateId = objees.InterStateId.TrimStart().TrimEnd();
                    string usppistatecode = "";

                    var resultState = _context.ExecuteQuery<SIPL_State>("EXEC dbo.AES_GETCountryStateCode_SP @name,@type",
                  new SqlParameter("name", objees.InterStateId),
                  new SqlParameter("type", 2)
                  ).FirstOrDefault();
                    if (resultState.StateCode != null)
                    {
                        usppistatecode = resultState.StateCode.ToUpper();
                    }
                    if (!string.IsNullOrEmpty(usppistatecode))
                    {
                        usppistatecode = RemoveAllSpecialCharacters(usppistatecode);
                        if (usppistatecode.Length > 2)
                        {
                            sb.Append(usppistatecode.Substring(0, 2).ToUpper());
                        }
                        else if (usppistatecode.Length < 2)
                        {
                            sb.Append(usppistatecode.ToUpper());
                            sb.Append(' ', 2 - usppistatecode.Length);
                        }
                        else
                        {
                            sb.Append(usppistatecode.ToUpper());
                        }
                    }
                    else
                    {
                        sb.Append(' ', 2);
                    }

                }
                else
                {
                    sb.Append(' ', 2);
                }
                string usppicountrycode = "";

                var resultCountry = _context.ExecuteQuery<SIPL_Country>("EXEC dbo.AES_GETCountryStateCode_SP @name,@type",
                new SqlParameter("name", objees.InterCountryId),
                new SqlParameter("type", 1)
                ).FirstOrDefault();
                if (resultCountry.CountryCode != null)
                {
                    usppicountrycode = resultCountry.CountryCode.ToUpper();
                }


                if (!string.IsNullOrEmpty(usppicountrycode))
                {
                    usppicountrycode = RemoveAllSpecialCharacters(usppicountrycode);

                    if (usppicountrycode.Length > 2)
                    {
                        sb.Append(usppicountrycode.Substring(0, 2).ToUpper());
                    }
                    else if (usppicountrycode.Length < 2)
                    {
                        sb.Append(usppicountrycode.ToUpper());
                        sb.Append(' ', 2 - usppicountrycode.Length);
                    }
                    else
                    {
                        sb.Append(usppicountrycode.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 2);
                }

                string postelcode = objees.InterPostalCodeId;
                if (!string.IsNullOrEmpty(postelcode))
                {
                    postelcode = RemoveAllSpecialCharacters(postelcode);

                    if (postelcode.Length > 9)
                    {
                        sb.Append(postelcode.Substring(0, 9).ToUpper());
                    }
                    else if (postelcode.Length < 9)
                    {
                        sb.Append(postelcode.ToUpper());
                        sb.Append(' ', 9 - postelcode.Length);
                    }
                    else
                    {
                        sb.Append(postelcode.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 9);
                }
                
                if (!string.IsNullOrEmpty(objees.InterIdNumber))
                {
                    string irsnumber = objees.InterIdNumber.TrimStart().TrimEnd();
                    irsnumber = RemoveAllSpecialCharacters(irsnumber);

                    if (irsnumber.Length > 9)
                    {
                        sb.Append(irsnumber.Substring(0, 9).ToUpper());
                    }
                    else if (irsnumber.Length < 9)
                    {
                        sb.Append(irsnumber.ToUpper());
                        sb.Append(' ', 9 - irsnumber.Length);
                    }
                    else
                    {
                        sb.Append(irsnumber.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 9);
                }


               
                if (!string.IsNullOrEmpty(objees.InterIdNumberTypeId))
                {
                    string irsidtype = objees.InterIdNumberTypeId.TrimStart().TrimEnd();
                    if (irsidtype.Length > 1)
                    {

                        sb.Append(irsidtype.Substring(0, 1).ToUpper());
                    }
                    else
                    {
                        sb.Append(irsidtype.ToUpper());
                    }



                }
                else
                {
                    sb.Append(' ');
                }

                string UCType = "";
                if (!string.IsNullOrEmpty(UCType))
                {
                    sb.Append(UCType.ToUpper());
                }
                else
                {
                    sb.Append(' ');
                }
                sb.Append(' ', 28);
            }

            //// ff

            else if (partytype == "ff")
            {
                sb.Append("N03");
                if (!string.IsNullOrEmpty(objees.FreightCity))
                {
                    objees.FreightCity = RemoveAllSpecialCharacters(objees.FreightCity);
                    objees.FreightCity = objees.FreightCity.TrimStart().TrimEnd();
                    if (objees.FreightCity.Length > 25)
                    {
                        sb.Append(objees.FreightCity.Substring(0, 25).ToUpper());
                    }
                    else if (objees.FreightCity.Length < 25)
                    {
                        sb.Append(objees.FreightCity.ToUpper());
                        sb.Append(' ', 25 - objees.FreightCity.Length);
                    }
                    else
                    {
                        sb.Append(objees.FreightCity.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 25);
                }

                if (!string.IsNullOrEmpty(objees.FreightStateId))
                {
                    objees.FreightStateId = RemoveAllSpecialCharacters(objees.FreightStateId);
                    objees.FreightStateId = objees.FreightStateId.TrimStart().TrimEnd();

                    string usppistatecode = "";


                    var resultState = _context.ExecuteQuery<SIPL_State>("EXEC dbo.AES_GETCountryStateCode_SP @name,@type",
                  new SqlParameter("name", objees.FreightStateId),
                  new SqlParameter("type", 2)
                  ).FirstOrDefault();
                    if (resultState != null)
                    {
                        usppistatecode = resultState.StateCode.ToUpper();
                    }

                    if (usppistatecode.Length > 2)
                    {
                        sb.Append(usppistatecode.Substring(0, 2).ToUpper());
                    }
                    else if (usppistatecode.Length < 2)
                    {
                        sb.Append(usppistatecode.ToUpper());
                        sb.Append(' ', 2 - usppistatecode.Length);
                    }
                    else
                    {
                        sb.Append(usppistatecode.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 2);
                }
                string usppicountrycode = "US"; //




                if (!string.IsNullOrEmpty(usppicountrycode))
                {


                    if (usppicountrycode.Length > 2)
                    {
                        sb.Append(usppicountrycode.Substring(0, 2).ToUpper());
                    }
                    else if (usppicountrycode.Length < 2)
                    {
                        sb.Append(usppicountrycode.ToUpper());
                        sb.Append(' ', 2 - usppicountrycode.Length);
                    }
                    else
                    {
                        sb.Append(usppicountrycode.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 2);
                }

                string postelcode = objees.FreightPostalCodeId;
                if (!string.IsNullOrEmpty(postelcode))
                {
                    postelcode = RemoveAllSpecialCharacters(postelcode);

                    if (postelcode.Length > 9)
                    {
                        sb.Append(postelcode.Substring(0, 9).ToUpper());
                    }
                    else if (postelcode.Length < 9)
                    {
                        sb.Append(postelcode.ToUpper());
                        sb.Append(' ', 9 - postelcode.Length);
                    }
                    else
                    {
                        sb.Append(postelcode.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 9);
                }
                string irsnumber = ""; // objees.FreightIdNumber;
                if (!string.IsNullOrEmpty(irsnumber))
                {


                    if (irsnumber.Length > 9)
                    {
                        sb.Append(irsnumber.Substring(0, 9).ToUpper());
                    }
                    else if (irsnumber.Length < 9)
                    {
                        sb.Append(irsnumber.ToUpper());
                        sb.Append(' ', 9 - irsnumber.Length);
                    }
                    else
                    {
                        sb.Append(irsnumber.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 9);
                }

                string irsidtype = "";// objees.FreightIdNumberTypeId;
                if (!string.IsNullOrEmpty(irsidtype))
                {
                    sb.Append(irsidtype.ToUpper());

                }
                else
                {
                    sb.Append(' ');
                }

                string UCType = "";
                if (!string.IsNullOrEmpty(UCType))
                {
                    sb.Append(UCType.ToUpper());
                }
                else
                {
                    sb.Append(' ');
                }
                sb.Append(' ', 28);
            }


            return sb.ToString();
        }
        private string GenerateClassC01C02EV(AppMGL.DTO.Report.EESSubmission objees)
        {
            StringBuilder sb = new StringBuilder(); // usppi
            int i = 1;
            foreach (var item in objees.CommodityDetail)
            {
                sb.Append("CL1");
                if (!string.IsNullOrEmpty(item.ExportCode))
                {
                    item.ExportCode = RemoveAllSpecialCharacters(item.ExportCode);
                    if (item.ExportCode.Length > 2)
                    {
                        sb.Append(item.ExportCode.Substring(0, 2).ToUpper());
                    }
                    else if (item.ExportCode.Length < 2)
                    {
                        sb.Append(item.ExportCode.ToUpper());
                        sb.Append(' ', 2 - item.ExportCode.Length);
                    }
                    else
                    {
                        sb.Append(item.ExportCode.ToUpper());
                    }

                    // sb.Append(item.ExportCode);
                }
                else
                    sb.Append(' ', 2);
                sb.Append(' ');
                sb.Append(i.ToString().PadLeft(4, '0'));
                if (!string.IsNullOrEmpty(item.CommodityDescription))
                {
                    item.CommodityDescription = RemoveAllSpecialCharacters(item.CommodityDescription);
                    item.CommodityDescription = item.CommodityDescription.TrimStart().TrimEnd();
                    if (item.CommodityDescription.Length > 45)
                    {
                        sb.Append(item.CommodityDescription.Substring(0, 45).ToUpper());
                    }
                    else if (item.CommodityDescription.Length < 45)
                    {
                        sb.Append(item.CommodityDescription.ToUpper());
                        sb.Append(' ', 45 - item.CommodityDescription.Length);
                    }
                    else
                    {
                        sb.Append(item.CommodityDescription.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 45);
                }

                if (!string.IsNullOrEmpty(item.LicValueAmount))
                {
                    item.LicValueAmount = RemoveAllSpecialCharacters(item.LicValueAmount);
                    if (item.LicValueAmount.Length > 10)
                    {
                        sb.Append(item.LicValueAmount.Substring(0, 10).ToUpper());
                    }
                    else if (item.LicValueAmount.Length < 10)
                    {

                        sb.Append('0', 10 - item.LicValueAmount.Length);
                        sb.Append(item.LicValueAmount.ToUpper());
                    }
                    else
                    {
                        sb.Append(item.LicValueAmount.ToUpper());
                    }
                }
                else
                {
                    sb.Append('0', 10);
                }

               // sb.Append('0', 10); // license value
                sb.Append(' ');
                if (!string.IsNullOrEmpty(item.ctype))   // tuhi ram concepts
                {
                    //sb.Append(item.ctype.ToUpper());
                    if (item.ctype.ToLower() == "x")
                    {
                        sb.Append("D");
                    }
                    else
                    {
                        sb.Append(item.ctype.ToUpper());
                    }
                }
                else
                {
                    sb.Append(" ");
                }

                //if (objees.Id == "0" || string.IsNullOrEmpty(objees.Id))
                //{
                //    sb.Append("A");

                //}
                //else
                //{
                //    if (item.IsType == "D")
                //    {
                //        sb.Append("D");
                //    }
                //    else
                //    {
                //        sb.Append("A");  //  sb.Append("C"); 
                //    }
                //}


                if (!string.IsNullOrEmpty(item.LicenseTypeCode))
                {
                    item.LicenseTypeCode = RemoveAllSpecialCharacters(item.LicenseTypeCode);
                    item.LicenseTypeCode = item.LicenseTypeCode.TrimStart().TrimEnd();
                    if (item.LicenseTypeCode.Length > 3)
                    {
                        sb.Append(item.LicenseTypeCode.Substring(0, 3).ToUpper());
                    }
                    else if (item.LicenseTypeCode.Length < 3)
                    {
                        sb.Append(item.LicenseTypeCode.ToUpper());
                        sb.Append(' ', 3 - item.LicenseTypeCode.Length);
                    }
                    else
                    {
                        sb.Append(item.LicenseTypeCode.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 3);
                }
                if (!string.IsNullOrEmpty(item.OriginofGoods))
                {
                    item.OriginofGoods = RemoveAllSpecialCharacters(item.OriginofGoods);

                    if (item.OriginofGoods.ToLower().IndexOf("domestic") > -1)
                    {
                        sb.Append("D");
                    }
                    else
                    {
                        sb.Append("F");
                    }
                }
                else
                    sb.Append(' ');
                sb.Append("N");
                sb.Append(' ', 8);

                // For deleted item only show cl1 
                if (item.ctype.ToLower() == "x" || item.ctype.ToLower() == "d")
                {
                    continue;
                }
                    //
                    sb.Append("CL2");
                string scb = "";
                if (!string.IsNullOrEmpty(item.ScheduleB))
                {
                    item.ScheduleB = RemoveAllSpecialCharacters(item.ScheduleB);
                    scb = item.ScheduleB.Replace(".", "");
                    if (!string.IsNullOrEmpty(scb))
                    {
                        if (scb.Length > 10)
                        {
                            sb.Append(scb.Substring(0, 10).ToUpper());
                        }
                        else if (item.LicenseTypeCode.Length < 10)
                        {
                            sb.Append(scb.ToUpper());
                            sb.Append(' ', 10 - scb.Length);
                        }
                        else
                        {
                            sb.Append(scb.ToUpper());
                        }
                    }
                    else
                    {
                        sb.Append(' ', 10);
                    }
                }
                else
                {
                    sb.Append(' ', 10);
                }

                if (!string.IsNullOrEmpty(item.FirstUOM))
                {



                    if (item.FirstUOM.Length > 3)
                    {
                        sb.Append(item.FirstUOM.Substring(0, 3).ToUpper());
                    }
                    else if (item.FirstUOM.Length < 3)
                    {
                        sb.Append(item.FirstUOM.ToUpper());
                        sb.Append(' ', 3 - item.FirstUOM.Length);
                    }
                    else
                    {
                        sb.Append(item.FirstUOM.ToUpper());
                    }
                }
                else
                {
                    sb.Append(' ', 3);
                }
                if (!string.IsNullOrEmpty(item.FirstQuantity))
                {
                    item.FirstQuantity = RemoveAllSpecialCharacters(item.FirstQuantity);
                    sb.Append(item.FirstQuantity.PadLeft(10, '0').ToUpper());

                }
                else
                {
                    sb.Append('0', 10);
                }
                ////
                if (!string.IsNullOrEmpty(item.ValueofGoods))
                {
                    item.ValueofGoods = RemoveAllSpecialCharacters(item.ValueofGoods);
                    sb.Append(item.ValueofGoods.PadLeft(10, '0'));
                }
                else
                    sb.Append('0', 10);
                if (!string.IsNullOrEmpty(item.SecondUOM))
                {
                    if (item.SecondUOM.Length > 3)
                    {
                        sb.Append(item.SecondUOM.Substring(0, 3).ToUpper());
                    }
                    else if (item.SecondUOM.Length < 3)
                    {
                        sb.Append(item.SecondUOM.ToUpper());
                        sb.Append(' ', 3 - item.SecondUOM.Length);
                    }
                    else
                    {
                        sb.Append(item.SecondUOM.ToUpper());
                    }

                }
                else
                {
                    sb.Append(' ', 3);
                }
                if (!string.IsNullOrEmpty(item.SecondQuantity))
                {
                    item.SecondQuantity = RemoveAllSpecialCharacters(item.SecondQuantity);
                    sb.Append(item.SecondQuantity.PadLeft(10, '0').ToUpper());

                }
                else
                {
                    sb.Append('0', 10);
                }

                if (!string.IsNullOrEmpty(item.ShippingWeight))
                {
                    item.ShippingWeight = RemoveAllSpecialCharacters(item.ShippingWeight);
                    sb.Append(item.ShippingWeight.PadLeft(10, '0').ToUpper());
                }
                else
                {
                    sb.Append('0', 10);
                }
                if (!string.IsNullOrEmpty(item.Eccn))
                {
                    item.Eccn = RemoveAllSpecialCharacters(item.Eccn);
                    if (item.Eccn.Length > 5)
                    {
                        sb.Append(item.Eccn.Substring(0, 5).ToUpper());
                    }
                    else if (item.Eccn.Length < 5)
                    {
                        sb.Append(item.Eccn.ToUpper());
                        sb.Append(' ', 5 - item.Eccn.Length);
                    }
                    else
                    {
                        sb.Append(item.Eccn.ToUpper());
                    }


                }
                else
                {
                    sb.Append(' ', 5);
                }

                // export license number

                if (!string.IsNullOrEmpty(item.ExpLic))
                {
                    item.ExpLic = RemoveAllSpecialCharacters(item.ExpLic);
                    if (item.ExpLic.Length > 14)
                    {
                        sb.Append(item.ExpLic.Substring(0, 14).ToUpper());
                    }
                    else if (item.ExpLic.Length < 14)
                    {
                        sb.Append(item.ExpLic.ToUpper());
                        sb.Append(' ', 14 - item.ExpLic.Length);
                    }
                    else
                    {
                        sb.Append(item.ExpLic.ToUpper());
                    }


                }
                else
                {
                    sb.Append(' ', 14);
                }




                // var resultState = _context.ExecuteQuery<NameValue>("EXEC dbo.AES_GETCountryStateCode_SP @name,@type",
                //new SqlParameter("name", item.LicenseTypeCode),
                //new SqlParameter("type", 5)
                //).FirstOrDefault();

                // if (resultState != null)
                // {
                //     if (!string.IsNullOrEmpty(resultState.Name))
                //     {
                //         if (resultState.Name.Length > 14)
                //         {
                //             sb.Append(resultState.Name.Substring(0, 14));
                //         }
                //         else if (resultState.Name.Length < 14)
                //         {
                //             sb.Append(resultState.Name);
                //             sb.Append(' ', 14 - resultState.Name.Length);
                //         }
                //     }
                //     else
                //     {
                //         sb.Append(' ', 14);
                //     }
                // }
                // else
                // {
                //     sb.Append(' ', 14);
                // }






                sb.Append(' ', 2);
                // ODI 
                if (item.IsGovermentAgency != null)
                {
                    if (item.IsGovermentAgency.ToLower() == "y")
                    {
                        sb.Append("ODT");
                        //  sb.Append(' ', 77); // for testing i added it 
                        if (!string.IsNullOrEmpty(item.dDTCITAR))
                        {
                            item.dDTCITAR = RemoveAllSpecialCharacters(item.dDTCITAR);
                            if (item.dDTCITAR.Length > 12)
                            {
                                sb.Append(item.dDTCITAR.Substring(0, 11).ToUpper());
                            }
                            else if (item.dDTCITAR.Length < 12)
                            {
                                sb.Append(item.dDTCITAR.ToUpper());
                                sb.Append(' ', 12 - item.dDTCITAR.Length);

                            }
                            else
                            {
                                sb.Append(item.dDTCITAR.ToUpper());
                            }
                        }
                        else
                        {
                            sb.Append(' ', 12);
                        }

                        if (!string.IsNullOrEmpty(item.dDTCReg))
                        {
                            item.dDTCReg = RemoveAllSpecialCharacters(item.dDTCReg);
                            if (item.dDTCReg.Length > 6)
                            {
                                sb.Append(item.dDTCReg.Substring(0, 6).ToUpper());
                            }
                            else if (item.dDTCReg.Length < 6)
                            {
                                sb.Append(item.dDTCReg.ToUpper());
                                sb.Append(' ', 6 - item.dDTCReg.Length);

                        }
                        else
                        {
                            sb.Append(item.dDTCReg.ToUpper());
                        }
                    }
                    else
                    {
                        sb.Append(' ', 6);
                    }

                    if (!string.IsNullOrEmpty(item.dDTCSignificant))
                    {
                        item.dDTCSignificant = RemoveAllSpecialCharacters(item.dDTCSignificant);
                        if (item.dDTCSignificant.ToLower() == "yes")
                        {
                            sb.Append("Y");
                        }
                        else
                        {
                            sb.Append("N");
                        }
                    }
                    else
                    {
                        sb.Append(' ', 1);
                    }
                    if (!string.IsNullOrEmpty(item.dDTCEligible))
                    {
                        if (item.dDTCEligible.ToLower() == "yes")
                        {
                            sb.Append("Y");
                        }
                        else
                        {
                            sb.Append("N");
                        }
                    }
                    else
                    {
                        sb.Append(' ', 1);
                    }
                    if (!string.IsNullOrEmpty(item.dDTCUSML))
                    {
                        item.dDTCUSML = RemoveAllSpecialCharacters(item.dDTCUSML);
                        if (item.dDTCUSML.Length > 2)
                        {
                            sb.Append(item.dDTCUSML.Substring(0, 2).ToUpper());
                        }
                        else if (item.dDTCUSML.Length < 2)
                        {
                            sb.Append(item.dDTCUSML.ToUpper());
                            sb.Append(' ', 2-item.dDTCUSML.Length );

                        }
                        else
                        {
                            sb.Append(item.dDTCUSML.ToUpper());
                        }



                      //  sb.Append(item.dDTCUSML);

                    }
                    else
                    {
                        sb.Append(' ', 2);
                    }
                    sb.Append(' ', 8);
                    if (!string.IsNullOrEmpty(item.dDTCUnit))
                    {
                        item.dDTCUnit = RemoveAllSpecialCharacters(item.dDTCUnit);
                        if (item.dDTCUnit.Length > 3)
                        {
                            sb.Append(item.dDTCUnit.Substring(0, 3).ToUpper());
                        }
                        else if (item.dDTCUnit.Length < 3)
                        {
                            sb.Append(item.dDTCUnit.ToUpper());
                            sb.Append(' ',3- item.dDTCUnit.Length);

                        }
                        else
                        {
                            sb.Append(item.dDTCUnit.ToUpper());
                        }



                        //sb.Append(item.dDTCUnit);

                    }
                    else
                    {
                        sb.Append(' ', 3);
                    }
                    if (!string.IsNullOrEmpty(item.dDTCQuantity))
                    {
                        item.dDTCQuantity = RemoveAllSpecialCharacters(item.dDTCQuantity);
                        //if (item.dDTCQuantity.Length > 7)
                        //{
                        //    sb.Append(item.dDTCQuantity.ToString().PadLeft(7, '0'));
                        //}

                        if (item.dDTCQuantity.Length > 7)
                        {
                            sb.Append(item.dDTCQuantity.Substring(0, 7).ToUpper());
                        }
                        else if (item.dDTCQuantity.Length < 7)
                        {
                            sb.Append('0',7- item.dDTCQuantity.Length);
                            sb.Append(item.dDTCQuantity.ToUpper());
                           

                        }
                        else
                        {
                            sb.Append(item.dDTCQuantity.ToUpper());
                        }

                    }
                    else
                    {
                        sb.Append(' ', 7);
                    }

                    if (!string.IsNullOrEmpty(item.dDTCLicense))
                    {
                        item.dDTCLicense = RemoveAllSpecialCharacters(item.dDTCLicense);
                        if (item.dDTCLicense.Length > 3)
                        {
                            sb.Append(item.dDTCLicense.Substring(0, 2).ToUpper());
                        }
                        else if (item.dDTCLicense.Length < 3)
                        {
                            sb.Append(item.dDTCLicense.PadLeft(3, '0').ToUpper());
                        }
                        else
                        {
                            sb.Append(item.dDTCLicense.ToUpper());
                        }

                        }
                        else
                        {
                            sb.Append(' ', 3);
                        }

                        sb.Append(' ', 34);

                        // end comments 
                    }
                }




                string line = (i - 1).ToString();
                List<Vin> ev = objees.VinDetail.Where(x => x.Line_No == line).ToList();

                foreach (var evitem in ev)
                {
                    sb.Append("EV1");

                    if (!string.IsNullOrEmpty(evitem.VINNumber))
                    {
                        evitem.VINNumber = RemoveAllSpecialCharacters(evitem.VINNumber);
                        if (evitem.VINNumber.Length > 25)
                        {
                            sb.Append(evitem.VINNumber.Substring(0, 25).ToUpper());
                        }
                        else if (evitem.VINNumber.Length < 25)
                        {
                            sb.Append(evitem.VINNumber.ToUpper());
                            sb.Append(' ', 25 - evitem.VINNumber.Length);

                        }
                        else
                        {
                            sb.Append(evitem.VINNumber.ToUpper());
                        }
                    }
                    else
                    {
                        sb.Append(' ', 25);
                    }
                    if (!string.IsNullOrEmpty(evitem.VIN))
                    {
                        evitem.VIN = RemoveAllSpecialCharacters(evitem.VIN);
                        if (evitem.VIN.Length > 1)
                        {
                            sb.Append(evitem.VIN.Substring(0, 1).ToUpper());
                        }
                        else if (evitem.VIN.Length < 1)
                        {
                            sb.Append(evitem.VIN.ToUpper());
                            sb.Append(' ', 1 - evitem.VIN.Length);

                        }
                        else
                        {
                            sb.Append(evitem.VIN.ToUpper());
                        }

                        // sb.Append(evitem.VIN);
                    }
                    else
                    {
                        sb.Append(' ');
                    }

                    if (!string.IsNullOrEmpty(evitem.VehicleTitleNum))
                    {
                        evitem.VehicleTitleNum = RemoveAllSpecialCharacters(evitem.VehicleTitleNum);
                        if (evitem.VehicleTitleNum.Length > 15)
                        {
                            sb.Append(evitem.VehicleTitleNum.Substring(0, 15).ToUpper());
                        }
                        else if (evitem.VehicleTitleNum.Length < 15)
                        {
                            sb.Append(evitem.VehicleTitleNum.ToUpper());
                            sb.Append(' ', 15 - evitem.VehicleTitleNum.Length);

                        }
                        else
                        {
                            sb.Append(evitem.VehicleTitleNum.ToUpper());
                        }
                    }
                    else
                    {
                        sb.Append(' ', 15);
                    }

                    if (!string.IsNullOrEmpty(evitem.VehicleTitleState))
                    {
                        evitem.VehicleTitleState = RemoveAllSpecialCharacters(evitem.VehicleTitleState);
                        if (evitem.VehicleTitleState.Length > 2)
                        {
                            sb.Append(evitem.VehicleTitleState.Substring(0, 2).ToUpper());
                        }
                        else if (evitem.VehicleTitleState.Length < 2)
                        {
                            sb.Append(evitem.VehicleTitleState.ToUpper());
                            sb.Append(' ', 2 - evitem.VehicleTitleState.Length);

                        }
                        else
                        {
                            sb.Append(evitem.VehicleTitleState.ToUpper());
                        }
                    }
                    else
                    {
                        sb.Append(' ', 2);
                    }

                    sb.Append(' ', 34);

                }





                //sb.Append("")

                i++;


            }


            return sb.ToString();

        }

        private string DDTC(AppMGL.DTO.Report.EESSubmission objees, string partytype)
        {
            StringBuilder sb = new StringBuilder();


            return sb.ToString();


        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult GetExportRegisterDetailEES(BookingListDTO objBooking)
        {
            try
            {
                List<EESLIST> result = _context.ExecuteQuery<EESLIST>("EXEC dbo.USP_LG_BOOKING_GET_EXPORT_REGISTER_DETAILSBYID_AES @DOCUMENTCOMMONID,@SIT_ID,@etype,@Id",
                                        new SqlParameter("DOCUMENTCOMMONID", objBooking.DocumentCommonID),
                                        new SqlParameter("SIT_ID", objBooking.SiteId),
                                        new SqlParameter("etype", objBooking.etype),
                                        new SqlParameter("Id", objBooking.Id == null ? "0" : objBooking.Id)).ToList();
                EESLIST objesslist = result[0];
                List<SqlParameter> list2 = new List<SqlParameter>
                {
                    new SqlParameter("@AesId", objesslist.Id),
                    new SqlParameter("@SiteId", objBooking.SiteId),
                    new SqlParameter("etype", objBooking.etype),
                    new SqlParameter("@DocumentCommonId", objBooking.DocumentCommonID)
                };
                List<CommodityLine> list3 = _context.ExecuteQuery<CommodityLine>(DocumentCommonQuery.GetCommodityLineDetail, list2.ToArray()).ToList();

                var CommodityDetail = list3.ToArray();

                List<SqlParameter> list4 = new List<SqlParameter>
                {

                    new SqlParameter("@DocumentCommonId", objBooking.DocumentCommonID),
                    new SqlParameter("@SiteId", objBooking.SiteId),
                    new SqlParameter("@AesId", objesslist.Id)

                };
                List<EquipmentLine> list5 = _context.ExecuteQuery<EquipmentLine>(DocumentCommonQuery.GetEquipmentLineDetail, list4.ToArray()).ToList();
                var EquipmentLineDetail = list5.ToArray();

                List<SqlParameter> vinlist = new List<SqlParameter>
                {

                    new SqlParameter("@DocumentCommonId", objBooking.DocumentCommonID),
                    new SqlParameter("@SiteId", objBooking.SiteId),
                    new SqlParameter("etype", objBooking.etype),
                    new SqlParameter("@AesId", objesslist.Id)

                };
                List<Vin> vinresult = _context.ExecuteQuery<Vin>(DocumentCommonQuery.GetVinDetail, vinlist.ToArray()).ToList();
                var VinDetail = vinresult.ToArray();


                List<SqlParameter> essfiles = new List<SqlParameter>
                {

                    new SqlParameter("@DocumentCommonId", objBooking.DocumentCommonID),
                    new SqlParameter("@AesId", objesslist.Id)

                };
                List<FileHistory> fileresult = _context.ExecuteQuery<FileHistory>(DocumentCommonQuery.GetFilesList, essfiles.ToArray()).ToList();
                var filelists = fileresult.ToArray();

                AppMGL.DTO.Report.EESSubmission ees = new DTO.Report.EESSubmission();
                ees.CommodityDetail = CommodityDetail;
                ees.EquipmentLineDetail = EquipmentLineDetail;
                ees.VinDetail = VinDetail;
                ees.Id = objesslist.Id;
                ees.EmailResponseAddress = objesslist.EmailResponseAddress == null ? objBooking.Email : objesslist.EmailResponseAddress;
                ees.DocumentCommonID = objesslist.DocumentCommonID;
                ees.siteId = objesslist.SiteId.ToString();
                ees.QuotationID = objesslist.QuotationID;
                ees.shipmentNumber = objesslist.shipmentNumber;
                ees.enquiryNo = objesslist.EnquiryNo;
                ees.quotationNo = objesslist.QuotationNo;
                ees.bookingNo = objesslist.BookingNo;
                ees.companyName = objesslist.CompanyName;
                ees.ModeofTransport = objesslist.ModeofTransport;
                ees.PortofExport = objesslist.PortofExport;
                ees.PortofExportCode = objesslist.PortofExportCode;
                ees.OriginState = objesslist.OriginState;
                ees.OriginStateCode = objesslist.OriginStateCode;
                ees.PortofUnloading = objesslist.PortofUnloading;
                ees.PortofUnloadingCode = objesslist.PortofUnloadingCode;
                ees.DepartureDate = objesslist.DepartureDate;
                ees.FilingOption = objesslist.FilingOption;
                ees.CountryOfDestination = objesslist.CountryOfDestination;
                ees.CountryCode = objesslist.CountryCode;
                ees.InbondType = objesslist.InbondType;
                ees.ForeginTradeZone = objesslist.ForeginTradeZone;
                ees.ImportEntry = objesslist.ImportEntry;
                ees.OriginalITN = objesslist.OriginalITN;

                ees.IsRoutedTransaction = objesslist.IsRoutedTransaction;
                ees.IsUltimateConsigneeCompanies = objesslist.IsUltimateConsigneeCompanies;
                ees.IsHazardousMaterial = objesslist.IsHazardousMaterial;


                ees.USPPIIDNumberTypeId = objesslist.USPPIIDNumberTypeId;
                ees.USPPIIDNumber = objesslist.USPPIIDNumber;
                ees.USPPICompanyName = objesslist.USPPICompanyName;
                ees.IRSNumber = objesslist.IRSNumber;
                ees.USPPIFirstName = objesslist.USPPIFirstName;
                ees.USPPILastName = objesslist.USPPILastName;
                ees.USPPIPhoneNumber = objesslist.USPPIPhoneNumber;
                ees.USPPIPostalCodeId = objesslist.USPPIPostalCodeId;
                ees.USPPIAddressLine1 = objesslist.USPPIAddressLine1;
                ees.USPPIAddressLine2 = objesslist.USPPIAddressLine2;
                ees.USPPICity = objesslist.USPPICity;
                ees.USPPIStateId = objesslist.USPPIStateId;
                ees.USPPIStateCode = objesslist.USPPIStateCode;
                ees.USPPIEmail = objesslist.USPPIEmail;

                ees.IsSoldEnRoute = objesslist.IsSoldEnRoute;
                ees.ConsigneeTypeId = objesslist.ConsigneeTypeId;
                ees.UltimateIdNumberTypeId = objesslist.UltimateIdNumberTypeId;
                ees.UltimateIdNumber = objesslist.UltimateIdNumber;
                ees.UltimateCompanyName = objesslist.UltimateCompanyName;
                ees.UltimateFirstName = objesslist.UltimateFirstName;
                ees.UltimateLastName = objesslist.UltimateLastName;
                ees.UltimatePhoneNumber = objesslist.UltimatePhoneNumber;
                ees.UltimateAddressLine1 = objesslist.UltimateAddressLine1;
                ees.UltimateAddressLine2 = objesslist.UltimateAddressLine2;
                ees.UltimateCountryId = objesslist.UltimateCountryId;
                ees.UltimateCountryCode = objesslist.UltimateCountryCode;
                ees.UltimatePostalCodeId = objesslist.UltimatePostalCodeId;
                ees.UltimateCity = objesslist.UltimateCity;
                ees.UltimateStateId = objesslist.UltimateStateId;
                ees.UltimateStateCode = objesslist.UltimateStateCode;
                ees.UltimateEmail = objesslist.UltimateEmail;

                ees.InterIdNumberTypeId = objesslist.InterIdNumberTypeId;
                ees.InterIdNumber = objesslist.InterIdNumber;
                ees.InterCompanyName = objesslist.InterCompanyName;
                ees.InterFirstName = objesslist.InterFirstName;
                ees.InterLastName = objesslist.InterLastName;
                ees.InterPhoneNumber = objesslist.InterPhoneNumber;
                ees.InterAddressLine1 = objesslist.InterAddressLine1;
                ees.InterAddressLine2 = objesslist.InterAddressLine2;
                ees.InterCountryId = objesslist.InterCountryId;
                ees.InterCountryCode = objesslist.InterCountryCode;
                ees.InterPostalCodeId = objesslist.InterPostalCodeId;
                ees.InterCity = objesslist.InterCity;
                ees.InterStateId = objesslist.InterStateId;
                ees.InterStateCode = objesslist.InterStateCode;
                ees.InterEmail = objesslist.InterEmail;

                ees.FreightIdNumberTypeId = objesslist.FreightIdNumberTypeId;
                ees.FreightIdNumber = objesslist.FreightIdNumber;
                ees.FreightCompanyName = objesslist.FreightCompanyName;
                ees.FreightFirstName = objesslist.FreightFirstName;
                ees.FreightLastName = objesslist.FreightLastName;
                ees.FreightPhoneNumber = objesslist.FreightPhoneNumber;
                ees.FreightPostalCodeId = objesslist.FreightPostalCodeId;
                ees.FreightAddressLine1 = objesslist.FreightAddressLine1;
                ees.FreightAddressLine2 = objesslist.FreightAddressLine2;
                ees.FreightCity = objesslist.FreightCity;
                ees.FreightStateId = objesslist.FreightStateId;
                ees.FreightStateCode = objesslist.FreightStateCode;
                ees.FreightEmail = objesslist.FreightEmail;

                ees.CarrierSCAC = objesslist.CarrierSCAC;
                ees.ConveyanceName = objesslist.ConveyanceName;
                ees.TransportationReferenceNumber = objesslist.TransportationReferenceNumber;
                ees.AesFileName = objesslist.AesFileName;
                ees.AESTYPE = objesslist.AESTYPE;
                ees.isuploaded = objesslist.isuploaded;
                ees.ExporterName = objesslist.ExporterName;
                ees.ExporterId = objesslist.ExporterId;
                ees.ExporterFirstName = objesslist.ExporterFirstName;
                ees.ExporterLastName = objesslist.ExporterLastName;
                ees.ExporterPhoneNumber = objesslist.ExporterPhoneNumber;
                ees.ExporterCountryId = objesslist.ExporterCountryId;
                ees.ExporterCountryCode = objesslist.ExporterCountryCode;
                ees.ExporterStateId = objesslist.ExporterStateId;
                ees.ExporterStateCode = objesslist.ExporterStateCode;
                ees.ExporterCity = objesslist.ExporterCity;
                ees.ExporterPostalCodeId = objesslist.ExporterPostalCodeId;
                ees.ExporterAddress = objesslist.ExporterAddress;
                ees.ExporterAddress2 = objesslist.ExporterAddress2;
                ees.ExporterEmail = objesslist.ExporterEmail;

                ees.FileList = filelists;
                ees.isDraft = objesslist.isDraft;
                ees.Aes_Status = objesslist.Aes_Status;
                ees.isFreeze = objesslist.isFreeze;
                ees.FreezeRemarks = objesslist.freezeRemarks;
                ees.PortId = objesslist.PortId;
                ees.UnloadPortId = objesslist.UnloadPortId;
                return AppResult(ees, 1, "", EnumResult.Success);
            }


            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        #endregion


        public ActionResult ChangeSite(EnquiryDetailDTO objEnquiry)
        {
            EnquiryController ec = new EnquiryController();

            try
            {
                string oldEnqNo = objEnquiry.EnquiryNo.ToString();
                string oldEnqId = objEnquiry.EnquiryID.ToString();
                string oldquotationid = objEnquiry.QuotationID.ToString();
                string newEnqNo = "";
                string newEnqId = "";
                string newsiteid = objEnquiry.NewSiteId.ToString();
                List<string> d = null;
                // objEnquiry.IsComplete = 0;


                List<EnquiryDetailDTO> result = _context.ExecuteQuery<EnquiryDetailDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_DETAIL @ENQUIRYID,@IS_COMPLETE,@SIT_ID",
                                      new SqlParameter("ENQUIRYID", objEnquiry.EnquiryID),
                                      new SqlParameter("IS_COMPLETE", objEnquiry.IsComplete),
                                      new SqlParameter("SIT_ID", objEnquiry.SiteId)).ToList();
                EnquiryDetailDTO objEnquiryDetail = result[0];
                string RefType = "";
                if (objEnquiry.IsComplete == 0)
                {
                    RefType = "EnquiryDraft";
                }
                else
                {
                    RefType = "Enquiry";
                }
                if (result[0].EnquiryID > 0)
                {
                    //get commodity
                    List<CommodityDTO> CommodityDTOList = _context.ExecuteQuery<CommodityDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_COMMODITY @RefId,@RefType",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", "Enquiry")).ToList();
                    if (CommodityDTOList.Count > 0)
                    {
                        objEnquiryDetail.CommodityDTOList = CommodityDTOList.ToArray();
                    }
                    ////get all Next Action remarks
                    int bookingid = 0;
                    int quotationID = 0;
                    IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                         new SqlParameter("QUOTATIONID", quotationID),
                         new SqlParameter("ENQUIRYID", result[0].EnquiryID),
                         new SqlParameter("BookingID", bookingid),
                         //new SqlParameter("RefType", "Enquiry")).ToList();  comment by vikas solanki on 12 Nov 2020
                         new SqlParameter("RefType", RefType)).ToList();
                    List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                    if (RemarksResultList.Count > 0)
                    {
                        objEnquiryDetail.NextActionRemarksDTOList = RemarksResult.ToArray();
                        objEnquiryDetail.LastRemarks = RemarksResultList[0].Remarks;

                        objEnquiryDetail.LastRemarkDate = RemarksResultList[0].CurrentDate.Value.Date;

                        if (RemarksResultList.Count > 1)
                        {
                            objEnquiryDetail.PrevActionDueDate = RemarksResultList[0].NextActionDate;
                        }
                        else
                        {
                            objEnquiryDetail.PrevActionDueDate = RemarksResultList[0].NextActionDate;
                        }
                    }
                    //get Container details / FCL                                     
                    List<EnquiryContainerServiceDTO> ContainerServiceDTOList = _context.ExecuteQuery<EnquiryContainerServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_CONTAINER_LIST @RefId,@RefType",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType)).ToList();
                    if (ContainerServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.ModeOfService = "1";
                        objEnquiryDetail.EnquiryContainerServiceDTOList = ContainerServiceDTOList.ToArray();
                    }
                    //get Air
                    List<EnquiryAIRServiceDTO> AirServiceDTOList = _context.ExecuteQuery<EnquiryAIRServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_AIR @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 7)).ToList();
                    if (AirServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.ModeOfService = "7";
                        objEnquiryDetail.EnquiryAIRServiceDTOList = AirServiceDTOList.ToArray();
                    }
                    //get Break Bulk
                    List<EnquiryBreakBulkServiceDTO> BreakBulkServiceDTOList = _context.ExecuteQuery<EnquiryBreakBulkServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_BREAK_BULK @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 5)).ToList();
                    if (BreakBulkServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.ModeOfService = "5";
                        objEnquiryDetail.EnquiryBreakBulkServiceDTOList = BreakBulkServiceDTOList.ToArray();
                    }
                    //get LCL
                    List<EnquiryLCLServiceDTO> LCLServiceDTOList = _context.ExecuteQuery<EnquiryLCLServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_LCL @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 2)).ToList();
                    if (LCLServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.ModeOfService = "2";
                        objEnquiryDetail.EnquiryLCLServiceDTOList = LCLServiceDTOList.ToArray();
                    }
                    //get RORO
                    List<EnquiryROROServiceDTO> ROROServiceDTOList = _context.ExecuteQuery<EnquiryROROServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_RORO @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 3)).ToList();
                    if (ROROServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.ModeOfService = "3";
                        objEnquiryDetail.EnquiryROROServiceDTOList = ROROServiceDTOList.ToArray();
                    }
                }



                // get new enqueryid 

                EnquiryDetailDTO objEnquiryList = _context.ExecuteQuery<EnquiryDetailDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_ENQUIRY_NO  @DEPTID", new SqlParameter("DEPTID", objEnquiryDetail.DepartmentID)).FirstOrDefault();
                objEnquiryDetail.EnquiryID = 0;
                newEnqNo = objEnquiryDetail.EnquiryNo = oldEnqNo + "T";       //objEnquiryList.EnquiryNo;
                objEnquiryDetail.SiteId = Convert.ToDecimal(newsiteid);
                // newEnqId = objEnquiryDetail.EnquiryID.ToString();
                objEnquiryDetail.IsDraft = 1;
                objEnquiryDetail.CreatedBy = objEnquiry.CreatedBy;
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_INSERT_UPDATE_DRAFT @Id,@EnquiryDate,@EnquiryNo,@fkCompanyID,@BillTo,@BillToCompanyId,@ShipmentMoveDate,@PickupType,@PickupRemark,@PortOfOrigin,@PortOfDischarge,@CityOfDischarge,@CityOfOrigin,@CountryOfDischarge,@CountryOfOrigin,@StateOfDischarge,@StateOfOrigin,@NoOfContainer,@TypeOfEnquiry,@ReceivedBy,@Remarks,@Type,@Description,@Class,@UNNo,@PackingType,@HazRemarks,@ChkHAZ,@CreatedBy,@UpdatedBy, @ModifiedOn, @Hazweight,@HazVolume,@DepartmentID,@LastEnquiryNo,@ControlNo,@IsDraft,@GalRepID, @OriginDoor,@DestinationDoor,@CustomerInqNo,@OriginRailRamp,@DestinationTerminal,@LicenseType,@SiteId",
               new SqlParameter("Id", objEnquiryDetail.EnquiryID),
               new SqlParameter("EnquiryDate", objEnquiryDetail.EnquiryDate),
               new SqlParameter("EnquiryNo", objEnquiryDetail.EnquiryNo),
               new SqlParameter("fkCompanyID", objEnquiryDetail.fkCompanyID ?? Convert.DBNull),
               new SqlParameter("BillTo", objEnquiryDetail.BillTo ?? Convert.DBNull),
               new SqlParameter("BillToCompanyId", objEnquiryDetail.BillToCompanyId ?? Convert.DBNull),
               new SqlParameter("ShipmentMoveDate", objEnquiryDetail.ShipmentMoveDate ?? Convert.DBNull),
               new SqlParameter("PickupType", objEnquiryDetail.PickupType ?? Convert.DBNull),
               new SqlParameter("PickupRemark", objEnquiryDetail.PickupRemark ?? Convert.DBNull),
               new SqlParameter("PortOfOrigin", objEnquiryDetail.OriginID),
               new SqlParameter("PortOfDischarge", objEnquiryDetail.DischargeID),
               new SqlParameter("CityOfDischarge", objEnquiryDetail.DestinationCityID ?? Convert.DBNull),
               new SqlParameter("CityOfOrigin", objEnquiryDetail.OriginCityID ?? Convert.DBNull),
               new SqlParameter("CountryOfDischarge", objEnquiryDetail.DestinationCountryID ?? Convert.DBNull),
               new SqlParameter("CountryOfOrigin", objEnquiryDetail.OriginCountryID ?? Convert.DBNull),
               new SqlParameter("StateOfDischarge", objEnquiryDetail.DestinationStateID ?? Convert.DBNull),
               new SqlParameter("StateOfOrigin", objEnquiryDetail.OrignStateID ?? Convert.DBNull),
               new SqlParameter("NoOfContainer", objEnquiryDetail.NoOfContainer ?? Convert.DBNull),
               new SqlParameter("TypeOfEnquiry", Convert.ToInt32(objEnquiryDetail.TypeOfEnquiry)),
               new SqlParameter("ReceivedBy", objEnquiryDetail.ReceivedByID ?? Convert.DBNull),
               new SqlParameter("Remarks", objEnquiryDetail.Remarks ?? Convert.DBNull),
               new SqlParameter("Type", "insert"),
               new SqlParameter("Description", objEnquiryDetail.Description ?? Convert.DBNull),
               new SqlParameter("Class", objEnquiryDetail.Class ?? Convert.DBNull),
               new SqlParameter("UNNo", objEnquiryDetail.UNNo ?? Convert.DBNull),
               new SqlParameter("PackingType", objEnquiryDetail.PackingType ?? Convert.DBNull),
               new SqlParameter("HazRemarks", objEnquiryDetail.HazRemarks ?? Convert.DBNull),
               new SqlParameter("ChkHAZ", objEnquiryDetail.IsHaz ?? Convert.DBNull),
               new SqlParameter("CreatedBy", objEnquiryDetail.UserID ?? Convert.DBNull),
               new SqlParameter("UpdatedBy", objEnquiryDetail.UpdatedBy ?? Convert.DBNull),
               new SqlParameter("ModifiedOn", System.DateTime.Now.Date),
               new SqlParameter("Hazweight", objEnquiryDetail.Hazweight ?? Convert.DBNull),
               new SqlParameter("HazVolume", objEnquiryDetail.HazVolume ?? Convert.DBNull),
               new SqlParameter("DepartmentID", objEnquiryDetail.DepartmentID ?? Convert.DBNull),
               new SqlParameter("LastEnquiryNo", objEnquiryDetail.LastEnquiryNo ?? Convert.DBNull),
               new SqlParameter("ControlNo", objEnquiryDetail.EnquiryControlNo ?? Convert.DBNull),
               new SqlParameter("IsDraft", objEnquiryDetail.IsDraft == null ? 0 : objEnquiryDetail.IsDraft),
               new SqlParameter("GalRepID", objEnquiryDetail.GalRepID ?? Convert.DBNull),
               new SqlParameter("OriginDoor", objEnquiryDetail.OriginDoorID ?? Convert.DBNull),
               new SqlParameter("DestinationDoor", objEnquiryDetail.DestinationDoorID ?? Convert.DBNull),
               new SqlParameter("CustomerInqNo", objEnquiryDetail.CustomerInqNo ?? Convert.DBNull),
               new SqlParameter("OriginRailRamp", objEnquiryDetail.OrgnRailRampId ?? Convert.DBNull),
               new SqlParameter("DestinationTerminal", objEnquiryDetail.DestnTerminalId ?? Convert.DBNull),
               new SqlParameter("LicenseType", objEnquiryDetail.LicenseType ?? Convert.DBNull),
               new SqlParameter("SiteId", objEnquiryDetail.SiteId)).ToList();

                List<int> objList = objResult.ToList();
                int EnquiryId = objList[0];
                newEnqId = EnquiryId.ToString();
                if (EnquiryId > 0)
                {
                    IEnumerable<int> objDeleteResult;
                    //delete existing enquiry Commdity details
                    objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_COMMODITY_DELETE @RefId,@RefType",
                        new SqlParameter("RefId", EnquiryId),
                        new SqlParameter("RefType", "Enquiry")).ToList();
                    if (objEnquiryDetail.CommodityDTOList.Count() > 0)
                    {
                        SaveEnquiryCommodityDetails(objEnquiryDetail.CommodityDTOList, EnquiryId, "Enquiry");
                    }
                    //save next action remarks
                    if (objEnquiryDetail.NextActionRemarks != "" && objEnquiryDetail.NextActionDueDate != null)
                    {
                        IEnumerable<int> objRemarksResult;
                        objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                        new SqlParameter("RefID", EnquiryId),
                        new SqlParameter("NextActionDate", objEnquiryDetail.NextActionDueDate),
                        new SqlParameter("Type", "Enquiry"),
                        new SqlParameter("ActivityID", 1),
                        new SqlParameter("Remarks", objEnquiryDetail.NextActionRemarks)).ToList();
                    }
                    int modeOfService = Convert.ToInt32(objEnquiryDetail.ModeOfService);
                    //delete existing Container Service details
                    objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_SERVICE_REF_DELETE @RefId,@RefType",
                        new SqlParameter("RefId", EnquiryId),
                        new SqlParameter("RefType", "Enquiry")).ToList();
                    //For containers & FCL = 1
                    if (objEnquiryDetail.EnquiryContainerServiceDTOList.Count() > 0)
                    {
                        SaveEnquiryContainerService(objEnquiryDetail.EnquiryContainerServiceDTOList, EnquiryId, modeOfService, "Enquiry", objEnquiryDetail.NoOfContainer);

                        if (modeOfService == 7) //AIR
                        {
                            if (objEnquiryDetail.EnquiryAIRServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryAIRService(objEnquiryDetail.EnquiryAIRServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                            }
                        }
                        if (modeOfService == 5) //Break Bulk
                        {
                            if (objEnquiryDetail.EnquiryBreakBulkServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryBreakBulkService(objEnquiryDetail.EnquiryBreakBulkServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                            }
                        }
                        if (modeOfService == 2) //LCL
                        {
                            if (objEnquiryDetail.EnquiryLCLServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryLCLService(objEnquiryDetail.EnquiryLCLServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                            }
                        }

                        if (modeOfService == 3)  //RORO Service details
                        {
                            if (objEnquiryDetail.EnquiryROROServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryROROService(objEnquiryDetail.EnquiryROROServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                            }
                        }
                    }
                }
                // create enquiry finish
                QuotationController quo = new QuotationController();
                QuotationDetailDTO QuotationDetailFilter = new QuotationDetailDTO();
                QuotationDetailFilter.EnquiryID = Convert.ToDecimal(oldEnqId);
                QuotationDetailFilter.QuotationID = Convert.ToDecimal(oldquotationid);
                QuotationDetailDTO old_quatationdetail = GetQuationDetailOnly(QuotationDetailFilter);

                QuotationDetailFilter.EnquiryID = Convert.ToDecimal(newEnqId);
                QuotationDetailFilter.QuotationID = 0;

                QuotationDetailDTO new_quatationdetail = GetQuationDetailOnly(QuotationDetailFilter);

                old_quatationdetail.QuotationID = 0;
                old_quatationdetail.QuotationNo = old_quatationdetail.QuotationNo.ToString().Replace("NRA", "") + "T";//   new_quatationdetail.QuotationNo;
                old_quatationdetail.EnquiryNo = newEnqNo;
                old_quatationdetail.EnquiryID = Convert.ToDecimal(newEnqId);
                old_quatationdetail.CreatedBy = objEnquiry.CreatedBy;
                decimal new_QuotationID = SaveQuationDetail(old_quatationdetail);

                old_quatationdetail.QuotationID = new_QuotationID;
                ApproveQuotation(old_quatationdetail.QuotationID, true, true, true);
                ApproveQuotationCarrier(old_quatationdetail.CarrierDTOList, new_QuotationID);
                SendEmail(new_QuotationID, "", "");
                _context.ExecuteQuery<int>("EXEC dbo.Add_Site_Change_Detail @OldQuotationId,@NewQuotationId",
                                        new SqlParameter("OldQuotationId", oldquotationid),
                                        new SqlParameter("NewQuotationId", new_QuotationID)).ToList();


                return AppResult(old_quatationdetail, "Site Change Successfully");
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        public void ApproveQuotation(decimal QuotationID, bool IsDraft, bool IsApproved, bool IsBooked)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_APPROVE @QuotationID,@IsDraft,@IsApproved,@IsBooked",
                                   new SqlParameter("QuotationID", QuotationID),
                                   new SqlParameter("IsDraft", IsDraft),
                                   new SqlParameter("IsApproved", IsApproved),
                                   new SqlParameter("IsBooked", IsBooked)).ToList();
                List<int> objList = objResult.ToList();

            }
            catch (Exception ex)
            {
                // return AppResult(ex);
            }
        }
        public decimal SaveQuationDetail(QuotationDetailDTO objQuotationDetail)
        {
            decimal QuotationId = 0;
            try
            {
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
                 QuotationId = objList[0];
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

                Logger.WriteInfo(" error in SaveQuationDetail" + ex.StackTrace.ToString());

            }
            return QuotationId;
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
        public void SaveEnquiryCommodityDetails(CommodityDTO[] objCommodityDTOList, int EnquiryID, string RefType)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (CommodityDTO objCommodityDTO in objCommodityDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_COMMODITY_INSERT @fkCommodityId,@RefId,@RefType",
                    new SqlParameter("fkCommodityId", objCommodityDTO.CommodityId),
                    new SqlParameter("RefId", EnquiryID),
                    new SqlParameter("RefType", RefType)).ToList();
            }
        }
        public void SaveCarrierCharges(CarrierChargesDTO[] objCarrierChargesDTOList, decimal QuotationCarrierId, decimal QuotationId)
        {
            //insert enquiry Commodity details
            IEnumerable<decimal> objResult;
            foreach (CarrierChargesDTO objCarrierChargesDTO in objCarrierChargesDTOList)
            {
                if (objCarrierChargesDTO.RefName == "Ocean Frieght" || objCarrierChargesDTO.RefName == "Ocean Freight")
                {
                    objResult = _context.ExecuteQuery<decimal>("EXEC dbo.USP_LG_QUOTATION_CARRIER_CONTAINER_RATE_INSERT @QuotationID,@QuotationCarrierID,@RefID,@Ref,@BuyingRate,@SellingRate,@fkContainerID,@IsApplicable",
                    new SqlParameter("@QuotationID", QuotationId),
                    new SqlParameter("@QuotationCarrierID", QuotationCarrierId),
                    new SqlParameter("@RefID", objCarrierChargesDTO.RefId),
                    new SqlParameter("@Ref", objCarrierChargesDTO.RefName),
                    new SqlParameter("@BuyingRate", objCarrierChargesDTO.BuyingRate),
                    new SqlParameter("@SellingRate", objCarrierChargesDTO.SellingRate),
                    new SqlParameter("@fkContainerID", objCarrierChargesDTO.fkContainerID),
                    new SqlParameter("@IsApplicable", objCarrierChargesDTO.IsApplicable)).ToList();
                }
            }
            foreach (CarrierChargesDTO objCarrierChargesDTO in objCarrierChargesDTOList)
            {
                if (objCarrierChargesDTO.RefName != "Ocean Frieght")
                {
                    if (objCarrierChargesDTO.RefName != "Ocean Freight")
                    {
                    objResult = _context.ExecuteQuery<decimal>("EXEC dbo.USP_LG_QUOTATION_CARRIER_RATES_INSERT @QuotationID,@QuotationCarrierID,@RefID,@Ref,@BuyingRate,@SellingRate,@fkContainerID,@IsApplicable",
                    new SqlParameter("@QuotationID", QuotationId),
                    new SqlParameter("@QuotationCarrierID", QuotationCarrierId),
                    new SqlParameter("@RefID", objCarrierChargesDTO.RefId),
                    new SqlParameter("@Ref", objCarrierChargesDTO.RefName),
                    new SqlParameter("@BuyingRate", objCarrierChargesDTO.BuyingRate),
                    new SqlParameter("@SellingRate", objCarrierChargesDTO.SellingRate),
                    new SqlParameter("@fkContainerID", objCarrierChargesDTO.fkContainerID),
                    new SqlParameter("@IsApplicable", objCarrierChargesDTO.IsApplicable)).ToList();
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
        public void SaveEnquiryContainerService(EnquiryContainerServiceDTO[] EnquiryContainerServiceDTOList, int EnquiryID, int modeOfService, string RefType, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryContainerServiceDTO objContainerServiceDTO in EnquiryContainerServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_FCL_INSERT @ServiceRequiredID,@NoofContainer,@ContainerSizeID,@QTY,@RefId,@RefType",
                    new SqlParameter("ServiceRequiredID", modeOfService),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("ContainerSizeID", objContainerServiceDTO.ContainerTypeID),
                    new SqlParameter("QTY", objContainerServiceDTO.Quantity),
                    new SqlParameter("RefId", EnquiryID),
                    new SqlParameter("RefType", RefType)).ToList();
            }
        }

        public void SaveEnquiryAIRService(EnquiryAIRServiceDTO[] EnquiryAIRServiceDTOList, int EnquiryID, string RefType, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryAIRServiceDTO objAIRService in EnquiryAIRServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_AIR_INSERT @ServiceRequiredID,@NoofContainer,@PieceCount,@Weight,@TotalWt,@Length,@Width,@Height,@Volume,@RefId,@RefType",
                    new SqlParameter("ServiceRequiredID", 7),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("PieceCount", objAIRService.PieceCount),
                    new SqlParameter("Weight", objAIRService.Weight),
                    new SqlParameter("TotalWt", objAIRService.TotalWt),
                    new SqlParameter("Length", objAIRService.Length),
                    new SqlParameter("Width", objAIRService.Width),
                    new SqlParameter("Height", objAIRService.Height),
                    new SqlParameter("Volume", objAIRService.Volume),
                    new SqlParameter("RefId", EnquiryID),
                    new SqlParameter("RefType", RefType)).ToList();
            }

        }

        public void SaveEnquiryBreakBulkService(EnquiryBreakBulkServiceDTO[] EnquiryBreakBulkServiceDTOList, int EnquiryID, string RefType, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryBreakBulkServiceDTO objBBulkService in EnquiryBreakBulkServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_BREAK_BULK_INSERT @ServiceRequiredID,@NoofContainer,@RefId,@RefType,@Weight,@Length,@Width,@Height,@Volume,@Description,@Qty,@ChkInland,@InlandRemarks,@MafiCharges",
                    new SqlParameter("ServiceRequiredID", 5),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("RefId", EnquiryID),
                    new SqlParameter("RefType", RefType),
                    new SqlParameter("Weight", objBBulkService.Weight),
                    new SqlParameter("Length", objBBulkService.Length),
                    new SqlParameter("Width", objBBulkService.Width),
                    new SqlParameter("Height", objBBulkService.Height),
                    new SqlParameter("Volume", objBBulkService.Volume ?? Convert.DBNull),
                    new SqlParameter("Description", objBBulkService.Description ?? Convert.DBNull),
                    new SqlParameter("Qty", objBBulkService.Qty),
                    new SqlParameter("ChkInland", objBBulkService.ChkInland ?? Convert.DBNull),
                    new SqlParameter("InlandRemarks", objBBulkService.InlandRemarks ?? Convert.DBNull),
                    new SqlParameter("MafiCharges", objBBulkService.MafiCharges ?? Convert.DBNull)).ToList();
            }
        }
        public void SaveEnquiryLCLService(EnquiryLCLServiceDTO[] EnquiryLCLServiceDTOList, int EnquiryID, string RefType, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryLCLServiceDTO objLCLService in EnquiryLCLServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_LCL_INSERT @ServiceRequiredID,@NoofContainer,@PieceCount,@Weight,@TotalWt,@Length,@Width,@Height,@Volume,@RefId,@RefType",
                    new SqlParameter("ServiceRequiredID", 2),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("PieceCount", objLCLService.PieceCount),
                    new SqlParameter("Weight", objLCLService.Weight),
                    new SqlParameter("TotalWt", objLCLService.TotalWt),
                    new SqlParameter("Length", objLCLService.Length),
                    new SqlParameter("Width", objLCLService.Width),
                    new SqlParameter("Height", objLCLService.Height),
                    new SqlParameter("Volume", objLCLService.Volume ?? Convert.DBNull),
                    new SqlParameter("RefId", EnquiryID),
                    new SqlParameter("RefType", RefType)).ToList();
            }

        }
        public void SaveEnquiryROROService(EnquiryROROServiceDTO[] EnquiryROROServiceDTOList, int EnquiryID, string RefType, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult1;
            foreach (EnquiryROROServiceDTO objROROService in EnquiryROROServiceDTOList)
            {
                objResult1 = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_RORO_INSERT @ServiceRequiredID,@NoofContainer,@RefId,@RefType,@NoofCrain,@Weight,@Length,@Width,@Volume,@Height,@ChkInland,@InlandRemarks,@MafiCharges",
                    new SqlParameter("ServiceRequiredID", 3),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("RefId", EnquiryID),
                    new SqlParameter("RefType", RefType),
                    new SqlParameter("NoofCrain", objROROService.NoofCrain ?? Convert.DBNull),
                    new SqlParameter("Weight", objROROService.Weight),
                    new SqlParameter("Length", objROROService.Length),
                    new SqlParameter("Width", objROROService.Width),
                    new SqlParameter("Volume", objROROService.Volume),
                    new SqlParameter("Height", objROROService.Height),
                    new SqlParameter("ChkInland", objROROService.ChkInland ?? Convert.DBNull),
                    new SqlParameter("InlandRemarks", objROROService.InlandRemarks ?? Convert.DBNull),
                    new SqlParameter("MafiCharges", objROROService.MafiCharges ?? Convert.DBNull)).ToList();
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
                    body = body.Replace("{{Equipment}}", "  " + EquipmentValue ?? "");
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
                                            CarrierTable = CarrierTable + "<td style='border:#666666 1px solid; text-align:left; padding:5px 3px; vertical-align:top;'>&nbsp;" + objCarrierDTO.Remarks + "</td>";
                                            CarrierTable = CarrierTable + "</tr>";
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

                result[0].EmailContent = body + "\n" + CarrierTable;
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


        public void ApproveQuotationCarrier(QuotationCarrierDTO[] objQuotationCarrierDTOList, decimal quationid)
        {
            try
            {
                IEnumerable<int> objResult = null;
                foreach (QuotationCarrierDTO objQuotationCarrierDTO in objQuotationCarrierDTOList)
                {
                    objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_CARRIER_APPROVE @QuotationID,@CarrierID,@IsApproved,@CustReqCutOffDate,@CustReqSailingDate",
                                       new SqlParameter("QuotationID", quationid),
                                       new SqlParameter("CarrierID", objQuotationCarrierDTO.CarrierID),
                                       new SqlParameter("IsApproved", objQuotationCarrierDTO.ApprovedForBooking),
                                       new SqlParameter("CustReqCutOffDate", objQuotationCarrierDTO.custReqCutOffDate ?? Convert.DBNull),
                                       new SqlParameter("CustReqSailingDate", objQuotationCarrierDTO.custReqSailingDate ?? Convert.DBNull)).ToList();
                }
                List<int> objList = objResult.ToList();

            }
            catch (Exception ex)
            {

            }
        }
        private void SendEmail(decimal QuotationID, string EmailTo, string EmailBody)
        {
            List<QuotationListDTO> result = _context.ExecuteQuery<QuotationListDTO>("EXEC dbo.USP_LG_QUOTATION_EMAIL_Content_UPDATE @QuotationID, @EmailTo,@EmailBcc,@EmailCC,@EmailBody,@EmailSubject",
                       new SqlParameter("QuotationID", QuotationID),
                       new SqlParameter("EmailTo", "Noreply@miamigloballines.com"),
                       new SqlParameter("EmailBcc", ""),
                       new SqlParameter("EmailCC", ""),
                       new SqlParameter("EmailBody", EmailBody),
                        new SqlParameter("EmailSubject", "Duplicate")

                       ).ToList();
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult getBookingDetail_BYQuationID(BookingDetailDTO objBooking)
        {
            try
            {
                List<BookingDetailDTO> result = _context.ExecuteQuery<BookingDetailDTO>("EXEC dbo.USP_GET_Quotation_BY_QuotaionId @QUOTATIONID",

                                        new SqlParameter("QUOTATIONID", objBooking.QuotationID)).ToList();
                BookingDetailDTO objBookingDetailDTO = result[0];
                //objBookingDetailDTO.EnquiryID = objBooking.EnquiryID;
                //objBookingDetailDTO.QuotationID = objBooking.QuotationID;

                ////get all Next Action remarks
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                     new SqlParameter("QUOTATIONID", objBookingDetailDTO.QuotationID),
                     new SqlParameter("ENQUIRYID", objBookingDetailDTO.EnquiryID),
                     new SqlParameter("BookingID", objBookingDetailDTO.DocumentCommonID),
                     new SqlParameter("RefType", "Booking")).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                if (RemarksResultList.Count > 0)
                {
                    objBookingDetailDTO.NextActionRemarksDTOList = RemarksResult.ToArray();
                    objBookingDetailDTO.LastRemarks = RemarksResultList[0].Remarks;
                    objBookingDetailDTO.LastRemarkDate = RemarksResultList[0].NextActionDate;
                }
                int RefId = 0;
                string RefType = "";
                if (result[0].DocumentCommonID > 0)
                {
                    RefId = result[0].DocumentCommonID;
                    RefType = "Booking";
                }
                else
                {
                    RefId = Convert.ToInt32(objBookingDetailDTO.EnquiryID);
                    RefType = "Enquiry";
                }
                List<CommodityDTO> CommodityDTOList = _context.ExecuteQuery<CommodityDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_COMMODITY @RefId,@RefType",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType)).ToList();
                if (CommodityDTOList.Count > 0)
                {
                    objBookingDetailDTO.CommodityDTOList = CommodityDTOList.ToArray();
                }
                //get Container details / FCL                                     
                List<EnquiryContainerServiceDTO> ContainerServiceDTOList = _context.ExecuteQuery<EnquiryContainerServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_CONTAINER_LIST @RefId,@RefType",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType)).ToList();
                if (ContainerServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryContainerServiceDTOList = ContainerServiceDTOList.ToArray();
                }
                //get Air
                List<EnquiryAIRServiceDTO> AirServiceDTOList = _context.ExecuteQuery<EnquiryAIRServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_AIR @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 7)).ToList();
                if (AirServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryAIRServiceDTOList = AirServiceDTOList.ToArray();
                }
                //get Break Bulk
                List<EnquiryBreakBulkServiceDTO> BreakBulkServiceDTOList = _context.ExecuteQuery<EnquiryBreakBulkServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_BREAK_BULK @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 5)).ToList();
                if (BreakBulkServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryBreakBulkServiceDTOList = BreakBulkServiceDTOList.ToArray();
                }
                //get LCL
                List<EnquiryLCLServiceDTO> LCLServiceDTOList = _context.ExecuteQuery<EnquiryLCLServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_LCL @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 2)).ToList();
                if (LCLServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryLCLServiceDTOList = LCLServiceDTOList.ToArray();
                }
                //get RORO
                List<EnquiryROROServiceDTO> ROROServiceDTOList = _context.ExecuteQuery<EnquiryROROServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_RORO @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", RefId),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 3)).ToList();
                if (ROROServiceDTOList.Count > 0)
                {
                    objBookingDetailDTO.EnquiryROROServiceDTOList = ROROServiceDTOList.ToArray();
                }

                // Add CarrierCharges by vikas solanki  on 21 june 2020
                //  List<CarrierChargesDTO> carrir = _context.ExecuteQuery<CarrierChargesDTO>("EXEC dbo.USP_LG_BOOKING_GET_CARRIER_RATES @DOCUMENTCOMMONID",
                //                 new SqlParameter("DOCUMENTCOMMONID", objBooking.DocumentCommonID)).ToList();
                List<CarrierChargesDTO> carrir = _context.ExecuteQuery<CarrierChargesDTO>("EXEC dbo.USP_LG_QUOTATION_GET_CARRIER_RATES @QuotationID,@CarrierID",
                                new SqlParameter("QuotationID", objBookingDetailDTO.QuotationID),
                                new SqlParameter("CarrierID", objBookingDetailDTO.ContactID)
                                ).ToList();

                if (carrir.Count > 0)
                {
                    objBookingDetailDTO.CarrierChargesDTOList = carrir.ToArray();
                }
                // end Code 



                return AppResult(objBookingDetailDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
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
        public virtual ActionResult SaveHtsCode(HtsCode dto)
        {
            try
            {
                HtsCode data = null;
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_HtsCode @Id,@htscode,@description,@unit,@unit1,@indent,@isvin,@isDel,@CreatedBY,@ActionType",
                               new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                               new SqlParameter("htscode", dto.HTSNumber ?? Convert.DBNull),
                               new SqlParameter("description", dto.description ?? Convert.DBNull),
                               new SqlParameter("unit", dto.FirstUOM ?? Convert.DBNull),
                               new SqlParameter("unit1", dto.SecondUOM ?? Convert.DBNull),
                               new SqlParameter("indent", dto.indent ?? Convert.DBNull),
                               new SqlParameter("isvin", dto.isvin == true ? 1 : 0),
                               new SqlParameter("isDel", dto.isDel == true ? 1 : 0),
                               new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                               new SqlParameter("ActionType", "insert")).ToList();
                List<int> objList = objResult.ToList();
               
                int num = objList[0];
                if (num > 0)
                {
                    List<HtsCode> list2 = _context.ExecuteQuery<HtsCode>("EXEC dbo.USP_LG_GET_HtsCode @Id", new object[1]
                   {
                        new SqlParameter("Id", num)
                   }).ToList();
                    data = list2[0];
                }
                return AppResult(data, 1L, "", EnumResult.Success);


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
        public virtual ActionResult PreAlert(BookingListDTO objBooking)
        {
            try
            {
                Logger.WriteInfo(" prealert: 1" + objBooking.CreatedBy.ToString());
                var userresult = _context.ExecuteQuery<LG_CONTACT>("EXEC dbo.USP_LG_USER_GET_CONTACT @USERID",
                                        new SqlParameter("USERID", objBooking.CreatedBy)
                                        ).FirstOrDefault();
                Logger.WriteInfo(" prealert: 34" + objBooking.DocumentCommonID + "|" + objBooking.SiteId);
                List<BookingListDTO> result1 = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_EXPORT_REGISTER_DETAILS @DOCUMENTCOMMONID,@SIT_ID",
                                        new SqlParameter("DOCUMENTCOMMONID", objBooking.DocumentCommonID),
                                        new SqlParameter("SIT_ID", objBooking.SiteId)).ToList();

                Logger.WriteInfo(" prealert: 2" + result1.Count());
                BookingListDTO objBookingListDTO = result1[0];

                string quotationID = Request.Headers.GetValues("EnqId").ToList()[0];
                var result = _context.ExecuteQuery<QuotationEmailData>("EXEC dbo.USP_Get_Quatation_Email_Deatil @QuotationID",
                    new SqlParameter("QuotationID", quotationID)
                    ).ToList();
                result[0].DocumentCommonID = objBooking.DocumentCommonID;
                string subject = "Pre – Alert for your shipment from  Load Port " + "(" + result1[0].Origin + ")" + " to Discharge Port " + "(" + result1[0].PortOfDischarge + ")";
                subject = subject + " vide HBL " + result1[0].ExportRef + " ,MBL " + result1[0].BLNumber + "  Shipped by " + result1[0].ShippingLine + "  on vessel ";
                subject = subject + result1[0].Vessel + " Voy " + result1[0].Voyage + " ETA at " + result1[0].Destination + " dated " + result1[0].ETA;
                //  IEnumerable<DispatchContainerDTO> ContainerResult = _context.ExecuteQuery<DispatchContainerDTO>("EXEC dbo.USP_LG_BOOKING_GET_CONTAINER_DISPATCH_TABLE @CommonID",
                //                          new SqlParameter("CommonID", objBooking.DocumentCommonID)).ToList();
                //  List<DispatchContainerDTO> DispatchContainerDTOList = ContainerResult.ToList();
                IEnumerable<DispatchContainerDTO> ContainerResult = _context.ExecuteQuery<DispatchContainerDTO>("EXEC dbo.USP_LG_BOOKING_GET_CONTAINER_DISPATCH_TABLE @CommonID",
                                         new SqlParameter("CommonID", objBooking.DocumentCommonID)).ToList();
                List<DispatchContainerDTO> DispatchContainerDTOList = ContainerResult.ToList();
                if (DispatchContainerDTOList.Count > 0)
                {
                    objBookingListDTO.DispatchContainerDTOList = DispatchContainerDTOList.ToArray();
                }
                IEnumerable<DispatchContainerDTO> DispatchTableResult = _context.ExecuteQuery<DispatchContainerDTO>("EXEC dbo.USP_LG_BOOKING_GET_DISPATCH_TABLE_DATA @CommonID",
                                        new SqlParameter("CommonID", objBooking.DocumentCommonID)).ToList();
                List<DispatchContainerDTO> DispatchTableList = DispatchTableResult.ToList();
                if (DispatchContainerDTOList.Count > 0 && DispatchTableList.Count > 0)
                {
                    foreach (DispatchContainerDTO ContainerItem in objBookingListDTO.DispatchContainerDTOList)
                    {
                        foreach (DispatchContainerDTO DispatchItem in DispatchTableList)
                        {
                            if (DispatchItem.ContainerID == ContainerItem.ContainerID && DispatchItem.SeqNo == ContainerItem.SeqNo)
                            {
                                ContainerItem.CNTNo = DispatchItem.CNTNo;
                                ContainerItem.SealNo = DispatchItem.SealNo;
                                ContainerItem.SeqNo = DispatchItem.SeqNo;
                                ContainerItem.DispatchRemarks = DispatchItem.DispatchRemarks;
                                ContainerItem.IsSelected = true;
                            }
                        }
                    }
                }

                if (DispatchContainerDTOList.Count > 0)
                {
                    objBookingListDTO.DispatchContainerDTOList = DispatchContainerDTOList.ToArray();
                }

                IEnumerable<ShipmentDocsDTO> DocsResult = _context.ExecuteQuery<ShipmentDocsDTO>("EXEC dbo.USP_LG_BOOKING_GET_CUSTOMER_DOCS @DocumentCommonID,@DocumentType",
                     new SqlParameter("DocumentCommonID", objBooking.DocumentCommonID),
                     new SqlParameter("DocumentType", "MBL")
                     ).ToList();
                List<ShipmentDocsDTO> ShipmentDocsResultList = DocsResult.ToList();
                IEnumerable<ShipmentDocsDTO> DocsResultHBL = _context.ExecuteQuery<ShipmentDocsDTO>("EXEC dbo.USP_LG_BOOKING_GET_CUSTOMER_DOCS @DocumentCommonID,@DocumentType",
                    new SqlParameter("DocumentCommonID", objBooking.DocumentCommonID),
                    new SqlParameter("DocumentType", "HBL")
                    ).ToList();
                List<ShipmentDocsDTO> ShipmentDocsResultListHBL = DocsResultHBL.ToList();
                if (ShipmentDocsResultListHBL.Count > 0)
                {
                    int indexC = 0;
                    foreach (var item in ShipmentDocsResultListHBL)
                    {
                        ShipmentDocsDTO HBLDOC = ShipmentDocsResultListHBL[indexC];
                        ShipmentDocsResultList.Add(HBLDOC);
                        indexC++;
                    }

                }
                IEnumerable<ShipmentDocsDTO> DocsResultCO = _context.ExecuteQuery<ShipmentDocsDTO>("EXEC dbo.USP_LG_BOOKING_GET_CUSTOMER_DOCS @DocumentCommonID,@DocumentType",
                   new SqlParameter("DocumentCommonID", objBooking.DocumentCommonID),
                   new SqlParameter("DocumentType", "CO")
                   ).ToList();
                List<ShipmentDocsDTO> ShipmentDocsResultListCO = DocsResultCO.ToList();
                if (ShipmentDocsResultListCO.Count > 0)
                {
                    int indexC = 0;
                    foreach (var item in ShipmentDocsResultListHBL)
                    {
                        ShipmentDocsDTO CODOC = ShipmentDocsResultListCO[indexC];
                        ShipmentDocsResultList.Add(CODOC);
                        indexC++;
                    }


                    // objBookingListDTO.ShipmentDocsDTOList.Concat(ShipmentDocsResultListCO.ToArray());
                }
                string mailbody = "<span style='background-color:yellow'>For any query please write to <b>" + userresult.CntEmail + "<b></span><br/>";

                mailbody += "Dear Sir<br/><br/> Kindly find below the details of your subject shipment scheduled to arrive " + result1[0].PortOfDischarge + " on " + result1[0].ETA + "<br/>";
                mailbody = mailbody + "You are requested to reconfirm ETA and arrival of your containers on the vessel with carrier’s local office / agent for any ";
                mailbody = mailbody + "last moment changes.<br/>";
                if (ShipmentDocsResultListHBL.Count > 0 || ShipmentDocsResultList.Count > 0 || ShipmentDocsResultListCO.Count > 0)
                {
                    mailbody = mailbody + "Also find attached ";
                }
                if (ShipmentDocsResultListHBL.Count > 0)
                {
                    mailbody = mailbody + " HBL " + result1[0].ExportRef;

                }
                if (ShipmentDocsResultList.Count > 0)
                {
                    mailbody = mailbody + " MBL " + result1[0].BLNumber;
                }

                if (ShipmentDocsResultListCO.Count > 0)
                {
                    mailbody = mailbody + " and Certificate of Origin " + result1[0].BLNumber;
                }

                mailbody = mailbody + "<div style='width: 50%; margin: 0 auto;'><div style='text-align: center;width: 100%;'>";
                mailbody = mailbody + "<br/><div>";


                string stable = "<table class='borderTable' border='1' style='width:100%;'><tr><td>Sr.No.</td><td>Container #</td><td>Seal #</td><td>MBL#</td></tr>";
                int index = 1;
                if (DispatchContainerDTOList != null)
                {

                    foreach (var item in DispatchContainerDTOList.OrderBy(x => x.SeqNo))
                    {
                        stable = stable + "<tr><td>" + index + "</td><td>" + item.CNTNo + "</td><td>" + item.SealNo + "</td><td> " + result1[0].BLNumber + " </td></tr>";
                        index++;
                    }
                }
                mailbody = mailbody + stable;

                mailbody = mailbody + "</table></div>";
                result[0].EmailBody = mailbody;
                result[0].EmailSubject = subject;

                //BookingListDTO objBookingListDTO = new BookingListDTO();
                //get all Shipment Docs details
                if (ShipmentDocsResultList.Count > 0)
                {
                    result[0].ShipmentDocsDTOList = ShipmentDocsResultList.ToArray();
                }
                if (!string.IsNullOrEmpty(result[0].EmailTo))
                {
                    result[0].EmailTo = result[0].EmailTo + ";" + userresult.CntEmail;
                }
                else
                {
                    result[0].EmailTo = userresult.CntEmail;
                }
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult SendEmailPreAlert(QuotationEmailData emailData)
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
                //IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_EMAIL_UPDATE @QuotationID",
                //                 new SqlParameter("QuotationID", emailData.QuotationID)).ToList();
                IEnumerable<int> objResult = null;

                List<int> objList = new List<int>();
                objList.Add(1);
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress("Noreply@miamigloballines.com");
                //mailMessage.To.Add(new MailAddress(emailData.EmailTo));
                if (emailData.EmailTo.Contains(";"))
                {
                    string[] strUser = emailData.EmailTo.Split(';');
                    for (int i = 0; i < strUser.Length; i++)
                    {
                        if (strUser[i] != "")
                        {
                            mailMessage.To.Add(new System.Net.Mail.MailAddress(strUser[i]));
                            EmailList.Add(strUser[i]);
                        }
                    }
                }
                else
                {
                    mailMessage.To.Add(new System.Net.Mail.MailAddress(emailData.EmailTo));
                    EmailList.Add(emailData.EmailTo);
                }
                if (!string.IsNullOrEmpty(emailData.Emailcc))
                {
                    if (emailData.Emailcc.Contains(";"))
                    {
                        string[] strUser = emailData.Emailcc.Split(';');
                        for (int i = 0; i < strUser.Length; i++)
                        {
                            if (strUser[i] != "")
                            {
                                mailMessage.CC.Add(new System.Net.Mail.MailAddress(strUser[i]));
                                EmailList.Add(strUser[i]);
                            }
                        }
                    }
                    else
                    {
                        mailMessage.CC.Add(new System.Net.Mail.MailAddress(emailData.Emailcc));
                        EmailList.Add(emailData.Emailcc);
                    }
                }
                if (!string.IsNullOrEmpty(emailData.EmailBcc))
                {
                    if (emailData.EmailBcc.Contains(";"))
                    {
                        string[] strUser = emailData.EmailBcc.Split(';');
                        for (int i = 0; i < strUser.Length; i++)
                        {
                            if (strUser[i] != "")
                            {
                                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(strUser[i]));
                                EmailList.Add(strUser[i]);
                            }
                        }
                    }
                    else
                    {
                        mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(emailData.EmailBcc));
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

                var ShipmentFolder = ConfigurationManager.AppSettings["ShipperDocPath"];
                var BookingFolder = ConfigurationManager.AppSettings["BookingDocPath"];
                string root1 = HttpContext.Current.Server.MapPath(ShipmentFolder);
                string root2 = HttpContext.Current.Server.MapPath(BookingFolder);
                System.Net.Mail.Attachment attachment;
                DirectoryInfo dir1 = new DirectoryInfo(root1);
                DirectoryInfo dir2 = new DirectoryInfo(root2);


                var Count = 0;

                if (emailData.ShipmentDocsDTOList != null)
                {
                    foreach (var objFileAttachement in emailData.ShipmentDocsDTOList)
                    {
                        string fileName = "";
                        if (objFileAttachement.DocumentType == "HBL" || objFileAttachement.DocumentType == "HBLWithoutLogo")
                        {
                            if (Convert.ToInt32(objFileAttachement.IsSysGenerated) == 0)
                            {
                                fileName = emailData.DocumentCommonID + "_" + objFileAttachement.DocName;
                            }
                            ShipmentFolder = ConfigurationManager.AppSettings["HBLDocPath"];
                        }
                        if (objFileAttachement.DocumentType == "MBL")
                        {
                            if (Convert.ToInt32(objFileAttachement.IsSysGenerated) == 0)
                            {
                                fileName = emailData.DocumentCommonID + "_" + objFileAttachement.DocName;
                            }
                            ShipmentFolder = ConfigurationManager.AppSettings["MBLDocPath"];
                        }
                        if (objFileAttachement.DocumentType == "CO")
                        {
                            if (Convert.ToInt32(objFileAttachement.IsSysGenerated) == 0)
                            {
                                fileName = emailData.DocumentCommonID + "_" + objFileAttachement.DocName;
                            }
                            ShipmentFolder = ConfigurationManager.AppSettings["CODocPath"];
                        }
                        if (objFileAttachement.DocumentType == "ExtDoc")
                        {

                            fileName = emailData.DocumentCommonID + "_" + objFileAttachement.DocName;

                            ShipmentFolder = ConfigurationManager.AppSettings["OtherDocPath"];
                        }
                        string filePath = HttpContext.Current.Server.MapPath(ShipmentFolder + "/") + fileName;


                        if (System.IO.File.Exists(filePath))
                        {
                            attachment = new System.Net.Mail.Attachment(filePath);
                            mailMessage.Attachments.Add(attachment);
                        }


                    }
                }
                // Logger.WriteInfo("Send prealert: 1" );
                EmailHelper.Send(principal, mailMessage);
                mailMessage.Attachments.Dispose();

                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        public void SaveExportInformationService(EnquiryBreakBulkServiceDTO[] EnquiryBreakBulkServiceDTOList)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryBreakBulkServiceDTO objBBulkService in EnquiryBreakBulkServiceDTOList)
            {
                //objResult = _context.ExecuteQuery<int>("EXEC dbo.SP_ExportInformationCode @ServiceRequiredID,@NoofContainer,@RefId,@RefType,@Weight,@Length,@Width,@Height,@Volume,@Description,@Qty,@ChkInland,@InlandRemarks,@MafiCharges",
                //new SqlParameter("ServiceRequiredID", 5)
                //new SqlParameter("NoofContainer", NoOfContainer),
                //new SqlParameter("RefId", EnquiryID),
                //new SqlParameter("RefType", RefType),
                //new SqlParameter("Weight", objBBulkService.Weight),
                //new SqlParameter("Length", objBBulkService.Length),
                //new SqlParameter("Width", objBBulkService.Width),
                //new SqlParameter("Height", objBBulkService.Height),
                //new SqlParameter("Volume", objBBulkService.Volume ?? Convert.DBNull),
                //new SqlParameter("Description", objBBulkService.Description ?? Convert.DBNull),
                //new SqlParameter("Qty", objBBulkService.Qty),
                //new SqlParameter("ChkInland", objBBulkService.ChkInland ?? Convert.DBNull),
                //new SqlParameter("InlandRemarks", objBBulkService.InlandRemarks ?? Convert.DBNull),
                //new SqlParameter("MafiCharges", objBBulkService.MafiCharges ?? Convert.DBNull)).ToList();
            }
        }
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UploadEssFile(Doc dd)
        {
            StandardJsonResult output = new StandardJsonResult
            {
                ResultId = Convert.ToInt32(EnumResult.Failed)
            };
            try
            {

                string path = ConfigurationManager.AppSettings["DocumentsPath"] + "\\AES\\";
                string phypath = HttpContext.Current.Server.MapPath(path) + dd.FileName;
                string folderpath = ConfigurationManager.AppSettings["AesUploadFilePath"] + dd.FileName;
                string _FTPUser = ConfigurationManager.AppSettings["FTPUser"];
                string _FTPPass = ConfigurationManager.AppSettings["FTPPass"];
                string _FTPHost = ConfigurationManager.AppSettings["FTPHost"];

                // System.Net.FtpWebRequest _FtpWebRequest = (System.Net.FtpWebRequest)System.Net.FtpWebRequest.Create(new Uri(ftpfullpath));
                //_FtpWebRequest.Credentials = new System.Net.NetworkCredential(_FTPUser, _FTPPass);
                //_FtpWebRequest.KeepAlive = true;
                //_FtpWebRequest.Timeout = -1;
                //_FtpWebRequest.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
                //_FtpWebRequest.UseBinary = true;
                //int bytesRead = 0;
                //byte[] buffer = new byte[1024];
                //using (FileStream fileInputStream = new System.IO.FileStream(phypath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                //{
                //    using (Stream _Stream = _FtpWebRequest.GetRequestStream())
                //    {
                //        while ((bytesRead = fileInputStream.Read(buffer, 0, 1024)) > 0)
                //        {
                //            _Stream.Write(buffer, 0, bytesRead);
                //            _Stream.Flush();
                //        }
                //        _Stream.Close();
                //        _Stream.Dispose();
                //    }

                //    fileInputStream.Close();
                //    fileInputStream.Dispose();
                //    //System.IO.File.Delete(tempfilePath);
                //}
                using (SftpClient client = new SftpClient(_FTPHost, 22, _FTPUser, _FTPPass))
                {
                    client.Connect();
                    using (FileStream fs = new FileStream(phypath, FileMode.Open))
                    {
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(fs, folderpath, null);

                    }
                    output.AddMessage("File is uploaded successfully.", clearLastMessages: true);

                    object[] obj = new object[1]
                                  {
                               new SqlParameter("@FileName", dd.FileName)
                                  };

                     _context.ExecuteQuery("EXEC dbo.UploadStatus_SP @FileName", obj).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        output
                    });
                }

            }
            catch (Exception ex)
            {
                output.AddMessage(ex.Message, clearLastMessages: true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    output
                });
            }
        }
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DownloadEssFile()
        {
            try
            {
                string fileName = "";
                string fileName1 = "";
                fileName = Request.Headers.GetValues("aesFileName").ToList()[0];
                fileName1 = fileName;
                fileName = fileName + ".out";
                string FTPPath = ConfigurationManager.AppSettings["AesDownloadFilePath"];
                string FTPUser = ConfigurationManager.AppSettings["FTPUser"];
                string FTPPassword = ConfigurationManager.AppSettings["FTPPass"];
                string _FTPHost = ConfigurationManager.AppSettings["FTPHost"];
                string LocalDirectory = ConfigurationManager.AppSettings["DocumentsPath"] + "\\AES\\download\\";
                string phypath = HttpContext.Current.Server.MapPath(LocalDirectory) + fileName;
                string phypath1 = HttpContext.Current.Server.MapPath(LocalDirectory+"/final/") + fileName;
                if (!File.Exists(phypath))
                {
                    using (SftpClient client = new SftpClient(_FTPHost, 22, FTPUser, FTPPassword))
                    {
                        client.Connect();
                        if ((fileName != ".") && (fileName != ".."))
                        {
                            string sourceFilePath = FTPPath + "/" + fileName;
                            string destFilePath = HttpContext.Current.Server.MapPath(LocalDirectory) + fileName;
                            using (Stream fileStream = File.Create(destFilePath))
                            {
                                client.DownloadFile(sourceFilePath, fileStream, null);

                                object[] obj = new object[1]
                                    {
                               new SqlParameter("@FileName", fileName1)
                                    };

                                _context.ExecuteQuery("EXEC dbo.DownloadStatus_SP @FileName", obj).ToList();
                            }

                        }
                    }
                }
                string fileText = string.Empty;
                string fileText1 = string.Empty;
                if (File.Exists(phypath))
                {
                    using (StreamReader sr = new StreamReader(phypath))
                    {
                        fileText = sr.ReadToEnd();
                    }
                }
                //string finalstring= Regex.Replace(fileText, "(.{" + 80 + "})", "$1" + Environment.NewLine);
                String[] finalstring = Regex.Replace(fileText, "(.{" + 80 + "})", "$1" + "~").Split('~');
                foreach (string s in finalstring)
                {
                    if (s.StartsWith("ES"))
                    {
                        fileText1 += "  " + s + Environment.NewLine;
                    }
                    else
                    {
                        fileText1 += s + Environment.NewLine;
                    }
                }
                if (fileText1.Length > 0 && finalstring.Length > 1)
                {
                    File.WriteAllText(phypath1, fileText1);
                }
                if (File.Exists(phypath1))
                {

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (FileStream fileStream = new FileStream(phypath, FileMode.Open, FileAccess.Read))
                        {
                            byte[] array = new byte[fileStream.Length];
                            fileStream.Read(array, 0, (int)fileStream.Length);
                            memoryStream.Write(array, 0, (int)fileStream.Length);
                            HttpResponseMessage httpResponseMessage2 = new HttpResponseMessage();
                            httpResponseMessage2.Content = new ByteArrayContent(array.ToArray());
                            httpResponseMessage2.Content.Headers.Add("x-filename", fileName);
                            httpResponseMessage2.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                            httpResponseMessage2.StatusCode = HttpStatusCode.OK;
                            return httpResponseMessage2;
                        }
                    }

                }
                else
                {
                    return base.Request.CreateResponse(HttpStatusCode.NotFound, "File not uploaded yet.");
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return base.Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }
        [System.Web.Http.HttpPost]
        public ContentResult ViewFile()
        {
            string fileText1 = string.Empty;
            string fileName = "";
            string fileName1 = "";
            fileName = Request.Headers.GetValues("aesFileName").ToList()[0];
            fileName1 = fileName;
            fileName = fileName + ".out";
            string FTPPath = ConfigurationManager.AppSettings["AesDownloadFilePath"];
            string FTPUser = ConfigurationManager.AppSettings["FTPUser"];
            string FTPPassword = ConfigurationManager.AppSettings["FTPPass"];
            string _FTPHost = ConfigurationManager.AppSettings["FTPHost"];
            string LocalDirectory = ConfigurationManager.AppSettings["DocumentsPath"] + "\\AES\\download\\";
            string phypath = HttpContext.Current.Server.MapPath(LocalDirectory) + fileName;
            try
            {
                
               using (SftpClient client = new SftpClient(_FTPHost, 22, FTPUser, FTPPassword))
                    {
                        client.Connect();
                        if ((fileName != ".") && (fileName != ".."))
                        {
                            string sourceFilePath = FTPPath + "/" + fileName;

                            var files = client.ListDirectory(FTPPath);
                            //var files = client.ListDirectory(FTPPath).FirstOrDefault(f => f.Name == fileName).ToList();
                            
                            string LastAccessTime="";
                            foreach (var file in files)
                            {
                                string remoteFileName = file.Name;

                                if (remoteFileName == fileName)
                                {
                                    LastAccessTime = file.LastAccessTime.ToString();
                                if (File.Exists(phypath))
                                {
                                    System.IO.FileInfo MyFile = new System.IO.FileInfo(phypath);
                                    double FileSize = MyFile.Length / 500;
                                    if (FileSize < 2)
                                    {
                                        File.Delete(phypath);
                                    }
                                }
                            }
                            }
                            string destFilePath = HttpContext.Current.Server.MapPath(LocalDirectory) + fileName;
                            using (Stream fileStream = File.Create(destFilePath))
                            {
                                client.DownloadFile(sourceFilePath, fileStream, null);

                                object[] obj = new object[2]
                                    {
                                    new SqlParameter("@FileName", fileName1),
                                    new SqlParameter("@LastAccessTime", LastAccessTime)
                                    };

                                _context.ExecuteQuery("EXEC dbo.DownloadStatus_SP @FileName,@LastAccessTime", obj).ToList();
                            }

                        }
                    }
                
                string fileText = string.Empty;

                if (File.Exists(phypath))
                {
                    using (StreamReader sr = new StreamReader(phypath))
                    {
                        fileText = sr.ReadToEnd();
                    }
                }
                String[] finalstring = Regex.Replace(fileText, "(.{" + 80 + "})", "$1" + "~").Split('~');
                string itanno = string.Empty;
                foreach (string s in finalstring)
                {
                    if (s.StartsWith("ES"))
                    {

                        if (s.StartsWith("ES1971") || s.StartsWith("ES1972") || s.StartsWith("ES1973") || s.StartsWith("ES1974") || s.StartsWith("ES1975") || s.StartsWith("ES1976") || s.StartsWith("ES1977") || s.StartsWith("ES1978") || s.StartsWith("ES1979"))
                        {
                            int len = s.IndexOf("X");
                            itanno = s.Substring(len, 15);
                        }
                        fileText1 += "<p class='myp' > <span class='myspan' > " + s + "</span><p></br>";
                    }
                    else
                    {
                        fileText1 += "<p>" + s + "</p></ br >";
                    }
                   

                }
                if (!string.IsNullOrEmpty(itanno))
                {
                    object[] obj = new object[2]
                                    {
                               new SqlParameter("@FileName", fileName1),
                               new SqlParameter("@OriginalITN", itanno)
                                    };
                    _context.ExecuteQuery("EXEC dbo.UpdateItanNO_SP @FileName,@OriginalITN", obj).ToList();
                    fileText1 += "~~~" + itanno+"###" ;
                }
                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = "" + fileText1 + ""
                };
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (message == "/aes/cmdtyfiling/output/" + fileName + "")
                    message = "<p class='myp' > <span class='myspan' >Please wait for the AES response file. AES server is processing your Submission.</span><p>";
               
                string fileText = string.Empty;
                if (File.Exists(phypath))
                {
                    using (StreamReader sr = new StreamReader(phypath))
                    {
                        fileText = sr.ReadToEnd();
                    }
                    String[] finalstring = Regex.Replace(fileText, "(.{" + 80 + "})", "$1" + "~").Split('~');
                    string itanno = string.Empty;
                    foreach (string s in finalstring)
                    {
                        if (s.StartsWith("ES"))
                        {

                            if (s.StartsWith("ES1971") || s.StartsWith("ES1972") || s.StartsWith("ES1973") || s.StartsWith("ES1974") || s.StartsWith("ES1975") || s.StartsWith("ES1976") || s.StartsWith("ES1977") || s.StartsWith("ES1978") || s.StartsWith("ES1979"))
                            {
                                int len = s.IndexOf("X");
                                itanno = s.Substring(len, 15);
                            }
                            fileText1 += "<p class='myp' > <span class='myspan' > " + s + "</span><p></br>";
                        }
                        else
                        {
                            fileText1 += "<p>" + s + "</p></ br >";
                        }


                    }
                }
                if (!String.IsNullOrEmpty(fileText1))
                    message = fileText1;
                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = "" + message + ""
                };
            }
            //return fileText1;
        }
        [System.Web.Http.HttpPost]
        public ContentResult ViewInputFile()
        {
            string fileText1 = string.Empty;
            try
            {
                string fileName = "";
                fileName = Request.Headers.GetValues("aesFileName").ToList()[0];
                string LocalDirectory = ConfigurationManager.AppSettings["DocumentsPath"] + "\\AES\\";
                string phypath = HttpContext.Current.Server.MapPath(LocalDirectory) + fileName;

                string fileText = string.Empty;
                if (File.Exists(phypath))
                {
                    using (StreamReader sr = new StreamReader(phypath))
                    {
                        fileText = sr.ReadToEnd();
                    }
                }
                String[] finalstring = Regex.Replace(fileText, "(.{" + 80 + "})", "$1" + "~").Split('~');
                foreach (string s in finalstring)
                {

                    fileText1 += "<p>" + s + "</p></ br >";

                }
                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = "" + fileText1 + ""
                };
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = "" + message + ""
                };
            }

        }
        private long FtpGetFileSize(string uri, string user_name, string password)
        {

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            request.Credentials =
                new NetworkCredential(user_name, password);

            try
            {
                using (FtpWebResponse response =
                    (FtpWebResponse)request.GetResponse())
                {

                    return response.ContentLength;
                }
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("File unavailable")) return -1;
                throw;
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult CompanySearch(ListParams listParams)
        {
            try
            {


                var result = _context.ExecuteQuery<CompanyDetail>("EXEC dbo.SP_ContactDetail_Search @SEARCHVALUE",
                    new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult CopyEnquiry(BookingListDTO objBookingListDTO)
        {

            EnquiryController ec = new EnquiryController();


            try
            {





                List<string> d = null;
                // objEnquiry.IsComplete = 0;


                List<EnquiryDetailDTO> result1 = _context.ExecuteQuery<EnquiryDetailDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_DETAIL @ENQUIRYID,@IS_COMPLETE,@SIT_ID",
                                      new SqlParameter("ENQUIRYID", objBookingListDTO.EnquiryID),
                                      new SqlParameter("IS_COMPLETE", "1"),
                                      new SqlParameter("SIT_ID", objBookingListDTO.SiteId)).ToList();
                EnquiryDetailDTO objEnquiryDetail = result1[0];

                objEnquiryDetail.CreatedBy = objBookingListDTO.CreatedBy;
                objEnquiryDetail.SiteId = objBookingListDTO.SiteId;
                objEnquiryDetail.NewSiteId = objBookingListDTO.SiteId.ToString();
                List<EnquiryDetailDTO> objEnquiryListn = _context.ExecuteQuery<EnquiryDetailDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_ENQUIRY_NO  @DEPTID", new SqlParameter("DEPTID", objEnquiryDetail.DepartmentID)).ToList();
                EnquiryDetailDTO objEnquiryDetailnew = objEnquiryListn[0];



                string oldEnqNo = objEnquiryDetail.EnquiryNo.ToString();
                string oldEnqId = objEnquiryDetail.EnquiryID.ToString();
                string oldquotationid = objEnquiryDetail.QuotationID.ToString();
                string newEnqNo = "";
                string newEnqId = "";
                string newsiteid = objBookingListDTO.SiteId.ToString();

                string RefType = "";
                if (objEnquiryDetail.IsComplete == 0)
                {
                    RefType = "EnquiryDraft";
                }
                else
                {
                    RefType = "Enquiry";
                }
                if (result1[0].EnquiryID > 0)
                {
                    //get commodity
                    List<CommodityDTO> CommodityDTOList = _context.ExecuteQuery<CommodityDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_COMMODITY @RefId,@RefType",
                                    new SqlParameter("RefId", result1[0].EnquiryID),
                                    new SqlParameter("RefType", "Enquiry")).ToList();
                    if (CommodityDTOList.Count > 0)
                    {
                        objEnquiryDetail.CommodityDTOList = CommodityDTOList.ToArray();
                    }
                    ////get all Next Action remarks
                    int bookingid = 0;
                    int quotationID = 0;
                    IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                         new SqlParameter("QUOTATIONID", quotationID),
                         new SqlParameter("ENQUIRYID", result1[0].EnquiryID),
                         new SqlParameter("BookingID", bookingid),
                         //new SqlParameter("RefType", "Enquiry")).ToList();  comment by vikas solanki on 12 Nov 2020
                         new SqlParameter("RefType", RefType)).ToList();
                    List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                    if (RemarksResultList.Count > 0)
                    {
                        objEnquiryDetail.NextActionRemarksDTOList = RemarksResult.ToArray();
                        objEnquiryDetail.LastRemarks = RemarksResultList[0].Remarks;

                        objEnquiryDetail.LastRemarkDate = RemarksResultList[0].CurrentDate.Value.Date;

                        if (RemarksResultList.Count > 1)
                        {
                            objEnquiryDetail.PrevActionDueDate = RemarksResultList[0].NextActionDate;
                        }
                        else
                        {
                            objEnquiryDetail.PrevActionDueDate = RemarksResultList[0].NextActionDate;
                        }
                    }
                    //get Container details / FCL                                     
                    List<EnquiryContainerServiceDTO> ContainerServiceDTOList = _context.ExecuteQuery<EnquiryContainerServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_CONTAINER_LIST @RefId,@RefType",
                                    new SqlParameter("RefId", result1[0].EnquiryID),
                                    new SqlParameter("RefType", RefType)).ToList();
                    if (ContainerServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.ModeOfService = "1";
                        objEnquiryDetail.EnquiryContainerServiceDTOList = ContainerServiceDTOList.ToArray();
                    }
                    //get Air
                    List<EnquiryAIRServiceDTO> AirServiceDTOList = _context.ExecuteQuery<EnquiryAIRServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_AIR @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result1[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 7)).ToList();
                    if (AirServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.ModeOfService = "7";
                        objEnquiryDetail.EnquiryAIRServiceDTOList = AirServiceDTOList.ToArray();
                    }
                    //get Break Bulk
                    List<EnquiryBreakBulkServiceDTO> BreakBulkServiceDTOList = _context.ExecuteQuery<EnquiryBreakBulkServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_BREAK_BULK @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result1[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 5)).ToList();
                    if (BreakBulkServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.ModeOfService = "5";
                        objEnquiryDetail.EnquiryBreakBulkServiceDTOList = BreakBulkServiceDTOList.ToArray();
                    }
                    //get LCL
                    List<EnquiryLCLServiceDTO> LCLServiceDTOList = _context.ExecuteQuery<EnquiryLCLServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_LCL @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result1[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 2)).ToList();
                    if (LCLServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.ModeOfService = "2";
                        objEnquiryDetail.EnquiryLCLServiceDTOList = LCLServiceDTOList.ToArray();
                    }
                    //get RORO
                    List<EnquiryROROServiceDTO> ROROServiceDTOList = _context.ExecuteQuery<EnquiryROROServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_RORO @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result1[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 3)).ToList();
                    if (ROROServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.ModeOfService = "3";
                        objEnquiryDetail.EnquiryROROServiceDTOList = ROROServiceDTOList.ToArray();
                    }
                }



                // get new enqueryid 

                EnquiryDetailDTO objEnquiryList = _context.ExecuteQuery<EnquiryDetailDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_ENQUIRY_NO  @DEPTID", new SqlParameter("DEPTID", objEnquiryDetail.DepartmentID)).FirstOrDefault();
                objEnquiryDetail.EnquiryID = 0;
                newEnqNo = objEnquiryDetail.EnquiryNo = objEnquiryDetailnew.EnquiryNo;       //objEnquiryList.EnquiryNo;
                objEnquiryDetail.SiteId = Convert.ToDecimal(newsiteid);
                // newEnqId = objEnquiryDetail.EnquiryID.ToString();
                if (objBookingListDTO.etype == "e")
                {
                    objEnquiryDetail.IsDraft = 1;
                }
                else
                {

                    objEnquiryDetail.IsDraft = 0;
                }

                objEnquiryDetail.CreatedBy = objBookingListDTO.CreatedBy;
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_INSERT_UPDATE_DRAFT @Id,@EnquiryDate,@EnquiryNo,@fkCompanyID,@BillTo,@BillToCompanyId,@ShipmentMoveDate,@PickupType,@PickupRemark,@PortOfOrigin,@PortOfDischarge,@CityOfDischarge,@CityOfOrigin,@CountryOfDischarge,@CountryOfOrigin,@StateOfDischarge,@StateOfOrigin,@NoOfContainer,@TypeOfEnquiry,@ReceivedBy,@Remarks,@Type,@Description,@Class,@UNNo,@PackingType,@HazRemarks,@ChkHAZ,@CreatedBy,@UpdatedBy, @ModifiedOn, @Hazweight,@HazVolume,@DepartmentID,@LastEnquiryNo,@ControlNo,@IsDraft,@GalRepID, @OriginDoor,@DestinationDoor,@CustomerInqNo,@OriginRailRamp,@DestinationTerminal,@LicenseType,@SiteId",
               new SqlParameter("Id", objEnquiryDetail.EnquiryID),
               new SqlParameter("EnquiryDate", objEnquiryDetail.EnquiryDate),
               new SqlParameter("EnquiryNo", objEnquiryDetail.EnquiryNo),
               new SqlParameter("fkCompanyID", objEnquiryDetail.fkCompanyID ?? Convert.DBNull),
               new SqlParameter("BillTo", objEnquiryDetail.BillTo ?? Convert.DBNull),
               new SqlParameter("BillToCompanyId", objEnquiryDetail.BillToCompanyId ?? Convert.DBNull),
               new SqlParameter("ShipmentMoveDate", objEnquiryDetail.ShipmentMoveDate ?? Convert.DBNull),
               new SqlParameter("PickupType", objEnquiryDetail.PickupType ?? Convert.DBNull),
               new SqlParameter("PickupRemark", objEnquiryDetail.PickupRemark ?? Convert.DBNull),
               new SqlParameter("PortOfOrigin", objEnquiryDetail.OriginID),
               new SqlParameter("PortOfDischarge", objEnquiryDetail.DischargeID),
               new SqlParameter("CityOfDischarge", objEnquiryDetail.DestinationCityID ?? Convert.DBNull),
               new SqlParameter("CityOfOrigin", objEnquiryDetail.OriginCityID ?? Convert.DBNull),
               new SqlParameter("CountryOfDischarge", objEnquiryDetail.DestinationCountryID ?? Convert.DBNull),
               new SqlParameter("CountryOfOrigin", objEnquiryDetail.OriginCountryID ?? Convert.DBNull),
               new SqlParameter("StateOfDischarge", objEnquiryDetail.DestinationStateID ?? Convert.DBNull),
               new SqlParameter("StateOfOrigin", objEnquiryDetail.OrignStateID ?? Convert.DBNull),
               new SqlParameter("NoOfContainer", objEnquiryDetail.NoOfContainer ?? Convert.DBNull),
               new SqlParameter("TypeOfEnquiry", Convert.ToInt32(objEnquiryDetail.TypeOfEnquiry)),
               new SqlParameter("ReceivedBy", objEnquiryDetail.ReceivedByID ?? Convert.DBNull),
               new SqlParameter("Remarks", objEnquiryDetail.Remarks ?? Convert.DBNull),
               new SqlParameter("Type", "insert"),
               new SqlParameter("Description", objEnquiryDetail.Description ?? Convert.DBNull),
               new SqlParameter("Class", objEnquiryDetail.Class ?? Convert.DBNull),
               new SqlParameter("UNNo", objEnquiryDetail.UNNo ?? Convert.DBNull),
               new SqlParameter("PackingType", objEnquiryDetail.PackingType ?? Convert.DBNull),
               new SqlParameter("HazRemarks", objEnquiryDetail.HazRemarks ?? Convert.DBNull),
               new SqlParameter("ChkHAZ", objEnquiryDetail.IsHaz ?? Convert.DBNull),
               new SqlParameter("CreatedBy", objEnquiryDetail.UserID ?? Convert.DBNull),
               new SqlParameter("UpdatedBy", objEnquiryDetail.UpdatedBy ?? Convert.DBNull),
               new SqlParameter("ModifiedOn", System.DateTime.Now.Date),
               new SqlParameter("Hazweight", objEnquiryDetail.Hazweight ?? Convert.DBNull),
               new SqlParameter("HazVolume", objEnquiryDetail.HazVolume ?? Convert.DBNull),
               new SqlParameter("DepartmentID", objEnquiryDetail.DepartmentID ?? Convert.DBNull),
               new SqlParameter("LastEnquiryNo", objEnquiryDetail.LastEnquiryNo ?? Convert.DBNull),
               new SqlParameter("ControlNo", objEnquiryDetail.EnquiryControlNo ?? Convert.DBNull),
               new SqlParameter("IsDraft", objEnquiryDetail.IsDraft == null ? 0 : objEnquiryDetail.IsDraft),
               new SqlParameter("GalRepID", objEnquiryDetail.GalRepID ?? Convert.DBNull),
               new SqlParameter("OriginDoor", objEnquiryDetail.OriginDoorID ?? Convert.DBNull),
               new SqlParameter("DestinationDoor", objEnquiryDetail.DestinationDoorID ?? Convert.DBNull),
               new SqlParameter("CustomerInqNo", objEnquiryDetail.CustomerInqNo ?? Convert.DBNull),
               new SqlParameter("OriginRailRamp", objEnquiryDetail.OrgnRailRampId ?? Convert.DBNull),
               new SqlParameter("DestinationTerminal", objEnquiryDetail.DestnTerminalId ?? Convert.DBNull),
               new SqlParameter("LicenseType", objEnquiryDetail.LicenseType ?? Convert.DBNull),
               new SqlParameter("SiteId", objEnquiryDetail.SiteId)).ToList();

                List<int> objList = objResult.ToList();
                int EnquiryId = objList[0];
                newEnqId = EnquiryId.ToString();
                if (EnquiryId > 0)
                {
                    IEnumerable<int> objDeleteResult;
                    //delete existing enquiry Commdity details
                    objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_COMMODITY_DELETE @RefId,@RefType",
                        new SqlParameter("RefId", EnquiryId),
                        new SqlParameter("RefType", "Enquiry")).ToList();
                    if (objEnquiryDetail.CommodityDTOList.Count() > 0)
                    {
                        SaveEnquiryCommodityDetails(objEnquiryDetail.CommodityDTOList, EnquiryId, "Enquiry");
                    }
                    //save next action remarks
                    if (objEnquiryDetail.NextActionRemarks != "" && objEnquiryDetail.NextActionDueDate != null)
                    {
                        IEnumerable<int> objRemarksResult;
                        objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                        new SqlParameter("RefID", EnquiryId),
                        new SqlParameter("NextActionDate", objEnquiryDetail.NextActionDueDate),
                        new SqlParameter("Type", "Enquiry"),
                        new SqlParameter("ActivityID", 1),
                        new SqlParameter("Remarks", objEnquiryDetail.NextActionRemarks)).ToList();
                    }
                    int modeOfService = Convert.ToInt32(objEnquiryDetail.ModeOfService);
                    //delete existing Container Service details
                    objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_SERVICE_REF_DELETE @RefId,@RefType",
                        new SqlParameter("RefId", EnquiryId),
                        new SqlParameter("RefType", "Enquiry")).ToList();
                    //For containers & FCL = 1
                    if (objEnquiryDetail.EnquiryContainerServiceDTOList.Count() > 0)
                    {
                        SaveEnquiryContainerService(objEnquiryDetail.EnquiryContainerServiceDTOList, EnquiryId, modeOfService, "Enquiry", objEnquiryDetail.NoOfContainer);

                        if (modeOfService == 7) //AIR
                        {
                            if (objEnquiryDetail.EnquiryAIRServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryAIRService(objEnquiryDetail.EnquiryAIRServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                            }
                        }
                        if (modeOfService == 5) //Break Bulk
                        {
                            if (objEnquiryDetail.EnquiryBreakBulkServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryBreakBulkService(objEnquiryDetail.EnquiryBreakBulkServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                            }
                        }
                        if (modeOfService == 2) //LCL
                        {
                            if (objEnquiryDetail.EnquiryLCLServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryLCLService(objEnquiryDetail.EnquiryLCLServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                            }
                        }

                        if (modeOfService == 3)  //RORO Service details
                        {
                            if (objEnquiryDetail.EnquiryROROServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryROROService(objEnquiryDetail.EnquiryROROServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                            }
                        }
                    }
                }
                // create enquiry finish

                QuotationController quo = new QuotationController();
                QuotationDetailDTO QuotationDetailFilter = new QuotationDetailDTO();
                QuotationDetailFilter.EnquiryID = Convert.ToDecimal(oldEnqId);
                QuotationDetailFilter.QuotationID = Convert.ToDecimal(oldquotationid);
                QuotationDetailDTO old_quatationdetail = GetQuationDetailOnly(QuotationDetailFilter);

                QuotationDetailFilter.EnquiryID = Convert.ToDecimal(newEnqId);
                QuotationDetailFilter.QuotationID = 0;

                QuotationDetailDTO new_quatationdetail = GetQuationDetailOnly(QuotationDetailFilter);

                old_quatationdetail.QuotationID = 0;

                IEnumerable<Int32> objResultQuano = _context.ExecuteQuery<Int32>("EXEC dbo.USP_LG_QUOTATION_GET_QUOTATION_NO").ToList();
                List<Int32> resultQuano = objResultQuano.ToList();


                old_quatationdetail.QuotationNo = resultQuano[0].ToString(); // old_quatationdetail.QuotationNo.ToString().Replace("NRA", "") + "C";//   new_quatationdetail.QuotationNo;
                old_quatationdetail.EnquiryNo = newEnqNo;
                old_quatationdetail.EnquiryID = Convert.ToDecimal(newEnqId);
                old_quatationdetail.CreatedBy = objBookingListDTO.CreatedBy;
                if (objBookingListDTO.etype == "q")
                {
                    decimal new_QuotationID = SaveQuationDetail(old_quatationdetail);
                    old_quatationdetail.QuotationID = new_QuotationID;


                    //  ApproveQuotation(old_quatationdetail.QuotationID, true, true, true);
                    //  ApproveQuotationCarrier(old_quatationdetail.CarrierDTOList, new_QuotationID);
                    //  SendEmail(new_QuotationID, "", "");
                    //  _context.ExecuteQuery<int>("EXEC dbo.Add_Site_Change_Detail @OldQuotationId,@NewQuotationId",
                    //                         new SqlParameter("OldQuotationId", oldquotationid),
                    //                         new SqlParameter("NewQuotationId", new_QuotationID)).ToList();
                }
                //objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_CANCEL @ID, @Remarks,@NextActionDueDate",
                //                       new SqlParameter("ID", objBookingListDTO.DocumentCommonID),
                //                       new SqlParameter("Remarks", "Cancel due to Copy Created "),
                //                       new SqlParameter("NextActionDueDate", "")).ToList();

                return AppResult(old_quatationdetail, "Copied Successfully");

            }




            //try
            //{
            //    IEnumerable<int> objResult = null;
            //    if (objBookingListDTO.DocumentCommonID > 0)
            //    {
            //        objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_COPYENQUIRY @ID, @type",
            //                            new SqlParameter("ID", objBookingListDTO.DocumentCommonID),
            //                            new SqlParameter("type", objBookingListDTO.etype)
            //                           ).ToList();
            //    }
            //    List<int> objList = objResult.ToList();
            //    return AppResult(objList, 1);
            //}
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult CopyQuotation(QuotationListDTO quotationListDTO)
        {
            try
            {
                List<QuotationDetailDTO> result = _context.ExecuteQuery<QuotationDetailDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ENQUIRY_DETAIL @ENQUIRYID,@QUOTATIONID",
                                           new SqlParameter("ENQUIRYID", quotationListDTO.EnquiryID),
                                           new SqlParameter("QUOTATIONID", quotationListDTO.QuotationID)).ToList();

            QuotationDetailDTO objQuotationDetailDTO = result[0];

            EnquiryController ec = new EnquiryController();
            List<string> d = null;

            List<EnquiryDetailDTO> result1 = _context.ExecuteQuery<EnquiryDetailDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_DETAIL @ENQUIRYID,@IS_COMPLETE,@SIT_ID",
                                  new SqlParameter("ENQUIRYID", objQuotationDetailDTO.EnquiryID),
                                  new SqlParameter("IS_COMPLETE", "1"),
                                  new SqlParameter("SIT_ID", quotationListDTO.SiteId)).ToList();
            EnquiryDetailDTO objEnquiryDetail = result1[0];

            objEnquiryDetail.CreatedBy = quotationListDTO.CreatedBy;
            objEnquiryDetail.SiteId = Convert.ToDecimal(quotationListDTO.SiteId);
            objEnquiryDetail.NewSiteId = quotationListDTO.SiteId.ToString();
            List<EnquiryDetailDTO> objEnquiryListn = _context.ExecuteQuery<EnquiryDetailDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_ENQUIRY_NO  @DEPTID", new SqlParameter("DEPTID", objEnquiryDetail.DepartmentID)).ToList();
            EnquiryDetailDTO objEnquiryDetailnew = objEnquiryListn[0];



            string oldEnqNo = objEnquiryDetail.EnquiryNo.ToString();
            string oldEnqId = objEnquiryDetail.EnquiryID.ToString();
            string oldquotationid = objEnquiryDetail.QuotationID.ToString();
            string newEnqNo = "";
            string newEnqId = "";
            string newsiteid = quotationListDTO.SiteId.ToString();

            string RefType = "";
            if (objEnquiryDetail.IsComplete == 0)
            {
                RefType = "EnquiryDraft";
            }
            else
            {
                RefType = "Enquiry";
            }
            if (result1[0].EnquiryID > 0)
            {
                //get commodity
                List<CommodityDTO> CommodityDTOList = _context.ExecuteQuery<CommodityDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_COMMODITY @RefId,@RefType",
                                new SqlParameter("RefId", result1[0].EnquiryID),
                                new SqlParameter("RefType", "Enquiry")).ToList();
                if (CommodityDTOList.Count > 0)
                {
                    objEnquiryDetail.CommodityDTOList = CommodityDTOList.ToArray();
                }
                ////get all Next Action remarks
                int bookingid = 0;
                int quotationID = 0;
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                     new SqlParameter("QUOTATIONID", quotationID),
                     new SqlParameter("ENQUIRYID", result1[0].EnquiryID),
                     new SqlParameter("BookingID", bookingid),
                     //new SqlParameter("RefType", "Enquiry")).ToList();  comment by vikas solanki on 12 Nov 2020
                     new SqlParameter("RefType", RefType)).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                if (RemarksResultList.Count > 0)
                {
                    objEnquiryDetail.NextActionRemarksDTOList = RemarksResult.ToArray();
                    objEnquiryDetail.LastRemarks = RemarksResultList[0].Remarks;

                    objEnquiryDetail.LastRemarkDate = RemarksResultList[0].CurrentDate.Value.Date;

                    if (RemarksResultList.Count > 1)
                    {
                        objEnquiryDetail.PrevActionDueDate = RemarksResultList[0].NextActionDate;
                    }
                    else
                    {
                        objEnquiryDetail.PrevActionDueDate = RemarksResultList[0].NextActionDate;
                    }
                }
                //get Container details / FCL                                     
                List<EnquiryContainerServiceDTO> ContainerServiceDTOList = _context.ExecuteQuery<EnquiryContainerServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_CONTAINER_LIST @RefId,@RefType",
                                new SqlParameter("RefId", result1[0].EnquiryID),
                                new SqlParameter("RefType", RefType)).ToList();
                if (ContainerServiceDTOList.Count > 0)
                {
                    objEnquiryDetail.ModeOfService = "1";
                    objEnquiryDetail.EnquiryContainerServiceDTOList = ContainerServiceDTOList.ToArray();
                }
                //get Air
                List<EnquiryAIRServiceDTO> AirServiceDTOList = _context.ExecuteQuery<EnquiryAIRServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_AIR @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", result1[0].EnquiryID),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 7)).ToList();
                if (AirServiceDTOList.Count > 0)
                {
                    objEnquiryDetail.ModeOfService = "7";
                    objEnquiryDetail.EnquiryAIRServiceDTOList = AirServiceDTOList.ToArray();
                }
                //get Break Bulk
                List<EnquiryBreakBulkServiceDTO> BreakBulkServiceDTOList = _context.ExecuteQuery<EnquiryBreakBulkServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_BREAK_BULK @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", result1[0].EnquiryID),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 5)).ToList();
                if (BreakBulkServiceDTOList.Count > 0)
                {
                    objEnquiryDetail.ModeOfService = "5";
                    objEnquiryDetail.EnquiryBreakBulkServiceDTOList = BreakBulkServiceDTOList.ToArray();
                }
                //get LCL
                List<EnquiryLCLServiceDTO> LCLServiceDTOList = _context.ExecuteQuery<EnquiryLCLServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_LCL @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", result1[0].EnquiryID),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 2)).ToList();
                if (LCLServiceDTOList.Count > 0)
                {
                    objEnquiryDetail.ModeOfService = "2";
                    objEnquiryDetail.EnquiryLCLServiceDTOList = LCLServiceDTOList.ToArray();
                }
                //get RORO
                List<EnquiryROROServiceDTO> ROROServiceDTOList = _context.ExecuteQuery<EnquiryROROServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_RORO @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", result1[0].EnquiryID),
                                new SqlParameter("RefType", RefType),
                                new SqlParameter("fkServiceRequiredID", 3)).ToList();
                if (ROROServiceDTOList.Count > 0)
                {
                    objEnquiryDetail.ModeOfService = "3";
                    objEnquiryDetail.EnquiryROROServiceDTOList = ROROServiceDTOList.ToArray();
                }
            }



            // get new enqueryid 

            EnquiryDetailDTO objEnquiryList = _context.ExecuteQuery<EnquiryDetailDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_ENQUIRY_NO  @DEPTID", new SqlParameter("DEPTID", objEnquiryDetail.DepartmentID)).FirstOrDefault();
            objEnquiryDetail.EnquiryID = 0;
            newEnqNo = objEnquiryDetail.EnquiryNo = objEnquiryDetailnew.EnquiryNo;       //objEnquiryList.EnquiryNo;
            objEnquiryDetail.SiteId = Convert.ToDecimal(newsiteid);
            // newEnqId = objEnquiryDetail.EnquiryID.ToString();
            objEnquiryDetail.IsDraft = 0;



            objEnquiryDetail.CreatedBy = objQuotationDetailDTO.CreatedBy;
            IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_INSERT_UPDATE_DRAFT @Id,@EnquiryDate,@EnquiryNo,@fkCompanyID,@BillTo,@BillToCompanyId,@ShipmentMoveDate,@PickupType,@PickupRemark,@PortOfOrigin,@PortOfDischarge,@CityOfDischarge,@CityOfOrigin,@CountryOfDischarge,@CountryOfOrigin,@StateOfDischarge,@StateOfOrigin,@NoOfContainer,@TypeOfEnquiry,@ReceivedBy,@Remarks,@Type,@Description,@Class,@UNNo,@PackingType,@HazRemarks,@ChkHAZ,@CreatedBy,@UpdatedBy, @ModifiedOn, @Hazweight,@HazVolume,@DepartmentID,@LastEnquiryNo,@ControlNo,@IsDraft,@GalRepID, @OriginDoor,@DestinationDoor,@CustomerInqNo,@OriginRailRamp,@DestinationTerminal,@LicenseType,@SiteId",
           new SqlParameter("Id", objEnquiryDetail.EnquiryID),
           new SqlParameter("EnquiryDate", objEnquiryDetail.EnquiryDate),
           new SqlParameter("EnquiryNo", objEnquiryDetail.EnquiryNo),
           new SqlParameter("fkCompanyID", objEnquiryDetail.fkCompanyID ?? Convert.DBNull),
           new SqlParameter("BillTo", objEnquiryDetail.BillTo ?? Convert.DBNull),
           new SqlParameter("BillToCompanyId", objEnquiryDetail.BillToCompanyId ?? Convert.DBNull),
           new SqlParameter("ShipmentMoveDate", objEnquiryDetail.ShipmentMoveDate ?? Convert.DBNull),
           new SqlParameter("PickupType", objEnquiryDetail.PickupType ?? Convert.DBNull),
           new SqlParameter("PickupRemark", objEnquiryDetail.PickupRemark ?? Convert.DBNull),
           new SqlParameter("PortOfOrigin", objEnquiryDetail.OriginID),
           new SqlParameter("PortOfDischarge", objEnquiryDetail.DischargeID),
           new SqlParameter("CityOfDischarge", objEnquiryDetail.DestinationCityID ?? Convert.DBNull),
           new SqlParameter("CityOfOrigin", objEnquiryDetail.OriginCityID ?? Convert.DBNull),
           new SqlParameter("CountryOfDischarge", objEnquiryDetail.DestinationCountryID ?? Convert.DBNull),
           new SqlParameter("CountryOfOrigin", objEnquiryDetail.OriginCountryID ?? Convert.DBNull),
           new SqlParameter("StateOfDischarge", objEnquiryDetail.DestinationStateID ?? Convert.DBNull),
           new SqlParameter("StateOfOrigin", objEnquiryDetail.OrignStateID ?? Convert.DBNull),
           new SqlParameter("NoOfContainer", objEnquiryDetail.NoOfContainer ?? Convert.DBNull),
           new SqlParameter("TypeOfEnquiry", Convert.ToInt32(objEnquiryDetail.TypeOfEnquiry)),
           new SqlParameter("ReceivedBy", objEnquiryDetail.ReceivedByID ?? Convert.DBNull),
           new SqlParameter("Remarks", objEnquiryDetail.Remarks ?? Convert.DBNull),
           new SqlParameter("Type", "insert"),
           new SqlParameter("Description", objEnquiryDetail.Description ?? Convert.DBNull),
           new SqlParameter("Class", objEnquiryDetail.Class ?? Convert.DBNull),
           new SqlParameter("UNNo", objEnquiryDetail.UNNo ?? Convert.DBNull),
           new SqlParameter("PackingType", objEnquiryDetail.PackingType ?? Convert.DBNull),
           new SqlParameter("HazRemarks", objEnquiryDetail.HazRemarks ?? Convert.DBNull),
           new SqlParameter("ChkHAZ", objEnquiryDetail.IsHaz ?? Convert.DBNull),
           new SqlParameter("CreatedBy", objEnquiryDetail.UserID ?? Convert.DBNull),
           new SqlParameter("UpdatedBy", objEnquiryDetail.UpdatedBy ?? Convert.DBNull),
           new SqlParameter("ModifiedOn", System.DateTime.Now.Date),
           new SqlParameter("Hazweight", objEnquiryDetail.Hazweight ?? Convert.DBNull),
           new SqlParameter("HazVolume", objEnquiryDetail.HazVolume ?? Convert.DBNull),
           new SqlParameter("DepartmentID", objEnquiryDetail.DepartmentID ?? Convert.DBNull),
           new SqlParameter("LastEnquiryNo", objEnquiryDetail.LastEnquiryNo ?? Convert.DBNull),
           new SqlParameter("ControlNo", objEnquiryDetail.EnquiryControlNo ?? Convert.DBNull),
           new SqlParameter("IsDraft", objEnquiryDetail.IsDraft == null ? 0 : objEnquiryDetail.IsDraft),
           new SqlParameter("GalRepID", objEnquiryDetail.GalRepID ?? Convert.DBNull),
           new SqlParameter("OriginDoor", objEnquiryDetail.OriginDoorID ?? Convert.DBNull),
           new SqlParameter("DestinationDoor", objEnquiryDetail.DestinationDoorID ?? Convert.DBNull),
           new SqlParameter("CustomerInqNo", objEnquiryDetail.CustomerInqNo ?? Convert.DBNull),
           new SqlParameter("OriginRailRamp", objEnquiryDetail.OrgnRailRampId ?? Convert.DBNull),
           new SqlParameter("DestinationTerminal", objEnquiryDetail.DestnTerminalId ?? Convert.DBNull),
           new SqlParameter("LicenseType", objEnquiryDetail.LicenseType ?? Convert.DBNull),
           new SqlParameter("SiteId", objEnquiryDetail.SiteId)).ToList();

            List<int> objList = objResult.ToList();
            int EnquiryId = objList[0];
            newEnqId = EnquiryId.ToString();
            if (EnquiryId > 0)
            {
                IEnumerable<int> objDeleteResult;
                //delete existing enquiry Commdity details
                objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_COMMODITY_DELETE @RefId,@RefType",
                    new SqlParameter("RefId", EnquiryId),
                    new SqlParameter("RefType", "Enquiry")).ToList();
                if (objEnquiryDetail.CommodityDTOList.Count() > 0)
                {
                    SaveEnquiryCommodityDetails(objEnquiryDetail.CommodityDTOList, EnquiryId, "Enquiry");
                }
                //save next action remarks
                if (objEnquiryDetail.NextActionRemarks != "" && objEnquiryDetail.NextActionDueDate != null)
                {
                    //IEnumerable<int> objRemarksResult;
                    //objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                    //new SqlParameter("RefID", EnquiryId),
                    //new SqlParameter("NextActionDate", objEnquiryDetail.NextActionDueDate),
                    //new SqlParameter("Type", "Enquiry"),
                    //new SqlParameter("ActivityID", 1),
                    //new SqlParameter("Remarks", objEnquiryDetail.NextActionRemarks)).ToList();
                }
                int modeOfService = Convert.ToInt32(objEnquiryDetail.ModeOfService);
                //delete existing Container Service details
                objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_SERVICE_REF_DELETE @RefId,@RefType",
                    new SqlParameter("RefId", EnquiryId),
                    new SqlParameter("RefType", "Enquiry")).ToList();
                //For containers & FCL = 1
                if (objEnquiryDetail.EnquiryContainerServiceDTOList.Count() > 0)
                {
                    SaveEnquiryContainerService(objEnquiryDetail.EnquiryContainerServiceDTOList, EnquiryId, modeOfService, "Enquiry", objEnquiryDetail.NoOfContainer);

                    if (modeOfService == 7) //AIR
                    {
                        if (objEnquiryDetail.EnquiryAIRServiceDTOList.Count() > 0)
                        {
                            SaveEnquiryAIRService(objEnquiryDetail.EnquiryAIRServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                        }
                    }
                    if (modeOfService == 5) //Break Bulk
                    {
                        if (objEnquiryDetail.EnquiryBreakBulkServiceDTOList.Count() > 0)
                        {
                            SaveEnquiryBreakBulkService(objEnquiryDetail.EnquiryBreakBulkServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                        }
                    }
                    if (modeOfService == 2) //LCL
                    {
                        if (objEnquiryDetail.EnquiryLCLServiceDTOList.Count() > 0)
                        {
                            SaveEnquiryLCLService(objEnquiryDetail.EnquiryLCLServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                        }
                    }

                    if (modeOfService == 3)  //RORO Service details
                    {
                        if (objEnquiryDetail.EnquiryROROServiceDTOList.Count() > 0)
                        {
                            SaveEnquiryROROService(objEnquiryDetail.EnquiryROROServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                        }
                    }
                }
            }
            // create enquiry finish

            QuotationController quo = new QuotationController();
            QuotationDetailDTO QuotationDetailFilter = new QuotationDetailDTO();
            QuotationDetailFilter.EnquiryID = Convert.ToDecimal(oldEnqId);
            QuotationDetailFilter.QuotationID = Convert.ToDecimal(oldquotationid);
            QuotationDetailDTO old_quatationdetail = GetQuationDetailOnly(QuotationDetailFilter);

            QuotationDetailFilter.EnquiryID = Convert.ToDecimal(newEnqId);
            QuotationDetailFilter.QuotationID = 0;

            QuotationDetailDTO new_quatationdetail = GetQuationDetailOnly(QuotationDetailFilter);

            old_quatationdetail.QuotationID = 0;

            IEnumerable<Int32> objResultQuano = _context.ExecuteQuery<Int32>("EXEC dbo.USP_LG_QUOTATION_GET_QUOTATION_NO").ToList();
            List<Int32> resultQuano = objResultQuano.ToList();


            old_quatationdetail.QuotationNo = resultQuano[0].ToString(); // old_quatationdetail.QuotationNo.ToString().Replace("NRA", "") + "C";//   new_quatationdetail.QuotationNo;
            old_quatationdetail.EnquiryNo = newEnqNo;
            old_quatationdetail.EnquiryID = Convert.ToDecimal(newEnqId);
                old_quatationdetail.CreatedBy = quotationListDTO.CreatedBy;

                decimal new_QuotationID = SaveQuationDetail(old_quatationdetail);
            old_quatationdetail.QuotationID = new_QuotationID;

            return AppResult(old_quatationdetail, "Copied Successfully");
        
    }
            catch (Exception ex)
            {
                return AppResult(ex);
              }
    }
        [System.Web.Http.HttpPost]
        public ActionResult UpdateStatus()
        {
            try
            {


                var statustype = Request.Headers.GetValues("statustype").ToList()[0];
                var aesid = Request.Headers.GetValues("aesid").ToList()[0];
                var documetcommanid = Request.Headers.GetValues("documetcommanid").ToList()[0];
                var itanno = Request.Headers.GetValues("itanno").ToList()[0];
                object[] obj = new object[4]
                 {
                 new SqlParameter("@StatusType", statustype),
                 new SqlParameter("@ITANNO", itanno),
                 new SqlParameter("@DocumentCommanId", documetcommanid),
                 new SqlParameter("@AesId", aesid)
                 };
                _context.ExecuteQuery("EXEC dbo.UpdateAesStatusSp @StatusType,@ITANNO,@DocumentCommanId,@AesId", obj).ToList();
                return AppResult(1, "Status Updated Successfully");
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage AESReport(Dictionary<string, string> exportParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/aesemail";
                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();

                reportParams.Add(new KeyValuePair<string, string>("V_aesid", exportParams["id"]));
                //reportParams.Add(new KeyValuePair<string, string>("FROMDATE", exportParams["fromDate"]));
              //  reportParams.Add(new KeyValuePair<string, string>("TODATE", exportParams["toDate"]));
              //  reportParams.Add(new KeyValuePair<string, string>("OPTIONVALUE", exportParams["optionValue"]));
              //  reportParams.Add(new KeyValuePair<string, string>("SEARCHVALUE", exportParams["seachValue"]));
              //  reportParams.Add(new KeyValuePair<string, string>("SIT_ID", exportParams["SitId"]));

                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.PDF);

                if (reportBuffer.Length > 0)
                {
                    string fileName = exportParams["SitId"].ToString() + "_AES.pdf";
                    try
                    {
                        string AESDocfileName = exportParams["SitId"].ToString() + "_AES"+"_" + exportParams["documentcommanid"]+"_"+DateTime.Now.ToString("yyyyMMddHHmmssfff")+".pdf";
                        var root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AESDocPath"]);
                        DirectoryInfo dir = new DirectoryInfo(root);
                        if (System.IO.File.Exists(dir.FullName + "\\" + AESDocfileName))
                        {
                            System.IO.File.Delete(dir.FullName + "\\" + AESDocfileName);
                        }
                        System.IO.File.WriteAllBytes(dir.FullName + "\\" + AESDocfileName, reportBuffer);
                        var docresult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_SHIPMENT_DOC_INSERT @DocumentCommonID,@DocName,@DocType,@CreatedBy,@IsSysGenerated",
                            new SqlParameter("DocumentCommonID", exportParams["documentcommanid"]),
                            new SqlParameter("DocName", AESDocfileName),
                            new SqlParameter("DocType", "AES"),
                            new SqlParameter("CreatedBy", exportParams["createdby"]),
                            new SqlParameter("IsSysGenerated", false)).ToList();

                    }
                    catch( Exception ex)
                    {
                        Logger.WriteError(ex);
                    }

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
        public virtual ActionResult GetEssFileList()
        {
            var documentCommonID = Request.Headers.GetValues("documentCommonID").ToList()[0];
            List<SqlParameter> essfiles = new List<SqlParameter>
                {

                    new SqlParameter("@DocumentCommonId", documentCommonID),
                    new SqlParameter("@AesId", 1)

                };
            List<FileHistory> fileresult = _context.ExecuteQuery<FileHistory>(DocumentCommonQuery.GetFilesList, essfiles.ToArray()).ToList();
            var filelists = fileresult.ToArray();
            return AppResult(filelists, 1, "", EnumResult.Success);
        }
        [System.Web.Http.HttpPost]
        public ActionResult GetDispatchContainerListAbandonement(ListParams listParams)
        {
            try
            {
               

                string startdate = "";
                string enddate = "";
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                 if(!string.IsNullOrEmpty(searchCriteria["DateofDestuffedFrom"]))
                  startdate = Convert.ToDateTime(searchCriteria["DateofDestuffedFrom"].ToString()).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(searchCriteria["DateofDestuffedTo"]))
                    enddate = Convert.ToDateTime(searchCriteria["DateofDestuffedTo"].ToString()).ToString("MM/dd/yyyy");

                int TotalRows = 0;
                List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_CAPTURE_CONTAINER_LIST_Abandonement @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@DEPARTMENTID,@SORTCOLUMN,@SORTORDER,@DASHBOARD_FILTER,@SIT_ID,@Destuffed,@DateofDestuffedFrom,@DateofDestuffedTo",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("DEPARTMENTID", searchCriteria["departmentID"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("DASHBOARD_FILTER", searchCriteria["dashboardOption"]),
                        new SqlParameter("SIT_ID", listParams.SiteId),
                        new SqlParameter("Destuffed", searchCriteria["destuffed"] ?? Convert.DBNull),
                        new SqlParameter("DateofDestuffedFrom", startdate ?? Convert.DBNull),
                        new SqlParameter("DateofDestuffedTo", enddate ?? Convert.DBNull)
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
        public virtual ActionResult GetDispatchContainerDetailAbandonement(BookingListDTO objBooking)
        {
            try
            {
                BookingListDTO objBookingListDTO = new BookingListDTO();
                objBookingListDTO.DocumentCommonID = objBooking.DocumentCommonID;
                objBookingListDTO.FileNo = objBooking.FileNo;
                objBookingListDTO.ExportRef = objBooking.ExportRef;
                objBookingListDTO.BLNumber = objBooking.BLNumber;

                IEnumerable<DispatchContainerDTO> ContainerResult = _context.ExecuteQuery<DispatchContainerDTO>("EXEC dbo.USP_LG_BOOKING_GET_CONTAINER_DISPATCH_TABLE_Abandonement @CommonID",
                                        new SqlParameter("CommonID", objBooking.DocumentCommonID)).ToList();
                List<DispatchContainerDTO> DispatchContainerDTOList = ContainerResult.ToList();
                if (DispatchContainerDTOList.Count > 0)
                {
                    objBookingListDTO.DispatchContainerDTOList = DispatchContainerDTOList.ToArray();
                }
                IEnumerable<DispatchContainerDTO> DispatchTableResult = _context.ExecuteQuery<DispatchContainerDTO>("EXEC dbo.USP_LG_BOOKING_GET_DISPATCH_TABLE_DATA @CommonID",
                                        new SqlParameter("CommonID", objBooking.DocumentCommonID)).ToList();
                List<DispatchContainerDTO> DispatchTableList = DispatchTableResult.ToList();
                if (DispatchContainerDTOList.Count > 0 && DispatchTableList.Count > 0)
                {
                    foreach (DispatchContainerDTO ContainerItem in objBookingListDTO.DispatchContainerDTOList)
                    {
                        foreach (DispatchContainerDTO DispatchItem in DispatchTableList)
                        {
                            if (DispatchItem.ContainerID == ContainerItem.ContainerID && DispatchItem.SeqNo == ContainerItem.SeqNo)
                            {
                                ContainerItem.CNTNo = DispatchItem.CNTNo;
                                ContainerItem.SealNo = DispatchItem.SealNo;
                                ContainerItem.SeqNo = DispatchItem.SeqNo;
                                ContainerItem.DispatchRemarks = DispatchItem.DispatchRemarks;
                                ContainerItem.IsSelected = true;
                            }
                        }
                    }
                }

                if (DispatchContainerDTOList.Count > 0)
                {
                    foreach (var item in DispatchContainerDTOList)
                    {
                        item.ContainerOwnerName = "LINE";
                    }
                    objBookingListDTO.DispatchContainerDTOList = DispatchContainerDTOList.ToArray();
                }
                ////get all Next Action remarks
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                     new SqlParameter("QUOTATIONID", objBookingListDTO.QuotationID),
                     new SqlParameter("ENQUIRYID", objBookingListDTO.EnquiryID),
                     new SqlParameter("BookingID", objBookingListDTO.DocumentCommonID),
                     new SqlParameter("RefType", "Booking")).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                if (RemarksResultList.Count > 0)
                {
                    objBookingListDTO.NextActionRemarksDTOList = RemarksResult.ToArray();
                    objBookingListDTO.LastRemarks = RemarksResultList[0].Remarks;
                    objBookingListDTO.LastRemarkDate = RemarksResultList[0].NextActionDate;
                }

                return AppResult(objBookingListDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }



        [System.Web.Http.HttpPost]
        public HttpResponseMessage downloadAttachmentAbandonement(ListParams listParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string startdate = "";
                string enddate = "";
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                if (!string.IsNullOrEmpty(searchCriteria["DateofDestuffedFrom"]))
                    startdate = Convert.ToDateTime(searchCriteria["DateofDestuffedFrom"].ToString()).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(searchCriteria["DateofDestuffedTo"]))
                    enddate = Convert.ToDateTime(searchCriteria["DateofDestuffedTo"].ToString()).ToString("MM/dd/yyyy");
                string destuffed = "";
                if (!string.IsNullOrEmpty(searchCriteria["destuffed"]))
                {
                    destuffed = searchCriteria["destuffed"];
                }

                string optionValue = "-All-";
                if (!string.IsNullOrEmpty(searchCriteria["optionValue"]))
                {
                    optionValue = searchCriteria["optionValue"];
                }

                string seachValue = "";
                if (!string.IsNullOrEmpty(searchCriteria["seachValue"]))
                {
                    seachValue = searchCriteria["seachValue"];
                }
                string departmentID = "0";
                if (!string.IsNullOrEmpty(searchCriteria["departmentID"]))
                {
                    departmentID = searchCriteria["departmentID"];
                }
               

                string reportPath = "/AppMGL.Report/ContainerAbondand_New";
                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();
                reportParams.Add(new KeyValuePair<string, string>("OPTIONVALUE", optionValue));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHVALUE", seachValue));
                reportParams.Add(new KeyValuePair<string, string>("DEPARTMENTID", departmentID));

                reportParams.Add(new KeyValuePair<string, string>("SORTCOLUMN", "FileNo"));
                reportParams.Add(new KeyValuePair<string, string>("SORTORDER", "desc"));
                reportParams.Add(new KeyValuePair<string, string>("DASHBOARD_FILTER", ""));

                reportParams.Add(new KeyValuePair<string, string>("SIT_ID", listParams.SiteId.ToString()));

                

                reportParams.Add(new KeyValuePair<string, string>("Destuffed", destuffed));
                reportParams.Add(new KeyValuePair<string, string>("DateofDestuffedFrom", startdate ));
                reportParams.Add(new KeyValuePair<string, string>("DateofDestuffedTo", enddate ));


                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);
                if (reportBuffer.Length > 0)
                {
                    string fileName = "ContainerAbondand_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";

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
        public ActionResult GetContainerList(ListParams listParams)
        {
            try
            {
               
                    var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                    var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                    int TotalRows = 0;
                    List<ConsolidateBookingDTO> result = _context.ExecuteQuery<ConsolidateBookingDTO>("EXEC dbo.LG_ContainerManagementListSP @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@SITEID",
                            new SqlParameter("PAGENO", listParams.PageIndex),
                            new SqlParameter("PAGESIZE", listParams.PageSize),
                            new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                            new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                            new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                            new SqlParameter("SORTORDER", sortOrder.First().Value),
                            new SqlParameter("SITEID", listParams.SiteId)
                            ).ToList();
                    if (result.Count > 0)
                    {
                        TotalRows = result[0].TotalCount.Value;
                        TotalRows = 0;
                    }
                    return AppResult(result, TotalRows);

                }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult SaveContainer(ConsolidateBookingDTO obtcont)
        {
            try
            {

                IEnumerable<DispatchContainerDTO> DispatchTableResult = _context.ExecuteQuery<DispatchContainerDTO>("EXEC dbo.USP_LG_COSOLIDATECONTAINERDETAIL_TABLE @BookedId,@Mode",
                                         new SqlParameter("BookedId", obtcont.BookedId),
                                         new SqlParameter("Mode", "DeteleContainer")).ToList();

                IEnumerable<int> objDispatchResult=null;
                bool status = true;
                foreach (DispatchContainerDTO objDispatchContainerDTO in obtcont.DispatchContainerList)
                {
                    if (string.IsNullOrEmpty(objDispatchContainerDTO.CNTNo) || string.IsNullOrEmpty(objDispatchContainerDTO.SealNo))
                    {
                        status = false;
                    }
                    objDispatchResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_COSOLIDATE_CONTAINER_INSERT @BookedId,@SealNo, @CNTNo, @fkServiceRefID, @fkContainerTypeID, @DispatchRemarks, @SeqNo,@ContainerOwner",
                        new SqlParameter("BookedId", obtcont.BookedId),
                        new SqlParameter("SealNo", objDispatchContainerDTO.SealNo ?? Convert.DBNull),
                        new SqlParameter("CNTNo", objDispatchContainerDTO.CNTNo ?? Convert.DBNull),
                        new SqlParameter("fkServiceRefID", objDispatchContainerDTO.ServiceReqdID ?? Convert.DBNull),
                        new SqlParameter("fkContainerTypeID", objDispatchContainerDTO.ContainerID ?? Convert.DBNull),
                        new SqlParameter("DispatchRemarks", objDispatchContainerDTO.DispatchRemarks ?? Convert.DBNull),
                        new SqlParameter("SeqNo", objDispatchContainerDTO.SeqNo ?? Convert.DBNull),
                        new SqlParameter("ContainerOwner", objDispatchContainerDTO.ContainerOwnerName ?? Convert.DBNull)).ToList();
                }

                return AppResult(objDispatchResult, 1, "", EnumResult.Success);
                
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult GetContainerDetail(ConsolidateBookingDTO objcont)
        {
            try
            {
                List<ConsolidateBookingDTO> result = _context.ExecuteQuery<ConsolidateBookingDTO>("EXEC dbo.USP_LG_COSOLIDATECONTAINERDETAIL @BookedId,@Mode",
                new SqlParameter("@BookedId", objcont.BookedId),
                new SqlParameter("Mode", "Detail")).ToList();
                ConsolidateBookingDTO objContainerDetail = result[0];

                IEnumerable<DispatchContainerDTO> DispatchTableResult = _context.ExecuteQuery<DispatchContainerDTO>("EXEC dbo.USP_LG_COSOLIDATECONTAINERDETAIL_TABLE @BookedId,@Mode",
                                        new SqlParameter("BookedId", objcont.BookedId),
                                        new SqlParameter("Mode", "Container")).ToList();
                List<DispatchContainerDTO> DispatchTableList = DispatchTableResult.ToList();

                IEnumerable<DispatchContainerDTO> FullDispatchTableList = _context.ExecuteQuery<DispatchContainerDTO>("EXEC dbo.USP_LG_COSOLIDATECONTAINERDETAIL_TABLE @BookedId,@Mode",
                                        new SqlParameter("BookedId", objcont.BookedId),
                                        new SqlParameter("Mode", "Booked")
                                        ).ToList();
                List<DispatchContainerDTO> FullDispatchList = FullDispatchTableList.ToList();

                if (FullDispatchList.Count > 0 && DispatchTableList.Count > 0)
                {
                    foreach (DispatchContainerDTO ContainerItem in FullDispatchList)
                    {
                        foreach (DispatchContainerDTO DispatchItem in DispatchTableList)
                        {
                            if (DispatchItem.ContainerID == ContainerItem.ContainerID && DispatchItem.SeqNo == ContainerItem.SeqNo)
                            {
                                ContainerItem.CNTNo = DispatchItem.CNTNo;
                                ContainerItem.SealNo = DispatchItem.SealNo;
                                ContainerItem.SeqNo = DispatchItem.SeqNo;
                                ContainerItem.DispatchRemarks = DispatchItem.DispatchRemarks;
                                ContainerItem.ContainerOwnerName = DispatchItem.ContainerOwnerName;
                                ContainerItem.IsSelected = true;
                            }
                        }
                    }
                }

                if (FullDispatchList.Count > 0)
                {
                    objContainerDetail.DispatchContainerList = FullDispatchList.ToArray();
                }
                return AppResult(objContainerDetail, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult DeleteContainer(ConsolidateBookingDTO objcont)
        {
            try
            {
                IEnumerable<DispatchContainerDTO> DispatchTableResult = _context.ExecuteQuery<DispatchContainerDTO>("EXEC dbo.USP_LG_COSOLIDATECONTAINERDETAIL_TABLE @BookedId,@Mode",
                                         new SqlParameter("BookedId", objcont.BookedId),
                                         new SqlParameter("Mode", "DeteleContainer")).ToList();
                return AppResult(DispatchTableResult, 1, PrepareMessage(EnumAction.Delete), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        
        
      
        [System.Web.Http.HttpPost]
        public ActionResult GetStuffingList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<ConsolidateBookingDTO> result = _context.ExecuteQuery<ConsolidateBookingDTO>("EXEC dbo.LG_ContainerStuffingListSP @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@SITEID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("SITEID", listParams.SiteId)
                        ).ToList();
                if (result.Count > 0)
                {
                    TotalRows = result[0].TotalCount.Value;
                    TotalRows = 0;
                }
                return AppResult(result, TotalRows);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult SaveStuffing(ConsolidateBookingDTO obtcont)
        {
            try
            {
                IEnumerable<int> objDeleteResult;
                //delete existing quotaion and container details
                objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.LG_StuffingQuotationDetail @BookedId,@Mode",
                    new SqlParameter("BookedId", obtcont.BookedId),
                    new SqlParameter("Mode", "QuotationDelete")).ToList();

                IEnumerable<int> objResult = null;

                foreach (CargoMaineFest objquot in obtcont.QuotationDetail)
                {
                    objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_ContainerStuffingQuotationSP @BookedId,@CargoMaineId,@WareHouseId,@StuffedQty,@ContainerNo",
                       new SqlParameter("BookedId", obtcont.BookedId),
                       new SqlParameter("CargoMaineId", objquot.CargoMaineId),
                       new SqlParameter("WareHouseId", objquot.WareHouseId),
                       new SqlParameter("StuffedQty", objquot.StuffedQty),
                       new SqlParameter("ContainerNo", objquot.ContainerNo)).ToList();

                }
                List<int> objList = objResult.ToList();

                return AppResult(objList, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult GetStuffingDetail(ConsolidateBookingDTO objcont)
        {
            try
            {
                List<ConsolidateBookingDTO> result = _context.ExecuteQuery<ConsolidateBookingDTO>("EXEC dbo.USP_LG_COSOLIDATEBOOKINGDETAIL @BookedId,@Mode",
                new SqlParameter("BookedId", objcont.BookedId),
                new SqlParameter("Mode", "Detail")).ToList();
                ConsolidateBookingDTO objBookedDetail = result[0];
                List<SqlParameter> list2 = new List<SqlParameter>
                {
                    new SqlParameter("BookedId", objcont.BookedId),
                    new SqlParameter("Mode", "QuotationDetail")
                };
                List<CargoMaineFest> list3 = _context.ExecuteQuery<CargoMaineFest>("EXEC dbo.LG_StuffingQuotationDetail @BookedId,@Mode", list2.ToArray()).ToList();
                objBookedDetail.QuotationDetail = list3.ToArray();
                return AppResult(objBookedDetail, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult DeleteStuffing(ConsolidateBookingDTO objcont)
        {
            try
            {
                IEnumerable<int> objDeleteResult;
                //delete existing quotaion and container details
                objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.LG_StuffingQuotationDetail @BookedId,@Mode",
                    new SqlParameter("BookedId", objcont.BookedId),
                    new SqlParameter("Mode", "QuotationDelete")).ToList();

                return AppResult(objDeleteResult, 1, PrepareMessage(EnumAction.Delete), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
          [System.Web.Http.HttpPost]
        public ActionResult SearchQuotation(ListParams listParams)
        {
            try
            {
                var result = _context.ExecuteQuery<NameValue>("EXEC dbo.LG_QuotationSearchSP @SEARCHVALUE",
                    new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpGet]
        public ActionResult PendingQuotationList(int id)
        {
            try
            {
                var result = _context.ExecuteQuery<CargoMaineFest>("EXEC dbo.LG_PendingQuotationSP @SITEID", new SqlParameter("SITEID", id)).ToList();
                return AppResult(result, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpGet]
        public virtual ActionResult GetBookedContainerList(int id)
        {
            try
            {
                IEnumerable<DispatchContainerDTO> ContainerListResult = _context.ExecuteQuery<DispatchContainerDTO>("EXEC dbo.USP_LG_COSOLIDATECONTAINERDETAIL_TABLE @BookedId,@Mode",
                                        new SqlParameter("BookedId", id),
                                        new SqlParameter("Mode", "Container")).ToList();
                List<DispatchContainerDTO> ContainerList = ContainerListResult.ToList();

                return AppResult(ContainerList, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult SearchContainer(ListParams listParams)
        {
            try
            {
                var result = _context.ExecuteQuery<ContainerManagementDTO>("EXEC dbo.LG_ContainerSearchSP @SEARCHVALUE",
                    new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
		
		[System.Web.Http.HttpPost]
        public ActionResult GetConsolidateBookingList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<ConsolidateBookingDTO> result = _context.ExecuteQuery<ConsolidateBookingDTO>("EXEC dbo.LG_ConsolidateBookingList @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@SITEID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("SITEID", listParams.SiteId)
                        ).ToList();
                if (result.Count > 0)
                {
                    TotalRows = result[0].TotalCount.Value;
                    TotalRows = 0;
                }
                return AppResult(result, TotalRows);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
		
		
		
		[System.Web.Http.HttpPost]
        public ActionResult GetContainerBookingList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<ContainerBookedDTO> result = _context.ExecuteQuery<ContainerBookedDTO>("EXEC dbo.LG_ContainerBookingListSP @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@SITEID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("SITEID", listParams.SiteId)
                        ).ToList();
                if (result.Count > 0)
                {
                    TotalRows = result[0].TotalCount.Value;
                    TotalRows = 0;
                }
                return AppResult(result, TotalRows);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult SaveContainerBooked(ContainerBookedDTO obtcont)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_ContainerBookedSP @BookedId,@BookedNo,@BookingDescription,@ContainerId,@SiteId,@UserId,@Mode",
                new SqlParameter("BookedId", obtcont.BookedId ?? Convert.DBNull),
                new SqlParameter("BookedNo", obtcont.BookedNo ?? Convert.DBNull),
                new SqlParameter("BookingDescription", obtcont.BookingDescription ?? Convert.DBNull),
                new SqlParameter("ContainerId", obtcont.ContainerId ?? Convert.DBNull),
                new SqlParameter("SiteId", obtcont.SiteId),
                new SqlParameter("UserId", obtcont.UserId ?? Convert.DBNull),
                new SqlParameter("Mode", obtcont.BookedId == 0 || obtcont.BookedId == null ? "Insert" : "Update")
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
        public virtual ActionResult GetContainerBookedDetail(ContainerBookedDTO objcont)
        {
            try
            {
                List<ContainerBookedDTO> result = _context.ExecuteQuery<ContainerBookedDTO>("EXEC dbo.LG_ContainerBookedSP @BookedId, @BookedNo, @BookingDescription, @ContainerId, @SiteId, @UserId, @Mode",
                new SqlParameter("@BookedId", objcont.BookedId),
                new SqlParameter("BookedNo", ""),
                new SqlParameter("BookingDescription", ""),
                new SqlParameter("ContainerId", ""),
                new SqlParameter("SiteId", objcont.SiteId),
                new SqlParameter("UserId", objcont.UserId),
                new SqlParameter("Mode", "Detail")).ToList();
                ContainerBookedDTO objBookedDetail = result[0];
                List<SqlParameter> list2 = new List<SqlParameter>
                {
                    new SqlParameter("@BookedId", objcont.BookedId)
                };
                List<ContainerDetail> list3 = _context.ExecuteQuery<ContainerDetail>("EXEC dbo.LG_ContainerBookedContainer @BookedId", list2.ToArray()).ToList();
                objBookedDetail.ContainerDetail = list3.ToArray();
                return AppResult(objBookedDetail, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult DeleteContainerBooked(ContainerBookedDTO objcont)
        {
            try
            {
                List<ContainerBookedDTO> result = _context.ExecuteQuery<ContainerBookedDTO>("EXEC dbo.LG_ContainerBookedSP @BookedId,@BookedNo,@BookingDescription,@ContainerId,@SiteId,@UserId,@Mode",
                 new SqlParameter("@BookedId", objcont.BookedId),
                new SqlParameter("BookedNo", ""),
                new SqlParameter("BookingDescription", ""),
                new SqlParameter("ContainerId", ""),
                new SqlParameter("SiteId", objcont.SiteId),
                new SqlParameter("UserId", objcont.UserId),
                new SqlParameter("Mode", "Delete")).ToList();
                ContainerBookedDTO objDetail = result[0];
                return AppResult(objDetail, 1, PrepareMessage(EnumAction.Delete), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

		
		
        public string RemoveAllSpecialCharacters(string text)
        {
            // Step 1: Remove non-printable characters
            text = Regex.Replace(text, @"[\x00-\x1F\x7F]", "");

            // Step 2: Remove specific unwanted characters
            string[] unwantedCharacters = { "�", "•", "™", "©","+" };
            foreach (var ch in unwantedCharacters)
            {
                text = text.Replace(ch, "");
            }

            // Step 3: Keep only ASCII letters, numbers, spaces, and basic punctuation
            text = Regex.Replace(text, @"[^a-zA-Z0-9\s.,?!]", "");

            return text;
        }

       
        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveConsolidateBooking(ConsolidateBookingDTO objBooking)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_COSOLIDATEBOOKING @BookedId,@FileNo,@CompanyName,@CompanyId,@CompanyAddress,@BookingDate,@ContactId,@ContractNo,@GALBookingType,@ModeOfService,@DepartmentID,@OriginCountry, @DestinationCountry, @OriginPortId,@OriginPort, @DestinationPortId,@DestinationPort," +
                    "@CustReqCutOffDate,@CustReqSailingDate, @PickupCategory,@PortcutoffDate,@RampCutOff,@ETA,@ETS,@CutOffDate,@EarliestPickUpDate,@ERD,@BookedBy,@Remarks,@BookingRemarks,@BookingNo,@Vessel,@Voyage,@SiteId,@OrignStateID,@OriginCityID,@DestinationStateID,@DestinationCityID,@NoOfContainer,@PickupLocationId,@PickupLocation,@ReturnLocationId,@ReturnLocation,@PickupLocationAddress,@ReturnLocationAddress,@UserId,@Mode",
                new SqlParameter("BookedId", objBooking.BookedId),
                new SqlParameter("FileNo", objBooking.FileNo ?? Convert.DBNull),
                new SqlParameter("CompanyName", objBooking.CompanyName ?? Convert.DBNull),
                new SqlParameter("CompanyId", objBooking.CompanyId ?? Convert.DBNull),
                new SqlParameter("CompanyAddress", objBooking.CompanyAddress ?? Convert.DBNull),
                new SqlParameter("BookingDate", objBooking.BookingDate ?? Convert.DBNull),
                new SqlParameter("ContactId", objBooking.ContactId),
                new SqlParameter("ContractNo", objBooking.ContractNo ?? Convert.DBNull),
                new SqlParameter("GALBookingType", objBooking.GALBookingType ?? Convert.DBNull),
                new SqlParameter("ModeOfService", objBooking.ModeOfService ?? Convert.DBNull),
                new SqlParameter("DepartmentID", objBooking.DepartmentId ?? Convert.DBNull),
                new SqlParameter("OriginCountry", Convert.ToString(objBooking.OriginCountryID) ?? Convert.DBNull),
                new SqlParameter("DestinationCountry", Convert.ToString(objBooking.DestinationCountryID) ?? Convert.DBNull),
                new SqlParameter("OriginPortId", objBooking.OriginPortId),
                new SqlParameter("OriginPort", objBooking.OriginPort ?? Convert.DBNull),
                new SqlParameter("DestinationPortId", objBooking.DestinationPortId),
                new SqlParameter("DestinationPort", objBooking.DestinationPort ?? Convert.DBNull),
                new SqlParameter("CustReqCutOffDate", objBooking.custReqCutOffDate ?? Convert.DBNull),
                new SqlParameter("CustReqSailingDate", objBooking.custReqSailingDate ?? Convert.DBNull),
                new SqlParameter("PickupCategory", objBooking.PickupCategory ?? Convert.DBNull),
                new SqlParameter("PortcutoffDate", objBooking.PortcutoffDate ?? Convert.DBNull),
                new SqlParameter("RampCutOff", objBooking.RampCutOff ?? Convert.DBNull),
                new SqlParameter("ETA", objBooking.ETA ?? Convert.DBNull),
                new SqlParameter("ETS", objBooking.ETS ?? Convert.DBNull),
                new SqlParameter("CutOffDate", objBooking.CutOffDate ?? Convert.DBNull),
                new SqlParameter("EarliestPickUpDate", objBooking.EarliestPickUpDate ?? Convert.DBNull),
                new SqlParameter("ERD", objBooking.ERD ?? Convert.DBNull),
                new SqlParameter("BookedBy", objBooking.BookedBy ?? Convert.DBNull),
                new SqlParameter("Remarks", objBooking.Remarks ?? Convert.DBNull),
                new SqlParameter("BookingRemarks", objBooking.BookingRemarks ?? Convert.DBNull),
                new SqlParameter("BookingNo", objBooking.BookingNo ?? Convert.DBNull),
                new SqlParameter("Vessel", objBooking.Vessel ?? Convert.DBNull),
                new SqlParameter("Voyage", objBooking.Voyage ?? Convert.DBNull),
                new SqlParameter("SiteId", objBooking.SiteId),
                new SqlParameter("OrignStateID", objBooking.OrignStateID ?? Convert.DBNull),
                new SqlParameter("OriginCityID", objBooking.OriginCityID ?? Convert.DBNull),
                new SqlParameter("DestinationStateID", objBooking.DestinationStateID ?? Convert.DBNull),
                new SqlParameter("DestinationCityID", objBooking.DestinationCityID ?? Convert.DBNull),
                new SqlParameter("NoOfContainer", objBooking.NoOfContainer ?? Convert.DBNull),
                new SqlParameter("PickupLocationId", objBooking.PickupLocationId ?? Convert.DBNull),
                new SqlParameter("PickupLocation", objBooking.PickupLocation ?? Convert.DBNull),
                new SqlParameter("ReturnLocationId", objBooking.ReturnLocationId ?? Convert.DBNull),
                new SqlParameter("ReturnLocation", objBooking.ReturnLocation ?? Convert.DBNull),
                new SqlParameter("PickupLocationAddress", objBooking.PickupLocationAddress ?? Convert.DBNull),
                new SqlParameter("ReturnLocationAddress", objBooking.ReturnLocationAddress ?? Convert.DBNull),
                new SqlParameter("UserId", objBooking.UserId ?? Convert.DBNull),
                new SqlParameter("Mode", 1)
                ).ToList();
                List<int> objList = objResult.ToList();
                int BookedId = objList[0];


               // int EnquiryId = Convert.ToInt32(objBooking.EnquiryID);
                IEnumerable<int> objDeleteResult;

                int modeOfService = Convert.ToInt32(objBooking.ModeOfService);
                //delete existing Container Service details
                objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_Consolidate_REF_DELETE @RefId,@RefType",
                    new SqlParameter("RefId", BookedId),
                    new SqlParameter("RefType", "Booking")).ToList();
                //For containers & FCL = 1
                if (objBooking.EnquiryContainerServiceDTOList.Count() > 0)
                {
                    SaveConsolidateContainerService(objBooking.EnquiryContainerServiceDTOList, BookedId, modeOfService, objBooking.NoOfContainer);

                    if (modeOfService == 7) //AIR
                    {
                        if (objBooking.EnquiryAIRServiceDTOList.Count() > 0)
                        {
                            SaveConsolidateAIRService(objBooking.EnquiryAIRServiceDTOList, BookedId, objBooking.NoOfContainer);
                        }
                    }
                    if (modeOfService == 5) //Break Bulk
                    {
                        if (objBooking.EnquiryBreakBulkServiceDTOList.Count() > 0)
                        {
                            SaveConsolidateBreakBulkService(objBooking.EnquiryBreakBulkServiceDTOList, BookedId, objBooking.NoOfContainer);
                        }
                    }
                    if (modeOfService == 2) //LCL
                    {
                        if (objBooking.EnquiryLCLServiceDTOList.Count() > 0)
                        {
                            SaveConsolidateLCLService(objBooking.EnquiryLCLServiceDTOList, BookedId, objBooking.NoOfContainer);
                        }
                    }

                    if (modeOfService == 3)  //RORO Service details
                    {
                        if (objBooking.EnquiryROROServiceDTOList.Count() > 0)
                        {
                            SaveConsolidateROROService(objBooking.EnquiryROROServiceDTOList, BookedId, objBooking.NoOfContainer);
                        }
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
        public virtual ActionResult GetConsolidateBookingDetail(ConsolidateBookingDTO objcont)
        {
            try
            {
                List<ConsolidateBookingDTO> result = _context.ExecuteQuery<ConsolidateBookingDTO>("EXEC dbo.USP_LG_COSOLIDATEBOOKINGDETAIL @BookedId,@Mode",
                new SqlParameter("@BookedId", objcont.BookedId),
                new SqlParameter("Mode", "Detail")).ToList();
                ConsolidateBookingDTO objBookedDetail = result[0];

                //get Container details / FCL                                     
                List<EnquiryContainerServiceDTO> ContainerServiceDTOList = _context.ExecuteQuery<EnquiryContainerServiceDTO>("EXEC dbo.USP_Consolidate_CONTAINER_LIST @RefId,@RefType",
                                    new SqlParameter("RefId", objcont.BookedId),
                                    new SqlParameter("RefType", "Booking")).ToList();
                if (ContainerServiceDTOList.Count > 0)
                {
                    objBookedDetail.EnquiryContainerServiceDTOList = ContainerServiceDTOList.ToArray();
                }
                //get Air
                List<EnquiryAIRServiceDTO> AirServiceDTOList = _context.ExecuteQuery<EnquiryAIRServiceDTO>("EXEC dbo.USP_Consolidate_GET_AIR @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", objcont.BookedId),
                                new SqlParameter("RefType", "Booking"),
                                new SqlParameter("fkServiceRequiredID", 7)).ToList();
                if (AirServiceDTOList.Count > 0)
                {
                    objBookedDetail.EnquiryAIRServiceDTOList = AirServiceDTOList.ToArray();
                }
                //get Break Bulk
                List<EnquiryBreakBulkServiceDTO> BreakBulkServiceDTOList = _context.ExecuteQuery<EnquiryBreakBulkServiceDTO>("EXEC dbo.USP_Consolidate_GET_BREAK_BULK @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", objcont.BookedId),
                                new SqlParameter("RefType", "Booking"),
                                new SqlParameter("fkServiceRequiredID", 5)).ToList();
                if (BreakBulkServiceDTOList.Count > 0)
                {
                    objBookedDetail.EnquiryBreakBulkServiceDTOList = BreakBulkServiceDTOList.ToArray();
                }
                //get LCL
                List<EnquiryLCLServiceDTO> LCLServiceDTOList = _context.ExecuteQuery<EnquiryLCLServiceDTO>("EXEC dbo.USP_Consolidate_GET_LCL @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", objcont.BookedId),
                                new SqlParameter("RefType", "Booking"),
                                new SqlParameter("fkServiceRequiredID", 2)).ToList();
                if (LCLServiceDTOList.Count > 0)
                {
                    objBookedDetail.EnquiryLCLServiceDTOList = LCLServiceDTOList.ToArray();
                }
                //get RORO
                List<EnquiryROROServiceDTO> ROROServiceDTOList = _context.ExecuteQuery<EnquiryROROServiceDTO>("EXEC dbo.USP_Consolidate_GET_RORO @RefId,@RefType,@fkServiceRequiredID",
                                new SqlParameter("RefId", objcont.BookedId),
                                new SqlParameter("RefType", "Booking"),
                                new SqlParameter("fkServiceRequiredID", 3)).ToList();
                if (ROROServiceDTOList.Count > 0)
                {
                    objBookedDetail.EnquiryROROServiceDTOList = ROROServiceDTOList.ToArray();
                }
                return AppResult(objBookedDetail, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult DeleteConsolidateBooking(ConsolidateBookingDTO objcont)
        {
            try
            {
                List<ContainerBookedDTO> result = _context.ExecuteQuery<ContainerBookedDTO>("EXEC dbo.USP_LG_COSOLIDATEBOOKINGDETAIL @BookedId,@Mode",
                 new SqlParameter("BookedId", objcont.BookedId),
                 new SqlParameter("Mode", "Delete")).ToList();
                ContainerBookedDTO objDetail = result[0];
                return AppResult(objDetail, 1, PrepareMessage(EnumAction.Delete), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        public void SaveConsolidateContainerService(EnquiryContainerServiceDTO[] EnquiryContainerServiceDTOList, int BookedId, int modeOfService, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryContainerServiceDTO objContainerServiceDTO in EnquiryContainerServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_Consolidate_FCL_INSERT @ServiceRequiredID,@NoofContainer,@ContainerSizeID,@QTY,@RefId,@RefType",
                    new SqlParameter("ServiceRequiredID", modeOfService),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("ContainerSizeID", objContainerServiceDTO.ContainerTypeID),
                    new SqlParameter("QTY", objContainerServiceDTO.Quantity),
                    new SqlParameter("RefId", BookedId),
                    new SqlParameter("RefType", "Booking")).ToList();
            }
        }

        public void SaveConsolidateAIRService(EnquiryAIRServiceDTO[] EnquiryAIRServiceDTOList, int BookedId, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryAIRServiceDTO objAIRService in EnquiryAIRServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_SIPL_Consolidate_AIR_INSERT @ServiceRequiredID,@NoofContainer,@PieceCount,@Weight,@TotalWt,@Length,@Width,@Height,@Volume,@RefId,@RefType",
                    new SqlParameter("ServiceRequiredID", 7),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("PieceCount", objAIRService.PieceCount),
                    new SqlParameter("Weight", objAIRService.Weight),
                    new SqlParameter("TotalWt", objAIRService.TotalWt),
                    new SqlParameter("Length", objAIRService.Length),
                    new SqlParameter("Width", objAIRService.Width),
                    new SqlParameter("Height", objAIRService.Height),
                    new SqlParameter("Volume", objAIRService.Volume),
                    new SqlParameter("RefId", BookedId),
                    new SqlParameter("RefType", "Booking")).ToList();
            }

        }

        public void SaveConsolidateBreakBulkService(EnquiryBreakBulkServiceDTO[] EnquiryBreakBulkServiceDTOList, int BookedId, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryBreakBulkServiceDTO objBBulkService in EnquiryBreakBulkServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_Consolidate_BREAK_BULK_INSERT @ServiceRequiredID,@NoofContainer,@RefId,@RefType,@Weight,@Length,@Width,@Height,@Volume,@Description,@Qty,@ChkInland,@InlandRemarks,@MafiCharges",
                    new SqlParameter("ServiceRequiredID", 5),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("RefId", BookedId),
                    new SqlParameter("RefType", "Booking"),
                    new SqlParameter("Weight", objBBulkService.Weight),
                    new SqlParameter("Length", objBBulkService.Length),
                    new SqlParameter("Width", objBBulkService.Width),
                    new SqlParameter("Height", objBBulkService.Height),
                    new SqlParameter("Volume", objBBulkService.Volume ?? Convert.DBNull),
                    new SqlParameter("Description", objBBulkService.Description ?? Convert.DBNull),
                    new SqlParameter("Qty", objBBulkService.Qty),
                    new SqlParameter("ChkInland", objBBulkService.ChkInland ?? Convert.DBNull),
                    new SqlParameter("InlandRemarks", objBBulkService.InlandRemarks ?? Convert.DBNull),
                    new SqlParameter("MafiCharges", objBBulkService.MafiCharges ?? Convert.DBNull)).ToList();
            }
        }
        public void SaveConsolidateLCLService(EnquiryLCLServiceDTO[] EnquiryLCLServiceDTOList, int BookedId, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryLCLServiceDTO objLCLService in EnquiryLCLServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_Consolidate_LCL_INSERT @ServiceRequiredID,@NoofContainer,@PieceCount,@Weight,@TotalWt,@Length,@Width,@Height,@Volume,@RefId,@RefType",
                    new SqlParameter("ServiceRequiredID", 2),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("PieceCount", objLCLService.PieceCount),
                    new SqlParameter("Weight", objLCLService.Weight),
                    new SqlParameter("TotalWt", objLCLService.TotalWt),
                    new SqlParameter("Length", objLCLService.Length),
                    new SqlParameter("Width", objLCLService.Width),
                    new SqlParameter("Height", objLCLService.Height),
                    new SqlParameter("Volume", objLCLService.Volume ?? Convert.DBNull),
                    new SqlParameter("RefId", BookedId),
                    new SqlParameter("RefType", "Booking")).ToList();
            }

        }
        public void SaveConsolidateROROService(EnquiryROROServiceDTO[] EnquiryROROServiceDTOList, int BookedId, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult1;
            foreach (EnquiryROROServiceDTO objROROService in EnquiryROROServiceDTOList)
            {
                objResult1 = _context.ExecuteQuery<int>("EXEC dbo.USP_Consolidate_RORO_INSERT @ServiceRequiredID,@NoofContainer,@RefId,@RefType,@NoofCrain,@Weight,@Length,@Width,@Volume,@Height,@ChkInland,@InlandRemarks,@MafiCharges",
                    new SqlParameter("ServiceRequiredID", 3),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("RefId", BookedId),
                    new SqlParameter("RefType", "Booking"),
                    new SqlParameter("NoofCrain", objROROService.NoofCrain ?? Convert.DBNull),
                    new SqlParameter("Weight", objROROService.Weight),
                    new SqlParameter("Length", objROROService.Length),
                    new SqlParameter("Width", objROROService.Width),
                    new SqlParameter("Volume", objROROService.Volume),
                    new SqlParameter("Height", objROROService.Height),
                    new SqlParameter("ChkInland", objROROService.ChkInland ?? Convert.DBNull),
                    new SqlParameter("InlandRemarks", objROROService.InlandRemarks ?? Convert.DBNull),
                    new SqlParameter("MafiCharges", objROROService.MafiCharges ?? Convert.DBNull)).ToList();
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult GetConsolidateDockReceiptList(Dictionary<string, string> listParams)
        {
            try
            {
                List<SqlParameter> list = new List<SqlParameter>
                {
                    new SqlParameter("PageIndex", listParams["PageIndex"]),
                    new SqlParameter("PageSize", listParams["PageSize"]),
                    new SqlParameter("Sort", Utility.GetSort(listParams["Sort"])),
                    new SqlParameter("SitId", listParams["SitId"]),
                    new SqlParameter("Count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    },
                    new SqlParameter("FileNo", listParams["FileNo"])
                };
                List<ConsolidateDockReceiptDTO> entity = _context.ExecuteQuery<ConsolidateDockReceiptDTO>(DocumentCommonQuery.GetConsolidateDockReceiptList, list.ToArray()).ToList();
                int count = Utility.GetParamValue(list, "Count", typeof(int));
                return AppResult(entity, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveConsolidateDockReceipt(ConsolidateDockReceiptDTO objBooking)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_SAVE_CONSOLIDATE_DOCKRECEIPT @DockReceiptId,@BookedId,@ExporterId,@ExporterName,@ExporterAddress,@Free_Text,@DockReceptNo,@BLNumber,@FileNo,@ConsignedToId,@ConsignedToName,@ConsignedToAddress,@FwdAgentId,@FwdAgentName,@FwdAgentAddress,@FTZNumber, @ConsigneeId,@ConsigneeName ,@ConsigneeAddress," +
                    "@ExportInstruction, @CarriageBy,@PlaceOfReceipt,@BuyingRate,@SellingRate,@Vessel,@LandingPortId,@LandingPort,@LoadingPert,@Voyage,@ForeignPortId,@ForeignPort," +
                    "@Transshipment,@Transshipmentdesc,@MoveType, @DeliveredBy,@ReqStuffingDate,@CheckedBy,@CheckedBydesc,@Issued,@Booking,@CutOff,@DepartureDate,@EmptyPickup,@pickupName,@FullReturn,@FullReturnName,@EmptyPickupId,@FullReturnId,@SiteId,@UserId",
                 new SqlParameter("DockReceiptId", objBooking.DockReceiptId),
                new SqlParameter("BookedId", objBooking.BookedId),
                new SqlParameter("ExporterId", objBooking.ExporterId ?? Convert.DBNull),
                new SqlParameter("ExporterName", objBooking.ExporterName ?? Convert.DBNull),
                new SqlParameter("ExporterAddress", objBooking.ExporterAddress ?? Convert.DBNull),
                new SqlParameter("Free_Text", objBooking.Free_Text ?? Convert.DBNull),
                new SqlParameter("DockReceptNo", objBooking.DockReceptNo ?? Convert.DBNull),
                new SqlParameter("BLNumber", objBooking.BLNumber ?? Convert.DBNull),
                new SqlParameter("FileNo", objBooking.FileNo ?? Convert.DBNull),
                new SqlParameter("ConsignedToId", objBooking.ConsignedToId),
                new SqlParameter("ConsignedToName", objBooking.ConsignedToName),
                new SqlParameter("ConsignedToAddress", objBooking.ConsignedToAddress ?? Convert.DBNull),
                new SqlParameter("FwdAgentId", objBooking.FwdAgentId ?? Convert.DBNull),
                new SqlParameter("FwdAgentName", objBooking.FwdAgentName ?? Convert.DBNull),
                new SqlParameter("FwdAgentAddress", objBooking.FwdAgentAddress ?? Convert.DBNull),
                new SqlParameter("FTZNumber", objBooking.FTZNumber ?? Convert.DBNull),
                new SqlParameter("ConsigneeId", objBooking.ConsigneeId ?? Convert.DBNull),
                new SqlParameter("ConsigneeName", objBooking.ConsigneeName ?? Convert.DBNull),
                new SqlParameter("ConsigneeAddress", objBooking.ConsigneeAddress ?? Convert.DBNull),
                new SqlParameter("ExportInstruction", objBooking.ExportInstruction),
                new SqlParameter("CarriageBy", objBooking.CarriageBy ?? Convert.DBNull),
                new SqlParameter("PlaceOfReceipt", objBooking.PlaceOfReceipt),
                new SqlParameter("BuyingRate", objBooking.BuyingRate),
                new SqlParameter("SellingRate", objBooking.SellingRate),
                new SqlParameter("Vessel", objBooking.Vessel ?? Convert.DBNull),
                new SqlParameter("LandingPortId", objBooking.LandingPortId ?? Convert.DBNull),
                new SqlParameter("LandingPort", objBooking.LandingPort ?? Convert.DBNull),
                new SqlParameter("LoadingPert", objBooking.LoadingPert ?? Convert.DBNull),
                new SqlParameter("Voyage", objBooking.Voyage ?? Convert.DBNull),
                new SqlParameter("ForeignPortId", objBooking.ForeignPortId ?? Convert.DBNull),
                 new SqlParameter("ForeignPort", objBooking.ForeignPort ?? Convert.DBNull),
                new SqlParameter("Transshipment", objBooking.Transshipment ?? Convert.DBNull),
                new SqlParameter("Transshipmentdesc", objBooking.Transshipmentdesc ?? Convert.DBNull),
                new SqlParameter("MoveType", objBooking.MoveType ?? Convert.DBNull),
                new SqlParameter("DeliveredBy", objBooking.DeliveredBy ?? Convert.DBNull),
                new SqlParameter("ReqStuffingDate", objBooking.ReqStuffingDate ?? Convert.DBNull),
                new SqlParameter("CheckedBy", objBooking.CheckedBY ?? Convert.DBNull),
                new SqlParameter("CheckedBydesc", objBooking.CheckedBydesc ?? Convert.DBNull),
                new SqlParameter("Issued", objBooking.Issued ?? Convert.DBNull),
                new SqlParameter("Booking", objBooking.Booking ?? Convert.DBNull),
                new SqlParameter("CutOff", objBooking.CutOff ?? Convert.DBNull),
                new SqlParameter("DepartureDate", objBooking.DepartureDate ?? Convert.DBNull),
                new SqlParameter("EmptyPickup", objBooking.EmptyPickup ?? Convert.DBNull),
                new SqlParameter("pickupName", objBooking.pickupName ?? Convert.DBNull),
                new SqlParameter("FullReturn", objBooking.FullReturn ?? Convert.DBNull),
                new SqlParameter("FullReturnName", objBooking.FullReturnName ?? Convert.DBNull),
                new SqlParameter("EmptyPickupId", objBooking.EmptyPickupId ?? Convert.DBNull),
                new SqlParameter("FullReturnId", objBooking.FullReturnId ?? Convert.DBNull),
                new SqlParameter("SiteId", objBooking.SiteId),
                new SqlParameter("UserId", objBooking.UserId ?? Convert.DBNull)
                ).ToList();
                List<int> objList = objResult.ToList();
                //int BookedId = objList[0];

                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult GetConsolidateDockReceipt(ConsolidateBookingDTO obj)
        {
            try
            {
                List<SqlParameter> list = new List<SqlParameter>
                {
                    new SqlParameter("BookedId", obj.BookedId)
                };
                ConsolidateDockReceiptDTO dockReceiptDTO = _context.ExecuteQuery<ConsolidateDockReceiptDTO>(DocumentCommonQuery.GetConsolidateDockReceipt, list.ToArray()).SingleOrDefault();
                //List<SqlParameter> list2 = new List<SqlParameter>
                //{
                //    new SqlParameter("Type", "DO"),
                //    new SqlParameter("DocumentCommonId", dockReceiptDTO.DocumentCommonId)
                //};
                //List<CertificationOfOriginODDTO> list3 = _context.ExecuteQuery<CertificationOfOriginODDTO>(DocumentCommonQuery.GetCommodityDetail, list2.ToArray()).ToList();
                //dockReceiptDTO.CommodityDetail = list3.ToArray();
                return AppResult(dockReceiptDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult MoveConsolidateBooking(int id)
        {
            try
            {
                IEnumerable<ConsolidateDockReceiptDTO> objResult = _context.ExecuteQuery<ConsolidateDockReceiptDTO>("EXEC dbo.USP_MOVE_COSOLIDATEBOOKING @BookedId,@ReturnValue OUT",
                new SqlParameter("BookedId", id),
                new SqlParameter("ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                }
                ).ToList();
                // List<int> objList = objResult.ToList();
                // int BookedId = objList[0];

                return AppResult(objResult, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult Country(ListParams listParams)
        {
            try
            {

               
                var result = _context.ExecuteQuery<CommonSearchData>("EXEC dbo.Country_Search_SP @SEARCHVALUE",
                    new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
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

                
                var result = _context.ExecuteQuery<CommonSearchData>("EXEC dbo.State_Search_SP @SEARCHVALUE",
                    new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult GetShiperList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<DocumentationListDTO> result = _context.ExecuteQuery<DocumentationListDTO>("EXEC dbo.USP_SLI_FORM_GET_LIST @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@DOS_ID,@SORTCOLUMN,@SORTORDER,@DASHBOARD_FILTER,@SIT_ID,@UserName,@UserType",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("DOS_ID", searchCriteria["dos_Id"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("DASHBOARD_FILTER", searchCriteria["dashboardOption"]),
                        new SqlParameter("SIT_ID", listParams.SiteId),
                        new SqlParameter("UserName", searchCriteria["userName"]),
                        new SqlParameter("UserType", searchCriteria["userType"])
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
        public ActionResult saveShiperData(SLIFORMDATA objees)
        {
            try
            {
                
                object[] obj = new object[75]
                 {
                     new SqlParameter("Id", objees.Id ),
                     new SqlParameter("DocumentCommonId", objees.documentCommonId ?? Convert.DBNull),
                     new SqlParameter("SiteId", objees.siteId ?? Convert.DBNull),
                     new SqlParameter("ShipmentNumber", objees.shipmentNumber ?? Convert.DBNull),
                     new SqlParameter("OriginState", objees.originState ?? Convert.DBNull),
                     new SqlParameter("OriginStateId", objees.originStateId ?? Convert.DBNull),
                     new SqlParameter("OriginStateCode", objees.originStateCode ?? Convert.DBNull),
                     new SqlParameter("CountryDestination", objees.countryDestination ?? Convert.DBNull),
                     new SqlParameter("CountryDestinationId", objees.countryDestinationId ?? Convert.DBNull),
                     new SqlParameter("CountryDestinationCode", objees.countryDestinationCode ?? Convert.DBNull),

                     new SqlParameter("PortofExport", objees.portofExport ?? Convert.DBNull),
                     new SqlParameter("PortofExportId", objees.portofExportId ?? Convert.DBNull),
                     new SqlParameter("PortofExportCode", objees.portofExportCode ?? Convert.DBNull),
                     new SqlParameter("PortofUnloading", objees.portofUnloading ?? Convert.DBNull),
                     new SqlParameter("PortofUnloadingId", objees.portofUnloadingId ?? Convert.DBNull),
                     new SqlParameter("PortofUnloadingCode", objees.portofUnloadingCode ?? Convert.DBNull),
                     new SqlParameter("DepartureDate", objees.departureDate ?? Convert.DBNull),


                     new SqlParameter("InbondType", objees.inbondType ?? Convert.DBNull),
                     new SqlParameter("ImportEntry", objees.importEntry ?? Convert.DBNull),
                     new SqlParameter("ForeginTradeZone", objees.foreginTradeZone ?? Convert.DBNull),
                     new SqlParameter("RelatedParty", objees.relatedParty ?? Convert.DBNull),
                     new SqlParameter("IsRoutedTransaction", objees.isRoutedTransaction ?? Convert.DBNull),
                     new SqlParameter("IsTIB", objees.isTIB ?? Convert.DBNull),
                     new SqlParameter("IsHazardousMaterial", objees.isHazardousMaterial ?? Convert.DBNull),

                     new SqlParameter("UsppiCompany", objees.usppiCompany ?? Convert.DBNull),
                     new SqlParameter("UsppiFirstName", objees.usppiFirstName ?? Convert.DBNull),
                     new SqlParameter("UsppiLastName", objees.usppiLastName ?? Convert.DBNull),
                     new SqlParameter("UsppiAddress", objees.usppiAddress ?? Convert.DBNull),
                     new SqlParameter("UsppiCity", objees.usppiCity ?? Convert.DBNull),
                     new SqlParameter("UsppiState", objees.usppiState ?? Convert.DBNull),
                     new SqlParameter("UsppiStateId", objees.usppiStateId ?? Convert.DBNull),
                     new SqlParameter("UsppiStateCode", objees.usppiStateCode ?? Convert.DBNull),
                     new SqlParameter("UsppiPostalCode", objees.usppiPostalCode ?? Convert.DBNull),
                     new SqlParameter("UsppiPhoneNumber", objees.usppiPhoneNumber ?? Convert.DBNull),
                     new SqlParameter("UsppiEmail", objees.usppiEmail ?? Convert.DBNull),
                     new SqlParameter("UsppiEIN", objees.usppiEIN ?? Convert.DBNull),
                     new SqlParameter("UsppiReference", objees.usppiReference ?? Convert.DBNull),


                     new SqlParameter("UltimateConsigneeType", objees.ultimateConsigneeType ?? Convert.DBNull),
                     new SqlParameter("UltimateCompany", objees.ultimateCompany ?? Convert.DBNull),
                     new SqlParameter("UltimateFirstName", objees.ultimateFirstName ?? Convert.DBNull),
                     new SqlParameter("UltimateLastName", objees.ultimateLastName ?? Convert.DBNull),
                     new SqlParameter("UltimateAddress", objees.ultimateAddress ?? Convert.DBNull),
                     new SqlParameter("UltimateCity", objees.ultimateCity ?? Convert.DBNull),
                     new SqlParameter("UltimateCountry", objees.ultimateCountry ?? Convert.DBNull),
                     new SqlParameter("UltimateCountryId", objees.ultimateCountryId ?? Convert.DBNull),
                     new SqlParameter("UltimateCountryCode", objees.ultimateCountryCode ?? Convert.DBNull),
                     new SqlParameter("UltimateState", objees.ultimateState ?? Convert.DBNull),
                     new SqlParameter("UltimateStateId", objees.ultimateStateId ?? Convert.DBNull),
                     new SqlParameter("UltimateStateCode", objees.ultimateStateCode ?? Convert.DBNull),
                     new SqlParameter("UltimatePostalCode", objees.ultimatePostalCode ?? Convert.DBNull),
                     new SqlParameter("UltimatePhoneNumber", objees.ultimatePhoneNumber ?? Convert.DBNull),


                     new SqlParameter("InterCompany", objees.interCompany ?? Convert.DBNull),
                     new SqlParameter("InterFirstName", objees.interFirstName ?? Convert.DBNull),
                     new SqlParameter("InterLastName", objees.interLastName ?? Convert.DBNull),
                     new SqlParameter("InterAddress", objees.interAddress ?? Convert.DBNull),
                     new SqlParameter("InterCity", objees.interCity ?? Convert.DBNull),
                     new SqlParameter("InterCountry", objees.interCountry ?? Convert.DBNull),
                     new SqlParameter("InterCountryId", objees.interCountryId ?? Convert.DBNull),
                     new SqlParameter("InterCountryCode", objees.interCountryCode ?? Convert.DBNull),
                     new SqlParameter("InterState", objees.interState ?? Convert.DBNull),
                     new SqlParameter("InterStateId", objees.interStateId ?? Convert.DBNull),
                     new SqlParameter("InterStateCode", objees.interStateCode ?? Convert.DBNull),
                     new SqlParameter("InterPostalCode", objees.interPostalCode ?? Convert.DBNull),
                     new SqlParameter("InterPhoneNumber", objees.interPhoneNumber ?? Convert.DBNull),


                    new SqlParameter("FreightCompany", objees.freightCompany ?? Convert.DBNull),
                    new SqlParameter("FreightFirstName", objees.freightFirstName ?? Convert.DBNull),
                    new SqlParameter("FreightLastName", objees.freightLastName ?? Convert.DBNull),
                    new SqlParameter("FreightAddress", objees.freightAddress ?? Convert.DBNull),
                    new SqlParameter("FreightCity", objees.freightCity ?? Convert.DBNull),
                    new SqlParameter("FreightState", objees.freightState ?? Convert.DBNull),
                    new SqlParameter("FreightStateId", objees.freightStateId ?? Convert.DBNull),
                    new SqlParameter("FreightStateCode", objees.freightStateCode ?? Convert.DBNull),
                    new SqlParameter("FreightPostalCode", objees.freightPostalCode ?? Convert.DBNull),
                    new SqlParameter("FreightPhoneNumber", objees.freightPhoneNumber ?? Convert.DBNull),
                    new SqlParameter("CreatedBy", objees.createdBy ?? Convert.DBNull)

                    };

                IEnumerable<int> source = _context.ExecuteQuery<int>("EXEC dbo.SIPL_SLI_FORMDATA_SP @Id,@DocumentCommonId,@SiteId,@ShipmentNumber,@OriginState,@OriginStateId,@OriginStateCode, @CountryDestination,@CountryDestinationId,@CountryDestinationCode,@PortofExport,@PortofExportId,@PortofExportCode,@PortofUnloading,@PortofUnloadingId,@PortofUnloadingCode,@DepartureDate,@InbondType,@ImportEntry,@ForeginTradeZone,@RelatedParty,@IsRoutedTransaction,@IsTIB,@IsHazardousMaterial,@UsppiCompany,@UsppiFirstName,@UsppiLastName,@UsppiAddress,@UsppiCity,@UsppiState,@UsppiStateId,@UsppiStateCode,@UsppiPostalCode,@UsppiPhoneNumber,@UsppiEmail,@UsppiEIN,@UsppiReference,@UltimateConsigneeType,@UltimateCompany,@UltimateFirstName,@UltimateLastName,@UltimateAddress,@UltimateCity,@UltimateCountry,@UltimateCountryId,@UltimateCountryCode,@UltimateState,@UltimateStateId,@UltimateStateCode, @UltimatePostalCode,@UltimatePhoneNumber,@InterCompany,@InterFirstName,@InterLastName,@InterAddress,@InterCity,@InterCountry,@InterCountryId,@InterCountryCode,@InterState,@InterStateId,@InterStateCode,@InterPostalCode,@InterPhoneNumber,@FreightCompany,@FreightFirstName,@FreightLastName,@FreightAddress,@FreightCity,@FreightState,@FreightStateId,@FreightStateCode,@FreightPostalCode,@FreightPhoneNumber,@CreatedBy", obj).ToList();
                List<int> list = source.ToList();
                int num = list[0];
                if (num > 0)
                {
                    if (objees.CommodityDetail != null)
                    {
                        if (objees.CommodityDetail.Length > 0)
                        {
                            for (int i = 0; i < objees.CommodityDetail.Length; i++)
                            {
                                // if (objees.CommodityDetail[i].ExportCode != "" && objees.CommodityDetail[i].ScheduleB != "" && objees.CommodityDetail[i].IsType != "D")
                                if (objees.CommodityDetail[i].ExportCode != "" && objees.CommodityDetail[i].ExportCode != "-Select-")
                                {
                                    object[] objLine = new object[26]
                                         {
                                 new SqlParameter("@idd", objees.CommodityDetail[i].idd?? Convert.DBNull),
                                 new SqlParameter("@ExportCode", objees.CommodityDetail[i].ExportCode?? Convert.DBNull),
                                 new SqlParameter("@ScheduleB", objees.CommodityDetail[i].ScheduleB?? Convert.DBNull),
                                 new SqlParameter("@FirstQuantity", objees.CommodityDetail[i].FirstQuantity?? Convert.DBNull),
                                 new SqlParameter("@FirstUOM", objees.CommodityDetail[i].FirstUOM?? Convert.DBNull),
                                 new SqlParameter("@SecondQuantity", objees.CommodityDetail[i].SecondQuantity?? Convert.DBNull),
                                 new SqlParameter("@SecondUOM", objees.CommodityDetail[i].SecondUOM?? Convert.DBNull),
                                 new SqlParameter("@OriginofGoods", objees.CommodityDetail[i].OriginofGoods?? Convert.DBNull),
                                 new SqlParameter("@ValueofGoods", objees.CommodityDetail[i].ValueofGoods?? Convert.DBNull),
                                 new SqlParameter("@ShippingWeight", objees.CommodityDetail[i].ShippingWeight?? Convert.DBNull),
                                 new SqlParameter("@Eccn", objees.CommodityDetail[i].Eccn?? Convert.DBNull),
                                 new SqlParameter("@LicenseTypeCode", objees.CommodityDetail[i].LicenseTypeCode?? Convert.DBNull),
                                 new SqlParameter("@ExpLic", objees.CommodityDetail[i].ExpLic?? Convert.DBNull),
                                 //new SqlParameter("@IsGovermentAgency", objees.CommodityDetail[i].IsGovermentAgency?? Convert.DBNull),
                                 new SqlParameter("@CommodityDescription", objees.CommodityDetail[i].CommodityDescription?? Convert.DBNull),
                                 new SqlParameter("@DocumentCommonId", objees.CommodityDetail[i].DocumentCommonId?? Convert.DBNull),
                                // new SqlParameter("@SiteId", objees.CommodityDetail[i].SiteId?? Convert.DBNull),
                                 new SqlParameter("@SliFormId", num.ToString()?? Convert.DBNull),
                                new SqlParameter("@DDTCITAR", objees.CommodityDetail[i].dDTCITAR==null?"": objees.CommodityDetail[i].dDTCITAR),
                                new SqlParameter("@DDTCReg", objees.CommodityDetail[i].dDTCReg==null?"": objees.CommodityDetail[i].dDTCReg),
                                new SqlParameter("@DDTCSignificant", objees.CommodityDetail[i].dDTCSignificant==null?"": objees.CommodityDetail[i].dDTCSignificant),
                                new SqlParameter("@DDTCEligible", objees.CommodityDetail[i].dDTCEligible==null?"": objees.CommodityDetail[i].dDTCEligible),
                                new SqlParameter("@DDTCUSML", objees.CommodityDetail[i].dDTCUSML==null?"": objees.CommodityDetail[i].dDTCUSML),
                                new SqlParameter("@DDTCUnit", objees.CommodityDetail[i].dDTCUnit==null?"": objees.CommodityDetail[i].dDTCUnit),
                                new SqlParameter("@DDTCQuantity", objees.CommodityDetail[i].dDTCQuantity==null?"": objees.CommodityDetail[i].dDTCQuantity),
                                new SqlParameter("@DDTCLicense", objees.CommodityDetail[i].dDTCLicense==null?"": objees.CommodityDetail[i].dDTCLicense),
                               // new SqlParameter("@ctype", objees.CommodityDetail[i].ctype?? Convert.DBNull),
                                new SqlParameter("@LicValueAmount", objees.CommodityDetail[i].LicValueAmount?? Convert.DBNull),
                                new SqlParameter("@HTSCodeId", objees.CommodityDetail[i].htsCodeId?? Convert.DBNull),

                                      };

                                    _context.ExecuteQuery("EXEC dbo.SLIForm_CommodityDetail_SP @idd,@ExportCode,@ScheduleB,@FirstQuantity,@FirstUOM,@SecondQuantity,@SecondUOM,@OriginofGoods,@ValueofGoods,@ShippingWeight,@Eccn,@LicenseTypeCode,@ExpLic,@CommodityDescription,@DocumentCommonId,@SliFormId,@DDTCITAR,@DDTCReg,@DDTCSignificant,@DDTCEligible,@DDTCUSML,@DDTCUnit,@DDTCQuantity,@DDTCLicense,@LicValueAmount,@HTSCodeId", objLine).ToList();

                                }

                            }
                        }
                    }
                    if (objees.CommodityDetail != null)
                    {
                        if (objees.VinDetail.Length > 0)
                        {
                            for (int i = 0; i < objees.VinDetail.Length; i++)
                            {
                                if (objees.VinDetail[i].VINNumber != "")
                                {
                                    object[] objvin = new object[8]
                                         {
                                 new SqlParameter("@VIN", objees.VinDetail[i].VIN?? Convert.DBNull),
                                 new SqlParameter("@VINNumber", objees.VinDetail[i].VINNumber?? Convert.DBNull),
                                 new SqlParameter("@VehicleTitleNum", objees.VinDetail[i].VehicleTitleNum?? Convert.DBNull),
                                 new SqlParameter("@VehicleTitleState", objees.VinDetail[i].VehicleTitleState?? Convert.DBNull),
                                 new SqlParameter("@DocumentCommonId", objees.VinDetail[i].DocumentCommonId?? Convert.DBNull),
                                 new SqlParameter("@SliFormId", num.ToString()?? Convert.DBNull),
                                 new SqlParameter("@HtsCode", objees.VinDetail[i].HtsCode?? Convert.DBNull),
                                 new SqlParameter("@HtsCodeId", objees.VinDetail[i].HtsCodeId?? Convert.DBNull)
                                      };

                                    _context.ExecuteQuery("EXEC dbo.SLIFORM_VINDetail_SP @VIN,@VINNumber,@VehicleTitleNum,@VehicleTitleState,@DocumentCommonId,@SliFormId,@HtsCode,@HtsCodeId", objvin).ToList();

                                }

                            }
                        }
                    }
                }
                return AppResult(list, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult getShiperDetail(DocumentationDetailDTO obj)
        {
            try
            {
                List<SLIFORMDATA> result = _context.ExecuteQuery<SLIFORMDATA>("EXEC dbo.SIPL_SLI_FORMGETDATA @DocumentCommonID,@SLIFORMID,@MODE",
                                        new SqlParameter("DocumentCommonID", obj.DocumentCommonID),
                                        new SqlParameter("SLIFORMID", "0"),
                                        new SqlParameter("MODE", "SLIFORMIDATA")).ToList();
                SLIFORMDATA objDetailDTO = result[0];
              
                IEnumerable<CommodityLine> CommodityResult = _context.ExecuteQuery<CommodityLine>("EXEC dbo.SIPL_SLI_FORMGETDATA @DocumentCommonID,@SLIFORMID,@MODE",
                     new SqlParameter("DocumentCommonID", obj.DocumentCommonID),
                     new SqlParameter("SLIFORMID", objDetailDTO.Id),
                     new SqlParameter("MODE", "COMODITY")).ToList();
                List<CommodityLine> CommodityResultList = CommodityResult.ToList();

                if (CommodityResultList.Count > 0)
                {
                    objDetailDTO.CommodityDetail = CommodityResult.ToArray();
                }
                IEnumerable<Vin> vinResult = _context.ExecuteQuery<Vin>("EXEC dbo.SIPL_SLI_FORMGETDATA @DocumentCommonID,@SLIFORMID,@MODE",
                     new SqlParameter("DocumentCommonID", obj.DocumentCommonID),
                     new SqlParameter("SLIFORMID", objDetailDTO.Id),
                     new SqlParameter("MODE", "VINDETAIL")).ToList();
                List<Vin> vinResultList = vinResult.ToList();
                if (vinResultList.Count > 0)
                {
                    objDetailDTO.VinDetail = vinResult.ToArray();

                }


                return AppResult(objDetailDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult GetTemplateList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<EESLIST> result = _context.ExecuteQuery<EESLIST>("EXEC dbo.LG_TEMPLATEDATA @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@SIT_ID",
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
        public ActionResult SaveAesTemplate(AESTemplate objees)
        {
            try
            {

                object[] obj = new object[20]
                 {
                     new SqlParameter("TemplateId", objees.templateId ?? Convert.DBNull),
                     new SqlParameter("EINNO", objees.einNo ?? Convert.DBNull),
                     new SqlParameter("CompanyName", objees.companyName ?? Convert.DBNull),
                     new SqlParameter("CompanyId", objees.companyId ?? Convert.DBNull),
                     new SqlParameter("FirstName", objees.firstName ?? Convert.DBNull),
                     new SqlParameter("LastName", objees.lastName ?? Convert.DBNull),
                     new SqlParameter("CountryName", objees.countryName ?? Convert.DBNull),
                     new SqlParameter("CountryId", objees.countryId ?? Convert.DBNull),
                     new SqlParameter("CountryCode", objees.countryCode ?? Convert.DBNull),
                     new SqlParameter("StateName", objees.stateName ?? Convert.DBNull),
                     new SqlParameter("StateId", objees.stateId ?? Convert.DBNull),
                     new SqlParameter("StateCode", objees.stateCode ?? Convert.DBNull),
                     new SqlParameter("CityName", objees.cityName ?? Convert.DBNull),
                     new SqlParameter("AddressLine1", objees.addressLine1 ?? Convert.DBNull),
                     new SqlParameter("AddressLine2", objees.addressLine2 ?? Convert.DBNull),
                     new SqlParameter("PostalCode", objees.postalCode ?? Convert.DBNull),
                     new SqlParameter("PhoneNumber", objees.phoneNumber ?? Convert.DBNull),
                     new SqlParameter("TemplateType", objees.templateType ?? Convert.DBNull),
                     new SqlParameter("IsActive", objees.IsActive),
                     new SqlParameter("CreatedBy", objees.createdBy ?? Convert.DBNull)

                    };

                IEnumerable<int> source = _context.ExecuteQuery<int>("EXEC dbo.SIPL_SAVE_AES_TEMPLATE_SP @TemplateId,@EINNO,@CompanyName,@CompanyId,@FirstName,@LastName,@CountryName,@CountryId,@CountryCode, @StateName,@StateId,@StateCode,@CityName,@AddressLine1,@AddressLine2,@PostalCode,@PhoneNumber,@TemplateType,@IsActive,@CreatedBy", obj).ToList();
                List<int> list = source.ToList();
               
                return AppResult(list, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult GetAesTemplateList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<AESTemplate> result = _context.ExecuteQuery<AESTemplate>("EXEC dbo.SIPL_AES_TEMPLATELIST_SP @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@SIT_ID",
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
        public virtual ActionResult getAesTemplateDetail(AESTemplate obj)
        {
            try
            {
                List<AESTemplate> result = _context.ExecuteQuery<AESTemplate>("EXEC dbo.SIPL_GET_AES_TEMPLATELIST_SP @TemplateId",
                                        new SqlParameter("TemplateId", obj.templateId)).ToList();
                AESTemplate objDetailDTO = result[0];

                return AppResult(objDetailDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult GetPartTemplateList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<AESTemplate> result = _context.ExecuteQuery<AESTemplate>("EXEC dbo.SIPL_GET_TEMPLATELIST_SP @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@TemplateType,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("TemplateType", searchCriteria["templateType"]),
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
    }
    
}
public class FillingOption
{
    public int Id { get; set; }
    public string name { get; set; }
}
public class htscode
{
    public int Id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
}
public class FillingOptionEntity
{
    public string Id { get; set; }
    public string name { get; set; }
    public string code { get; set; }
}
public class ItemValue
{
    public string Id { get; set; }
    public string name { get; set; }

    }
    public class ItemValue1
    {
        public string PortId { get; set; }
        public string Id { get; set; }
        public string name { get; set; }

    }
    public class HtcCodeItem
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string htscode { get; set; }
        public string Unit { get; set; }
        public string Unit1 { get; set; }
        public string description { get; set; }
        public string isvin { get; set; }
    }
    public class Company
    {
        public string ContactID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }

    }

    public class CompanySepatra
    {
        public int sepatraCustomerId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }

    }
    public class Doc
    {
        public string DocumentCommonID { get; set; }
        public string FileName { get; set; }

    }
    public class HtsCode
    {
        public int? Id { get; set; }
        public string HTSNumber { get; set; }
        public string description { get; set; }
        public string FirstUOM { get; set; }
        public string SecondUOM { get; set; }
        public int TotalCount { get; set; }
        public string indent { get; set; }
        public int? CreatedBy { get; set; }
        public bool isvin { get; set; }
        public bool isDel { get; set; }
        public string htsfirst { get; set; }
        public string htssecond { get; set; }
        public string htsthird { get; set; }
        public string htsfourth { get; set; }
    }
    public class CompanyDetail
    {
        public string ContactID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string CityName { get; set; }
        public string ContactPerson { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ZipCode { get; set; }
        public string TelNo { get; set; }
        public string Email { get; set; }
        public string IDNumber { get; set; }
        public string IDNumberType { get; set; }
        public string IRSNumber { get; set; }

    }
    public class NameValue
    {
        public string Name { get; set; }
        public string Value { get; set; }

    }
    public class ddtcitar
    {
        public string ddtcitarcode { get; set; }
        public string ddtcitardesc { get; set; }

    }
    public class ddtcusml
    {
        public string ddtcitarcode { get; set; }
        public string ddtcitardesc { get; set; }

    }
    public class LienseExemptionCode
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string licval { get; set; }
    }
    public class CommonSearchData
     {
    public string ItemId { get; set; }
    public string ItemName { get; set; }
    public string ItemCode { get; set; }

   }
public class AESTemplate
{
    public string templateId { get; set; }
    public string einNo { get; set; }
    public string companyName{ get; set; }
    public string companyId { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string countryName { get; set; }
    public string countryId { get; set; }
    public string countryCode { get; set; }
    public string stateName { get; set; }
    public string stateId { get; set; }
    public string stateCode { get; set; }
    public string cityName { get; set; }
    public string addressLine1 { get; set; }
    public string addressLine2 { get; set; }
    public string postalCode { get; set; }
    public string phoneNumber { get; set; }
    public string templateType { get; set; }
    public bool IsActive { get; set; }
    public string createdBy { get; set; }
    public int? TotalCount { get; set; }


}
public class Filter1
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string FieldName { get; set; }
    public string Value { get; set; }
    public string Operator { get; set; }
    public string ValueT { get; set; }
}