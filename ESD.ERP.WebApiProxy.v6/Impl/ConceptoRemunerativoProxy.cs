using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ConceptoRemunerativoProxy : WebApiProxyClient, IConceptoRemunerativoProxy
	{
		private const string API_CONTROLLER = "ConceptoRemunerativo";
		public ConceptoRemunerativoDTO GetById(ConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<ConceptoRemunerativoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ConceptoRemunerativoDTO> Search(ConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<List<ConceptoRemunerativoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ConceptoRemunerativoDTO SingleSearch(ConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<ConceptoRemunerativoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ConceptoRemunerativoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
