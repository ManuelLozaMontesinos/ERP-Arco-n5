using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PlanillaProcesoRegularizacionProxy : WebApiProxyClient, IPlanillaProcesoRegularizacionProxy
	{
		private const string API_CONTROLLER = "PlanillaProcesoRegularizacion";
		public PlanillaProcesoRegularizacionDTO GetById(PlanillaProcesoRegularizacionDTO dto)
		{
			var result = RestClient.PostApi<PlanillaProcesoRegularizacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PlanillaProcesoRegularizacionDTO> Search(PlanillaProcesoRegularizacionDTO dto)
		{
			var result = RestClient.PostApi<List<PlanillaProcesoRegularizacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PlanillaProcesoRegularizacionDTO SingleSearch(PlanillaProcesoRegularizacionDTO dto)
		{
			var result = RestClient.PostApi<PlanillaProcesoRegularizacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PlanillaProcesoRegularizacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PlanillaProcesoRegularizacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PlanillaProcesoRegularizacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
