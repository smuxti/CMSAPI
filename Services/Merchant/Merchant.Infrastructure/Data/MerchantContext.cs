using Merchants.Core.Common;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Merchants.Infrastructure.Data
{
    public class MerchantContext:DbContext
    {
        public MerchantContext(DbContextOptions<MerchantContext> dbContext):base(dbContext)
        {
            
        }
        //public DbSet<Tenant> Tenants { get; set; }
        //public DbSet<Merchant> Merchant { get; set; }
        //public DbSet<MerchantCategory> MerchantCategories { get; set; }
        //public DbSet<Terminal> Terminals { get; set; }
        //public DbSet<FeeSlab> FeeSlabs { get; set; }
        //public DbSet<FeeSlabDetail> FeeSlabDetails { get; set; }
        //public DbSet<TransactionType> TransactionTypes{ get; set; }
        //public DbSet<MerchantTransactionType> merchantTransactionTypes { get; set; }
        //public DbSet<Bank> Banks { get; set; }






        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<ComplaintCategory> ComplaintCategories { get; set; }
        public DbSet<ComplaintType> ComplaintTypes { get; set; }
        public DbSet<Channel> Channels { get; set; }
        //public DbSet<Role> Roles { get; set; }

        public DbSet<Escalation> Escalations { get; set; }
        public DbSet<ManagementHierarchy> ManagementHierarchies { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<ComplaintDetails> ComplaintDetails { get; set; }
        public DbSet<Complainer> Complainers { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Routes> Routes { get; set; }
        public DbSet<RoleRouts> RoleRoutes { get; set; }
        public DbSet<UserResource> UserResources { get; set; }
        public DbSet<MerchantLocations> MerchantLocations { get; set; }
        public DbSet<Equipment> Equipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Complaint>().HasQueryFilter(r => !r.isDeleted);
            modelBuilder.Entity<ComplaintCategory>().HasQueryFilter(r => !r.isDeleted);
            modelBuilder.Entity<ComplaintType>().HasQueryFilter(r => !r.isDeleted);
            modelBuilder.Entity<Escalation>().HasQueryFilter(r => !r.isDeleted);
            modelBuilder.Entity<ManagementHierarchy>().HasQueryFilter(r => !r.isDeleted);
            modelBuilder.Entity<Merchant>().HasQueryFilter(r => !r.isDeleted);
            modelBuilder.Entity<ComplaintDetails>().HasQueryFilter(r => !r.isDeleted);
            modelBuilder.Entity<Complainer>().HasQueryFilter(r => !r.isDeleted);
            modelBuilder.Entity<User>().HasQueryFilter(r => !r.isDeleted);
            modelBuilder.Entity<UserType>().HasQueryFilter(r => !r.isDeleted);
            modelBuilder.Entity<UserResource>().HasQueryFilter(r => !r.isDeleted);
            modelBuilder.Entity<MerchantLocations>().HasQueryFilter(r => !r.isDeleted);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entity in ChangeTracker.Entries<BaseEntity>()) 
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.CreatedAt = DateTime.Now;
                        entity.Entity.Status = "Active";
                        //entity.Entity.CreatedBy = Guid.Parse("0A7EB5DF-B507-4620-938A-E7CB493B465A");
                        break;
                    case EntityState.Modified:
                        entity.Entity.UpdatedAt = DateTime.Now;
                        //entity.Entity.UpdatedBy = Guid.Parse("0A7EB5DF-B507-4620-938A-E7CB493B465A");
                        break;
                }

            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
