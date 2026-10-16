namespace ESD.Core.Service.Security
{
   public interface ITokenGenerator
   {
      string GenerateTokenJwt(string userId, string userName);
   }
}
