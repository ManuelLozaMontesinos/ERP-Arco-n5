using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoDocumentoComercialProxy : WebApiProxyClient, ITipoDocumentoComercialProxy
	{
		private const string API_CONTROLLER = "TipoDocumentoComercial";
		public TipoDocumentoComercialDTO GetById(TipoDocumentoComercialDTO dto)
		{
			var result = RestClient.PostApi<TipoDocumentoComercialDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoDocumentoComercialDTO> Search(TipoDocumentoComercialDTO dto)
		{
			var result = RestClient.PostApi<List<TipoDocumentoComercialDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoDocumentoComercialDTO SingleSearch(TipoDocumentoComercialDTO dto)
		{
			var result = RestClient.PostApi<TipoDocumentoComercialDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoDocumentoComercialDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoDocumentoComercialDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoDocumentoComercialDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoDocumentoComercialDTO ObtenerPorCodigo(TipoDocumentoComercialDTO dto)
		{
			var result = RestClient.PostApi<TipoDocumentoComercialDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
