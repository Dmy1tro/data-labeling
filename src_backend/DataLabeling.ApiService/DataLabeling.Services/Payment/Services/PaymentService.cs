using DataLabeling.Api.Common.Configuration.Settings;
using DataLabeling.Data.Context;
using DataLabeling.Infrastructure.Guard;
using DataLabeling.Services.Interfaces.Payment.Models;
using DataLabeling.Services.Interfaces.Payment.Services;
using DataLabeling.Services.Order.Mappers;
using DataLabeling.Services.Payment.Mappers;
using DataLabeling.Services.Payment.Models;
using DataLabeling.Services.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataLabeling.Services.Payment.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly DateLabelingDbContext _context;
        private readonly LiqPayConfiguration _liqPayConfiguration;

        public PaymentService(DateLabelingDbContext context,
                              IOptions<LiqPayConfiguration> liqPayConfiguration)
        {
            _context = context;
            _liqPayConfiguration = liqPayConfiguration.Value;
        }

        public async Task AddPayment(IOrderPaymentModel orderPaymentModel)
        {
            var payment = orderPaymentModel.MapToEntity();
            var order = await _context.Orders.FindAsync(orderPaymentModel.OrderId);

            Guard.ObjectFound(order);
            OrderValidator.CheckIsNotPaid(order.MapToModel());

            _context.OrderPayments.Add(payment);
            await _context.SaveChangesAsync();

            order.OrderPaymentId = payment.Id;
            await _context.SaveChangesAsync();
        }

        public async Task AddPayment(IJobPaymentModel jobPaymentModel)
        {
            var payment = jobPaymentModel.MapToEntity();
            var performer = await _context.Performers.FindAsync(payment.PerformerId);

            Guard.ObjectFound(performer);

            if (payment.Price <= 0)
            {
                throw new Common.Exceptions.ValidationException("Invalid price.");
            }

            if (performer.Balance < payment.Price)
            {
                throw new Common.Exceptions.ValidationException("Not enough money on balance.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                performer.Balance -= payment.Price;

                _context.JobPayments.Add(payment);
                _context.Performers.Update(performer);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch(Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ICollection<IJobPaymentModel>> GetJobPaymentHistory(int performerId)
        {
            var history = await _context.JobPayments.AsNoTracking()
                                                    .Where(x => x.PerformerId == performerId)
                                                    .OrderByDescending(x => x.CreatedDate)
                                                    .ToListAsync();

            return history.Select(x => x.MapToModel()).ToList();
        }

        public async Task<ICollection<IOrderPaymentHistoryModel>> GetOrderPaymentHistory(int customerId)
        {
            var history = await _context.OrderPayments.AsNoTracking()
                                                      .Where(x => x.CustomerId == customerId)
                                                      .Include(x => x.Order)
                                                      .OrderByDescending(x => x.CreatedDate)
                                                      .ToListAsync();



            return history.Select(x => x.MapToHistoryModel()).ToList();
        }

        public Task<ILiqPayDataModel> GetLiqPayData(decimal price, string postDataUrl)
        {
            var payload = new LiqPayPayload
            {
                PublicKey = _liqPayConfiguration.PublicKey,
                Version = 3,
                ACtion = "pay",
                Amount = price,
                Currency = "USD",
                Description = "Purchasing data-labeling order",
                OrderId = Guid.NewGuid().ToString(),
                Sandbox = 1,
                ResultUrl = postDataUrl,
                Info = "Some info about purchasing....",
                ProductCategory = "Data-Science",
                ProductDescription = "Super data",
                ProductName = "Data-labeling order"
            };

            var json = JsonConvert.SerializeObject(payload);
            var dataHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            var signature = _liqPayConfiguration.PrivateKey + dataHash + _liqPayConfiguration.PrivateKey;
            var signatureBytes = Encoding.UTF8.GetBytes(signature);
            using var sha1 = SHA1.Create();
            var signatureHashBytes = sha1.ComputeHash(signatureBytes);
            var signatureHash = Convert.ToBase64String(signatureHashBytes);

            var model = new LiqPayDataModel
            {
                SignatureHash = signatureHash,
                DataHash = dataHash
            };

            return Task.FromResult<ILiqPayDataModel>(model);
        }

        public Task<bool> VerifyLiqPaySignature(string liqPayData, string liqPaySignature)
        {
            var signature = _liqPayConfiguration.PrivateKey + liqPayData + _liqPayConfiguration.PrivateKey;
            var signatureBytes = Encoding.UTF8.GetBytes(signature);
            using var sha1 = SHA1.Create();
            var signatureHashBytes = sha1.ComputeHash(signatureBytes);
            var signatureHash = Convert.ToBase64String(signatureHashBytes);

            return Task.FromResult(signatureHash == liqPaySignature);
        }
    }
}
