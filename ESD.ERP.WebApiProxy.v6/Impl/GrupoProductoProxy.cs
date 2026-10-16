using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class GrupoProductoProxy : WebApiProxyClient, IGrupoProductoProxy
	{
		private const string API_CONTROLLER = "GrupoProducto";
		public GrupoProductoDTO GetById(GrupoProductoDTO dto)
		{
			var result = RestClient.PostApi<GrupoProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<GrupoProductoDTO> Search(GrupoProductoDTO dto)
		{
			var result = RestClient.PostApi<List<GrupoProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public GrupoProductoDTO SingleSearch(GrupoProductoDTO dto)
		{
			var result = RestClient.PostApi<GrupoProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(GrupoProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(GrupoProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(GrupoProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
