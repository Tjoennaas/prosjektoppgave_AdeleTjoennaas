
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProsjektOppgave_AdeleTjoennaas.Dto;
using ProsjektOppgave_AdeleTjoennaas.Services;



namespace ProsjektOppgave_AdeleTjoennaas.Controllers {

    [ApiController]
    [Route("api/[controller]")]

    public class CalculatorController : ControllerBase{
    private readonly ILogger<CalculatorController> _logger;
    private readonly Calculator _calculator;

    public CalculatorController(ILogger<CalculatorController> logger, Calculator calculator)

    {
     
      _logger = logger;
      _calculator = calculator;
 
    }

    [HttpPost ("Customer-calulator")]

    public ActionResult<List<CustomerCalculation>> Calculate(CustomerInput input)
    {
        var results = new List<CustomerCalculation>();

        
    for (int periodNumber = 1; periodNumber <= input.RetentionPeriods; periodNumber++)
       
   {
                var result = _calculator.CalculateForPeriod(input, periodNumber);
                results.Add(result);
            }

            return Ok(results);
        }
    };}



   
