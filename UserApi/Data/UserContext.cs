using Microsoft.EntityFrameworkCore;

namespace UserManagementApp.UserApi.Data
{
    public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
    {
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(b =>
            {
                b.HasData(
                    new AppUser { Id = new Guid("c16aa979-da57-47d3-9ec2-c7dab1dad506"), Name = "John", Gender = Gender.Male, Age = 21 },
                    new AppUser { Id = new Guid("1b258905-c6d1-428b-ad49-fd5c256fe1e2"), Name = "Marry", Gender = Gender.Female, Age = 22 },
                    new AppUser { Id = new Guid("6b7501ba-df26-4a9f-bc40-74e6deb02ffc"), Name = "Gepgre", Gender = Gender.Male, Age = 20 }
                );
            });
        }
    }
}
