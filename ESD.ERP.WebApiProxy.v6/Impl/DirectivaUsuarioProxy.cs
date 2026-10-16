using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class DirectivaUsuarioProxy : WebApiProxyClient, IDirectivaUsuarioProxy
	{
		private const string API_CONTROLLER = "DirectivaUsuario";
		public DirectivaUsuarioDTO GetById(DirectivaUsuarioDTO dto)
		{
			var result = RestClient.PostApi<DirectivaUsuarioDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<DirectivaUsuarioDTO> Search(DirectivaUsuarioDTO dto)
		{
			var result = RestClient.PostApi<List<DirectivaUsuarioDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public DirectivaUsuarioDTO SingleSearch(DirectivaUsuarioDTO dto)
		{
			var result = RestClient.PostApi<DirectivaUsuarioDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(DirectivaUsuarioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(DirectivaUsuarioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(DirectivaUsuarioDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
