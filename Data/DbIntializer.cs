using CampusFix.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CampusFix.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var db = services.GetRequiredService<ApplicationDbContext>();

            foreach (var role in new[] { "Student", "Maintenance", "Administrator" })
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            var adminEmail = "admin@campus.com";
            var old = await userManager.FindByEmailAsync(adminEmail);
            if (old != null) await userManager.DeleteAsync(old);

            var admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRolesAsync(admin, new[] { "Administrator", "Maintenance", "Student" });

            var sEmail = "student@campus.com";
            if (await userManager.FindByEmailAsync(sEmail) == null)
            {
                var s = new ApplicationUser { UserName = sEmail, Email = sEmail, EmailConfirmed = true };
                await userManager.CreateAsync(s, "Student123!");
                await userManager.AddToRoleAsync(s, "Student");
            }

            var stEmail = "staff@campus.com";
            if (await userManager.FindByEmailAsync(stEmail) == null)
            {
                var st = new ApplicationUser { UserName = stEmail, Email = stEmail, EmailConfirmed = true };
                await userManager.CreateAsync(st, "Staff123!");
                await userManager.AddToRoleAsync(st, "Maintenance");
            }
        }
    }
}