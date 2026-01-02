using AppMGL.DAL.Contract;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppMGL.DAL.Models
{
    public partial class AppMGL : IdentityDbContext<IdentityUser>, IQueryableUnitOfWork, IUnitOfWork, IDisposable, ISql
    {
        #region Constructor
        public virtual DbSet<AspNetRefreshToken> AspNetRefreshToken
        {
            get;
            set;
        }

        public virtual DbSet<AspNetClient> AspNetClient
        {
            get;
            set;
        }

        public virtual DbSet<LG_ACTION> LG_ACTION
        {
            get;
            set;
        }

        public virtual DbSet<LG_CONTACT> LG_CONTACT
        {
            get;
            set;
        }

        public virtual DbSet<LG_COUNTRY> LG_COUNTRY
        {
            get;
            set;
        }

        public virtual DbSet<LG_DEPARTMENT> LG_DEPARTMENT
        {
            get;
            set;
        }

        public virtual DbSet<LG_LOCATION> LG_LOCATION
        {
            get;
            set;
        }

        public virtual DbSet<LG_MAM_ROL_MAP> LG_MAM_ROL_MAP
        {
            get;
            set;
        }

        public virtual DbSet<LG_MODULE> LG_MODULE
        {
            get;
            set;
        }

        public virtual DbSet<LG_MODULE_TYPE> LG_MODULE_TYPE
        {
            get;
            set;
        }

        public virtual DbSet<LG_ROLE> LG_ROLE
        {
            get;
            set;
        }

        public virtual DbSet<LG_SITE> LG_SITE
        {
            get;
            set;
        }

        public virtual DbSet<LG_SITE_CONTACT_ROLE> LG_SITE_CONTACT_ROLE
        {
            get;
            set;
        }

        public virtual DbSet<LG_TIMEZONE> LG_TIMEZONE
        {
            get;
            set;
        }

        public virtual DbSet<LG_TITLE> LG_TITLE
        {
            get;
            set;
        }

        public virtual DbSet<LG_USER> LG_USER
        {
            get;
            set;
        }

        public virtual DbSet<LG_USSTATE> LG_USSTATE
        {
            get;
            set;
        }

        public virtual DbSet<DashboardData_Job> DashboardData_Job
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_Commodity> SIPL_Commodity
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_ContainerType> SIPL_ContainerType
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_LoadType> SIPL_LoadType
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_City> SIPL_City
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_ContactBranch> SIPL_ContactBranch
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_Continent> SIPL_Continent
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_Country> SIPL_Country
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_State> SIPL_State
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_TradeService> SIPL_TradeService
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_CommodityType> SIPL_CommodityType
        {
            get;
            set;
        }

        public virtual DbSet<LG_VW_CONTACTCATEGORY> LG_VW_CONTACTCATEGORY
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_Department> SIPL_Department
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_Contract> SIPL_Contract
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_Contact> SIPL_Contact
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_CompanyGradation> SIPL_CompanyGradation
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_Port> SIPL_Port
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_User> SIPL_User
        {
            get;
            set;
        }

        public virtual DbSet<LG_VW_Commodity> LG_VW_Commodity
        {
            get;
            set;
        }

        public virtual DbSet<LG_VW_Port> LG_VW_Port
        {
            get;
            set;
        }

        public virtual DbSet<LG_VW_SIPLCity> LG_VW_SIPLCity
        {
            get;
            set;
        }

        public virtual DbSet<LG_VW_SIPLState> LG_VW_SIPLState
        {
            get;
            set;
        }

        public virtual DbSet<LGV_CONTACT_INFO> LGV_CONTACT_INFO
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_RailRamp> SIPL_RailRamp
        {
            get;
            set;
        }

        public virtual DbSet<Sipl_Terminal> Sipl_Terminal
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_PortGroup> SIPL_PortGroup
        {
            get;
            set;
        }

        public virtual DbSet<LG_VW_Alias> LG_VW_Alias
        {
            get;
            set;
        }

        public virtual DbSet<LG_VW_SurchargeGroup> LG_VW_SurchargeGroup
        {
            get;
            set;
        }

        public virtual DbSet<LG_CONTACT_WORK_TYPE> LG_CONTACT_WORK_TYPE
        {
            get;
            set;
        }

        public virtual DbSet<LG_LOGIN_HISTORY> LG_LOGIN_HISTORY
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_BookingStatus> SIPL_BookingStatus
        {
            get;
            set;
        }

        public virtual DbSet<SIPL_DocumentCommon> SIPL_DocumentCommon
        {
            get;
            set;
        }

        public virtual DbSet<LG_ACCT_CATEGORY> LG_ACCT_CATEGORY
        {
            get;
            set;
        }

        public virtual DbSet<LG_SP_FEE_CATEGORY> LG_SP_FEE_CATEGORY
        {
            get;
            set;
        }

        public virtual DbSet<LG_VW_SITE_CONTACT> LG_VW_SITE_CONTACT
        {
            get;
            set;
        }

        public virtual DbSet<LG_SHIPMENT_DOCS> LG_SHIPMENT_DOCS
        {
            get;
            set;
        }

        public virtual DbSet<LG_VW_Contract> LG_VW_Contract
        {
            get;
            set;
        }

        public virtual DbSet<LG_VW_SIPL_CONTACT> LG_VW_SIPL_CONTACT
        {
            get;
            set;
        }

        public virtual DbSet<tbl_scheduler_TempContractRateDetail> tbl_scheduler_TempContractRateDetail
        {
            get;
            set;
        }

        public virtual DbSet<LG_VW_DisplayRate> LG_VW_DisplayRate
        {
            get;
            set;
        }


        public AppMGL()
            : base("name=AppMGL")
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
           /// for time out issue by vikas  
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 0;
        }

        public static AppMGL Create()
        {
            return new AppMGL();
           
        }

        #endregion Constructor

        #region IQueryableUnitOfWork Members

        public DbSet<T> CreateSet<T>()
            where T : class
        {
            return Set<T>();
        }

        public void Attach<T>(T item)
            where T : class
        {
            //attach and set as unchanged
            Entry(item).State = EntityState.Unchanged;
        }

        public void SetModified<T>(T item)
            where T : class
        {
            //this operation also attach item in object state manager
            Entry(item).State = EntityState.Modified;
        }

        public void ApplyCurrentValues<T>(T original, T current)
            where T : class
        {
            //if it is not attached, attach original and set current values
            Entry(original).CurrentValues.SetValues(current);
        }

        public void Commit()
        {
            SaveChanges();
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed;

            do
            {
                try
                {
                    SaveChanges();

                    saveFailed = false;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                              .ForEach(entry => entry.OriginalValues.SetValues(entry.GetDatabaseValues()));

                }
            } while (saveFailed);
        }

        public void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            ChangeTracker.Entries()
                .ToList()
                .ForEach(entry => entry.State = EntityState.Unchanged);
        }

        public DbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        public IEnumerable<T> ExecuteQuery<T>(string sqlQuery, params object[] parameters)
        {
            return Database.SqlQuery<T>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public virtual ObjectResult<USP_GET_CUSTOMERCONTACT_DETAIL_Result> USP_GET_CUSTOMERCONTACT_DETAIL(long? cONTACTID)
        {
            ObjectParameter objectParameter = cONTACTID.HasValue ? new ObjectParameter("CONTACTID", cONTACTID) : new ObjectParameter("CONTACTID", typeof(long));
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_GET_CUSTOMERCONTACT_DETAIL_Result>("USP_GET_CUSTOMERCONTACT_DETAIL", new ObjectParameter[1]
			{
				objectParameter
			});
        }

        public virtual ObjectResult<USP_GET_CUSTOMERCONTACT_LIST_Result> USP_GET_CUSTOMERCONTACT_LIST(int? pAGENO, int? pAGESIZE, string companyName, string customerCode, string miamiRep, string category, string companyGradation, string originCountries, string commodity, string region, string country, string state, string city, bool? isVendor, string sIT_ID, string sORTCOLUMN, string sORTORDER)
        {
            ObjectParameter objectParameter = pAGENO.HasValue ? new ObjectParameter("PAGENO", pAGENO) : new ObjectParameter("PAGENO", typeof(int));
            ObjectParameter objectParameter2 = pAGESIZE.HasValue ? new ObjectParameter("PAGESIZE", pAGESIZE) : new ObjectParameter("PAGESIZE", typeof(int));
            ObjectParameter objectParameter3 = (companyName != null) ? new ObjectParameter("CompanyName", companyName) : new ObjectParameter("CompanyName", typeof(string));
            ObjectParameter objectParameter4 = (customerCode != null) ? new ObjectParameter("CustomerCode", customerCode) : new ObjectParameter("CustomerCode", typeof(string));
            ObjectParameter objectParameter5 = (miamiRep != null) ? new ObjectParameter("MiamiRep", miamiRep) : new ObjectParameter("MiamiRep", typeof(string));
            ObjectParameter objectParameter6 = (category != null) ? new ObjectParameter("Category", category) : new ObjectParameter("Category", typeof(string));
            ObjectParameter objectParameter7 = (companyGradation != null) ? new ObjectParameter("CompanyGradation", companyGradation) : new ObjectParameter("CompanyGradation", typeof(string));
            ObjectParameter objectParameter8 = (originCountries != null) ? new ObjectParameter("OriginCountries", originCountries) : new ObjectParameter("OriginCountries", typeof(string));
            ObjectParameter objectParameter9 = (commodity != null) ? new ObjectParameter("Commodity", commodity) : new ObjectParameter("Commodity", typeof(string));
            ObjectParameter objectParameter10 = (region != null) ? new ObjectParameter("Region", region) : new ObjectParameter("Region", typeof(string));
            ObjectParameter objectParameter11 = (country != null) ? new ObjectParameter("Country", country) : new ObjectParameter("Country", typeof(string));
            ObjectParameter objectParameter12 = (state != null) ? new ObjectParameter("State", state) : new ObjectParameter("State", typeof(string));
            ObjectParameter objectParameter13 = (city != null) ? new ObjectParameter("City", city) : new ObjectParameter("City", typeof(string));
            ObjectParameter objectParameter14 = isVendor.HasValue ? new ObjectParameter("IsVendor", isVendor) : new ObjectParameter("IsVendor", typeof(bool));
            ObjectParameter objectParameter15 = (sIT_ID != null) ? new ObjectParameter("SIT_ID", sIT_ID) : new ObjectParameter("SIT_ID", typeof(string));
            ObjectParameter objectParameter16 = (sORTCOLUMN != null) ? new ObjectParameter("SORTCOLUMN", sORTCOLUMN) : new ObjectParameter("SORTCOLUMN", typeof(string));
            ObjectParameter objectParameter17 = (sORTORDER != null) ? new ObjectParameter("SORTORDER", sORTORDER) : new ObjectParameter("SORTORDER", typeof(string));
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_GET_CUSTOMERCONTACT_LIST_Result>("USP_GET_CUSTOMERCONTACT_LIST", new ObjectParameter[17]
			{
				objectParameter,
				objectParameter2,
				objectParameter3,
				objectParameter4,
				objectParameter5,
				objectParameter6,
				objectParameter7,
				objectParameter8,
				objectParameter9,
				objectParameter10,
				objectParameter11,
				objectParameter12,
				objectParameter13,
				objectParameter14,
				objectParameter15,
				objectParameter16,
				objectParameter17
			});
        }

        public virtual ObjectResult<USP_GET_CUSTOMERCONTACT_DATA_Result> USP_GET_CUSTOMERCONTACT_DATA(string sEARCH, string cOLNAME)
        {
            ObjectParameter objectParameter = (sEARCH != null) ? new ObjectParameter("SEARCH", sEARCH) : new ObjectParameter("SEARCH", typeof(string));
            ObjectParameter objectParameter2 = (cOLNAME != null) ? new ObjectParameter("COLNAME", cOLNAME) : new ObjectParameter("COLNAME", typeof(string));
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_GET_CUSTOMERCONTACT_DATA_Result>("USP_GET_CUSTOMERCONTACT_DATA", new ObjectParameter[2]
			{
				objectParameter,
				objectParameter2
			});
        }

        public virtual int USP_UPDATE_CUSTOMERCONTACT(int? contactID, string companyName, string telNo, string address, string contactPerson, string email, string accountDetail, string webSite, string zipCode, string mobNo, string fax, string taxID, string accountNo, string customerCode, bool? isDeleted, string remarks, string attachment, string companyGradation, long? representativeID, string contactCategoryID, string continent, string state, string countryName, string city, string galRepresentative, string branchName)
        {
            ObjectParameter objectParameter = contactID.HasValue ? new ObjectParameter("ContactID", contactID) : new ObjectParameter("ContactID", typeof(int));
            ObjectParameter objectParameter2 = (companyName != null) ? new ObjectParameter("CompanyName", companyName) : new ObjectParameter("CompanyName", typeof(string));
            ObjectParameter objectParameter3 = (telNo != null) ? new ObjectParameter("TelNo", telNo) : new ObjectParameter("TelNo", typeof(string));
            ObjectParameter objectParameter4 = (address != null) ? new ObjectParameter("Address", address) : new ObjectParameter("Address", typeof(string));
            ObjectParameter objectParameter5 = (contactPerson != null) ? new ObjectParameter("ContactPerson", contactPerson) : new ObjectParameter("ContactPerson", typeof(string));
            ObjectParameter objectParameter6 = (email != null) ? new ObjectParameter("Email", email) : new ObjectParameter("Email", typeof(string));
            ObjectParameter objectParameter7 = (accountDetail != null) ? new ObjectParameter("AccountDetail", accountDetail) : new ObjectParameter("AccountDetail", typeof(string));
            ObjectParameter objectParameter8 = (webSite != null) ? new ObjectParameter("WebSite", webSite) : new ObjectParameter("WebSite", typeof(string));
            ObjectParameter objectParameter9 = (zipCode != null) ? new ObjectParameter("ZipCode", zipCode) : new ObjectParameter("ZipCode", typeof(string));
            ObjectParameter objectParameter10 = (mobNo != null) ? new ObjectParameter("MobNo", mobNo) : new ObjectParameter("MobNo", typeof(string));
            ObjectParameter objectParameter11 = (fax != null) ? new ObjectParameter("Fax", fax) : new ObjectParameter("Fax", typeof(string));
            ObjectParameter objectParameter12 = (taxID != null) ? new ObjectParameter("TaxID", taxID) : new ObjectParameter("TaxID", typeof(string));
            ObjectParameter objectParameter13 = (accountNo != null) ? new ObjectParameter("AccountNo", accountNo) : new ObjectParameter("AccountNo", typeof(string));
            ObjectParameter objectParameter14 = (customerCode != null) ? new ObjectParameter("CustomerCode", customerCode) : new ObjectParameter("CustomerCode", typeof(string));
            ObjectParameter objectParameter15 = isDeleted.HasValue ? new ObjectParameter("IsDeleted", isDeleted) : new ObjectParameter("IsDeleted", typeof(bool));
            ObjectParameter objectParameter16 = (remarks != null) ? new ObjectParameter("Remarks", remarks) : new ObjectParameter("Remarks", typeof(string));
            ObjectParameter objectParameter17 = (attachment != null) ? new ObjectParameter("Attachment", attachment) : new ObjectParameter("Attachment", typeof(string));
            ObjectParameter objectParameter18 = (companyGradation != null) ? new ObjectParameter("CompanyGradation", companyGradation) : new ObjectParameter("CompanyGradation", typeof(string));
            ObjectParameter objectParameter19 = representativeID.HasValue ? new ObjectParameter("RepresentativeID", representativeID) : new ObjectParameter("RepresentativeID", typeof(long));
            ObjectParameter objectParameter20 = (contactCategoryID != null) ? new ObjectParameter("ContactCategoryID", contactCategoryID) : new ObjectParameter("ContactCategoryID", typeof(string));
            ObjectParameter objectParameter21 = (continent != null) ? new ObjectParameter("Continent", continent) : new ObjectParameter("Continent", typeof(string));
            ObjectParameter objectParameter22 = (state != null) ? new ObjectParameter("State", state) : new ObjectParameter("State", typeof(string));
            ObjectParameter objectParameter23 = (countryName != null) ? new ObjectParameter("CountryName", countryName) : new ObjectParameter("CountryName", typeof(string));
            ObjectParameter objectParameter24 = (city != null) ? new ObjectParameter("City", city) : new ObjectParameter("City", typeof(string));
            ObjectParameter objectParameter25 = (galRepresentative != null) ? new ObjectParameter("GalRepresentative", galRepresentative) : new ObjectParameter("GalRepresentative", typeof(string));
            ObjectParameter objectParameter26 = (branchName != null) ? new ObjectParameter("BranchName", branchName) : new ObjectParameter("BranchName", typeof(string));
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("USP_UPDATE_CUSTOMERCONTACT", objectParameter, objectParameter2, objectParameter3, objectParameter4, objectParameter5, objectParameter6, objectParameter7, objectParameter8, objectParameter9, objectParameter10, objectParameter11, objectParameter12, objectParameter13, objectParameter14, objectParameter15, objectParameter16, objectParameter17, objectParameter18, objectParameter19, objectParameter20, objectParameter21, objectParameter22, objectParameter23, objectParameter24, objectParameter25, objectParameter26);
        }


        
        #endregion
    }

    public static class ContextExtensions
    {
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            return objectContext.GetTableName<T>();
        }

        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex(@"FROM\s+(?<table>.+)\s+AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }
    }
}
