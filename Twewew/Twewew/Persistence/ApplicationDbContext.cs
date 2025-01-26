using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Twewew.Entities;

namespace Twewew.Persistence;

public class ApplicationDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> Items { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);

        #region Identity

        builder.Entity<IdentityUser<Guid>>(e =>
        {
            var hasher = new PasswordHasher<IdentityUser<Guid>>();

            var user = new IdentityUser<Guid>
            {
                Id = new Guid("daf69601-0fa9-48f0-98f8-14bc4ecf16ba"),
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "davronbek8733@gmail.com",
                NormalizedEmail = "DAVRONBEK8733@GMAIL.COM",
                EmailConfirmed = true,
                LockoutEnd = null,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            user.PasswordHash = hasher.HashPassword(user, "Admin123@");

            e.HasData(user);

            e.ToTable("User");



        });

        builder.Entity<IdentityUserClaim<Guid>>(e =>
        {
            e.ToTable("UserClaim");
        });

        builder.Entity<IdentityUserLogin<Guid>>(e =>
        {
            e.ToTable("UserLogin");
        });

        builder.Entity<IdentityUserToken<Guid>>(e =>
        {
            e.ToTable("UserToken");
        });

        builder.Entity<IdentityRole<Guid>>(e =>
        {
            e.HasData(
                new IdentityRole<Guid>
                {
                    Id = new Guid("16398ef6-088b-416a-8424-423ace734bc1"),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });

            e.ToTable("Role");
        });

        builder.Entity<IdentityRoleClaim<Guid>>(e =>
        {
            e.ToTable("RoleClaim");
        });

        builder.Entity<IdentityUserRole<Guid>>(e =>
        {
            e.HasData(
                new IdentityUserRole<Guid>
                {
                    RoleId = new Guid("16398ef6-088b-416a-8424-423ace734bc1"),
                    UserId = new Guid("daf69601-0fa9-48f0-98f8-14bc4ecf16ba")
                });
            e.ToTable("UserRole");
        });



        #endregion

    }
}
