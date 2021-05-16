using System;

namespace CleanTemplate.UnitOfWork.Exceptions
{
    public class CommitException: Exception
    {
        public CommitException(string message, Exception innerException): base(message, innerException)
        {

        }
    }
}
