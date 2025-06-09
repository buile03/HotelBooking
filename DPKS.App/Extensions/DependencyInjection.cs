using DPKS.Service;
using Microsoft.Extensions.DependencyInjection;

namespace DPKS.App.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ITienNghiService, TienNghiService>();
            services.AddTransient<IOrganizationService, OrganizationService>();
            services.AddTransient<IStorageService, FileStorageService>();
            return services;
        }
    }
}
