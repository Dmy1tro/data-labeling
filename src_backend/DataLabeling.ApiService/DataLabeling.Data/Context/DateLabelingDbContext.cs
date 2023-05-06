using DataLabeling.Entities;
using DataLabeling.Infrastructure.Date;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataLabeling.Data.Context
{
    public class DateLabelingDbContext : DbContext
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public DateLabelingDbContext(DbContextOptions<DateLabelingDbContext> options, IDateTimeProvider dateTimeProvider) : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public DbSet<Entities.Data> DataSet { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<JobPayment> JobPayments{ get; set; }
        public DbSet<OrderPayment> OrderPayments { get; set; }
        public DbSet<Review> Reviews{ get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Performer> Performers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.Data>(builder =>
            {
                builder.HasKey(d => d.Id);

                builder.HasOne(d => d.Order)
                    .WithMany(o => o.DataSet)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(d => d.Performer)
                    .WithMany(p => p.CompletedJobs)
                    .HasForeignKey(d => d.PerformerId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Performer>(builder => 
            { 
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Balance).HasPrecision(16, 6);
            });
            modelBuilder.Entity<Customer>(builder => { builder.HasKey(c => c.Id); });

            modelBuilder.Entity<Order>(builder =>
            {
                builder.HasKey(o => o.Id);

                builder.Property(o => o.Price).HasPrecision(16, 6);

                builder.HasOne(o => o.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(o => o.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(o => o.OrderPayment)
                    .WithOne(p => p.Order)
                    .HasForeignKey<Order>(o => o.OrderPaymentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<OrderPayment>(builder =>
            {
                builder.HasKey(p => p.Id);

                builder.Property(p => p.Price).HasPrecision(16, 6);

                builder.HasOne(p => p.Order)
                    .WithOne(o => o.OrderPayment)
                    .HasForeignKey<OrderPayment>(p => p.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(p => p.Customer)
                    .WithMany(c => c.OrderPayments)
                    .HasForeignKey(p => p.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<JobPayment>(builder =>
            {
                builder.HasKey(j => j.Id);

                builder.Property(j => j.Price).HasPrecision(16, 6);

                builder.HasOne(j => j.Performer)
                    .WithMany(p => p.JobPayments)
                    .HasForeignKey(j => j.PerformerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Review>(builder =>
            {
                builder.HasKey(r => r.Id);

                builder.HasOne(r => r.Performer)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(r => r.PerformerId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(r => r.Customer)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(r => r.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(r => r.Order)
                    .WithMany(o => o.Reviews)
                    .HasForeignKey(r => r.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public override int SaveChanges()
        {
            ProcessAuditEntities();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcessAuditEntities();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void ProcessAuditEntities()
        {
            var auditEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is AuditEntity && 
                            (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entryEntity in auditEntries)
            {
                var entity = (AuditEntity)entryEntity.Entity;

                entity.LastModified = _dateTimeProvider.UtcNow;
                
                if (entryEntity.State == EntityState.Added)
                {
                    entity.CreatedDate = _dateTimeProvider.UtcNow;
                }
            }
        }
    }
}
