
//Default verdier lagt inn i appsettings.json noen av verdiene er ikke brukt i formelen

namespace CostPricingEngine.Models.Config {


public class  MiscSeting {

         //Average Size of Event in Kafka (bytes) = 1200
         public decimal AvgSizeOfEventInKafka{get; set;}

         
         //Average size of stored attachment (bytes) = 20000
         public decimal AvgSizeOfStoredAttachment{get; set;}

         
         //Number of events logged per month = 100000000
         public decimal EventsLoggedPerMonthCount{get; set;} 

         
         //Average number of attachments per event = 0,9
          public decimal AvgAttachmentsPerEventCount{get; set;}


         //Average size of indexed event in TX storage (bytes) = 1000
           public decimal AvgSizeOfIndexedEventdecimaTxStorage{get; set;}

         
         //Average size of event in TX storage (bytes) = 1200
           public decimal AvgSizeOfEventdecimaTxStorage{get; set;}



         //Average number of events per batch ingested = 300
           public decimal AvgEventsPerBatchIngestedCount{get; set;}



         //Average table batch fill factor when indexing = 0,5
            public decimal TableBatchFillFactor{get; set;}



        //Number of tables to write to when indexing = 3
          public decimal TablesToWriteToWhenIndexingCount{get; set;}


        //Average Months of Retention = 6
          public decimal AverageMonthsOfRetention{get; set;}


        //Average number of events per TX = 3
          public decimal AverageNumberOfEventsPerTx{get; set;}


        //CosmosDB max RUs = 2000
          public decimal CosmosDbMaxRus{get; set;}


        //CosmosDB average billed RU factor = 0,5
          public decimal CosmosDbAverageBilledRuFacto{get; set;}
               

        //Average Size of Raw Event (bytes) = 2500
          public decimal AverageSizeOfRawEvent{get; set;}


        //Employee - Num Needed Per Customer = 10
          public decimal EmployeeNumNeededPerCustomer{get; set;}


        //Seller - Commission Factor = 1,15
          public decimal SellerCommissionFactor{get; set;}


        //Currency - USD to NOK Factor = 9,6
          public decimal CurrencyUsdToNokFactor{get; set;}


        //WorkOS SSO cost per month = 125
          public decimal WorkOsSsoCostPerMonth{get; set;}


        //Simplification hedge factor = 1,2
          public decimal SimplificationHedgeFactor{get; set;}


        //Currency devaluation hedge factor = 1,15. 
          public decimal CurrencyDevaluationHedge{get; set;}
}}