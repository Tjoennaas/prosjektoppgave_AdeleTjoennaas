


            using Microsoft.AspNetCore.Mvc;
            using Microsoft.EntityFrameworkCore;
            using CostPricingEngine.Data;


//Denne koden er ikke til klient men for at informasjonen skal være tilgjenglig i nettleseren
    namespace CostPricingEngine.Controller {

            [ApiController]
            [Route("api/azure-prices")]
            public class AzurePriceController : ControllerBase {

            private readonly CostDbContext _db;
            public AzurePriceController(CostDbContext db) {
                        
                 _db = db; }

            [HttpGet]

            public async Task<IActionResult> GetAll() {

                var azurePrices = await _db.AzureApiPricesDto.ToListAsync();
                return Ok(azurePrices);      
    }};}










