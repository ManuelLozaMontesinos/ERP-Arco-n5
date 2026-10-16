using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoVentaClienteProxy : WebApiProxyClient, ITipoVentaClienteProxy
	{
		private const string API_CONTROLLER = "TipoVentaCliente";
		public TipoVentaClienteDTO GetById(TipoVentaClienteDTO dto)
		{
			var result = RestClient.PostApi<TipoVentaClienteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoVentaClienteDTO> Search(TipoVentaClienteDTO dto)
		{
			var result = RestClient.PostApi<List<TipoVentaClienteDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoVentaClienteDTO SingleSearch(TipoVentaClienteDTO dto)
		{
			var result = RestClient.PostApi<TipoVentaClienteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoVentaClienteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoVentaClienteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoVentaClienteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
