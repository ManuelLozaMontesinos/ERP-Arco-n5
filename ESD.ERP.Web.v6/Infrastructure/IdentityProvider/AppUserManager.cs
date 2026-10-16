using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESD.ERP.Web.Infrastructure.IdentityProvider
{
   public class AppUserManager : UserManager<AppIdentityUser>
   {
      private readonly IPeriodoProxy periodoProxy;

      public AppUserManager(IUserStore<AppIdentityUser> store, 
         IOptions<IdentityOptions> optionsAccessor,
         IPasswordHasher<AppIdentityUser> passwordHasher,
         IEnumerable<IUserValidator<AppIdentityUser>> userValidators,
         IEnumerable<IPasswordValidator<AppIdentityUser>> passwordValidators,
         ILookupNormalizer keyNormalizer,
         IdentityErrorDescriber errors,
         IServiceProvider services,
         ILogger<UserManager<AppIdentityUser>> logger,
         IPeriodoProxy periodoProxy
         ) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
      {
         this.periodoProxy = periodoProxy;
      }
      public override Task<AppIdentityUser> FindByNameAsync(string userName)
      {
         return base.FindByNameAsync(userName);
      }
   }
}
