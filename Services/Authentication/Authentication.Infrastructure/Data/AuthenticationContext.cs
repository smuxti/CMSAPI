using Authentication.Core.Common;
using Authentication.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Data
{
    public class AuthenticationContext:DbContext
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> dbContext) :base(dbContext)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Routes> Routes { get; set; }
        public DbSet<RoleRouts> RoleRoutes { get; set; }
        public DbSet<UserResource> UserResources { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entity in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.CreatedAt = DateTime.Now;
                       // entity.Entity.CreatedBy = 1;
                        break;
                    case EntityState.Modified:
                        entity.Entity.UpdatedAt = DateTime.Now;
                        //entity.Entity.UpdatedBy = 1;
                        break;
                }

            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
    
}
