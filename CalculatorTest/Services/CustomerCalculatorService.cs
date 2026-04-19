


/*
Loggede events → Logged events
Events beholdt utover perioden de ble logget i → Events retained beyond the period they were logged in
Aktive brukere → Active users
Ekstra event collectors → Additional event collectors
Total variabel pris → Total variable cost
Grunnpris → Base price */
/*

=HVIS($B$13>10000000; (($B$13-10000000)/1000000)*$B$3;0)

=HVIS($B$12>10;($B$12-10)*$B$6;0)
/*
=HVIS($B$14>1;HVIS((KOLONNE()-KOLONNE($F$4)+1)<$B14;
((($B$13-HVIS($B$13>10000000;10000000;0))/1000000)*(KOLONNE()-KOLONNE($F$4)+1)*$B$4);
((($B$13-HVIS($B$13>10000000;10000000;0))/1000000)*($B$14-1))*$B$4);0)
*/







using ProsjektOppgave_AdeleTjoennaas.Models;


namespace ProsjektOppgave_AdeleTjoennaas.Services
{
    public class Calculator
    {
       public double CalculateEventsPerPeriod (CustomerInput input) {

         
         if (input.EventsPerPeriod <= 10000000)
            {
                return 0;
            }
            {
                return (input.EventsPerPeriod - 10000000) / 1000000 * 100;
            }}

       
              public double CalculateActiveUser (CustomerInput input) {   

                if (input.ActiveUsers <= 10)
            {
                return 0;
            }
                  return (input.ActiveUsers -10) * 10;
              }


            //Hentet fra chatGPT:
         public double CalculateRetentionCost(CustomerInput input, int periodNumber)
        {
            if (input.RetentionPeriods <= 1)
            {
                return 0;
            }

            double billableMillions =
                (input.EventsPerPeriod - Math.Min(input.EventsPerPeriod, 10000000)) / 1000000.0;

            int storedPeriods = Math.Min(periodNumber, input.RetentionPeriods - 1);

            return billableMillions * storedPeriods * 100;
        }
        
         public double CalculateTotalForPeriod(CustomerInput input, int periodNumber)
        {
            double eventCost = CalculateEventsPerPeriod(input);
            double userCost = CalculateActiveUser(input);
            double retentionCost = CalculateRetentionCost(input, periodNumber);

            return eventCost + userCost + retentionCost;
    
        
        }}}


                
                    



     










