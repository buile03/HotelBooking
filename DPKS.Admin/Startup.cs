using DPKS.Admin.Data;
using DPKS.Admin.Extensions;
using DPKS.Common.Helper;
using DPKS.Common.Helper.DPKS.Common.Helper;
using DPKS.Common.System;
using DPKS.Data.EF;
using DPKS.Data.Entites;
using DPKS.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace DPKS.Admin
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // CSDL
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(SystemConstants.AppSettings.MainConnectionString)),
                ServiceLifetime.Transient);

            // Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // Auth - KHÔNG cần Google, Facebook
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.LogoutPath = "/Account/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
            });

            // Service, Helper
            services.AddInfrastructure();
            services.AddTransient(typeof(ILogger<>), typeof(Logger<>));
            services.AddTransient<MoMoHelper>();
            services.AddTransient<VnPayHelper>();
            services.AddHttpClient();
            PayPalHelper.Configure(Configuration);
            services.AddHttpContextAccessor();

            // Email (nếu cần)
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            services.AddTransient<IEmailSenderService, SmtpEmailSender>();

            // Session + MVC
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            if (Env.IsDevelopment())
            {
                services.AddControllersWithViews().AddRazorRuntimeCompilation();
            }
            else
            {
                services.AddControllersWithViews();
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });

        }
    }
}
