using System.Collections.Generic;
namespace Letovi.Web.Models.Domain
{
    public class Reservation
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public Flight Flight { get; set; }
        public string UserId { get; set; }
       
        public int NumberOfSeats { get; set; }
        public bool IsApproved { get; set; }

    }
}
