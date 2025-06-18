using DPKS.Service;
using Microsoft.Extensions.DependencyInjection;

namespace DPKS.App.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IOrganizationService, OrganizationService>();
            services.AddTransient<IStorageService, FileStorageService>();

            services.AddTransient<ITienNghiService, TienNghiService>();
            services.AddTransient<IPhongService, PhongService>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddTransient<IDatPhongService, DatPhongService>();
            services.AddTransient<IThanhToanService, ThanhToanService>();
            

            services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IEmailSenderService, SmtpEmailSender>();
            services.AddScoped<IDanhMucService, DanhMucService>();
            return services;
        }
    }
}
