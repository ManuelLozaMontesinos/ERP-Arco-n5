using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PlanillaConceptoRemunerativoProxy : WebApiProxyClient, IPlanillaConceptoRemunerativoProxy
	{
		private const string API_CONTROLLER = "PlanillaConceptoRemunerativo";
		public PlanillaConceptoRemunerativoDTO GetById(PlanillaConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<PlanillaConceptoRemunerativoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PlanillaConceptoRemunerativoDTO> Search(PlanillaConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<List<PlanillaConceptoRemunerativoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PlanillaConceptoRemunerativoDTO SingleSearch(PlanillaConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<PlanillaConceptoRemunerativoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PlanillaConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PlanillaConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PlanillaConceptoRemunerativoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
