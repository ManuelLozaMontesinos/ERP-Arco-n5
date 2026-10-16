using System.Web.Http;

namespace ESD.ERP.IntegrationApi
{
   public class Global : System.Web.HttpApplication
   {

      protected void Application_Start()
      {
         GlobalConfiguration.Configure(WebApiConfig.Register);
      }
   }
}