using System;
using System.Data.Entity;

namespace AppMGL.DAL
{
	public interface IUnitOfWork : IDisposable
	{
		void Commit();

		void CommitAndRefreshChanges();

		void RollbackChanges();

		DbContextTransaction BeginTransaction();
	}
}
