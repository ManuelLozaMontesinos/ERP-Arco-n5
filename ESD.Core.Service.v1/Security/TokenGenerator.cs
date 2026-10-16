using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ESD.Core.Service.Security
{
   public class TokenGenerator : ITokenGenerator
   {

      private readonly IConfiguration configuration;
      public TokenGenerator(IConfiguration configuration)
      {
         this.configuration = configuration;
      }
      public string GenerateTokenJwt(string userId, string userName)
      {

         TokenJwtSettings tokenJwtSettings = getConfigurationSection();

         SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(tokenJwtSettings.SecretKey));
         SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

         // create a claimsIdentity
         ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { 
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.NameIdentifier, userId)
         });

         // create token to the user
         JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
         var jwtSecurityToken = jwtSecurityTokenHandler.CreateJwtSecurityToken(
             audience: tokenJwtSettings.AudienceToken,
             issuer: tokenJwtSettings.IssuerToken,
             subject: claimsIdentity,
             //notBefore: DateTime.UtcNow,
             expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(tokenJwtSettings.ExpireTime)),
             signingCredentials: signingCredentials);

         var jwtTokenString = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

         return jwtTokenString;
      }
      public TokenValidationParameters GenerateTokenValidationParameters() {

         TokenJwtSettings tokenJwtSettings = getConfigurationSection();

         return new TokenValidationParameters()
         {
            ValidateIssuer = true,
            ValidIssuer = tokenJwtSettings.IssuerToken,
            ValidateAudience = true,
            ValidAudience = tokenJwtSettings.AudienceToken,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenJwtSettings.SecretKey)),
         };
      }
      private TokenJwtSettings getConfigurationSection() {
         IConfigurationSection configurationSection = configuration.GetSection("TokenJwt");

         TokenJwtSettings tokenJwtSettings = configurationSection.Get<TokenJwtSettings>();

         return tokenJwtSettings;
      }
   }
}