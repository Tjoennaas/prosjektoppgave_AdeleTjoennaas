


using Microsoft.EntityFrameworkCore;
using ProsjektOppgave_AdeleTjoennaas.Models;


namespace ProsjektOppgave_AdeleTjoennaas.Data {

public class PriceAzureContext : DbContext {  

public PriceAzureContext(DbContextOptions<PriceAzureContext> options)
              : base(options) { }

     public DbSet<AzurePrice> AzurePrices { get; set; }
}}
    

