using System.Collections.Generic;
using System.Linq;

namespace DataLabeling.Common.Exceptions
{
    public class ParameterInvalidException : BusinessLogicException
    {
        public ParameterInvalidException(string parameterName) : base($"Please provide valid {parameterName}.")
        {
        }

        public ParameterInvalidException(IEnumerable<ErrorField> errorFields) : base(string.Join("\n", errorFields.Select(x => x.ToMessage())))
        {
        }
    }

    public record ErrorField(string Field, string Message)
    {
        public string ToMessage() => $"Invalid parameter {Field}. {Message}";
    }
}
