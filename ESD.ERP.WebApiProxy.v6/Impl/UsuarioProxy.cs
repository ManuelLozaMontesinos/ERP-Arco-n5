using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class UsuarioProxy : WebApiProxyClient, IUsuarioProxy
	{
		private const string API_CONTROLLER = "Usuario";
		public UsuarioDTO GetById(UsuarioDTO dto)
		{
			var result = RestClient.PostApi<UsuarioDTO>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<UsuarioDTO> Search(UsuarioDTO dto)
		{
			var result = RestClient.PostApi<List<UsuarioDTO>>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.SEARCH), dto);
			return result;
		}
		public UsuarioDTO SingleSearch(UsuarioDTO dto)
		{
			var result = RestClient.PostApi<UsuarioDTO>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(UsuarioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(UsuarioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(UsuarioDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.DELETE), dto);
		}
		public UsuarioDTO ObtenerPorCodigo(UsuarioDTO dto)
		{
			var result = RestClient.PostApi<UsuarioDTO>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
		public void ActualizarEstado(UsuarioDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER,"ActualizarEstado"), dto);
		}
		public void ActualizarContrasena(UsuarioDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER,"ActualizarContrasena"), dto);
		}
		public void ActualizarContrasenaAnterior(UsuarioDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarContrasenaAnterior"), dto);
		}
	}
}
