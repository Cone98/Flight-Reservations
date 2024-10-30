using Letovi.Web.Controllers;
using Letovi.Web.Data;
using Letovi.Web.Hubs;
using Letovi.Web.Models.Domain;
using Letovi.Web.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace Letovi.Web.Tests.Controllers
{
    public class AgentControllerTests
    {
        
        private readonly Mock<IHubContext<ReservationHub>> _mockHubContext;
        private readonly AgentController _controller;
        private readonly FlightDbContext _flightDbContext;

        public AgentControllerTests()
        {
            var options = new DbContextOptionsBuilder<FlightDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _flightDbContext = new FlightDbContext(options);
            _mockHubContext = new Mock<IHubContext<ReservationHub>>();

            
            
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _controller = new AgentController(_flightDbContext, _mockHubContext.Object)
            {
                TempData = tempData
            };
        }

        [Fact]
        public async Task Add_ValidFlight()
        {
            
            var request = new AddFlightRequest
            {
                MestoPolaska = "Beograd",
                MestoDolaska = "Nis",
                BrojPresedanja = 1,
                Datum = DateTime.UtcNow.Date.AddDays(1),
                BrojMesta = 100
            };

            
            var result = await _controller.Add(request) as RedirectToActionResult;

            
            Assert.NotNull(result);
            Assert.Equal("List", result.ActionName);
        }

        [Fact]
        public async Task Add_InvalidFlight()
        {
            
            var request = new AddFlightRequest
            {
                MestoPolaska = "Beograd",
                MestoDolaska = "Beograd", 
                BrojPresedanja = 0, 
                Datum = DateTime.UtcNow.Date.AddDays(1), 
                BrojMesta = 10 
            };

            
            var result = await _controller.Add(request) as ViewResult;

            
            Assert.NotNull(result);
            Assert.True(result.ViewData.ContainsKey("ErrorMessage"));
        }
        [Fact]
       
        public async Task List_Sucsess()
        {
            
            var flights = new List<Flight>
            {
                new Flight { Id = 1, DepartureCity = "Beograd", ArrivalCity = "Nis", SeatCount = 100, FlightDate = DateTime.UtcNow.Date.AddDays(1), NumberOfStops = 1, AvailableSeats = 100 },
                new Flight { Id = 2, DepartureCity = "Nis", ArrivalCity = "Beograd", SeatCount = 100, FlightDate = DateTime.UtcNow.Date.AddDays(2), NumberOfStops = 0, AvailableSeats = 100 }
            };
            _flightDbContext.Flights.AddRange(flights);
            await _flightDbContext.SaveChangesAsync();

            
            var result = await _controller.List() as ViewResult;

            
            Assert.NotNull(result);
            var model = result.Model as List<Flight>;
            Assert.NotNull(model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task List_Error()
        {
           
            var result = await _controller.List() as RedirectToActionResult;

            
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }

        [Fact]
        public async Task ListReservations()
        {
            
            var flight = new Flight { Id = 1, DepartureCity = "Beograd", ArrivalCity = "Nis", SeatCount = 100, FlightDate = DateTime.UtcNow.Date.AddDays(1), NumberOfStops = 1, AvailableSeats = 100 };
            var reservations = new List<Reservation>
            {
                new Reservation { Id = 1, UserId = "user1", Flight = flight, NumberOfSeats = 2, IsApproved = false },
                new Reservation { Id = 2, UserId = "user2", Flight = flight, NumberOfSeats = 3, IsApproved = true }
            };
            _flightDbContext.Flights.Add(flight);
            _flightDbContext.Reservations.AddRange(reservations);
            await _flightDbContext.SaveChangesAsync();

            
            var result = await _controller.ListReservations() as ViewResult;

            
            Assert.NotNull(result);
            var model = result.Model as List<ReservationViewModel>;
            Assert.NotNull(model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Odobri()
        {
            
            var reservation = new Reservation { Id = 1, UserId = "user1", NumberOfSeats = 2, IsApproved = false };
            _flightDbContext.Reservations.Add(reservation);
            await _flightDbContext.SaveChangesAsync();

            var mockClients = new Mock<IHubClients>();
            _mockHubContext.Setup(hub => hub.Clients).Returns(mockClients.Object);

            var mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);


            var result = await _controller.Odobri(1) as JsonResult;

            Assert.NotNull(result);
          
            // Provera da li je rezervacija ažurirana
            var updatedReservation = await _flightDbContext.Reservations.FindAsync(1);
            Assert.True(updatedReservation.IsApproved);
        }


    }
}
