using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoAccionAuditoriaProxy : WebApiProxyClient, ITipoAccionAuditoriaProxy
	{
		private const string API_CONTROLLER = "TipoAccionAuditoria";
		public TipoAccionAuditoriaDTO GetById(TipoAccionAuditoriaDTO dto)
		{
			var result = RestClient.PostApi<TipoAccionAuditoriaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoAccionAuditoriaDTO> Search(TipoAccionAuditoriaDTO dto)
		{
			var result = RestClient.PostApi<List<TipoAccionAuditoriaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoAccionAuditoriaDTO SingleSearch(TipoAccionAuditoriaDTO dto)
		{
			var result = RestClient.PostApi<TipoAccionAuditoriaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoAccionAuditoriaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoAccionAuditoriaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoAccionAuditoriaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoAccionAuditoriaDTO ObtenerPorCodigo(TipoAccionAuditoriaDTO dto)
		{
			var result = RestClient.PostApi<TipoAccionAuditoriaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
