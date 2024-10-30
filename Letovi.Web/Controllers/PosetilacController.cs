using Letovi.Web.Data;
using Letovi.Web.Models.Domain;
using Letovi.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Letovi.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Letovi.Web.Controllers
{
    [Authorize(Roles = "Posetilac")]
    public class PosetilacController : Controller
    {   //Ažuriranje kontrolera da koristi SignalR hub
        private readonly IHubContext<ReservationHub> _hubContext;
        private readonly FlightDbContext flightDbContext;
        private readonly UserManager<IdentityUser> userManager;

        public PosetilacController(FlightDbContext flightDbContext, UserManager<IdentityUser> userManager, IHubContext<ReservationHub> hubContext)
        {
            this.flightDbContext = flightDbContext;
            this.userManager = userManager;
            _hubContext = hubContext;
        }
        [HttpGet]
        [ActionName("SearchFlights")]
        public async Task<IActionResult> SearchData(string searchString, string searchString2, bool noStops)
        {

            var flights = await flightDbContext.Flights.ToListAsync();
            //ako polja za pretragu nisu prazna vrsi proveru da li je chekirano dugme za presedanja
            if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(searchString2))
            {
                if (noStops)
                {
                    flights = await flightDbContext.Flights
                        .Where(x => x.DepartureCity.Equals(searchString) && x.ArrivalCity.Equals(searchString2)
                        && x.NumberOfStops == 0 && x.AvailableSeats>0)
                        .ToListAsync();
                }
                else
                {
                    flights = await flightDbContext.Flights
                        .Where(x => x.DepartureCity.Equals(searchString) && x.ArrivalCity.Equals(searchString2)
                        && x.AvailableSeats > 0)
                        .ToListAsync();
                }
            }

            //ako su prazna izbacice sve letove bez presedanja ako je dugme chekirano
            else if (String.IsNullOrEmpty(searchString) && String.IsNullOrEmpty(searchString2))
            {
                if (noStops)
                {
                    flights = await flightDbContext.Flights
                        .Where(x => x.NumberOfStops == 0)
                        .ToListAsync();
                }
            }
            
            return View(flights);
        }


        [HttpPost]
        [ActionName("MakeReservation")]
        public async Task<IActionResult> MakeReservation(int id, string uid, int numOfSeats)
        {
            //pronalazi let preko Id-a
            var flight = await flightDbContext.Flights.FirstOrDefaultAsync(x => x.Id == id);

            if (flight == null || numOfSeats > flight.AvailableSeats)
            {
                TempData["ErrorMessage"] = "Doslo je do greske pri rezervaciji!";
                return RedirectToAction("SearchFlights");
            }
            if(flight.FlightDate.Subtract(DateTime.Now).Days < 3)
            {
                TempData["ErrorMessage"] = "Do leta je ostalo manje od tri dana, nije moguce napraviti rezervaciju";
                return RedirectToAction("SearchFlights");
            }
           

            //pravim rezervaciju pomocu prosledjenih parametara id, userid(uid), i broja zeljenih mesta numofSeats
            var reservation = new Reservation
            {
                FlightId = id,
                Flight = flight,
                UserId = uid,
                NumberOfSeats = numOfSeats,
                IsApproved = false,
            };




            //skidam dostupna mesta sa leta kada se rezervacija kreira
            flight.AvailableSeats -= numOfSeats;
            await flightDbContext.Reservations.AddAsync(reservation);
            await flightDbContext.SaveChangesAsync();

            //pravim viewmodel da bih ga serializovao u json i prosledio preko huba informaciju klijent strani
            
            var res = new ReservationViewModel
            {
                Flight = flight,
                Id = reservation.Id,
                NumberOfSeats = numOfSeats,
                isApproverd = false,
            };
            var json = JsonConvert.SerializeObject(res);
            
            if (_hubContext == null)
            {
                TempData["ErrorMessage"] = "Hub context nije inicijalizovan.";
                return RedirectToAction(nameof(MakeReservation));
            }
            else
            {
                try
                {
                    //poziv SignalR hub da obavesti klijente o promeni.
                    await _hubContext.Clients.All.SendAsync("ReceiveReservationUpdate", json);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error poruka: {ex.Message}";
                    return RedirectToAction(nameof(MakeReservation));
                }
            }

            TempData["SuccessMessage"] = "Rezervacija kreirana uspešno";
            return RedirectToAction(nameof(ListReservations));
        }

        [HttpGet]
        [ActionName("ListReservations")]
        public async Task<IActionResult> ListReservations()
        {
            var userID = userManager.GetUserId(User);
            // prikazuje sve rezervacije trenutnog korisnika, povezuje sa letove preko flightid radi punog prikaza
            var res = await flightDbContext.Reservations
                .Where(r => r.UserId == userID)
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
    }

    
}
