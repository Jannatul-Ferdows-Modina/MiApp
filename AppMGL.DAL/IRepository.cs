using AppMGL.DAL.Helper;
using System;
using System.Linq;

namespace AppMGL.DAL
{
	public interface IRepository<T> : IDisposable where T : class
	{
		IUnitOfWork UnitOfWork
		{
			get;
		}

		IQueryable<T> GetAll();

		IQueryable<T> List(ListParams listParams, out int count);

		T Detail(long id);

		T Insert(T item);

		T Update(T item);

		T Delete(T item);
	}
}
