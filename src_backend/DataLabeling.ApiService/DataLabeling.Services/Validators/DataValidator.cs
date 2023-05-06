using DataLabeling.Common.Order;
using DataLabeling.Infrastructure.Guard;
using DataLabeling.Services.Interfaces.Data.Models;
using FluentValidation;

namespace DataLabeling.Services.Validators
{
    public class DataValidator : AbstractValidator<IDataModel>
    {
        public DataValidator(OrderType orderType)
        {
            if (orderType == OrderType.CollectData)
            {
                RuleFor(d => d.RawImageSource).NotEmpty().WithMessage("Raw image is required.");
            }

            if (orderType == OrderType.LabelData)
            {
                RuleFor(d => d.LabeledImageSource).NotEmpty().WithMessage("Labeled image is required.");
            }
        }

        public static void CheckDataNotLabeled(IDataModel dataModel)
        {
            Guard.ObjectFound(dataModel, "Data");

            if (!string.IsNullOrEmpty(dataModel.LabeledImageSource))
            {
                throw new Common.Exceptions.ValidationException("Data already labeled.");
            }
        }
    }
}
