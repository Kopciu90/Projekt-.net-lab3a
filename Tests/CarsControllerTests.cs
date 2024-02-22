using Komis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Controllers;

public class CarsControllerTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
    private readonly ApplicationDbContext _dbContext;
    private readonly CarsController _controller;

    public CarsControllerTests()
    {
        // Konfiguracja DbContextOptions dla fa³szywej bazy danych w pamiêci
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb") // U¿ywa bazy danych w pamiêci
            .Options;

        // Stworzenie instancji ApplicationDbContext z opcjami
        _dbContext = new ApplicationDbContext(_dbContextOptions);

        // Stworzenie instancji CarsController
        _controller = new CarsController(_dbContext);
    }

    [Fact]
    public async Task Details_ReturnsNotFoundResult_WhenCarDoesNotExist()
    {
        // Arrange
        var nonExistingId = 9999;

        // Act
        var result = await _controller.Details(nonExistingId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    [Fact]
    public async Task Index_ReturnsAViewResult_WithAListOfCars()
    {
        // Arrange
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new CarsController(context);
            context.Cars.Add(new Car { Id = 1, Make = "TestMake1", Model = "TestModel1", Year = 2020, Price = 10000 });
            context.Cars.Add(new Car { Id = 2, Make = "TestMake2", Model = "TestModel2", Year = 2021, Price = 20000 });
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new CarsController(context);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Car>>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

    }
    [Fact]
    public async Task Create_Post_AddsCarAndRedirectsToIndex()
    {
        // Arrange
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new CarsController(context);
            var newCar = new Car { Id = 99, Make = "NewMake", Model = "NewModel", Year = 2022, Price = 30000 };

            // Act
            var result = await controller.Create(newCar);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

    }
    [Fact]
    public async Task Edit_Post_UpdatesCarAndRedirectsToIndex()
    {
        // Arrange
        int testCarId = 999;
        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            context.Cars.Add(new Car { Id = testCarId, Make = "OldMake", Model = "OldModel", Year = 2020, Price = 10000 });
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var controller = new CarsController(context);
            var updatedCar = new Car { Id = testCarId, Make = "UpdatedMake", Model = "UpdatedModel", Year = 2021, Price = 11000 };

            // Act
            var result = await controller.Edit(testCarId, updatedCar);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            var car = context.Cars.Find(testCarId);
            Assert.Equal("UpdatedMake", car.Make);
            Assert.Equal("UpdatedModel", car.Model);
        }
    }

}
