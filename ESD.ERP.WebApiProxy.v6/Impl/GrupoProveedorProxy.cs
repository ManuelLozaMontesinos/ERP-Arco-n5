using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class GrupoProveedorProxy : WebApiProxyClient, IGrupoProveedorProxy
	{
		private const string API_CONTROLLER = "GrupoProveedor";
		public GrupoProveedorDTO GetById(GrupoProveedorDTO dto)
		{
			var result = RestClient.PostApi<GrupoProveedorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<GrupoProveedorDTO> Search(GrupoProveedorDTO dto)
		{
			var result = RestClient.PostApi<List<GrupoProveedorDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public GrupoProveedorDTO SingleSearch(GrupoProveedorDTO dto)
		{
			var result = RestClient.PostApi<GrupoProveedorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(GrupoProveedorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(GrupoProveedorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(GrupoProveedorDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
