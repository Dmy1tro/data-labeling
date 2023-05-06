namespace DataLabeling.Common.Exceptions
{
    public class ValidationException : BusinessLogicException
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}
