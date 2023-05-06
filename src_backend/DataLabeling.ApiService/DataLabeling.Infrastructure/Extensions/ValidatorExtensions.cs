using DataLabeling.Common.Exceptions;
using FluentValidation;
using System.Linq;

namespace DataLabeling.Infrastructure.Extensions
{
    public static class ValidatorExtensions
    {
        public static void ValidateAndThrowException<T>(this IValidator<T> validator, T data)
        {
            var result = validator.Validate(data);

            if (!result.IsValid)
            {
                throw new ParameterInvalidException(result.Errors.Select(e => new ErrorField(e.PropertyName, e.ErrorMessage)));
            }
        }
    }
}
