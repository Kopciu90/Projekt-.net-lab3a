using Microsoft.AspNetCore.Identity;
using Komis.Models; 
using System.Linq;
using Komis.Models;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        string[] roleNames = { "Admin", "User" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var adminEmail = "admin@admin.com";
        var adminPassword = "Admin123!";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true }; 
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
              
            }
        }

        var userEmail = "user@user.com";
        var userPassword = "User123!";
        var user = await userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            user = new IdentityUser { UserName = userEmail, Email = userEmail, EmailConfirmed = true }; 
            var result = await userManager.CreateAsync(user, userPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
            }
        }



        if (!context.Cars.Any())
        {
            context.Cars.AddRange(
                new Car { Make = "Toyota", Model = "Corolla", Year = 2020, Price = 20000 },
                new Car { Make = "Ford", Model = "Focus", Year = 2019, Price = 18000 }
            );
            context.SaveChanges();
        }

        if (!context.Customers.Any())
        {
            context.Customers.AddRange(
                new Customer { FirstName = "Jan", LastName = "Kowalski", Email = "jan.kowalski@example.com" },
                new Customer { FirstName = "Anna", LastName = "Nowak", Email = "anna.nowak@example.com" }
            );
            context.SaveChanges();
        }

        if (!context.Sales.Any())
        {
            var firstCar = context.Cars.First();
            var firstCustomer = context.Customers.First();
            context.Sales.Add(
                new Sale { CarId = firstCar.Id, CustomerId = firstCustomer.Id, DateSold = DateTime.Now }
            );
            context.SaveChanges();
        }
    }
}
