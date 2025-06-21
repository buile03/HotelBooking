using DPKS.Admin;
using DPKS.Admin.Data;
using DPKS.Data.Entites;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DPKS.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Thực hiện Seed trước khi chạy ứng dụng
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                    await SeedData.SeedRolesAsync(roleManager);
                    await SeedData.SeedAdminUserAsync(userManager);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi seed dữ liệu: {ex.Message}");
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
