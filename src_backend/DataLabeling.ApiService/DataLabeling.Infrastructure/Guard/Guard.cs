using DataLabeling.Common.Exceptions;
using DataLabeling.Infrastructure.Extensions;

namespace DataLabeling.Infrastructure.Guard
{
    public class Guard
    {
        public static void PropertyNotEmpty<T>(T value, string parameterName)
        {
            if (value.IsEmpty())
            {
                throw new ParameterInvalidException(parameterName);
            }
        }

        public static void ObjectFound<T>(T value, string objName = null)
        {
            if (value.IsNullOrDefault())
            {
                objName ??= typeof(T).Name;
                throw new NotFoundException(objName);
            }
        }
    }
}
