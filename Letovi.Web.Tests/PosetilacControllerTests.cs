using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Letovi.Web.Controllers;
using Letovi.Web.Data;
using Letovi.Web.Hubs;
using Letovi.Web.Models.Domain;
using Letovi.Web.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Letovi.Web.Tests.Controllers
{
    public class PosetilacControllerTests
    {
        private readonly Mock<FlightDbContext> _mockContext;
        private readonly Mock<IHubContext<ReservationHub>> _mockHubContext;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly PosetilacController _controller;
        private readonly FlightDbContext _flightDbContext;


        public PosetilacControllerTests()
        {
            // Mock FlightDbContext using in-memory database
            var options = new DbContextOptionsBuilder<FlightDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _flightDbContext = new FlightDbContext(options);

            
            _mockHubContext = new Mock<IHubContext<ReservationHub>>();

            // Mock FlightDbContext and UserManager
            _mockContext = new Mock<FlightDbContext>(new DbContextOptions<FlightDbContext>());
            _mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(),
                null, null, null, null, null, null, null, null);

            _controller = new PosetilacController(_flightDbContext, _mockUserManager.Object, _mockHubContext.Object);
        }

        private PosetilacController CreatePosetilacController()
        {
            var tempData = new Mock<ITempDataDictionary>().Object;
            var controller = new PosetilacController(_flightDbContext, _mockUserManager.Object, _mockHubContext.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, "testUserId")
                        }))
                    }
                }
            };
            return controller;
        }

        [Fact]
        public async Task SearchData()
        {
            
            var controller = CreatePosetilacController();
            await _flightDbContext.Flights.AddRangeAsync(new List<Flight>
            {
                new Flight { DepartureCity = "Beograd", ArrivalCity = "Nis", NumberOfStops = 0, AvailableSeats = 100},
                new Flight { DepartureCity = "Beograd", ArrivalCity = "Nis", NumberOfStops = 0, AvailableSeats = 120},
                new Flight { DepartureCity = "Beograd", ArrivalCity = "Nis", NumberOfStops = 0, AvailableSeats = 60},
                new Flight { DepartureCity = "Kraljevo", ArrivalCity = "Pristina", NumberOfStops = 1, AvailableSeats=100 }
            });
            await _flightDbContext.SaveChangesAsync();

            
            var result = await controller.SearchData("Beograd", "Nis", true) as ViewResult;
            var flights = result?.Model as List<Flight>;

            
            Assert.NotNull(result);
            Assert.NotNull(flights);
            Assert.Equal(3, flights.Count);
        }

        [Fact]
        public async Task MakeReservation_Valid()
        {
           
            var controller = CreatePosetilacController();
            await _flightDbContext.Flights.AddAsync(new Flight { Id = 1, DepartureCity = "beograd", ArrivalCity = "pristina", FlightDate = DateTime.Now.AddDays(5), NumberOfStops = 0, SeatCount = 100, AvailableSeats = 50 });
            await _flightDbContext.SaveChangesAsync();

            //Zbog SignalR
            var mockClients = new Mock<IHubClients>();
            _mockHubContext.Setup(hub => hub.Clients).Returns(mockClients.Object);

            var mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            

            var result = await controller.MakeReservation(1, "testUserId", 10) as RedirectToActionResult;

            
            Assert.NotNull(result);
            Assert.Equal("ListReservations", result.ActionName);
        }

        [Fact]
        public async Task MakeReservation_InvalidSeats()
        {
            
            var controller = CreatePosetilacController();
            await _flightDbContext.Flights.AddAsync(new Flight { Id = 1, DepartureCity = "beograd", ArrivalCity = "pristina", FlightDate = DateTime.Now.AddDays(5), NumberOfStops = 0, SeatCount = 100, AvailableSeats = 5 });
            await _flightDbContext.SaveChangesAsync();

            var mockClients = new Mock<IHubClients>();
            _mockHubContext.Setup(hub => hub.Clients).Returns(mockClients.Object);

            var mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            var result = await controller.MakeReservation(1, "testUserId", 20) as RedirectToActionResult;

            
            Assert.NotNull(result);
            Assert.Equal("SearchFlights", result.ActionName);
        }

        [Fact]
        public async Task ListReservations()
        {
            
            var controller = CreatePosetilacController();
            var userId = "testUserId";
            var user = new IdentityUser { UserName = "testuser", Id = userId };

            _mockUserManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);

            await _flightDbContext.Reservations.AddRangeAsync(new List<Reservation>
            {
                new Reservation { UserId = userId, FlightId = 1, NumberOfSeats = 1, IsApproved = true, Flight = new Flight { DepartureCity = "Beograd", ArrivalCity = "Kraljevo", NumberOfStops = 1, SeatCount = 100, AvailableSeats = 99 } },
                new Reservation { UserId = userId, FlightId = 2, NumberOfSeats = 2, IsApproved = false, Flight = new Flight { DepartureCity = "Nis", ArrivalCity = "Beograd", NumberOfStops = 0, SeatCount = 150, AvailableSeats = 148 } }
            });
            await _flightDbContext.SaveChangesAsync();

            
            var result = await controller.ListReservations() as ViewResult;
            var reservations = result?.Model as List<ReservationViewModel>;

            
            Assert.NotNull(result); 
            Assert.NotNull(reservations);
            Assert.Equal(2, reservations.Count);
        }
    }
}
