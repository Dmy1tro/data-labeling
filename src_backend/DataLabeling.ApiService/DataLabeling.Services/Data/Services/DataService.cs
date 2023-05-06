using DataLabeling.Common.Order;
using DataLabeling.Data.Context;
using DataLabeling.Entities;
using DataLabeling.Infrastructure.Guard;
using DataLabeling.Infrastructure.ImageHelpers;
using DataLabeling.Services.Data.Mappers;
using DataLabeling.Services.Data.Models;
using DataLabeling.Services.Interfaces.Data.Models;
using DataLabeling.Services.Interfaces.Data.Services;
using DataLabeling.Services.Interfaces.FileStorage.Services;
using DataLabeling.Services.Order.Mappers;
using DataLabeling.Services.Validators;
using Ionic.Zip;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataLabeling.Services.Data.Services
{
    public class DataService : IDataService
    {
        private readonly DateLabelingDbContext _context;
        private readonly IFileStorage _fileStorage;

        public DataService(DateLabelingDbContext context, IFileStorage fileStorage)
        {
            _context = context;
            _fileStorage = fileStorage;
        }

        public async Task<IDataModel> GetDataAsync(int dataId)
        {
            var data = await _context.DataSet.AsNoTracking().FirstOrDefaultAsync(d => d.Id == dataId);

            return data.MapToModel();
        }

        public async Task<IList<IDataModel>> GetCompletedDataSetAsync(IDataFilterModel filter)
        {
            var order = await GetOrderAsync(filter.OrderId);
            var model = order?.MapToModel();

            OrderValidator.CheckCustomerMatch(model, filter.CustomerId.Value);
            OrderValidator.CheckIsCompleted(model);
            OrderValidator.CheckIsPaid(model);

            var dataSet = await _context.DataSet.AsNoTracking()
                .Where(d => d.OrderId == filter.OrderId)
                .Where(d => filter.PerformerId == null || d.PerformerId == filter.PerformerId)
                .OrderBy(d => order.Type == OrderType.CollectData ? d.CreatedDate : d.LastModified)
                .Skip(filter.Skip)
                .Take(filter.Take)
                .ToListAsync();

            var mapped = dataSet.Select(d => d.MapToModel()).ToList();

            return mapped;
        }

        public async Task SaveDataSetToZip(int orderId, int customerId)
        {
            var order = await GetOrderAsync(orderId);
            var model = order?.MapToModel();

            OrderValidator.CheckCustomerMatch(model, customerId);
            OrderValidator.CheckIsCompleted(model);
            OrderValidator.CheckIsPaid(model);

            if (order.ZipPath != null)
            {
                return;
            }

            var dataSet = await _context.DataSet.AsNoTracking()
                .Where(d => d.OrderId == orderId)
                .ToListAsync();

            using var zip = new ZipFile();

            if (order.Type == OrderType.CollectData)
            {
                dataSet.ForEach(d => zip.AddFile(_fileStorage.GetFullPath(d.RawImageSource), "Raw_Images"));
            }

            if (order.Type == OrderType.LabelData)
            {
                dataSet.ForEach(d =>
                {
                    zip.AddFile(_fileStorage.GetFullPath(d.RawImageSource), "Raw_Images");
                    zip.AddFile(_fileStorage.GetFullPath(d.LabeledImageSource), "Labeled_Images");
                    zip.AddFile(_fileStorage.GetFullPath(d.LabeledXmlSource), "Labeled_Xml");
                });
            }

            var relativePath = $"{Guid.NewGuid()}.zip";
            var zipPath = _fileStorage.GetFullPath(relativePath);
            zip.Save(zipPath);

            order.ZipPath = relativePath;
            await _context.SaveChangesAsync();
        }

        public async Task<IDataModel> GetDataForLabelingAsync(int orderId, int performerId)
        {
            var order = await GetOrderAsync(orderId);
            Guard.ObjectFound(order);
            OrderValidator.CheckType(order.MapToModel(), OrderType.LabelData);

            var performer = await GetPerformerAsync(performerId);
            Guard.ObjectFound(performer);

            OrderValidator.CheckPerformerCanPerfomOrder(order.Priority, performer.Rating);

            if (order.IsCompleted) return null;

            var data = await _context.DataSet.AsNoTracking()
                .FirstOrDefaultAsync(d => d.OrderId == orderId && d.LabeledImageSource == null);

            return data.MapToModel();
        }

        public async Task<IDataModel> LabelDataAsync(int dataId, int performerId, string labeledImgSource, string variant)
        {
            Guard.PropertyNotEmpty(dataId, nameof(dataId));
            Guard.PropertyNotEmpty(labeledImgSource, nameof(labeledImgSource));
            Guard.PropertyNotEmpty(performerId, nameof(performerId));

            var data = await _context.DataSet.Include(o => o.Order).FirstOrDefaultAsync(d => d.Id == dataId);
            Guard.ObjectFound(data);

            var performer = await GetPerformerAsync(performerId);
            Guard.ObjectFound(performer);

            DataValidator.CheckDataNotLabeled(data?.MapToModel());
            OrderValidator.CheckPerformerCanPerfomOrder(data.Order.Priority, performer.Rating);

            var (imageBytes, contentType) = await _fileStorage.GetFileAsync(labeledImgSource);
            var dataXml = new DataXmlModel(variant, Convert.ToBase64String(imageBytes));
            var serializer = new XmlSerializer(typeof(DataXmlModel));
            using var ms = new MemoryStream();
            serializer.Serialize(ms, dataXml);

            var fileBytes = ms.ToArray();
            var xmlFilePath = await _fileStorage.UploadFileAsync($"{variant}.xml", fileBytes);

            data.LabeledImageSource = labeledImgSource;
            data.LabeledXmlSource = xmlFilePath;
            data.PerformerId = performerId;
            data.Variant = variant;

            await _context.SaveChangesAsync();

            return data.MapToModel();
        }

        public async Task<IDataModel> AddRawDataAsync(int orderId, int performerId, string rawImgSource, string hash)
        {
            Guard.PropertyNotEmpty(orderId, nameof(orderId));
            Guard.PropertyNotEmpty(performerId, nameof(performerId));
            Guard.PropertyNotEmpty(rawImgSource, nameof(rawImgSource));
            Guard.PropertyNotEmpty(hash, nameof(hash));

            var performer = await GetPerformerAsync(performerId);
            Guard.ObjectFound(performer);
            var order = await GetOrderAsync(orderId);
            Guard.ObjectFound(order);

            OrderValidator.CheckPerformerCanPerfomOrder(order.Priority, performer.Rating);

            var dataSet = new Entities.Data
            {
                OrderId = orderId,
                RawImageSource = rawImgSource,
                RawImageHash = hash,
                PerformerId = performerId
            };

            _context.DataSet.Add(dataSet);

            await _context.SaveChangesAsync();

            return dataSet.MapToModel();
        }

        public async Task<IDataModel> AddRawDataAsync(int orderId, string rawImgSource, string hash)
        {
            Guard.PropertyNotEmpty(orderId, nameof(orderId));
            Guard.PropertyNotEmpty(rawImgSource, nameof(rawImgSource));
            Guard.PropertyNotEmpty(hash, nameof(hash));

            var order = await GetOrderAsync(orderId);
            Guard.ObjectFound(order);

            var dataSet = new Entities.Data
            {
                OrderId = orderId,
                RawImageSource = rawImgSource,
                RawImageHash = hash
            };

            _context.DataSet.Add(dataSet);

            await _context.SaveChangesAsync();

            return dataSet.MapToModel();
        }

        public async Task ValidateRawImages(int orderId, int performerId, IEnumerable<Func<Stream>> images, int imagesCount)
        {
            var order = await GetOrderAsync(orderId);
            Guard.ObjectFound(order);

            var performer = await GetPerformerAsync(performerId);
            Guard.ObjectFound(performer);

            OrderValidator.CheckPerformerCanPerfomOrder(order.Priority, performer.Rating);

            if (order.Type != OrderType.CollectData)
            {
                throw new Common.Exceptions.ValidationException("Order need to be of type 'Collect data'.");
            }

            if (order.DatSetRequiredCount - order.CurrentProgress < imagesCount)
            {
                throw new Common.Exceptions.ValidationException("Too many images.");
            }

            ValidateImageOriginality(images);

            foreach (var image in images)
            {
                var hash = await ImageHelper.GetHashAsync(image);
                var alreadyExists = await _context.DataSet.AnyAsync(d => d.OrderId == orderId && d.RawImageHash == hash);

                if (alreadyExists)
                {
                    throw new Common.Exceptions.ValidationException("There are images that uploaded already.");
                }
            }
        }

        public async Task ValidateLabeledImage(int dataId, int performerId, string varinat, Func<Stream> labeledImage)
        {
            var data = await GetDataAsync(dataId);
            Guard.ObjectFound(data, "Data");

            var order = (await GetOrderAsync(data.OrderId))?.MapToModel();
            Guard.ObjectFound(order, "Order");

            var performer = await GetPerformerAsync(performerId);
            Guard.ObjectFound(performer, "Performer");

            OrderValidator.CheckPerformerCanPerfomOrder(order.Priority, performer.Rating);

            if (order.Type != OrderType.LabelData)
            {
                throw new Common.Exceptions.ValidationException("Order need to be of type 'Collect data'.");
            }

            DataValidator.CheckDataNotLabeled(data);
            OrderValidator.CheckVariantExists(order, varinat);

            var rawImage = _fileStorage.GetFileStream(data.RawImageSource);

            var duplicateResult = ImageHelper.Compare(new[] { rawImage, labeledImage }, 0.01);

            if (duplicateResult == ImageComparasionResult.Similar)
            {
                throw new Common.Exceptions.ValidationException("Image need to be labeled.");
            }
        }

        public void ValidateImageOriginality(IEnumerable<Func<Stream>> images)
        {
            var duplicateResult = ImageHelper.Compare(images);

            if (duplicateResult == ImageComparasionResult.Similar)
            {
                throw new Common.Exceptions.ValidationException("There are simillar images.");
            }
        }

        public async Task<ICollection<IDataModel>> GetPerformerJobs(int orderId, int performerId)
        {
            var order = await GetOrderAsync(orderId);

            OrderValidator.CheckIsCompleted(order?.MapToModel());

            var data = await _context.DataSet.AsNoTracking()
                .Where(d => d.OrderId == orderId && d.PerformerId == performerId)
                .ToListAsync();

            return data.Select(d => d.MapToModel()).ToList();
        }

        private async Task<Entities.Order> GetOrderAsync(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(p => p.Id == id);
        }

        private async Task<Performer> GetPerformerAsync(int id)
        {
            return await _context.Performers.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
