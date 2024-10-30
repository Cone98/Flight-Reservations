using Letovi.Web.Controllers;
using Letovi.Web.Data;
using Letovi.Web.Models.Domain;
using Letovi.Web.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Letovi.Web.Tests.Controllers
{
    public class AdminControllerTests
    {
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly FlightDbContext _dbContext;
        private readonly AdminController _controller;

        public AdminControllerTests()

        {
            var options = new DbContextOptionsBuilder<FlightDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new FlightDbContext(options);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            var userStore = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                userStore.Object, null, null, null, null, null, null, null, null);

            _controller = new AdminController(_dbContext, _mockUserManager.Object)
            {
                TempData = tempData
            };
        }


        [Fact]
        public async Task NewUser_Agent()
        {
           
            var registerViewModel = new AddUserModel
            {
                Username = "testuser",
                Email = "testuser@example.com",
                Password = "Password123!",
                Role = "Agent"
            };

            _mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((IdentityUser)null);
            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(m => m.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var _controller = new AdminController(_dbContext, _mockUserManager.Object);

            var result = await _controller.NewUser(registerViewModel);

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);

            Assert.Equal("Agent je uspešno dodat!", _controller.ViewData["SuccessMessage"]);
        }
        [Fact]
        public async Task NewUser_Posetilac()
        {
            
            var registerViewModel = new AddUserModel
            {
                Username = "testuser",
                Email = "testuser@example.com",
                Password = "Password123!",
                Role = "Posetilac"
            };

            _mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((IdentityUser)null);
            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(m => m.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var _controller = new AdminController(_dbContext, _mockUserManager.Object);

            var result = await _controller.NewUser(registerViewModel);

           
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);

            Assert.Equal("Posetilac je uspešno dodat!", _controller.ViewData["SuccessMessage"]);
        }

       


        [Fact]
        public async Task ManageFlights()
        {
            var flights = new List<Flight>
            {
                new Flight { Id = 1, DepartureCity = "Beograd", ArrivalCity = "Pristina" },
                new Flight { Id = 2, DepartureCity = "Kraljevo", ArrivalCity = "Nis" }
            };

            await _dbContext.Flights.AddRangeAsync(flights);
            await _dbContext.SaveChangesAsync();

            var result = await _controller.ManageFlights() as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as List<Flight>;
            Assert.NotNull(model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task DeleteFlight_ValidId()
        {
            var flightId = 1;
            var flight = new Flight { Id = flightId, DepartureCity = "Kraljevo", ArrivalCity = "Nis" };

            await _dbContext.Flights.AddAsync(flight);
            await _dbContext.SaveChangesAsync();

            var result = await _controller.DeleteFlight(flightId);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ManageFlights", redirectResult.ActionName);
        }

        [Fact]
        public async Task DeleteFlight_InvalidId()
        {
            var flightId = 1;

            var result = await _controller.DeleteFlight(flightId);

            Assert.Equal("Neuspesno otkazivanje leta!", _controller.ViewData["ErrorMessage"]);
        }

        [Fact]
        public async Task NewUser_UserNameExist()
        {
            
            var registerViewModel = new AddUserModel
            {
                Username = "testuser",
                Email = "testuser@example.com",
                Password = "Password123!",
                Role = "Agent"
            };

           
            var existingUser = new IdentityUser
            {
                UserName = "testuser",
                Email = "testuser@example.com"
            };

            _mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                            .ReturnsAsync((string userName) => userName == existingUser.UserName ? existingUser : null);

            
            var result = await _controller.NewUser(registerViewModel);

            
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.Equal(registerViewModel, viewResult.Model);
            Assert.Equal("Korisničko ime već postoji. Pokušajte sa drugim korisničkim imenom.", _controller.ViewData["ErrorMessage"]);
        }



    }
}
