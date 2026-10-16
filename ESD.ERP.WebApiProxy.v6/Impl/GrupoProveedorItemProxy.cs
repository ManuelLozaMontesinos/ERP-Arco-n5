using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class GrupoProveedorItemProxy : WebApiProxyClient, IGrupoProveedorItemProxy
	{
		private const string API_CONTROLLER = "GrupoProveedorItem";
		public GrupoProveedorItemDTO GetById(GrupoProveedorItemDTO dto)
		{
			var result = RestClient.PostApi<GrupoProveedorItemDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<GrupoProveedorItemDTO> Search(GrupoProveedorItemDTO dto)
		{
			var result = RestClient.PostApi<List<GrupoProveedorItemDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public GrupoProveedorItemDTO SingleSearch(GrupoProveedorItemDTO dto)
		{
			var result = RestClient.PostApi<GrupoProveedorItemDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(GrupoProveedorItemDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(GrupoProveedorItemDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(GrupoProveedorItemDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
