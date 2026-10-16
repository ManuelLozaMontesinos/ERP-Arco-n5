using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ListaPrecioProveedorProductoProxy : WebApiProxyClient, IListaPrecioProveedorProductoProxy
	{
		private const string API_CONTROLLER = "ListaPrecioProveedorProducto";
		public ListaPrecioProveedorProductoDTO GetById(ListaPrecioProveedorProductoDTO dto)
		{
			var result = RestClient.PostApi<ListaPrecioProveedorProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ListaPrecioProveedorProductoDTO> Search(ListaPrecioProveedorProductoDTO dto)
		{
			var result = RestClient.PostApi<List<ListaPrecioProveedorProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ListaPrecioProveedorProductoDTO SingleSearch(ListaPrecioProveedorProductoDTO dto)
		{
			var result = RestClient.PostApi<ListaPrecioProveedorProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ListaPrecioProveedorProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ListaPrecioProveedorProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ListaPrecioProveedorProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public string ListarSaldoAlmacenGrid(ListaPrecioProveedorProductoDTO dto)
		{
			var result = RestClient.PostApi<string>(WebApiMethod.Create(API_CONTROLLER, "ListarSaldoAlmacenGrid"), dto);
			return result;
		}
	}
}
