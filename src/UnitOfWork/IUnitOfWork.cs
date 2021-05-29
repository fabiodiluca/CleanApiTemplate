using System;

namespace CleanTemplate.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        object Session { get; }
        void BeginTransaction();
        /// <summary>
        /// Throws CommitException
        /// </summary>
        void Commit();
        /// <summary>
        /// Throws CommitException
        /// </summary>
        /// <param name="startNewSession"></param>
        /// <param name="startNewTransaction"></param>
        void Commit(bool startNewSession, bool startNewTransaction);
        void RollbackTransaction();
        void NewSession();
    }
}
