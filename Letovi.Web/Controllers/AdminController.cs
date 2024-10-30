using Letovi.Web.Data;
using Letovi.Web.Models.Domain;
using Letovi.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Letovi.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly FlightDbContext flightDbContext;

        public AdminController(FlightDbContext flightDbContext, UserManager<IdentityUser> userManager)
        {
            this.flightDbContext = flightDbContext;
            this.userManager = userManager;
        }

        [HttpGet]
        
        public IActionResult NewUser()
        {
            return View();
        }

        [HttpPost]
        [ActionName("NewUser")]
        public async Task<IActionResult> NewUser(AddUserModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Model state nije validan.";
                return View(registerViewModel);
            }

            var existingUser = await userManager.FindByNameAsync(registerViewModel.Username);
            if (existingUser != null)
            {
                ViewData["ErrorMessage"] = "Korisničko ime već postoji. Pokušajte sa drugim korisničkim imenom.";
                return View(registerViewModel);
            }

            var identityUser = new IdentityUser
            {
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email
            };
            //dodavanje novog korisnika ako ne postoji u bazi 
            var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);
            if (identityResult.Succeeded)
            {
                //dodeljivanje uloge posetioca korisniku
                IdentityResult roleIdentityResult;
                if (registerViewModel.Role.Equals("Posetilac"))
                {
                    roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "Posetilac");
                    if (roleIdentityResult.Succeeded)
                    {
                        ViewData["SuccessMessage"] = "Posetilac je uspešno dodat!";

                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Neuspešno dodavanje role posetioca.";
                        return View(registerViewModel);
                    }
                }
                else
                {
                    //dodeljivanje uloge agenta
                    roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "Agent");
                    if (roleIdentityResult != null && roleIdentityResult.Succeeded)
                    {
                        ViewData["SuccessMessage"] = "Agent je uspešno dodat!";
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Neuspešno dodavanje role agenta.";
                        return View(registerViewModel);
                    }
                }
                ModelState.Clear();
                return View(new AddUserModel());
            }
            else
            {
               
                ViewData["ErrorMessage"] = "Šifra mora sadržati malo, veliko slovo, broj i nonAlphaNum karakter!";
                return View(registerViewModel);
            }
        }
        //prikazivanje letova
        [HttpGet]
        [ActionName("ManageFlights")]
        public async Task<IActionResult> ManageFlights()
        {
            var flights = await flightDbContext.Flights.ToListAsync();
            if (flights != null)
            {
                return View(flights);
            }
            else
            {
                TempData["ErrorMessage"] = "Doslo je do greske u Prikazu letova!";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ActionName("DeleteFlight")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            //dohvatanje leta po Id
            var let = await flightDbContext.Flights.FindAsync(id);
            if (let != null)
            {
                //dohvatanje rezervacija koje su povezane sa ovim letom preko flightid
                //jer kada otkazujem let treba otkazati i sve rezervacije za taj let
                var reservations = await flightDbContext.Reservations.Where(r => r.FlightId == id).ToListAsync();
                if (reservations.Count > 0 )
                {
                   flightDbContext.Reservations.RemoveRange(reservations);
                }

                flightDbContext.Flights.Remove(let);
                await flightDbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = "Uspesno otkazivanje leta i njegovih rezervacija!";

                return RedirectToAction("ManageFlights");
            }
            else
            {
                ViewData["ErrorMessage"] = "Neuspesno otkazivanje leta!";
                return RedirectToAction("ManageFlights");
            }
        }
    }
}
