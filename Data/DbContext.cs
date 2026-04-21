


using Microsoft.EntityFrameworkCore;
using ProsjektOppgave_AdeleTjoennaas.Models;


namespace ProsjektOppgave_AdeleTjoennaas.Data {

public class PriceDbContext : DbContext {  

public PriceDbContext(DbContextOptions<PriceDbContext> options)
              : base(options) { }

     public DbSet<AzurePrice> AzurePrices { get; set; }
}}
    

