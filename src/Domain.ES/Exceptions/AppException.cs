using System;

namespace Domain.ES.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string message)
            : base(message)
        {
        }

        public AppException(Exception ex) : base(ex.Message)
        {
        }
    }
}