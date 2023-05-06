namespace DataLabeling.Common.Exceptions
{
    public class NotFoundException : BusinessLogicException
    {
        public NotFoundException(string name) : base($"{name} was not found.")
        {
        }
    }
}
