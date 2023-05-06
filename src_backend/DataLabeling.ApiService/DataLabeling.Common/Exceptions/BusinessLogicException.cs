using System;

namespace DataLabeling.Common.Exceptions
{
    public abstract class BusinessLogicException : Exception
    {
        public BusinessLogicException(string message) : base(message)
        {
        }
    }
}
