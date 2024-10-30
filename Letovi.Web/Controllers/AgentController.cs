using Letovi.Web.Data;
using Letovi.Web.Hubs;
using Letovi.Web.Models.Domain;
using Letovi.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace Letovi.Web.Controllers
{

    [Authorize(Roles = "Agent")]
    public class AgentController : Controller
    {
        private readonly FlightDbContext flightDbContext;
        private readonly IHubContext<ReservationHub> _hubContext;

        public AgentController(FlightDbContext flightDbContext, IHubContext<ReservationHub> hubContext)
        {
            this.flightDbContext = flightDbContext;
            _hubContext = hubContext;
        }

        [Authorize(Roles = "Agent")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddFlightRequest addLetRequest)
        {
            
            var let = new Flight
            {
                DepartureCity = addLetRequest.MestoPolaska,
                ArrivalCity = addLetRequest.MestoDolaska,
                NumberOfStops = addLetRequest.BrojPresedanja,
                FlightDate = addLetRequest.Datum,
                SeatCount = addLetRequest.BrojMesta,
                AvailableSeats = addLetRequest.BrojMesta
            };
            
                if (let != null && addLetRequest.MestoDolaska != addLetRequest.MestoPolaska &&
                    addLetRequest.BrojMesta > 0 &&
                    addLetRequest.Datum >= DateTime.UtcNow.Date && ModelState.IsValid)
                {

                    flightDbContext.Flights.Add(let);
                    await flightDbContext.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Uspesno dodavanje leta!";
                    return RedirectToAction("List");
               }
            
            else
            {
                ViewData["ErrorMessage"] = "Neuspesno dodavanje leta, proverite datum i ostala polja!";
                var model = new AddFlightRequest();
                return View(model);
            }
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            try
            {
                var flights = await flightDbContext.Flights.ToListAsync();
                if (flights != null && flights.Count > 0)
                {
                    return View(flights);
                }
                else
                {
                    TempData["ErrorMessage"] = "Nema dostupnih letova za prikaz.";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Doslo je do greske u Prikazu letova!";
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        [ActionName("ManageReservations")]
        public async Task<IActionResult> ListReservations()
        {
            var res = await flightDbContext.Reservations
                                            .Include(r => r.Flight)
                                            .Select(r => new ReservationViewModel
                                            {
                                                Id = r.Id,                                                
                                                Flight = r.Flight,
                                                NumberOfSeats = r.NumberOfSeats,
                                                isApproverd = r.IsApproved
                                            })
                                            .ToListAsync();
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Odobri(int id)
        {
            var res = await flightDbContext.Reservations.FirstOrDefaultAsync(x => x.Id == id);
            if (res != null)
            {
                res.IsApproved = true;
                await flightDbContext.SaveChangesAsync();

                var reservationDetails = new ReservationViewModel
                {
                    
                    Id = res.Id,
                    Flight=res.Flight,
                    NumberOfSeats = res.NumberOfSeats,
                    isApproverd = res.IsApproved
                };
                var json = JsonConvert.SerializeObject(reservationDetails);

                await _hubContext.Clients.All.SendAsync("ReceiveReservationUpdate", json);
            }
            return Json(new { success = true, message = "Reservation approved successfully" });
        }
    }

}



