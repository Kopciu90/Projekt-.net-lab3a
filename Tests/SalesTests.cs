using Komis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Controllers;

public class SalesControllerTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
    private readonly ApplicationDbContext _dbContext;
    private readonly CarsController _controller;

    public SalesControllerTests()
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
    public async Task Index_ReturnsAViewResult_WithAListOfSales()
    {
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new SalesController(context);
            context.Sales.Add(new Sale { Id = 1, CarId = 1, CustomerId = 1, DateSold = DateTime.Now });
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new SalesController(context);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Sale>>(
                viewResult.ViewData.Model);
            Assert.Single(model);
        }
    }
    [Fact]
    public async Task Details_ReturnsNotFoundResult_WhenIdIsNull()
    {
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new SalesController(context);

            var result = await controller.Details(null);

            Assert.IsType<NotFoundResult>(result);
        }
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WithSale()
    {
        int testSaleId = 123213;
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var sale = new Sale { Id = testSaleId, CarId = 1, CustomerId = 1, DateSold = DateTime.Now };
            context.Sales.Add(sale);
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new SalesController(context);

            var result = await controller.Details(testSaleId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Sale>(viewResult.ViewData.Model);
            Assert.Equal(testSaleId, model.Id);
        }
    }

    [Fact]
    public async Task Edit_Post_UpdatesSaleAndRedirectsToIndex()
    {
        int testSaleId = 12312;
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var sale = new Sale { Id = testSaleId, CarId = 1, CustomerId = 1, DateSold = DateTime.Now };
            context.Sales.Add(sale);
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new SalesController(context);
            var updatedSale = new Sale { Id = testSaleId, CarId = 2, CustomerId = 2, DateSold = DateTime.Now.AddDays(-2) };

            var result = await controller.Edit(testSaleId, updatedSale);

         
            var sale = context.Sales.Find(testSaleId);
            Assert.Equal(1, sale.CarId);
        }
    }

}


