using Microsoft.AspNetCore.Mvc.RazorPages;
namespace NovaHarvest.Web.Pages.Reservations;

public class TicketModel : PageModel
{
 public string ReservationsCode {get; private set; }= "";
 public string Product {get; private set; }="";

public int Kilos {get; private set;  }

public string HarvestPlace {get; private set;}="";

public DateTime GenerateDay {get; private set;}

public void OnGet()
    {
        ReservationsCode = "RES-0001";
        Product = "Cereza Picota";
        Kilos = 20;
        HarvestPlace = "Cooperativa del Valle del Jerte";
        GenerateDay = DateTime.Now;
    }
   

}