using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ListaPrecioProductoPresentacionProxy : WebApiProxyClient, IListaPrecioProductoPresentacionProxy
	{
		private const string API_CONTROLLER = "ListaPrecioProductoPresentacion";
		public ListaPrecioProductoPresentacionDTO GetById(ListaPrecioProductoPresentacionDTO dto)
		{
			var result = RestClient.PostApi<ListaPrecioProductoPresentacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ListaPrecioProductoPresentacionDTO> Search(ListaPrecioProductoPresentacionDTO dto)
		{
			var result = RestClient.PostApi<List<ListaPrecioProductoPresentacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ListaPrecioProductoPresentacionDTO SingleSearch(ListaPrecioProductoPresentacionDTO dto)
		{
			var result = RestClient.PostApi<ListaPrecioProductoPresentacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ListaPrecioProductoPresentacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ListaPrecioProductoPresentacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ListaPrecioProductoPresentacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
