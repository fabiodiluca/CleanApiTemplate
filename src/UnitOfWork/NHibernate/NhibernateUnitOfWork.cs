using NHibernate;
using System;
using CleanTemplate.UnitOfWork.Exceptions;

namespace CleanTemplate.UnitOfWork.NHibernateImplementation
{
    public class NhibernateUnitOfWork : IUnitOfWork
    {
        public ISessionFactory SessionFactory { get; }
        private ITransaction _transaction;

        public NhibernateUnitOfWork(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;
        }

        protected ISession _Session { get; set; }
        public object Session { 
            get 
            {
                _OpenSessionIfNecessary();
                return _Session; 
            } 
        }

        private void _OpenSessionIfNecessary()
        {
            if (_Session == null)
                _Session = SessionFactory.OpenSession();
        }

        public void BeginTransaction()
        {
            _OpenSessionIfNecessary();
            _transaction = _Session.BeginTransaction();
        }

        /// <summary>
        /// Throws CommitException
        /// </summary>
        public void Commit()
        {
            Commit(false, false);
        }

        /// <summary>
        /// Throws CommitException
        /// </summary>
        /// <param name="recreateSession"></param>
        /// <param name="startNewTransaction"></param>
        public void Commit(bool recreateSession, bool startNewTransaction)
        {
            try
            {
                if (_transaction.IsActive && !_transaction.WasRolledBack)
                {
                    _transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                if (_Session.GetCurrentTransaction().IsActive)
                    _Session.GetCurrentTransaction().Rollback();

                throw new CommitException("Commit exception.", ex);
            }

            if (recreateSession)
                this.NewSession(startNewTransaction);
            else
            {
                if (startNewTransaction)
                {
                    try
                    {
                        _Session.Close();
                    }
                    catch { }
                    try
                    {
                        _Session.Dispose();
                    }
                    catch { }
                    _Session = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction.IsActive)
                _transaction.Rollback();
        }

        public void Dispose()
        {
            _Session.Dispose();
            GC.SuppressFinalize(this);
        }

        public void NewSession()
        {
            NewSession(false);
        }

        public void NewSession(bool startNewTransaction)
        {
            try
            {
                _Session.Dispose();
            }
            catch { }
            try
            {
                _Session.Clear();
            }
            catch { }
            _Session = SessionFactory.OpenSession();
            if (startNewTransaction)
                _transaction = _Session.BeginTransaction();
        }

        public void ClearSession()
        {
            _Session.Clear();
        }
    }
}
