using System;

namespace CleanTemplate.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        object Session { get; }
        void BeginTransaction();
        void Commit();
        void Commit(bool startNewSession, bool startNewTransaction);
        void RollbackTransaction();
        void NewSession();
    }
}
