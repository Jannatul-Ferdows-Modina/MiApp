using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Helper.Logging;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Security;
using AppMGL.DAL.Repository.Setup;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Security;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;
using AppMGL.Manager.Infrastructure.Helper;
using AppMGL.Manager.Infrastructure.Results;
using AutoMapper;
using Newtonsoft.Json;
using AppMGL.DTO.Operation;

namespace AppMGL.Manager.Areas.Security.Controllers
{
    public class UserController : BaseController<UserDTO, UserRepository, LG_USER>
    {
        #region Constructor

        public ContactRepository contactRepository;

        public UserController(UserRepository context, ContactRepository contactRepository)
        {
            _context = context;
            BaseModule = EnumModule.User;
            KeyField = "UserId";

            this.contactRepository = contactRepository;
        }

        #endregion

        #region Override Methods

        [System.Web.Http.HttpGet]
        public override ActionResult Detail(long id)
        {
            try
            {
                var result = _context.Detail(id);
                var dtoResult = Mapper.Map<UserDTO>(result);

                if (!string.IsNullOrEmpty(dtoResult.UsrSmtpPassword))
                {
                    dtoResult.UsrSmtpPassword = SecurityHelper.DecryptString(dtoResult.UsrSmtpPassword, "sblw-3hn8-sqoy19");
                }

                dtoResult.Contact = Mapper.Map<ContactDTO>(result.LG_CONTACT);
                //get User's Unit & Role details
                List<SiteRoleDTO> SiteRoleDTOList = _context.ExecuteQuery<SiteRoleDTO>("EXEC dbo.USP_LG_USER_UNIT_ROLE_GET @CONTACT_ID",
                                    new SqlParameter("CONTACT_ID", dtoResult.Contact.CntId)).ToList();
                if (SiteRoleDTOList.Count > 0)
                {
                    dtoResult.UserUnitRoleList = SiteRoleDTOList.ToArray();
                }

                return AppResult(dtoResult, "");
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Insert(UserDTO dto)
        {
            try
            {
                Logger.WriteWarning("1",false);
                //LG_USER entiy = new LG_USER();
                //  entiy.UsrId = dto.UsrId ;
                //      entiy.CntId = dto.CntId;
                //      entiy.AspNetUserId = dto.AspNetUserId;
                //      entiy.UsrPwd = dto.UsrPwd;
                //      entiy.UsrTempPwd = dto.UsrTempPwd;
                //      entiy.UsrPwdCreatedTs = dto.UsrPwdCreatedTs;
                //      entiy.UsrValidFrom = dto.UsrValidFrom ;
                //      entiy.UsrValidTo = dto.UsrValidTo ;
                //      entiy.UsrFailedCount = dto.UsrFailedCount ;
                //      entiy.UsrIsLocked = dto.UsrIsLocked;
                //      entiy.UsrLockedComments =dto.UsrLockedComments;
                //      entiy.UsrPwdNeverExpire = dto.UsrPwdNeverExpire ;
                //      entiy.UsrLastLoginTs = dto.UsrLastLoginTs;
                //      entiy.UsrIsPowerUser = dto.UsrIsPowerUser ;
                //       entiy.UsrCreatedTs = dto.UsrCreatedTs ;
                //        entiy.UsrUpdatedTs = dto.UsrUpdatedTs;
                //         entiy.UsrCreatedBy = dto.UsrCreatedBy ;
                //            entiy.UsrUpdatedBy = dto.UsrUpdatedBy ;
                //             entiy.UsrSmtpUsername = dto.UsrSmtpUsername;
                //              entiy.UsrSmtpPassword =dto.UsrSmtpPassword ;
                //              entiy.LG_CONTACT. = dto.Contact;
                //               //entiy.LG_CONTACT. =dto.FullName;
                //               //  entiy.UserName = 
                //               //    entiy.DptName =
                //               //      entiy.TotalCount =
                //               //       entiy.Contact =
                //               //        entiy.TotalCount =
                //               //         entiy.TotalCount =
                //               //          entiy.TotalCount =

                
                var entity = Mapper.Map<LG_USER>(dto);

                Logger.WriteWarning("2", false);
                string useremail = entity.LG_CONTACT.CntEmail;
                Logger.WriteWarning("21" + useremail, false);
                var user = _context.RegisterUser(entity);
                var token = _context.GenerateEmailConfirmationToken(user);
                var url = Url.Link("ConfirmEmailRoute", new { UserId = user.Id, Token = token });
                Logger.WriteWarning("22" + user.Id + "|" + token, false);
                _context.SendEmailConfirmation(entity, user, url);
                Logger.WriteWarning("23 send mail"  , false);
                entity.UsrPwd = null;
                entity.AspNetUserId = user.Id;

                if (!string.IsNullOrEmpty(entity.UsrSmtpPassword))
                {
                    entity.UsrSmtpPassword = SecurityHelper.EncryptString(entity.UsrSmtpPassword, "sblw-3hn8-sqoy19");
                }

                _context.Insert(entity);
                _context.UnitOfWork.Commit();

                var result = _context.Detail(GetPrimaryKeyValue(entity));
            //    var dtoResult = Mapper.Map<UserDTO>(result);

               // dtoResult.Contact = Mapper.Map<ContactDTO>(result.LG_CONTACT);
                if (dto.UserUnitRoleList.Length > 0)
                {
                    InsertUserUnitRoles(dto.UserUnitRoleList, result.LG_CONTACT.CntId);
                }
                //get User's Unit & Role details
                List<SiteRoleDTO> SiteRoleDTOList = _context.ExecuteQuery<SiteRoleDTO>("EXEC dbo.USP_LG_USER_UNIT_ROLE_GET @CONTACT_ID",
                                    new SqlParameter("CONTACT_ID", result.LG_CONTACT.CntId)).ToList();
                if (SiteRoleDTOList.Count > 0)
                {
                    dto.UserUnitRoleList = SiteRoleDTOList.ToArray();
                }
                return AppResult(dto, PrepareMessage(EnumAction.Insert));
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPut]
        public override ActionResult Update(long id, UserDTO dto)
        {
            try
            {
                var entity = Mapper.Map<LG_USER>(dto);

                if (!string.IsNullOrEmpty(entity.UsrSmtpPassword))
                {
                    entity.UsrSmtpPassword = SecurityHelper.EncryptString(entity.UsrSmtpPassword, "sblw-3hn8-sqoy19");
                }

                _context.Update(entity);

                _context.UnitOfWork.Commit();
                //delete user's existing Unit & Roles
                IEnumerable<int> objDeleteResult;                
                objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_USER_UNIT_ROLE_DELETE @CONTACT_ID",
                    new SqlParameter("@CONTACT_ID", dto.CntId)).ToList();
                if (dto.UserUnitRoleList.Length > 0)
                {
                    InsertUserUnitRoles(dto.UserUnitRoleList, dto.CntId);
                }
                var result = _context.Detail(GetPrimaryKeyValue(entity));
                var dtoResult = Mapper.Map<UserDTO>(result);

                dtoResult.Contact = Mapper.Map<ContactDTO>(result.LG_CONTACT);
                //get User's Unit & Role details
                List<SiteRoleDTO> SiteRoleDTOList = _context.ExecuteQuery<SiteRoleDTO>("EXEC dbo.USP_LG_USER_UNIT_ROLE_GET @CONTACT_ID",
                                    new SqlParameter("CONTACT_ID", dtoResult.Contact.CntId)).ToList();
                if (SiteRoleDTOList.Count > 0)
                {
                    dtoResult.UserUnitRoleList = SiteRoleDTOList.ToArray();
                }
                return AppResult(dtoResult, PrepareMessage(EnumAction.Update));
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Delete(UserDTO dto)
        {
            try
            {
                var repo = new AuthRepository();
                List<LG_SITE> userSite;
                var contact = contactRepository.Detail(Convert.ToInt64(dto.CntId));
                if (contact.CwtId == 2)
                {
                    userSite = repo.FindSite(dto.UsrId.ToString());
                    if (userSite.Count > 0)
                    {
                        throw new Exception("This User is associated with Unit. Please remove this from Unit");
                    }
                }

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_USER_DELETE_CONTACT @CONTACT_ID",
                                    new SqlParameter("CONTACT_ID", dto.CntId)).ToList();

                var entity = Mapper.Map<LG_USER>(dto);
                var result = _context.Delete(entity);
                _context.UnitOfWork.Commit();

                var dtoResult = Mapper.Map<UserDTO>(result);
                return AppResult(dtoResult, PrepareMessage(EnumAction.Delete));
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        
        [System.Web.Http.HttpPost]
        public override ActionResult List(ListParams listParams)
        {
            try
            {                
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                Dictionary<string, string> dictionary = new Dictionary<string, string>() ;
                if (listParams.Filter != "[]")
                {
                    dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                }
                else
                {
                    dictionary.Add("firstNameF", "");
                    dictionary.Add("lastNameF", "");
                    dictionary.Add("dptNameF", "");
                    dictionary.Add("cntTypeF", "");
                    
                }
                
                int TotalRows = 0;
                int cntTypeF = 3;
                if (!string.IsNullOrEmpty(dictionary["cntTypeF"]))
                {
                    cntTypeF = Convert.ToInt32(dictionary["cntTypeF"]);
                }
                List<UserDTO> result = _context.ExecuteQuery<UserDTO>("EXEC dbo.USP_LG_USER_GET_LIST @PAGENO, @PAGESIZE,@USER_ID,@USER_WORKTYPE_ID,@SORTCOLUMN,@SORTORDER,@firstNameF,@lastNameF,@dptNameF,@ContactType",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("USER_ID", listParams.UserId),
                        new SqlParameter("USER_WORKTYPE_ID", listParams.UserWorkTypeId),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("FirstNameF", dictionary.ContainsKey("firstNameF") ? dictionary["firstNameF"] : string.Empty),
                        new SqlParameter("LastNameF", dictionary.ContainsKey("lastNameF") ? dictionary["lastNameF"] : string.Empty),
                        new SqlParameter("DptNameF", dictionary.ContainsKey("dptNameF") ? dictionary["dptNameF"] : string.Empty),
                         new SqlParameter("ContactType", cntTypeF)

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

        #endregion

        #region Public Method

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                {
                    throw new Exception("User Id and Token are required");
                }

                var result = await _context.ConfirmEmail(userId, token);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    throw new Exception("Failed: " + string.Join(",", result.Errors));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult Info(Dictionary<string, string> param)
        {
            try
            {
                var result = _context.Info(param);
                var dtoResult = Mapper.Map<UserDTO>(result);
                dtoResult.Contact = Mapper.Map<ContactDTO>(result.LG_CONTACT);
                return AppResult(dtoResult, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult Roles(Dictionary<string, string> param)
        {
            try
            {
                var result = _context.Roles(param);
                int count = result.Count;
                var dtoResult = Mapper.Map<List<RoleMapDTO>>(result);
                return AppResult(dtoResult, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public ActionResult GetUserRoles(int id)
        {
            try
            {
                List<RoleDTO> result = _context.ExecuteQuery<RoleDTO>("EXEC dbo.USP_LG_USER_GET_ROLES @USER_WORKTYPE_ID",
                    new SqlParameter("USER_WORKTYPE_ID", id) ).ToList();
                
                return AppResult(result, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult ChangePassword(Dictionary<string, string> param)
        {
            try
            {
                var result = _context.ChangePassword(param);

                return AppResult(null, "You have changed password successfully.");
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult CheckEmail(Dictionary<string, string> param)
        {
            try
            {
                var result = _context.CheckEmail(param);
                var dtoResult = Mapper.Map<UserDTO>(result);
                string message = (result == null) ? "You can use this email." : "Email is already in use.";
                return AppResult(dtoResult, message);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult ResetPassword(Dictionary<string, string> param)
        {
            try
            {
                var result = _context.ResetPassword(param);
                var dtoResult = Mapper.Map<UserDTO>(result);
                string message = (result == null) ? "Email is not exist the system." : "Email has been sent successfully to reset your password.";
                return AppResult(dtoResult, message);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult ResendEmail(Dictionary<string, string> param)
        {
            try
            {
                var user = _context.GetIdentityUserByEmail(param);
                var token = _context.GenerateEmailConfirmationToken(user);
                var url = Url.Link("ConfirmEmailRoute", new { UserId = user.Id, Token = token });
                var entity = _context.GetUserByAspNetUserId(user.Id);
                _context.SendEmailConfirmation(entity, user, url);
                var dtoResult = Mapper.Map<UserDTO>(entity);
                string message = "Email has been re-sent successfully.";
                return AppResult(dtoResult, message);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult ResetPasswordA(Dictionary<string, string> param)
        {
            try
            {
                var result = _context.ResetPasswordA(param);

                return AppResult(null, "Email has been sent successfully to reset user password.");
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        public void InsertUserUnitRoles(SiteRoleDTO[] objSiteRoleDTOList, decimal contactId)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            int siteId = 0;
            foreach (SiteRoleDTO objSiteRoleDTO in objSiteRoleDTOList)
            {                
                if (objSiteRoleDTO.SitId == null)
                {
                    siteId = 0;
                }
                else {
                    siteId = (int)objSiteRoleDTO.SitId;
                }
                
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_USER_UNIT_ROLE_INSERT @SITE_ID,@CNT_ID,@ROLE_ID,@CREATEDBY_ID",
                    new SqlParameter("SITE_ID", siteId),
                    new SqlParameter("CNT_ID", contactId),
                    new SqlParameter("ROLE_ID", objSiteRoleDTO.RleId),
                    new SqlParameter("CREATEDBY_ID", DBNull.Value)).ToList();
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
                var uploadFolder = ConfigurationManager.AppSettings["UserProfile"];
                var root = HttpContext.Current.Server.MapPath(uploadFolder);
                var provider = new MultipartFormDataStreamProvider(root);

                var result = await Request.Content.ReadAsMultipartAsync(provider);

                if (result.FormData.HasKeys())
                {
                    var fileName = GetUnescapeData(result, "FileName").ToString();
                    var UserId = GetUnescapeData(result, "UserId").ToString();
                    DirectoryInfo dir = new DirectoryInfo(root);
                    FileInfo[] files = dir.GetFiles(UserId + "_*");
                    foreach (FileInfo file in files)
                    {
                        file.Delete();
                    }
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
        public HttpResponseMessage GetUserImage()
        {
            try
            {
                string fileName = "";
                int UserId;
                fileName = Request.Headers.GetValues("fileName").ToList()[0];
                UserId = Int32.Parse(Request.Headers.GetValues("userId").ToList()[0]);
                //fileName = UserId + "_" + fileName;
                //string filelike = UserId+
                DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CustomerPath"]));
                FileInfo[] files = dir.GetFiles(UserId+"_*");
                fileName = files[0].Name;
                //fileName = "male_user_icon.png";
                HttpResponseMessage result = null;
                if (!System.IO.File.Exists(fileName))
                {
                    result = Request.CreateResponse(HttpStatusCode.Gone);
                }
                if (!string.IsNullOrEmpty(fileName))
                {
                    var CustomerFolder = ConfigurationManager.AppSettings["CustomerPath"];
                    string filePath = HttpContext.Current.Server.MapPath(CustomerFolder + "/") + fileName;

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
        public ActionResult CreateCompanyUser(ListParams listParams)
        {
            StandardJsonResult standardJsonResult = new StandardJsonResult
            {
                ResultId = Convert.ToInt32(EnumResult.Failed)
            };
            try
            {
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var c =  (dictionary["companyname"]);
                var contactid =  (dictionary["companyid"]);
                var createdby =  (dictionary["createdby"]);

               

                List<CustomerContactDTO> list = _context.ExecuteQuery<CustomerContactDTO>("EXEC dbo.USP_LG_CONTACT_GET_DETAIL @ContactID", new object[1]
                {
                    new SqlParameter("ContactID", contactid)
                }).ToList();
                CustomerContactDTO customerContactDTO = list[0];
                var lstuser = _context.ExecuteQuery<int>("EXEC dbo.USP_Check_User_Email @Email", new object[1]
              {
                    new SqlParameter("Email", customerContactDTO.Email)
              }).FirstOrDefault();
                if (lstuser > 0)
                {
                    return AppResult(lstuser,1, "Email is aleady exits", EnumResult.Failed);
                }



                AppMGL.DTO.Security.UserDTO dto = new DTO.Security.UserDTO();
                dto.FullName = customerContactDTO.ContactPersonFirstName + ' ' + customerContactDTO.ContactPersonLastName;
                dto.UserName = customerContactDTO.Email;
                dto.UsrCreatedBy = Convert.ToDecimal(createdby);
                dto.UsrValidFrom = DateTime.Now;
                dto.DptName = "";
                dto.UsrCreatedTs = DateTime.Now;
                dto.UsrPwdCreatedTs = null;
                dto.UsrValidFrom = DateTime.Now;
                dto.UsrValidTo = null;
                dto.UsrIsLocked = 0;
                dto.UsrUpdatedTs = null;
                dto.UsrPwdCreatedTs = null;
                //dto.CwtId

                ContactDTO con = new ContactDTO();
                
                con.CntCreatedBy = Convert.ToDecimal( createdby);
                con.DptId = Convert.ToDecimal(ConfigurationManager.AppSettings["deptid"]);
                con.CntType = Convert.ToDecimal(ConfigurationManager.AppSettings["cntTypeId"]);
                con.FullName = dto.FullName;
                con.CntFirstName = dto.FullName.Split(' ')[0].ToString();
                con.TtlId = Convert.ToDecimal(ConfigurationManager.AppSettings["titleid"]);
                 
                if (dto.FullName.Split(' ').Count()==2)
                con.CntLastName = dto.FullName.Split(' ')[1].ToString();

                con.CntEmail = customerContactDTO.Email;
                con.CntCreatedTs = DateTime.Now;
                con.CntStatus = true;
                con.CntUpdatedTs = DateTime.Now;
                con.CwtId = Convert.ToDecimal( ConfigurationManager.AppSettings["createUserCwtId"]);
               // con.cnt
                dto.Contact = con;
                // dto. = "1";

                List<SiteRoleDTO> lstrole = new List<SiteRoleDTO>();
                string role = ConfigurationManager.AppSettings["externaluserrole"];
                foreach (var item in role.Split(','))
                {
                    SiteRoleDTO s = new SiteRoleDTO();

                    s.SitId = Convert.ToDecimal(ConfigurationManager.AppSettings["createUserSiteId"]);
                    s.RleId = Convert.ToDecimal(item);
                    lstrole.Add(s);
                }


                dto.UserUnitRoleList = lstrole.ToArray();

                try
                {
                    Logger.WriteWarning("1", false);
                    
                    var entity = Mapper.Map<LG_USER>(dto);
                    entity.UsrPwdCreatedTs = DateTime.Now;
                    Logger.WriteWarning("2", false);
                    string useremail = entity.LG_CONTACT.CntEmail;
                    Logger.WriteWarning("21" + useremail, false);
                    var user = _context.RegisterUser(entity);
                    var token = _context.GenerateEmailConfirmationToken(user);
                    var url = Url.Link("ConfirmEmailRoute", new { UserId = user.Id, Token = token });
                    Logger.WriteWarning("22" + user.Id + "|" + token, false);
                    _context.SendEmailConfirmation(entity, user, url);
                    Logger.WriteWarning("23 send mail", false);
                    entity.UsrPwd = null;
                    entity.AspNetUserId = user.Id;

                    if (!string.IsNullOrEmpty(entity.UsrSmtpPassword))
                    {
                        entity.UsrSmtpPassword = SecurityHelper.EncryptString(entity.UsrSmtpPassword, "sblw-3hn8-sqoy19");
                    }
                   
                    _context.Insert(entity);
                    _context.UnitOfWork.Commit();

                    var result = _context.Detail(GetPrimaryKeyValue(entity));
                    //    var dtoResult = Mapper.Map<UserDTO>(result);

                    // dtoResult.Contact = Mapper.Map<ContactDTO>(result.LG_CONTACT);
                    if (dto.UserUnitRoleList.Length > 0)
                    {
                        InsertUserUnitRoles(dto.UserUnitRoleList, result.LG_CONTACT.CntId);
                    }
                    //get User's Unit & Role details
                    List<SiteRoleDTO> SiteRoleDTOList = _context.ExecuteQuery<SiteRoleDTO>("EXEC dbo.USP_LG_USER_UNIT_ROLE_GET @CONTACT_ID",
                                        new SqlParameter("CONTACT_ID", result.LG_CONTACT.CntId)).ToList();
                    if (SiteRoleDTOList.Count > 0)
                    {
                        dto.UserUnitRoleList = SiteRoleDTOList.ToArray();
                    }
                    return AppResult(dto, PrepareMessage(EnumAction.Insert));
                }
                catch (Exception ex)
                {
                    return AppResult(ex);
                }




                //return AppResult(null, 1L, "User created successfully", EnumResult.Success);

            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        #endregion
    }
}
