using AppMGL.DAL.UDT;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Operation;

namespace AppMGL.DAL.Repository.Operation
{
    public class ContactBranchRepository : Repository<SIPL_ContactBranch>, IContactBranchRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="context">Associated context</param>
        public ContactBranchRepository(Models.AppMGL context)
            : base(context)
        {
            Query = new ContactBranchQuery();
        }

        #endregion
    }


    public class StateRepository : Repository<SIPL_State>, IStateRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="context">Associated context</param>
        public StateRepository(Models.AppMGL context)
            : base(context)
        {
            Query = new StateRQuery();
        }

        public override SIPL_State Update(SIPL_State item)
        {
            if (item != null)
            {
                _unitOfWork.SetModified(item);
                _unitOfWork.SetModified(item.SIPL_Country);
            }
            return null;
        }

        #endregion
    }


    public class UserRepository : Repository<SIPL_User>, IUserRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="context">Associated context</param>
        public UserRepository(Models.AppMGL context)
            : base(context)
        {
            Query = new UserRQuery();
        }

        #endregion
    }

    public class CityRepository : Repository<SIPL_City>, ICityRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="context">Associated context</param>
        public CityRepository(Models.AppMGL context)
            : base(context)
        {
            Query = new CityRQuery();
        }
        #endregion
    }

    public class CountryRepository : Repository<SIPL_Country>, ICountryRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="context">Associated context</param>
        public CountryRepository(Models.AppMGL context)
            : base(context)
        {
            Query = new CountryRQuery();
        }

        public override SIPL_Country Update(SIPL_Country item)
        {
            if (item != null)
            {
                _unitOfWork.SetModified(item);
                _unitOfWork.SetModified(item.SIPL_Continent);
            }
            return null;
        }

        #endregion
    }

    public class ContinentRepository : Repository<SIPL_Continent>, IContinentRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="context">Associated context</param>
        public ContinentRepository(Models.AppMGL context)
            : base(context)
        {
            Query = new ContinentRQuery();
        }

        #endregion
    }

   

}
