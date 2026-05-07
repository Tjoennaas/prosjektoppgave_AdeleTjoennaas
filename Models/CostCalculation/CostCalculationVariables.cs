
//Mellom regingner og hjelpe variabler som brukes videre i formelen

namespace CostPricingEngine.Models.CostCalculation {

//public class IntermediateCostVariables 
public class CostCalculationVariables { 

public decimal TotalRetainedEventMonths{get; set;}
public decimal OneWayDataTransferGbKafka{get; set;}
public decimal BlobTxsLoggedGb{get; set;}
public decimal BlobAttachmentsLoggedGb{get; set;}
public decimal TablesLoggedGb{get; set;}

}}