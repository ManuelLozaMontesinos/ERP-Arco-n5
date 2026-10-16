namespace ESD.Core.Service.Security
{
   public class TokenJwtSettings
   {
      public string SecretKey { get; set; }
      public string AudienceToken { get; set; }
      public string IssuerToken { get; set; }
      public string ExpireTime { get; set; }
   }
}
