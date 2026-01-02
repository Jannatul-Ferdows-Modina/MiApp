using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AppMGL.DAL;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Helper.Logging;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Security;
using AppMGL.Manager.Infrastructure.Results;
using AutoMapper;

namespace AppMGL.Manager.Infrastructure
{
    public abstract class BaseController<T, TR, TE> : ApiController
        where T : class
        where TR : Repository<TE>
        where TE : class
    {
        #region Properties

        protected TR _context;
        protected EnumModule BaseModule { get; set; }
        protected string KeyField { get; set; }
        protected string RequestUrl
        {
            get
            {
                return "http://" + HttpContext.Current.Request.UrlReferrer.Host + HttpContext.Current.Request.UrlReferrer.LocalPath;
            }
        }

        #endregion

        #region AppResult Methods

        protected StandardJsonResult JsonValidationError()
        {
            var result = new StandardJsonResult { ResultId = (int)EnumResult.ValidationFailed };
            foreach (var validationError in ModelState.Values.SelectMany(v => v.Errors))
            {
                result.AddMessage(validationError.ErrorMessage, false);
            }
            return result;
        }

        protected StandardJsonResult AppResult<TD>(List<TD> entity, int count)
        {
            return AppResult(entity, count, "", EnumResult.Success);
        }

        protected StandardJsonResult AppResult<TD>(TD entity, string message)
        {
            return AppResult(entity, 1, message, EnumResult.Success);
        }

        protected StandardJsonResult AppResult(List<T> entity, int count)
        {
            return AppResult(entity, count, "", EnumResult.Success);
        }

        protected StandardJsonResult AppResult(T entity, string message)
        {
            return AppResult(entity, 1, message, EnumResult.Success);
        }

        protected StandardJsonResult AppResult(Exception ex)
        {
            Logger.WriteError(ex);
            InsertErrorLog(ex);
            if (ex is UnauthorizedAccessException)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            return AppResult(null, 0, ex.Message, EnumResult.Failed);
        }

        protected StandardJsonResult AppResult(OperationResult result)
        {
            return AppResult(result.Data, result.Count, result.Message, result.SqlException);
        }

        protected StandardJsonResult AppResult(OperationResult result, T entity)
        {
            return AppResult(entity, result.Count, result.Message, result.SqlException);
        }

        protected StandardJsonResult AppResult(object data, long count, string message, EnumResult resultId)
        {
            var result = new StandardJsonResult { ResultId = (int)resultId };
            result.AddMessage(message, false);
            result.Data = data;
            result.Count = count;
            return result;
        }

        #endregion

        #region Other Methods

        protected object GetPropertyInfoValue(object entity, PropertyInfo propertyInfo)
        {
            var propertyValue = propertyInfo.GetValue(entity);
            if (propertyInfo.Name == "LastAccess" && Convert.ToDateTime(propertyValue) < new DateTime(1753, 1, 1))
            {
                return DateTime.Now;
            }
            return propertyValue ?? DBNull.Value;
        }

        protected List<SqlParameter> PrepareParameters(ListParams listParams)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("LoginId", GetUserId()),
                new SqlParameter("PageIndex", listParams.PageIndex),
                new SqlParameter("PageSize", listParams.PageSize),
                new SqlParameter("Sort", Utility.GetSort(listParams.Sort)),
                new SqlParameter("Filter", Utility.GetFilter(listParams.Filter)),
                new SqlParameter("Count", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("ResultId", SqlDbType.Int) {Direction = ParameterDirection.Output}
            };

            return parameters;
        }

        protected List<SqlParameter> PrepareParameters(T entity)
        {
            return PrepareParameters<T>(entity);
        }

        protected List<SqlParameter> PrepareParameters<TEntity>(object entity)
        {
            return PrepareParameters<TEntity>(entity, GetUserId());
        }

        protected List<SqlParameter> PrepareParameters<TEntity>(object entity, long userId)
        {
            var cacheKeyName = entity.GetType().Name;
            var propertyInfos = ParameterCache.GetCachedItem(cacheKeyName);

            if (propertyInfos == null)
            {
                propertyInfos = ((TEntity)entity).GetType().GetProperties();
                ParameterCache.AddToCache(cacheKeyName, propertyInfos, CacheItemPriority.Default);
            }

            var sqlParams = (from f in propertyInfos.Where(x => !x.PropertyType.IsArray && !x.PropertyType.Namespace.Contains("AppMGL"))
                             select new SqlParameter(f.Name, GetPropertyInfoValue(entity, f))).ToList();

            sqlParams.Add(new SqlParameter("LoginId", userId));
            sqlParams.Add(new SqlParameter("ScopeId", SqlDbType.BigInt) { Direction = ParameterDirection.Output });
            sqlParams.Add(new SqlParameter("ResultId", SqlDbType.BigInt) { Direction = ParameterDirection.Output });

            return sqlParams;
        }

        protected List<SqlParameter> PrepareParameters(long parameterValue)
        {
            return PrepareParameters(KeyField, parameterValue);
        }

        protected List<SqlParameter> PrepareParameters(string parameterKey, long parameterValue)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("LoginId", GetUserId()),
                new SqlParameter(parameterKey, parameterValue),
                new SqlParameter("ResultId", SqlDbType.BigInt) { Direction = ParameterDirection.Output }
            };

            return parameters;
        }

        protected long GetUserId()
        {
            long userId = 0;
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            if (principal.Identity.IsAuthenticated)
            {
                long.TryParse(principal.Claims.Where(c => c.Type == "UserId").Single().Value, out userId);
            }
            else
            {
                throw new UnauthorizedAccessException("Session Timeout, please re-login again.");
            }
            return userId;
        }

        protected string GetUserDisplayName()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return principal.Claims.Where(c => c.Type == "DisplayName").Single().Value;
        }

        protected string PrepareMessage(EnumAction enumAction)
        {
            if (enumAction == EnumAction.Insert)
            {
                return string.Format("{0} has been added successfully.", BaseModule.ToString());
            }
            else if (enumAction == EnumAction.Update)
            {
                return string.Format("{0} has been updated successfully.", BaseModule.ToString());
            }
            else if (enumAction == EnumAction.Delete)
            {
                return string.Format("{0} has been deleted successfully.", BaseModule.ToString());
            }
            return "Unhandled Success Message";
        }

        protected string PrepareMessage(EnumResult enumResult, EnumAction enumAction)
        {
            return "Unknown Error";
        }

        protected string PrepareMessage(EnumResult enumResult)
        {
            if (enumResult == EnumResult.AnotherUserModifiedRecord)
            {
                return "Another user modified the record.";
            }
            else if (enumResult == EnumResult.InvalidGrant)
            {
                return "The user name or password is incorrect.";
            }
            else if (enumResult == EnumResult.UnauthorizedAccess)
            {
                return "You are unauthorized to access this data.";
            }
            else if (enumResult == EnumResult.UserDeactivated)
            {
                return "Your Account has been Deactivated, contact administrator.";
            }
            return "Unknown Error";
        }
        
        protected void InsertErrorLog(Exception ex)
        {
            try
            {
                var errorLog = new ErrorLogDTO()
                {
                    ErrorTypeId = 2001,
                    Source = ex.Source,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = (ex.InnerException != null) ? string.Format("SOURCE: {0} \n MESSAGE: {1} \n STACK TRACE\n {2}", ex.InnerException.Source, ex.InnerException.Message, ex.InnerException.StackTrace) : null,
                    ModuleId = Convert.ToInt64(BaseModule)
                };
                long userId = 1;
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                if (principal.Identity.IsAuthenticated)
                {
                    long.TryParse(principal.Claims.Where(c => c.Type == "UserId").Single().Value, out userId);
                }
                var parameters = PrepareParameters<ErrorLogDTO>(errorLog, userId);
                var result = _context.ExecuteCommand((new ErrorLogQuery()).Insert, parameters.ToArray());
            }
            catch (Exception iex)
            {
                Logger.WriteError(iex);
            }
        }

        protected long GetPrimaryKeyValue(TE entity)
        {
            foreach (var property in typeof(TE).GetProperties())
            {
                foreach (var attribute in property.GetCustomAttributes(true))
                {
                    if (attribute is KeyAttribute)
                    {
                        var propertyValue = typeof(TE).GetProperty(property.Name).GetValue(entity, null);
                        return Convert.ToInt64(propertyValue);
                    }
                }
            }
            return 0;
        }

        protected string GetUnescapeData(MultipartFormDataStreamProvider result, string key)
        {
            return Uri.UnescapeDataString(result.FormData.GetValues(key).FirstOrDefault() ?? string.Empty);
        }
        
        #endregion

        #region Non-Transaction Methods

        [System.Web.Http.HttpPost]
        public virtual IHttpActionResult ValidateAccessToken()
        {
            try
            {
                Logger.WriteWarning("Validate access token" ,false);
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                if (principal.Identity.IsAuthenticated)
                {
                    Logger.WriteWarning("Validate access token OK", false);
                    return Ok();
                }
                else
                {
                    Logger.WriteWarning("Validate access token OK No", false);
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                InsertErrorLog(ex);
                return InternalServerError();
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult List(ListParams listParams)
        {
            try
            {
                int count;
                var result = _context.List(listParams, out count).ToList();
                var dtoResult = Mapper.Map<List<T>>(result);
                return AppResult(dtoResult, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult Detail(long id)
        {
            try
            {
                var result = _context.Detail(id);
                if (typeof(T).Name.ToString() == "RoleDTO") { 
                    var dtoResult = Mapper.Map<T>(result);
                    return AppResult(dtoResult, "");
                }
                else
                {
                    return AppResult(result, "");
                }
                
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult Lookup(ListParams listParams)
       {
            try
            {
                int count;
                var result = _context.List(listParams, out count).ToList();
                if (typeof(T).Name.ToString() == "ContactCategoryDTO")
                {
                   var i = result.ToList();
                   return AppResult(i, count);                  
                }
                else
                {

                    var dtoResult = Mapper.Map<List<T>>(result);
                    return AppResult(dtoResult, count);
                }
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        #endregion

        #region Transaction Methods

        [System.Web.Http.HttpPost]
        public virtual ActionResult Insert(T dto)
        {
            try
            {
                var entity = Mapper.Map<TE>(dto);
                _context.Insert(entity);
                _context.UnitOfWork.Commit();
                var result = _context.Detail(GetPrimaryKeyValue(entity));
                var dtoResult = Mapper.Map<T>(result);
                return AppResult(dtoResult, PrepareMessage(EnumAction.Insert));
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPut]
        public virtual ActionResult Update(long id, T dto)
        {
            try
            {
                var entity = Mapper.Map<TE>(dto);
                _context.Update(entity);
                _context.UnitOfWork.Commit();
                var result = _context.Detail(GetPrimaryKeyValue(entity));
                var dtoResult = Mapper.Map<T>(result);
                return AppResult(dtoResult, PrepareMessage(EnumAction.Update));
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult Delete(T dto)
        {
            try
            {
                var entity = Mapper.Map<TE>(dto);
                var result = _context.Delete(entity);
                _context.UnitOfWork.Commit();
                var dtoResult = Mapper.Map<T>(result);
                return AppResult(dtoResult, PrepareMessage(EnumAction.Delete));
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}