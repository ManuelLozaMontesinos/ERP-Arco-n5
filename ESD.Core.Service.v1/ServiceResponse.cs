namespace ESD.Core.Service
{
   public class ServiceResponse
   {
      public int ResponseCode { get; set; }
      public string Message { get; set; }
      public object Data { get; set; }
      public bool Success { get; set; } = true;
      public string Token { get; set; }
   }
}
