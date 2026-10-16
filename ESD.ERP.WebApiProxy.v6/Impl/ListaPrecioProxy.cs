using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ListaPrecioProxy : WebApiProxyClient, IListaPrecioProxy
	{
		private const string API_CONTROLLER = "ListaPrecio";
		public ListaPrecioDTO GetById(ListaPrecioDTO dto)
		{
			var result = RestClient.PostApi<ListaPrecioDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ListaPrecioDTO> Search(ListaPrecioDTO dto)
		{
			var result = RestClient.PostApi<List<ListaPrecioDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ListaPrecioDTO SingleSearch(ListaPrecioDTO dto)
		{
			var result = RestClient.PostApi<ListaPrecioDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ListaPrecioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ListaPrecioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ListaPrecioDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
    }
}
