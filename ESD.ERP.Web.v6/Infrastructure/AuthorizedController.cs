using Microsoft.AspNetCore.Authorization;

namespace ESD.ERP.Web.Infrastructure
{
   [Authorize]
   public class AuthorizedController: DataController
   {
   }
}
