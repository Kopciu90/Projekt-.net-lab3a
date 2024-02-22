using Komis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Controllers;

public class CustomerControllerTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
    private readonly ApplicationDbContext _dbContext;
    private readonly CarsController _controller;

    public CustomerControllerTests()
    {
        // Konfiguracja DbContextOptions dla fałszywej bazy danych w pamięci
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb") // Używa bazy danych w pamięci
            .Options;

        // Stworzenie instancji ApplicationDbContext z opcjami
        _dbContext = new ApplicationDbContext(_dbContextOptions);

        // Stworzenie instancji CarsController
        _controller = new CarsController(_dbContext);
    }

    [Fact]
    public async Task Index_ReturnsAViewResult_WithAListOfCustomers()
    {
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new CustomersController(context);
            context.Customers.Add(new Customer { Id = 1, FirstName = "TestFirstName1", LastName = "TestLastName1", Email = "test1@test.com" });
            context.Customers.Add(new Customer { Id = 2, FirstName = "TestFirstName2", LastName = "TestLastName2", Email = "test2@test.com" });
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new CustomersController(context);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Customer>>(
                viewResult.ViewData.Model);
        }

    }
    [Fact]
    public async Task Details_ReturnsNotFoundResult_WhenIdIsNull()
    {
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new CustomersController(context);

            var result = await controller.Details(null);

            Assert.IsType<NotFoundResult>(result);
        }
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WithCustomer()
    {
        int testCustomerId = 128;
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            context.Customers.Add(new Customer { Id = testCustomerId, FirstName = "TestFirstName", LastName = "TestLastName", Email = "test@test.com" });
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new CustomersController(context);

            var result = await controller.Details(testCustomerId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Customer>(viewResult.ViewData.Model);
            Assert.Equal(testCustomerId, model.Id);
        }
    }
    [Fact]
    public async Task Create_Post_AddsCustomerAndRedirectsToIndex()
    {
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new CustomersController(context);
            var newCustomer = new Customer { Id = 213, FirstName = "NewFirstName", LastName = "NewLastName", Email = "new@test.com" };

            var result = await controller.Create(newCustomer);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("NewFirstName", context.Customers.Single().FirstName);
        }
    }
    [Fact]
    public async Task DeleteConfirmed_DeletesCustomerAndRedirectsToIndex()
    {
        int testCustomerId = 12321;
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            context.Customers.Add(new Customer { Id = testCustomerId, FirstName = "FirstName", LastName = "LastName", Email = "test@test.com" });
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new CustomersController(context);

            var result = await controller.DeleteConfirmed(testCustomerId);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.False(context.Customers.Any(c => c.Id == testCustomerId));
        }
    }

}


