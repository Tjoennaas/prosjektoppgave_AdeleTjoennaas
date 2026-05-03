
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProsjektOppgave_AdeleTjoennaas.Dto;
using ProsjektOppgave_AdeleTjoennaas.Services;
using ProsjektOppgave_AdeleTjoennaas.Models;

using CostPrices.Data;



namespace CostCalculation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CostCalculationController: ControllerBase
    {
        private readonly ILogger<CostCalculationController> _logger;
        private readonly CustomerCalculator _customerCalculator;     
        private readonly PriceCalculator _priceCalculator; 
        private readonly CalculationServices _calculationServices;
        private readonly CostDbContext _db;

        public CostCalculationController (

                ILogger<CostCalculationController> logger,
                CustomerCalculator customerCalculator,
                PriceCalculator priceCalculator,
                CalculationServices calculationServices,
                CostDbContext db)
            {
                _logger = logger;
                _customerCalculator = customerCalculator;
                _priceCalculator = priceCalculator;
                _calculationServices = calculationServices;
                _db = db;
            }

            
            
            [HttpPost("calculate-all")]
                        public async Task<IActionResult> CalculateAll([FromBody] CustomerInput input)
                        {
                            try
                            {
                                var result = await _calculationServices.CalculateAndSaveAll(input);
                                return Ok(result);
                            }
                            catch (Exception ex)
                            {
                                return StatusCode(500, ex.ToString());
                            }
}

[HttpGet("margins")]
public async Task<ActionResult<List<CalculationMargin>>> GetMargins()
{
    var margins = await _db.CalculationMargins
        .OrderBy(x => x.CreatedAt)
        .ThenBy(x => x.PeriodNumber)
        .ToListAsync();

    return Ok(margins);
}


            [HttpPost("azure-cost")]
                    public async Task<IActionResult> CalculateAzureCost() {
                            try
                            {
                                var result = await _priceCalculator.CalculateAndSaveAzureCostAsync();
                                return Ok(result);
                            }
                            catch (Exception ex)
                            {
                                return StatusCode(500, ex.ToString());
                            } }
            [HttpPost("Customer-calculator")]
public async Task<ActionResult<List<CustomerCalculationResult>>> Calculate([FromBody] CustomerInput input)
{
    if (input == null)
    {
        return BadRequest("Input kan ikke være tom.");
    }

    _logger.LogInformation("Starter beregning");

    var results = await _customerCalculator.CalculateAndSaveAllAsync(input);

    return Ok(results);
}};}








          
