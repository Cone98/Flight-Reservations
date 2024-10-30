using Newtonsoft.Json;

namespace Letovi.Web.Models.Domain
{
    public class Flight
    {
        public int Id { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public DateTime FlightDate { get; set; }
        public int NumberOfStops { get; set; }
        public int SeatCount { get; set; }
        public int AvailableSeats { get; set; }
        [JsonIgnore]
        public ICollection<Reservation> Reservations { get; set; }
    }
}
