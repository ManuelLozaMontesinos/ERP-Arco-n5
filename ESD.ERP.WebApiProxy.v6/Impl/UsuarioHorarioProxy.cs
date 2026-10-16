using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class UsuarioHorarioProxy : WebApiProxyClient, IUsuarioHorarioProxy
	{
		private const string API_CONTROLLER = "UsuarioHorario";
		public UsuarioHorarioDTO GetById(UsuarioHorarioDTO dto)
		{
			var result = RestClient.PostApi<UsuarioHorarioDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<UsuarioHorarioDTO> Search(UsuarioHorarioDTO dto)
		{
			var result = RestClient.PostApi<List<UsuarioHorarioDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public UsuarioHorarioDTO SingleSearch(UsuarioHorarioDTO dto)
		{
			var result = RestClient.PostApi<UsuarioHorarioDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(UsuarioHorarioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(UsuarioHorarioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(UsuarioHorarioDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public List<UsuarioHorarioDTO> ListarPorUsuarioId(UsuarioHorarioDTO dto)
		{
			var result = RestClient.PostApi<List<UsuarioHorarioDTO>>(WebApiMethod.Create(API_CONTROLLER, "ListarPorUsuarioId"), dto);
			return result;
		}
		public void ActualizarLote(List<UsuarioHorarioDTO> list)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarLote"), list);
		}
	}
}
