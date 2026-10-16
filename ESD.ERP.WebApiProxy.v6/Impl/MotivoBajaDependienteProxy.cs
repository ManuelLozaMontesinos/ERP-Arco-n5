using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class MotivoBajaDependienteProxy : WebApiProxyClient, IMotivoBajaDependienteProxy
	{
		private const string API_CONTROLLER = "MotivoBajaDependiente";
		public MotivoBajaDependienteDTO GetById(MotivoBajaDependienteDTO dto)
		{
			var result = RestClient.PostApi<MotivoBajaDependienteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<MotivoBajaDependienteDTO> Search(MotivoBajaDependienteDTO dto)
		{
			var result = RestClient.PostApi<List<MotivoBajaDependienteDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public MotivoBajaDependienteDTO SingleSearch(MotivoBajaDependienteDTO dto)
		{
			var result = RestClient.PostApi<MotivoBajaDependienteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(MotivoBajaDependienteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(MotivoBajaDependienteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(MotivoBajaDependienteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
