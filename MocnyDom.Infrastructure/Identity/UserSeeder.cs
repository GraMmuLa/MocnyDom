using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Identity;

namespace MocnyDom.Infrastructure.Identity
{
    public static class UserSeeder
    {
        public static async Task SeedAdmin(UserManager<IdentityUser> userManager)
        {
            var adminEmail = "admin@mocnydom.local";
            var adminPass = "Admin123!";

            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                await userManager.CreateAsync(admin, adminPass);
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
