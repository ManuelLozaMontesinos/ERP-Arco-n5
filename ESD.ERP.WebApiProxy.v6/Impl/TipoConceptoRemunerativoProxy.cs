using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoConceptoRemunerativoProxy : WebApiProxyClient, ITipoConceptoRemunerativoProxy
	{
		private const string API_CONTROLLER = "TipoConceptoRemunerativo";
		public TipoConceptoRemunerativoDTO GetById(TipoConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<TipoConceptoRemunerativoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoConceptoRemunerativoDTO> Search(TipoConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<List<TipoConceptoRemunerativoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoConceptoRemunerativoDTO SingleSearch(TipoConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<TipoConceptoRemunerativoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoConceptoRemunerativoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoConceptoRemunerativoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
