namespace ESD.ERP.IntegrationApi.Infrastructure
{
   public class ServiceResponse
   {
      public int ResponseCode { get; set; }
      public string Message { get; set; }
      public object Data { get; set; }
      public bool Success { get; set; } = true;
   }
}
