using Letovi.Web.Models.Domain;
using Letovi.Web.Models.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Letovi.Web.Hubs
{
   // SignalR hub koji će upravljati komunikacijom između servera i klijenta.
    public class ReservationHub : Hub
    {
        public async Task SendReservationUpdate(ReservationViewModel res)
        {
            await Clients.All.SendAsync("ReceiveReservationUpdate", res);
        }
    }
}