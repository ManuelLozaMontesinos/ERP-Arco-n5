using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class DocumentoComercialSerieProxy : WebApiProxyClient, IDocumentoComercialSerieProxy
	{
		private const string API_CONTROLLER = "DocumentoComercialSerie";
		public DocumentoComercialSerieDTO GetById(DocumentoComercialSerieDTO dto)
		{
			var result = RestClient.PostApi<DocumentoComercialSerieDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<DocumentoComercialSerieDTO> Search(DocumentoComercialSerieDTO dto)
		{
			var result = RestClient.PostApi<List<DocumentoComercialSerieDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public DocumentoComercialSerieDTO SingleSearch(DocumentoComercialSerieDTO dto)
		{
			var result = RestClient.PostApi<DocumentoComercialSerieDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(DocumentoComercialSerieDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(DocumentoComercialSerieDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(DocumentoComercialSerieDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
