using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace TradingCardGame.Data.Seeders
{
    public class RoleSeeder
    {
        private readonly List<string> roles = new List<string>()
        {
            "Administrator",
            "Moderator",
            "User",
        };

        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach(var role in roles)
            {
                await SeedRoleAsync(roleManager, role);
            }
        }

        public static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
