





using ProsjektOppgave_AdeleTjoennaas.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProsjektOppgave_AdeleTjoennaas.Controllers
{
    [ApiController]
    [Route("[controller]")]

public class PriceController : ControllerBase {

private readonly PriceDbContext _db;
public PriceController(PriceDbContext db)
        {
            
            _db = db;
        }

[HttpGet("Azure-price")]

 public async Task<IActionResult> GetAll()
{
     var azurePrices = await _db.AzurePrices.ToListAsync();
        return Ok(azurePrices);


}
};}










