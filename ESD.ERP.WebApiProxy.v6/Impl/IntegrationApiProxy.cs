using ESD.Core.Service.WebApi;

namespace ESD.ERP.WebApiProxy.Impl
{
   public class IntegrationApiProxy : WebApiProxyClient, IIntegrationApiProxy
	{
		private const string API_KEY_NAME_SUNAT = "padronsunat";

		public string ObtenerPorRuc(string ruc)
		{
			return RestClient.GetApi<string>(API_KEY_NAME_SUNAT, WebApiMethod.Create("Ruc", string.Empty), ruc);
		}
   }
}