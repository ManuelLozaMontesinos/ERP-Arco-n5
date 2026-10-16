using ESD.Core.Service;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class SeguridadProxy : WebApiProxyClient, ISeguridadProxy
	{
		private const string API_CONTROLLER = "Seguridad";
		public ServiceResponse Autenticar(UsuarioDTO dto)
		{
			var result = RestClient.PostApi<ServiceResponse>(WebApiMethod.Create(API_CONTROLLER, "Autenticar"), dto);
			return result;
		}

	}
}
