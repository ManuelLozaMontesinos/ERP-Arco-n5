using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoDocumentoVinculoFamiliarProxy : WebApiProxyClient, ITipoDocumentoVinculoFamiliarProxy
	{
		private const string API_CONTROLLER = "TipoDocumentoVinculoFamiliar";
		public TipoDocumentoVinculoFamiliarDTO GetById(TipoDocumentoVinculoFamiliarDTO dto)
		{
			var result = RestClient.PostApi<TipoDocumentoVinculoFamiliarDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoDocumentoVinculoFamiliarDTO> Search(TipoDocumentoVinculoFamiliarDTO dto)
		{
			var result = RestClient.PostApi<List<TipoDocumentoVinculoFamiliarDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoDocumentoVinculoFamiliarDTO SingleSearch(TipoDocumentoVinculoFamiliarDTO dto)
		{
			var result = RestClient.PostApi<TipoDocumentoVinculoFamiliarDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoDocumentoVinculoFamiliarDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoDocumentoVinculoFamiliarDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoDocumentoVinculoFamiliarDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
