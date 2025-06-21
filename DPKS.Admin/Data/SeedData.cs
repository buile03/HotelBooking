using DPKS.Common.Enum;
using DPKS.Data.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DPKS.Admin.Data
{
    public static class SeedData
    {
        public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            foreach (var roleName in Enum.GetNames(typeof(enRoles)))
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper(),
                        Descritption = $"Vai trò {roleName}",
                        IsActive = true
                    });
                }
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            var adminEmail = "admin1@gmail.com";
            var adminUserName = "admin1";

            if (await userManager.FindByNameAsync(adminUserName) == null)
            {
                var admin1 = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    HoTen = "Quản trị viên",
                    EmailConfirmed = true,
                    IsActive = true,
                    QuocGiaId = 242,
                    TinhId = 1,
                    PhotoName = "user.png",
                    CreatedBy = "system"
                };

                var result = await userManager.CreateAsync(admin1, "Admin@123");

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin1, enRoles.ADMIN.ToString());
            }
        }
    }
}
