using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class GrupoParametroSistemaProxy : WebApiProxyClient, IGrupoParametroSistemaProxy
	{
		private const string API_CONTROLLER = "GrupoParametroSistema";
		public GrupoParametroSistemaDTO GetById(GrupoParametroSistemaDTO dto)
		{
			var result = RestClient.PostApi<GrupoParametroSistemaDTO>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<GrupoParametroSistemaDTO> Search(GrupoParametroSistemaDTO dto)
		{
			var result = RestClient.PostApi<List<GrupoParametroSistemaDTO>>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.SEARCH), dto);
			return result;
		}
		public GrupoParametroSistemaDTO SingleSearch(GrupoParametroSistemaDTO dto)
		{
			var result = RestClient.PostApi<GrupoParametroSistemaDTO>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(GrupoParametroSistemaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(GrupoParametroSistemaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(GrupoParametroSistemaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.DELETE), dto);
		}
	}
}
