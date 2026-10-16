using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ProveedorListaPrecioProxy : WebApiProxyClient, IProveedorListaPrecioProxy
	{
		private const string API_CONTROLLER = "ProveedorListaPrecio";
		public ProveedorListaPrecioDTO GetById(ProveedorListaPrecioDTO dto)
		{
			var result = RestClient.PostApi<ProveedorListaPrecioDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ProveedorListaPrecioDTO> Search(ProveedorListaPrecioDTO dto)
		{
			var result = RestClient.PostApi<List<ProveedorListaPrecioDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ProveedorListaPrecioDTO SingleSearch(ProveedorListaPrecioDTO dto)
		{
			var result = RestClient.PostApi<ProveedorListaPrecioDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ProveedorListaPrecioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ProveedorListaPrecioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ProveedorListaPrecioDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
