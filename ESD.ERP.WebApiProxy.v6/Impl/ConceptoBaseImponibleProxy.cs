using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ConceptoBaseImponibleProxy : WebApiProxyClient, IConceptoBaseImponibleProxy
	{
		private const string API_CONTROLLER = "ConceptoBaseImponible";
		public ConceptoBaseImponibleDTO GetById(ConceptoBaseImponibleDTO dto)
		{
			var result = RestClient.PostApi<ConceptoBaseImponibleDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ConceptoBaseImponibleDTO> Search(ConceptoBaseImponibleDTO dto)
		{
			var result = RestClient.PostApi<List<ConceptoBaseImponibleDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ConceptoBaseImponibleDTO SingleSearch(ConceptoBaseImponibleDTO dto)
		{
			var result = RestClient.PostApi<ConceptoBaseImponibleDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ConceptoBaseImponibleDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ConceptoBaseImponibleDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ConceptoBaseImponibleDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public List<ConceptoBaseImponibleDTO> ListarPorConceptoRemunerativoId(ConceptoBaseImponibleDTO dto)
		{
			var result = RestClient.PostApi<List<ConceptoBaseImponibleDTO>>(WebApiMethod.Create(API_CONTROLLER,
				"ListarPorConceptoRemunerativoId"), dto);
			return result;
		}
	}
}
