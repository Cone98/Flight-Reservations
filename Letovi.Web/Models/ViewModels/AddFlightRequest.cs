namespace Letovi.Web.Models.ViewModels;


public class AddFlightRequest
{

    
    public string MestoPolaska { get; set; } =string.Empty;   
    public string MestoDolaska { get; set; } =string.Empty;
    public int BrojPresedanja { get; set; }
    public DateTime Datum { get; set; }
    public int BrojMesta {  get; set; }  

}
