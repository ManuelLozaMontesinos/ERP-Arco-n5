using Microsoft.AspNetCore.Authorization;

namespace ESD.ERP.WebApi.Infrastructure
{
   [Authorize]
   public class AuthorizedController: BaseController
   {
   }
}