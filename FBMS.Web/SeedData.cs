using FBMS.Core.Entities;
using FBMS.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Web
{
    public static class SeedData
    {
        public static readonly ToDoItem ToDoItem1 = new ToDoItem
        {
            Title = "Get Sample Working",
            Description = "Try to get the sample to build."
        };
        public static readonly ToDoItem ToDoItem2 = new ToDoItem
        {
            Title = "Review Solution",
            Description = "Review the different projects in the solution and how they relate to one another."
        };
        public static readonly ToDoItem ToDoItem3 = new ToDoItem
        {
            Title = "Run and Review Tests",
            Description = "Make sure all the tests run and review what they are doing."
        };

        public static void Initialize(IServiceProvider serviceProvider)
        {
            //var dbContext = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null);
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            PopulateRoleData(roleManager);
            PopulateUserData(userManager, roleManager);
        }

        public static void PopulateTestData(AppDbContext dbContext)
        {
            foreach (var item in dbContext.ToDoItems)
            {
                dbContext.Remove(item);
            }
            dbContext.SaveChanges();
            dbContext.ToDoItems.Add(ToDoItem1);
            dbContext.ToDoItems.Add(ToDoItem2);
            dbContext.ToDoItems.Add(ToDoItem3);

            dbContext.SaveChanges();
        }

        public static void PopulateRoleData(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        public static void PopulateUserData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (userManager.FindByNameAsync("Admin").Result == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = "Admin";
                user.Email = "thihakyawhtin.mm@gmail.com";
                user.PhoneNumber = "95424432870";
                IdentityResult userResult = userManager.CreateAsync(user, "welCome123!@#").Result;

                var adminUser = userManager.FindByNameAsync(user.UserName).Result;
                var role = roleManager.FindByNameAsync(user.UserName).Result;
                if (role != null)
                {
                    IdentityResult adminUserResult = userManager.AddToRoleAsync(adminUser, role.Name).Result;
                }
            }
        }
    }
}
