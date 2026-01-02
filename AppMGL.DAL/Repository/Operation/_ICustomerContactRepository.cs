using AppMGL.DAL.Models;

namespace AppMGL.DAL.Modules.Operation
{
    public interface ICustomerContactRepository : IRepository<AppMGL.DAL.Models.USP_GET_CUSTOMERCONTACT_LIST_Result >
    {
    }

    public interface ICustomerConcatDetailRepository : IRepository<AppMGL.DAL.Models.USP_GET_CUSTOMERCONTACT_DETAIL_Result>
    {

    }
}
