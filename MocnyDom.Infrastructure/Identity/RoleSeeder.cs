using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Identity;

namespace MocnyDom.Infrastructure.Identity
{
    public static class RoleSeeder
    {
        private static readonly string[] roles = new[]
        {
            "Admin",
            "Manager",
            "User"
        };

        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
