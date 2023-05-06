using DataLabeling.Services.Interfaces.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DataLabeling.Services.Interfaces.Data.Services
{
    public interface IDataService
    {
        Task<IDataModel> GetDataAsync(int dataId);

        Task<IDataModel> LabelDataAsync(int dataId, int performerId, string labeledImgSource, string variant);

        Task<IDataModel> AddRawDataAsync(int orderId, int performerId, string rawImgSource, string hash);

        Task<IDataModel> AddRawDataAsync(int orderId, string rawImgSource, string hash);

        Task<IList<IDataModel>> GetCompletedDataSetAsync(IDataFilterModel filter);

        Task SaveDataSetToZip(int orderId, int customerId);

        Task<IDataModel> GetDataForLabelingAsync(int orderId, int performerId);

        void ValidateImageOriginality(IEnumerable<Func<Stream>> images);

        Task ValidateRawImages(int orderId, int performerId, IEnumerable<Func<Stream>> images, int imagesCount);

        Task ValidateLabeledImage(int dataId, int performerId, string varinat, Func<Stream> image);

        Task<ICollection<IDataModel>> GetPerformerJobs(int orderId, int performerId);
    }
}
