using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ProveedorListaPrecioProductoProxy : WebApiProxyClient, IProveedorListaPrecioProductoProxy
	{
		private const string API_CONTROLLER = "ProveedorListaPrecioProducto";
		public ProveedorListaPrecioProductoDTO GetById(ProveedorListaPrecioProductoDTO dto)
		{
			var result = RestClient.PostApi<ProveedorListaPrecioProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ProveedorListaPrecioProductoDTO> Search(ProveedorListaPrecioProductoDTO dto)
		{
			var result = RestClient.PostApi<List<ProveedorListaPrecioProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ProveedorListaPrecioProductoDTO SingleSearch(ProveedorListaPrecioProductoDTO dto)
		{
			var result = RestClient.PostApi<ProveedorListaPrecioProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ProveedorListaPrecioProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ProveedorListaPrecioProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ProveedorListaPrecioProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public void ActualizarLote(List<ProveedorListaPrecioProductoDTO> list)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarLote"), list);
		}
	}
}