


            using Microsoft.AspNetCore.Mvc;
            using Microsoft.EntityFrameworkCore;
            using CostPricingEngine.Data;



    namespace CostPricingEngine.Controller {
                [ApiController]
                [Route("[controller]")]

            public class AzurePriceController : ControllerBase {

            private readonly CostDbContext _db;
            public AzurePriceController(CostDbContext db) {
                        
                 _db = db; }

            [HttpGet("Azure-price")]

            public async Task<IActionResult> GetAll() {

                var azurePrices = await _db.AzureApiPricesDto.ToListAsync();
                return Ok(azurePrices);      
    }};}










