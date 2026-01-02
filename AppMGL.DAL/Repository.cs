using AppMGL.DAL.Contract;
using AppMGL.DAL.Helper;
using AppMGL.DAL.UDT;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace AppMGL.DAL
{
	public class Repository<T> : IRepository<T>, IDisposable where T : class
	{
		protected readonly IQueryableUnitOfWork _unitOfWork;

		protected BaseQuery Query;

        public IUnitOfWork UnitOfWork { get { return _unitOfWork; } }

		public Repository(IQueryableUnitOfWork unitOfWork)
		{
			if (unitOfWork == null)
			{
				throw new ArgumentNullException("unitOfWork");
			}
			_unitOfWork = unitOfWork;
		}

		public virtual IQueryable<T> GetAll()
		{
			return GetSet();
		}

		public virtual IEnumerable<T> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<T, KProperty>> orderByExpression, bool ascending)
		{
			IDbSet<T> set = GetSet();
			if (ascending)
			{
				return set.OrderBy(orderByExpression).Skip(pageCount * pageIndex).Take(pageCount);
			}
			return set.OrderByDescending(orderByExpression).Skip(pageCount * pageIndex).Take(pageCount);
		}

		public virtual IEnumerable<T> GetFiltered(Expression<Func<T, bool>> filter)
		{
			return GetSet().Where(filter);
		}

		public virtual void Merge(T persisted, T current)
		{
			_unitOfWork.ApplyCurrentValues(persisted, current);
		}

		public virtual IQueryable<T> List(ListParams listParams, out int count)
		{
			int count2 = (listParams.PageIndex - 1) * listParams.PageSize;
			IQueryable<T> source = GetSet().Where(Utility.GetWhere(listParams.Filter));
			count = source.Count();
			return source.OrderBy(Utility.GetSort(listParams.Sort)).Skip(count2).Take(listParams.PageSize);
		}

		public virtual T Detail(long id)
		{
			if (id > 0)
			{
				return GetSet().Find(id);
			}
			return null;
		}

		public virtual T Insert(T item)
		{
			if (item != null)
			{
				return GetSet().Add(item);
			}
			return null;
		}

		public virtual T Update(T item)
		{
			if (item != null)
			{
				_unitOfWork.SetModified(item);
			}
			return null;
		}

		public virtual T Delete(T item)
		{
			if (item != null)
			{
				_unitOfWork.Attach(item);
				GetSet().Remove(item);
			}
			return null;
		}

		public virtual IEnumerable<T> ExecuteQuery(string sqlQuery, params object[] parameters)
		{
			return _unitOfWork.ExecuteQuery<T>(sqlQuery, parameters);
		}

		public virtual IEnumerable<TD> ExecuteQuery<TD>(string sqlQuery, params object[] parameters)
		{
			return _unitOfWork.ExecuteQuery<TD>(sqlQuery, parameters);
		}

		public virtual int ExecuteCommand(string sqlCommand, params object[] parameters)
		{
			return _unitOfWork.ExecuteCommand(sqlCommand, parameters);
		}

		public void Dispose()
		{
			if (_unitOfWork != null)
			{
				_unitOfWork.Dispose();
			}
		}

		protected IDbSet<T> GetSet()
		{
			return _unitOfWork.CreateSet<T>();
		}

		public virtual DbContextTransaction BeginTransaction()
		{
			return _unitOfWork.BeginTransaction();
		}
	}
}
