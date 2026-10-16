using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class AnticipoClienteProxy : WebApiProxyClient, IAnticipoClienteProxy
	{
		private const string API_CONTROLLER = "AnticipoCliente";
		public AnticipoClienteDTO GetById(AnticipoClienteDTO dto)
		{
			var result = RestClient.PostApi<AnticipoClienteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<AnticipoClienteDTO> Search(AnticipoClienteDTO dto)
		{
			var result = RestClient.PostApi<List<AnticipoClienteDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public AnticipoClienteDTO SingleSearch(AnticipoClienteDTO dto)
		{
			var result = RestClient.PostApi<AnticipoClienteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(AnticipoClienteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(AnticipoClienteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(AnticipoClienteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public void Extornar(AnticipoClienteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Extornar"), dto);
		}
      public void Cerrar(AnticipoClienteDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Cerrar"), dto);
		}
		public void Emitir(AnticipoClienteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Emitir"), dto);
		}
		public void Aplicar(AnticipoClienteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Aplicar"), dto);
		}
		public void AnularAplicacion(AnticipoClienteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "AnularAplicacion"), dto);
		}
		public void AnularAsignacionPedido(AnticipoClienteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "AnularAsignacionPedido"), dto);
		}
		public void DividirMonto(AnticipoClienteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "DividirMonto"), dto);
		}
	}
}
