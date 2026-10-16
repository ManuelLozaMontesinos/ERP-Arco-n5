using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ListaPrecioProveedorProxy : WebApiProxyClient, IListaPrecioProveedorProxy
	{
		private const string API_CONTROLLER = "ListaPrecioProveedor";
		public ListaPrecioProveedorDTO GetById(ListaPrecioProveedorDTO dto)
		{
			var result = RestClient.PostApi<ListaPrecioProveedorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ListaPrecioProveedorDTO> Search(ListaPrecioProveedorDTO dto)
		{
			var result = RestClient.PostApi<List<ListaPrecioProveedorDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ListaPrecioProveedorDTO SingleSearch(ListaPrecioProveedorDTO dto)
		{
			var result = RestClient.PostApi<ListaPrecioProveedorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ListaPrecioProveedorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ListaPrecioProveedorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ListaPrecioProveedorDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public List<ListaPrecioProveedorDTO> ListarPorProveedorId(ListaPrecioProveedorDTO dto)
      {
			var result = RestClient.PostApi<List<ListaPrecioProveedorDTO>>(WebApiMethod.Create(API_CONTROLLER, "ListarPorProveedorId"), dto);
			return result;
		}
   }
}
