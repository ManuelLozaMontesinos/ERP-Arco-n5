using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class RolUsuarioProxy : WebApiProxyClient, IRolUsuarioProxy
	{
		private const string API_CONTROLLER = "RolUsuario";
		public RolUsuarioDTO GetById(RolUsuarioDTO dto)
		{
			var result = RestClient.PostApi<RolUsuarioDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<RolUsuarioDTO> Search(RolUsuarioDTO dto)
		{
			var result = RestClient.PostApi<List<RolUsuarioDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public RolUsuarioDTO SingleSearch(RolUsuarioDTO dto)
		{
			var result = RestClient.PostApi<RolUsuarioDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(RolUsuarioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(RolUsuarioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(RolUsuarioDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
