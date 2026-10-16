using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class SctrSaludProxy : WebApiProxyClient, ISctrSaludProxy
	{
		private const string API_CONTROLLER = "SctrSalud";
		public SctrSaludDTO GetById(SctrSaludDTO dto)
		{
			var result = RestClient.PostApi<SctrSaludDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<SctrSaludDTO> Search(SctrSaludDTO dto)
		{
			var result = RestClient.PostApi<List<SctrSaludDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public SctrSaludDTO SingleSearch(SctrSaludDTO dto)
		{
			var result = RestClient.PostApi<SctrSaludDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(SctrSaludDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(SctrSaludDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(SctrSaludDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public SctrSaludDTO ObtenerPorCodigo(SctrSaludDTO dto)
		{
			var result = RestClient.PostApi<SctrSaludDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
