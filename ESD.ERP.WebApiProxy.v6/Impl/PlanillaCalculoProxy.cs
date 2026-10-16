using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PlanillaCalculoProxy : WebApiProxyClient, IPlanillaCalculoProxy
	{
		private const string API_CONTROLLER = "PlanillaCalculo";
		public PlanillaCalculoDTO GetById(PlanillaCalculoDTO dto)
		{
			var result = RestClient.PostApi<PlanillaCalculoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PlanillaCalculoDTO> Search(PlanillaCalculoDTO dto)
		{
			var result = RestClient.PostApi<List<PlanillaCalculoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PlanillaCalculoDTO SingleSearch(PlanillaCalculoDTO dto)
		{
			var result = RestClient.PostApi<PlanillaCalculoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PlanillaCalculoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PlanillaCalculoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PlanillaCalculoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
