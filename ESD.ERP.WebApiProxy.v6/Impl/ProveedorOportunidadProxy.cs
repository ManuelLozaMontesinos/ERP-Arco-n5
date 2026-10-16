using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ProveedorOportunidadProxy : WebApiProxyClient, IProveedorOportunidadProxy
	{
		private const string API_CONTROLLER = "ProveedorOportunidad";
		public ProveedorOportunidadDTO GetById(ProveedorOportunidadDTO dto)
		{
			var result = RestClient.PostApi<ProveedorOportunidadDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ProveedorOportunidadDTO> Search(ProveedorOportunidadDTO dto)
		{
			var result = RestClient.PostApi<List<ProveedorOportunidadDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ProveedorOportunidadDTO SingleSearch(ProveedorOportunidadDTO dto)
		{
			var result = RestClient.PostApi<ProveedorOportunidadDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ProveedorOportunidadDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ProveedorOportunidadDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ProveedorOportunidadDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
