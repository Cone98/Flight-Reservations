
using Letovi.Web.Models.Domain;

namespace Letovi.Web.Models.ViewModels

{
    public class ReservationViewModel
    {
        
        public int Id { get; set; }
        public Flight Flight{ get; set; }    
      
        public int NumberOfSeats { get; set; }
        public bool isApproverd { get; set; }
        
    }


}

