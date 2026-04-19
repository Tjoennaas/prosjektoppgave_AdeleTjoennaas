
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProsjektOppgave_AdeleTjoennaas.Models;
using ProsjektOppgave_AdeleTjoennaas.Services;



namespace ProsjektOppgave_AdeleTjoennaas.Controllers {

    [ApiController]
    [Route("api/[controller]")]

    public class CatchController : ControllerBase{
    private readonly ILogger<CatchController> _logger;
    private readonly Calculator _calculator;

    public CatchController(ILogger<CatchController> logger, Calculator calculator)

    {
     
      _logger = logger;
      _calculator = calculator;
 
    }

    [HttpPost ("Customer-calulator")]

    public ActionResult<List<CustomerCalculationResult>> Calculate(CustomerInput input)
    {
        
        var results = new List<CustomerCalculationResult>();

        for (int i = 1; i <= input.RetentionPeriods; i++){
            
            results.Add(new CustomerCalculationResult
            { 
                Period = i, 
                TotalPrice = _calculator.CalculateTotalForPeriod(input, i)
                });}
        return Ok(results);
    }
}}

   
