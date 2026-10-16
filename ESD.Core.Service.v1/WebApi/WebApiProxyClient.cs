using Unity;

namespace ESD.Core.Service.WebApi
{
   public class WebApiProxyClient
   {
      public IRestClient RestClient { get; private set; }

      [InjectionMethod]
      public void UseRestClient(IRestClient restClient)
      {
         RestClient = restClient;
      }

   }
}
