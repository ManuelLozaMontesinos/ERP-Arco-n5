using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PlanillaProcesoProxy : WebApiProxyClient, IPlanillaProcesoProxy
	{
		private const string API_CONTROLLER = "PlanillaProceso";
		public PlanillaProcesoDTO GetById(PlanillaProcesoDTO dto)
		{
			var result = RestClient.PostApi<PlanillaProcesoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PlanillaProcesoDTO> Search(PlanillaProcesoDTO dto)
		{
			var result = RestClient.PostApi<List<PlanillaProcesoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PlanillaProcesoDTO SingleSearch(PlanillaProcesoDTO dto)
		{
			var result = RestClient.PostApi<PlanillaProcesoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PlanillaProcesoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PlanillaProcesoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PlanillaProcesoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public void Calcular(PlanillaProcesoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Calcular"), dto);
		}
	}
}
