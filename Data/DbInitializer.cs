using Lab1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Data
{
    public static class DbInitializer
    {

        public static AppSecrets appSecrets = new AppSecrets();
        public static async Task<int> SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            // create the database if it doesn't exist
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Check if roles already exist and exit if there are
            if (roleManager.Roles.Count() > 0)
                return 1;  // should log an error message here

            // Seed roles
            int result = await SeedRoles(roleManager);
            if (result != 0)
                return 2;  // should log an error message here

            // Check if users already exist and exit if there are
            if (userManager.Users.Count() > 0)
                return 3;  // should log an error message here

            // Seed users
            result = await SeedUsers(userManager);
            if (result != 0)
                return 4;  // should log an error message here

            return 0;
        }

        private static async Task<int> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Create Manager Role
            var result = await roleManager.CreateAsync(new IdentityRole("Manager"));
            if (!result.Succeeded)
                return 1;  // should log an error message here

            // Create Player Role
            result = await roleManager.CreateAsync(new IdentityRole("Player"));
            if (!result.Succeeded)
                return 2;  // should log an error message here

            return 0;
        }

        private static async Task<int> SeedUsers(UserManager<ApplicationUser> userManager)
        {
            // Create Manager User
            var managerUser = new ApplicationUser
            {
                UserName = "the_manager@mohawkcollege.ca",
                Email = "the_manager@mohawkcollege.ca",
                FirstName = "Manager Name",
                LastName = "Manager Last Name",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(managerUser, appSecrets.ManagerPassword);
            if (!result.Succeeded)
                return 1;  // should log an error message here

            // Assign user to Admin role
            result = await userManager.AddToRoleAsync(managerUser, "Manager");
            if (!result.Succeeded)
                return 2;  // should log an error message here

            // Create Player User
            var playerUser = new ApplicationUser
            {
                UserName = "the_player@mohawkcollege.ca",
                Email = "the_player@mohawkcollege.ca",
                FirstName = "Player Name",
                LastName = "Player last name",
                EmailConfirmed = true
            };
            result = await userManager.CreateAsync(playerUser, appSecrets.PlayerPassword);
            if (!result.Succeeded)
                return 3;  // should log an error message here

            // Assign user to Member role
            result = await userManager.AddToRoleAsync(playerUser, "Player");
            if (!result.Succeeded)
                return 4;  // should log an error message here

            return 0;
        }
    }
}
