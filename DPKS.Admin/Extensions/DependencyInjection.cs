using DPKS.Data.EF;
using DPKS.Data.Entites;
using DPKS.Service;
using Microsoft.AspNetCore.Identity;
using PayPalCheckoutSdk.Orders;

namespace DPKS.Admin.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IOrganizationService, OrganizationService>();
            services.AddTransient<IStorageService, FileStorageService>();

            services.AddScoped<IDanhMucService, DanhMucService>();
            services.AddScoped<IPhongService, PhongService>();
            services.AddScoped<ILoaiPhongService, LoaiPhongService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITrackingService, TrackingService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            return services;
        }
    }
}
