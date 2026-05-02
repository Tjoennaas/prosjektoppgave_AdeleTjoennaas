
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProsjektOppgave_AdeleTjoennaas.Dto;
using ProsjektOppgave_AdeleTjoennaas.Services;
using ProsjektOppgave_AdeleTjoennaas.Models;
using ProsjektOppgave_AdeleTjoennaas.Data;





namespace ProsjektOppgave_AdeleTjoennaas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ILogger<CalculatorController> _logger;
        private readonly CustomerCalculator _customerCalculator;     
        private readonly PriceCalculator _priceCalculator; 
        private readonly CalculationServices _calculationServices;
      

        public CalculatorController (

                ILogger<CalculatorController> logger,
                CustomerCalculator customerCalculator,
                PriceCalculator priceCalculator,
                CalculationServices calculationServices )
            {
                _logger = logger;
                _customerCalculator = customerCalculator;
                _priceCalculator = priceCalculator;
                _calculationServices = calculationServices;
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








          
