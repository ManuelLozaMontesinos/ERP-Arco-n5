using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ListaPrecioProductoProxy : WebApiProxyClient, IListaPrecioProductoProxy
	{
		private const string API_CONTROLLER = "ListaPrecioProducto";
		public ListaPrecioProductoDTO GetById(ListaPrecioProductoDTO dto)
		{
			var result = RestClient.PostApi<ListaPrecioProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ListaPrecioProductoDTO> Search(ListaPrecioProductoDTO dto)
		{
			var result = RestClient.PostApi<List<ListaPrecioProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ListaPrecioProductoDTO SingleSearch(ListaPrecioProductoDTO dto)
		{
			var result = RestClient.PostApi<ListaPrecioProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ListaPrecioProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ListaPrecioProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ListaPrecioProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public void ActualizarLote(List<ListaPrecioProductoDTO> list)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarLote"), list);
		}
		public string ListarSaldoAlmacenGrid(ListaPrecioProductoDTO dto)
		{
			var result = RestClient.PostApi<string>(WebApiMethod.Create(API_CONTROLLER, "ListarSaldoAlmacenGrid"), dto);
			return result;
		}
	}
}
