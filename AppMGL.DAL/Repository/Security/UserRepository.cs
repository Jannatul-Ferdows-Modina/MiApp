using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AppMGL.DAL.Helper.Logging;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Security;
using AppMGL.DAL.UDT;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace AppMGL.DAL.Repository.Security
{
	public class UserRepository : Repository<LG_USER>, IUserRepository, IRepository<LG_USER>, IDisposable
	{
		private AppUserManager _userManager;

        protected AppUserManager AppUserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.Request.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="context">Associated context</param>
        public UserRepository(Models.AppMGL context)
            : base(context)
        {
            Query = new UserQuery();
        }

       

        #region Override Methods

        public override LG_USER Update(LG_USER item)
        {
            if (item != null)
            {
                if (!string.IsNullOrEmpty(item.UsrPwd))
                {
                    var resetResult = ResetPassword(item.AspNetUserId, item.UsrPwd);
                    if (!resetResult.Succeeded)
                    {
                        throw new Exception("Failed: " + string.Join(",", resetResult.Errors));
                    }
                    item.UsrPwd = null;
                }
                _unitOfWork.SetModified(item);
                _unitOfWork.SetModified(item.LG_CONTACT);
            }
            return null;
        }

        public override LG_USER Delete(LG_USER item)
        {
            if (item != null)
            {
                //_unitOfWork.Attach(item);
                //GetSet().Remove(item);
                DeleteAspNetUser(item.AspNetUserId);
            }
            return null;
        }

        #endregion

        #region Public Methods

        public LG_USER Info(Dictionary<string, string> param)
        {
            decimal usrId = Convert.ToDecimal(param["UsrId"]);

            var info = from u in _unitOfWork.CreateSet<LG_USER>()
                       join c in _unitOfWork.CreateSet<LG_CONTACT>() on u.CntId equals c.CntId
                       where u.UsrId == usrId
                       select u;

            return info.SingleOrDefault();
        }

        public List<LG_MAM_ROL_MAP> Roles(Dictionary<string, string> param)
        {
            decimal usrId = Convert.ToDecimal(param["UsrId"]);
            decimal sitId = Convert.ToDecimal(param["SitId"]);
            var info = from u in _unitOfWork.CreateSet<LG_USER>()
                       where u.UsrId == usrId
                       select u;
            decimal usrWorkTypeId = (decimal)info.ToList()[0].LG_CONTACT.CwtId;
            
            if (usrWorkTypeId == 1 || usrWorkTypeId == 0)
                {
                 var list = from u in _unitOfWork.CreateSet<LG_USER>()
                           join cr in _unitOfWork.CreateSet<LG_SITE_CONTACT_ROLE>() on u.CntId equals cr.CntId
                           join rm in _unitOfWork.CreateSet<LG_MAM_ROL_MAP>() on cr.RleId equals rm.RleId
                           where u.UsrId == usrId                           
                           && rm.MamIsEnable == true
                           select rm;
                return list.ToList();
            }
            else
            {
                var list = from u in _unitOfWork.CreateSet<LG_USER>()
                           join cr in _unitOfWork.CreateSet<LG_SITE_CONTACT_ROLE>() on u.CntId equals cr.CntId
                           join rm in _unitOfWork.CreateSet<LG_MAM_ROL_MAP>() on cr.RleId equals rm.RleId
                           where u.UsrId == usrId
                           && cr.SitId == sitId
                           && rm.MamIsEnable == true
                           select rm;
                return list.ToList();
            }
            
            //return list.ToList();
        }

        public IdentityUser RegisterUser(LG_USER item)
        {
           // Logger.WriteWarning("12", false);
           item.UsrTempPwd = GenerateRandomPassword(10);
           Logger.WriteWarning("error in item.LG_CONTACT.CntEmail" + item.LG_CONTACT.CntEmail, false);
            var user = new IdentityUser
            {
               	UserName = item.LG_CONTACT.CntEmail,
				Email = item.LG_CONTACT.CntEmail
            };
            Logger.WriteWarning("13", false);

             var result = AsyncHelper.RunSync<IdentityResult>(() => CreateUser(user, item.UsrTempPwd));

            if (!result.Succeeded)
            {

                Logger.WriteWarning("14  " + result.Errors, false);
                throw new Exception("Failed: " + string.Join(",", result.Errors));
            }
            Logger.WriteWarning("15", false);
            return user;
        }

        public string GenerateEmailConfirmationToken(IdentityUser user)
        {
            string token = AsyncHelper.RunSync<string>(() => this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id));

            return token;
        }

        public void SendEmailConfirmation(LG_USER item, IdentityUser user, string callbackUrl)
        {
            string confirmUrl = ConfigurationManager.AppSettings["WebPath"] + "confirmEmail.html?CallbackUrl=" + callbackUrl;
            string url = ConfigurationManager.AppSettings["WebPath"] + "index.html";
            string subject = "Welcome to MGL Portal - Password and Confirmation";
            string body = string.Empty;
            
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/UserConfirmation.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{fullName}}", (item.LG_CONTACT.CntLastName + ", " + item.LG_CONTACT.CntFirstName));
            body = body.Replace("{{callbackUrl}}", confirmUrl);
            body = body.Replace("{{usrPwd}}", item.UsrTempPwd);
            body = body.Replace("{{url}}", url);
            try
            {
                AsyncHelper.RunSync(() => this.AppUserManager.SendEmailAsync(user.Id, subject, body));
            }
            catch(Exception ex)
            {
                Logger.WriteWarning("50" + ex.InnerException, false);

            }

            Logger.WriteWarning("51" + "Success", false);
        }

        public async Task<IdentityResult> ConfirmEmail(string userId, string code)
        {
            var result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            return result;
        }

        public IdentityResult ChangePassword(Dictionary<string, string> param)
        {
            try
            {
                string userid = param["AspNetUserId"];
                string currentPassword = param["CurrentPassword"];
                string newPassword = param["NewPassword"];

                var result = AsyncHelper.RunSync<IdentityResult>(() => this.AppUserManager.ChangePasswordAsync(userid, currentPassword, newPassword));

                if (!result.Succeeded)
                {
                    throw new Exception("Failed: " + string.Join(",", result.Errors));
                }

                var item = (from u in _unitOfWork.CreateSet<LG_USER>()
                            where u.AspNetUserId == userid
                            select u).SingleOrDefault();

                item.UsrTempPwd = null;

                _unitOfWork.SetModified(item);
                _unitOfWork.Commit();

                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public LG_USER CheckEmail(Dictionary<string, string> param)
        {
            try
            {
                string email = param["Email"];

                var user = AsyncHelper.RunSync<IdentityUser>(() => this.AppUserManager.FindByEmailAsync(email));

                if (user != null)
                {
                    var _user = (from u in GetSet()
                                 where u.AspNetUserId == user.Id
                                 select u).SingleOrDefault();

                    return _user;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public LG_USER ResetPassword(Dictionary<string, string> param)
        {
            try
            {
                string password = GenerateRandomPassword(10);
                string email = param["Email"];

                var user = AsyncHelper.RunSync<IdentityUser>(() => this.AppUserManager.FindByEmailAsync(email));

                if (user != null)
                {

                    AsyncHelper.RunSync<IdentityResult>(() => this.AppUserManager.SetLockoutEndDateAsync(user.Id, DateTime.Now.AddDays(-1)));

                    var token = AsyncHelper.RunSync<string>(() => this.AppUserManager.GeneratePasswordResetTokenAsync(user.Id));

                    var result = AsyncHelper.RunSync<IdentityResult>(() => this.AppUserManager.ResetPasswordAsync(user.Id, token, password));

                    if (!result.Succeeded)
                    {
                        throw new Exception("Failed: " + string.Join(",", result.Errors));
                    }
                    AppMGL.DAL.Models.AppMGL ent = new AppMGL.DAL.Models.AppMGL();

                    var _user = ent.LG_USER.Where(s => s.AspNetUserId == user.Id)
                             .FirstOrDefault();
                     _user = (from u in GetSet()
                                 where u.AspNetUserId == user.Id
                                 select u).SingleOrDefault();

                    //var _user = (from u in _unitOfWork.CreateSet<LG_USER>()
                    //             where u.AspNetUserId == user.Id
                    //             select u).SingleOrDefault();

                    _user.UsrTempPwd = password;
                    _user.UsrPwd = null;

                    _unitOfWork.SetModified(_user);
                    _unitOfWork.Commit();

                    string url = ConfigurationManager.AppSettings["WebPath"] + "index.html";
                    string subject = "Unlock / Reset Email";
                    string body = string.Empty;

                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/ResetPassword.html")))
                    {
                        body = reader.ReadToEnd();
                    }

                    body = body.Replace("{{fullName}}", (_user.LG_CONTACT.CntLastName + ", " + _user.LG_CONTACT.CntFirstName));
                    body = body.Replace("{{password}}", password);
                    body = body.Replace("{{url}}", url);

                    AsyncHelper.RunSync(() => this.AppUserManager.SendEmailAsync(user.Id, subject, body));

                    return _user;
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.WriteWarning("reset password error" + ex.StackTrace.ToString(), false);
                throw (ex);
            }
        }

        public IdentityUser GetIdentityUserByEmail(Dictionary<string, string> param)
        {
            try
            {
                string email = param["Email"];

                var user = AsyncHelper.RunSync<IdentityUser>(() => this.AppUserManager.FindByEmailAsync(email));

                return user;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public LG_USER GetUserByAspNetUserId(string aspNetUserId)
        {
            try
            {
                var _user = (from u in GetSet()
                             where u.AspNetUserId == aspNetUserId
                             select u).SingleOrDefault();
                return _user;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public IdentityResult ResetPasswordA(Dictionary<string, string> param)
        {
            try
            {
                string password = GenerateRandomPassword(10);
                string aspNetUserId = param["AspNetUserId"];

                var result = ResetPassword(aspNetUserId, password);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed: " + string.Join(",", result.Errors));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region Private Methods

        private async Task<IdentityResult> CreateUser(IdentityUser user, string password)
        {
            var result = await this.AppUserManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                
                throw new Exception("Failed: " + string.Join(",", result.Errors));
            }
            
            return result;
        }

        private IdentityResult ResetPassword(string aspNetUserid, string password)
        {
            try
            {
                var result = AsyncHelper.RunSync<IdentityResult>(() => this.AppUserManager.RemovePasswordAsync(aspNetUserid));
                if (result.Succeeded)
                {
                    result = AsyncHelper.RunSync<IdentityResult>(() => this.AppUserManager.AddPasswordAsync(aspNetUserid, password));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private IdentityResult DeleteAspNetUser(string aspNetUserid)
        {
            try
            {
                var user = this.AppUserManager.FindById(aspNetUserid);
                var result = AsyncHelper.RunSync<IdentityResult>(() => this.AppUserManager.DeleteAsync(user));
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private string GenerateRandomPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        #endregion
    }
}
