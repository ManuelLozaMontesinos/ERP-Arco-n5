using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class AlmacenSerieProxy : WebApiProxyClient, IAlmacenSerieProxy
	{
		private const string API_CONTROLLER = "AlmacenSerie";
		public AlmacenSerieDTO GetById(AlmacenSerieDTO dto)
		{
			var result = RestClient.PostApi<AlmacenSerieDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<AlmacenSerieDTO> Search(AlmacenSerieDTO dto)
		{
			var result = RestClient.PostApi<List<AlmacenSerieDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public AlmacenSerieDTO SingleSearch(AlmacenSerieDTO dto)
		{
			var result = RestClient.PostApi<AlmacenSerieDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(AlmacenSerieDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(AlmacenSerieDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(AlmacenSerieDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public void ActualizarListaDocumentoComercialSerieId(AlmacenSerieDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarListaDocumentoComercialSerieId"), dto);
		}
	}
}
