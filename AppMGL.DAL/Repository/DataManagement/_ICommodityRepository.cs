using AppMGL.DAL.Models;
using System;

namespace AppMGL.DAL.Modules.DataManagement
{
	public interface ICommodityRepository : IRepository<SIPL_Commodity>, IDisposable
	{
	}
}
