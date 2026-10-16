using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class SubProcesoProduccionProxy : WebApiProxyClient, ISubProcesoProduccionProxy
	{
		private const string API_CONTROLLER = "SubProcesoProduccion";
		public SubProcesoProduccionDTO GetById(SubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<SubProcesoProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<SubProcesoProduccionDTO> Search(SubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<List<SubProcesoProduccionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public SubProcesoProduccionDTO SingleSearch(SubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<SubProcesoProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(SubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(SubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(SubProcesoProduccionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
