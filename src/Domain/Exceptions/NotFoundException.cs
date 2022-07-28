using System;

namespace Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
            
        }
        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(Exception ex) : base(ex.Message)
        {
        }
    }
}