using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ParametroSistemaProxy : WebApiProxyClient, IParametroSistemaProxy
	{
		private const string API_CONTROLLER = "ParametroSistema";
		public ParametroSistemaDTO GetById(ParametroSistemaDTO dto)
		{
			var result = RestClient.PostApi<ParametroSistemaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ParametroSistemaDTO> Search(ParametroSistemaDTO dto)
		{
			var result = RestClient.PostApi<List<ParametroSistemaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ParametroSistemaDTO SingleSearch(ParametroSistemaDTO dto)
		{
			var result = RestClient.PostApi<ParametroSistemaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ParametroSistemaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ParametroSistemaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ParametroSistemaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
