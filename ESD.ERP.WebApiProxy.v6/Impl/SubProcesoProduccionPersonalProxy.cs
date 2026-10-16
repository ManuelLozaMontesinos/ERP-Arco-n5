using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class SubProcesoProduccionPersonalProxy : WebApiProxyClient, ISubProcesoProduccionPersonalProxy
	{
		private const string API_CONTROLLER = "SubProcesoProduccionPersonal";
		public SubProcesoProduccionPersonalDTO GetById(SubProcesoProduccionPersonalDTO dto)
		{
			var result = RestClient.PostApi<SubProcesoProduccionPersonalDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<SubProcesoProduccionPersonalDTO> Search(SubProcesoProduccionPersonalDTO dto)
		{
			var result = RestClient.PostApi<List<SubProcesoProduccionPersonalDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public SubProcesoProduccionPersonalDTO SingleSearch(SubProcesoProduccionPersonalDTO dto)
		{
			var result = RestClient.PostApi<SubProcesoProduccionPersonalDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(SubProcesoProduccionPersonalDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(SubProcesoProduccionPersonalDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(SubProcesoProduccionPersonalDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
