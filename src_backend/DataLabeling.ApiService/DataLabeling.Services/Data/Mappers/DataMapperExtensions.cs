using DataLabeling.Services.Data.Models;
using DataLabeling.Services.Interfaces.Data.Models;

namespace DataLabeling.Services.Data.Mappers
{
    public static class DataMapperExtensions
    {
        public static Entities.Data MapToEntity(this IDataModel model)
        {
            return new Entities.Data
            {
                LabeledImageSource = model.LabeledImageSource,
                RawImageSource = model.RawImageSource,
                OrderId = model.OrderId,
                PerformerId = model.PerformerId,
                RawImageHash = model.RawImageHash,
                Variant = model.Variant
            };
        }

        public static IDataModel MapToModel(this Entities.Data data)
        {
            if (data is null) return null;

            return new DataModel
            {
                Id = data.Id,
                LabeledImageSource = data.LabeledImageSource,
                RawImageSource = data.RawImageSource,
                OrderId = data.OrderId,
                PerformerId = data.PerformerId,
                RawImageHash = data.RawImageHash,
                Variant = data.Variant
            };
        }
    }
}
