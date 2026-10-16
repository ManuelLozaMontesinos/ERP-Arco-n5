using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PlanillaProcesoHoraProxy : WebApiProxyClient, IPlanillaProcesoHoraProxy
	{
		private const string API_CONTROLLER = "PlanillaProcesoHora";
		public PlanillaProcesoHoraDTO GetById(PlanillaProcesoHoraDTO dto)
		{
			var result = RestClient.PostApi<PlanillaProcesoHoraDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PlanillaProcesoHoraDTO> Search(PlanillaProcesoHoraDTO dto)
		{
			var result = RestClient.PostApi<List<PlanillaProcesoHoraDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PlanillaProcesoHoraDTO SingleSearch(PlanillaProcesoHoraDTO dto)
		{
			var result = RestClient.PostApi<PlanillaProcesoHoraDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PlanillaProcesoHoraDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PlanillaProcesoHoraDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PlanillaProcesoHoraDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public string ListarHoraGrid(PlanillaProcesoHoraDTO dto)
		{
			var result = RestClient.PostApi<string>(WebApiMethod.Create(API_CONTROLLER, "ListarHoraGrid"), dto);
			return result;
		}
		public void ActualizarLote(List<PlanillaProcesoHoraDTO> list)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarLote"), list);
		}
		public void ActualizarLotePorPersonal(List<PlanillaProcesoHoraDTO> list)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarLotePorPersonal"), list);
		}
      public void ActualizarListaPersonalId(PlanillaProcesoHoraDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarListaPersonalId"), dto);
		}
      public void ActualizarDesdeMarcacion(PlanillaProcesoHoraDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarDesdeMarcacion"), dto);
		}
   }
}
