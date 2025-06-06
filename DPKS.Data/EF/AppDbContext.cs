using DPKS.Data.Config;
using DPKS.Data.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DPKS.Data.EF
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Configure Identity tables
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<ApplicationRole>().ToTable("Roles");

            // Configure custom fields in Identity tables
            builder.Entity<ApplicationRole>().Property(role => role.Descritption).HasMaxLength(256);

            // Identity User Claims
            builder.Entity<IdentityUserClaim<int>>(e => { e.ToTable("UserClaims"); });

            // Identity User Logins
            builder.Entity<IdentityUserLogin<int>>(e =>
            {
                e.ToTable("UserLogins");
                e.HasKey(login => new { login.UserId, login.LoginProvider, login.ProviderKey });
            });

            // Identity User Tokens
            builder.Entity<IdentityUserToken<int>>(e =>
            {
                e.ToTable("UserTokens");
                e.HasKey(token => new { token.UserId, token.LoginProvider, token.Name });
            });

            // Identity Role Claims
            builder.Entity<IdentityRoleClaim<int>>(e => { e.ToTable("RoleClaims"); });

            // Identity User Roles
            builder.Entity<IdentityUserRole<int>>(e =>
            {
                e.ToTable("UserRoles");
                e.HasKey(ur => new { ur.UserId, ur.RoleId });
            });

            // Apply configurations from assembly
            builder.ApplyConfigurationsFromAssembly(typeof(QuocGiaConfiguration).Assembly);

            builder.ApplyConfiguration(new TienNghiConfiguration());



        }

        public DbSet<QuocGia> quocGias { get; set; }

        public DbSet<Tinh> tinhs { get; set; }


        public DbSet<Phong> phongs { get; set; }


        public DbSet<TienNghi> tienNghis { get; set; }

        public DbSet<FeedBack> FeedBacks { get; set; }

        public DbSet<ThanhToan> thanhToans { get; set; }

        public DbSet<PhuongThucThanhToan> phuongThucThanhToans { get; set; }

        public DbSet<DatPhong> datPhongs { get; set; }

        public DbSet<TrangThaiDatPhong> trangThaiDatPhongs { get; set; }

        public DbSet<TienNghiTheoLoaiPhong> tienNghiTheoLoaiPhongs { get; set; }

        public DbSet<TrangThaiPhong> trangThaiPhongs { get; set; }

        public DbSet<LoaiPhong> loaiPhongs { get; set; }

        public DbSet<AnhPhong> anhPhongs { get; set; }




    }
}
