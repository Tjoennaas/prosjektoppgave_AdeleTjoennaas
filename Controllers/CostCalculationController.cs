
          




using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CostPricingEngine.Data;
using CostPricingEngine.Dto;
using CostPricingEngine.Models.CostMargin;
using CostPricingEngine.Services;
using CostPricingEngine.Services.AzureCostCalculator;

namespace CostPricingEngine.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CostCalculationController : ControllerBase
{
    private readonly ILogger<CostCalculationController> _logger;
    private readonly CustomerCalculator _customerCalculator;
    private readonly AzureCostCalculationService _azureCostCalculationService;
    private readonly CalculationServices _calculationServices;
    private readonly CostDbContext _db;

    public CostCalculationController(
        ILogger<CostCalculationController> logger,
        CustomerCalculator customerCalculator,
        AzureCostCalculationService azureCostCalculationService,
        CalculationServices calculationServices,
        CostDbContext db) {
        _logger = logger;
        _customerCalculator = customerCalculator;
        _azureCostCalculationService = azureCostCalculationService;
        _calculationServices = calculationServices;
        _db = db;
    }

    [HttpPost("calculate-all")]
    public async Task<IActionResult> CalculateAll([FromBody] CustomerInput input) {
        var result = await _calculationServices.CalculateAndSaveAll(input);
        return Ok(result);
    }

    [HttpGet("margins")]
    public async Task<ActionResult<List<CalculationMargin>>> GetMargins() {
        var margins = await _db.CalculationMargins
            .OrderBy(x => x.CreatedAt)
            .ThenBy(x => x.PeriodNumber)
            .ToListAsync();

        return Ok(margins);
    }

    [HttpPost("azure-cost")]
    public async Task<IActionResult> CalculateAzureCost() {
        var result = await _azureCostCalculationService.CalculateAndSaveAzureCostAsync();
        return Ok(result);
    }

    [HttpPost("customer-calculator")]
    public async Task<ActionResult<List<CustomerCalculator>>> Calculate(
        [FromBody] CustomerInput input) {
        if (input == null) {
            return BadRequest("Input kan ikke være tom.");
        }

        _logger.LogInformation("Starter beregning");

        var results = await _customerCalculator.CalculateAndSaveAllAsync(input);

        return Ok(results);
    }}



          
