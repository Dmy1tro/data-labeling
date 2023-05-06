using DataLabeling.Api.Common.Authentication;
using DataLabeling.Api.Common.Extensions;
using DataLabeling.Api.Common.Models;
using DataLabeling.ApiService.Data.Mappers;
using DataLabeling.ApiService.Data.Models;
using DataLabeling.Common.Order;
using DataLabeling.DAL.Services.Interfaces.User.Services;
using DataLabeling.Infrastructure.ImageHelpers;
using DataLabeling.Services.Interfaces.Data.Services;
using DataLabeling.Services.Interfaces.FileStorage.Services;
using DataLabeling.Services.Interfaces.Order.Services;
using DataLabeling.Services.Interfaces.Sallary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace DataLabeling.ApiService.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly IFileStorage _fileStorage;
        private readonly IPriceService _priceService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IUserAccessor _userAccessor;

        public DataController(IDataService dataService, 
                              IFileStorage fileStorage,
                              IPriceService sallaryService,
                              IUserService userService,
                              IOrderService orderService,
                              IUserAccessor userAccessor)
        {
            _dataService = dataService;
            _fileStorage = fileStorage;
            _priceService = sallaryService;
            _userService = userService;
            _orderService = orderService;
            _userAccessor = userAccessor;
        }

        [HttpPost("label-data")]
        [Authorize(Policy = PolicyName.ForPerformer)]
        [DisableRequestSizeLimit]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> LabelDataAsync([FromForm] LabelDataRequest request)
        {
            await _dataService.ValidateLabeledImage(request.DataId, _userAccessor.User.Id, request.Variant, request.ImageFile.OpenReadStream);

            var imgPath = await _fileStorage.UploadFileAsync(request.ImageFile.FileName, request.ImageFile.OpenReadStream);
            var dataModel =  await _dataService.LabelDataAsync(request.DataId, _userAccessor.User.Id, imgPath, request.Variant);
            var sallary = await _priceService.CalculateSallaryAsync(_userAccessor.User.Id, OrderType.LabelData);

            await _userService.AddToPerformerBalance(_userAccessor.User.Id, sallary);
            await _orderService.RefreshProgressAsync(dataModel.OrderId);

            return NoContent();
        }

        [HttpPost("raw-data")]
        [Authorize(Policy = PolicyName.ForPerformer)]
        [DisableRequestSizeLimit]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadRawDataAsync([FromForm] UploadRawDataRequest request)
        {
            var imagesFunc = request.ImageFiles.Select(i => (Func<Stream>)i.OpenReadStream);
            await _dataService.ValidateRawImages(request.OrderId, _userAccessor.User.Id, imagesFunc, request.ImageFiles.Count);

            foreach (var file in request.ImageFiles)
            {
                var hash = await ImageHelper.GetHashAsync(file.OpenReadStream);
                var imgPath = await _fileStorage.UploadFileAsync(file.FileName, file.OpenReadStream);
                await _dataService.AddRawDataAsync(request.OrderId, _userAccessor.User.Id, imgPath, hash);
                var sallary = await _priceService.CalculateSallaryAsync(_userAccessor.User.Id, OrderType.CollectData);
                await _userService.AddToPerformerBalance(_userAccessor.User.Id, sallary);
            }

            await _orderService.RefreshProgressAsync(request.OrderId);

            return NoContent();
        }

        [HttpGet("data-for-labeling/{orderId}")]
        [Authorize(Policy = PolicyName.ForPerformer)]
        public async Task<IActionResult> GetDataForLabelingAsync(int orderId)
        {
            var dataForLabeling = await _dataService.GetDataForLabelingAsync(orderId, _userAccessor.User.Id);

            if (dataForLabeling is null)
            {
                return NoContent();
            }

            var (image, contentType) = await _fileStorage.GetFileAsync(dataForLabeling.RawImageSource);

            return this.OkApiResponse(new DataWithImageResponse { Data = dataForLabeling, Image = image, ContentType = contentType });
        }

        [HttpGet("completed-dataset")]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> GetCompletedDataSetAsync([FromQuery] GetDataSetRequest request)
        {
            var filterModel = request.MapToFilterModel(_userAccessor.User.Id);
            var dataset = await _dataService.GetCompletedDataSetAsync(filterModel);

            return this.OkApiResponse(dataset);
        }

        [HttpGet("completed-dataset-zip/{orderId}")]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> GetCompletedDataSetZipAsync(int orderId)
        {
            await _dataService.SaveDataSetToZip(orderId, _userAccessor.User.Id);

            var order = await _orderService.GetOrderForCustomerAsync(orderId, _userAccessor.User.Id);

            return PhysicalFile(_fileStorage.GetFullPath(order.ZipPath), "application/zip", $"{order.Name}.zip");
        }

        [HttpGet("data-for-review/{orderId}/{performerId}")]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> GetDataForReview(int orderId, int performerId)
        {
            var dataset = await _dataService.GetPerformerJobs(orderId, performerId);
            var order = await _orderService.GetOrderForCustomerAsync(orderId, _userAccessor.User.Id);

            var response = new List<DataWithImageResponse>();

            foreach (var data in dataset)
            {
                byte[] image = null;
                string contentType = null;

                if (order.Type == OrderType.CollectData)
                {
                    (image, contentType) = await _fileStorage.GetFileAsync(data.RawImageSource);
                    
                }
                else
                {
                    (image, contentType) = await _fileStorage.GetFileAsync(data.LabeledImageSource);
                }

                response.Add(new DataWithImageResponse { Data = data, Image = image, ContentType = contentType });
            }

            return this.OkApiResponse(response);
        }
    }
}
