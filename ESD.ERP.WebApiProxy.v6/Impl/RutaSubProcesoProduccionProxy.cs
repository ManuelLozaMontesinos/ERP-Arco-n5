using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class RutaSubProcesoProduccionProxy : WebApiProxyClient, IRutaSubProcesoProduccionProxy
	{
		private const string API_CONTROLLER = "RutaSubProcesoProduccion";
		public RutaSubProcesoProduccionDTO GetById(RutaSubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<RutaSubProcesoProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<RutaSubProcesoProduccionDTO> Search(RutaSubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<List<RutaSubProcesoProduccionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public RutaSubProcesoProduccionDTO SingleSearch(RutaSubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<RutaSubProcesoProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(RutaSubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(RutaSubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(RutaSubProcesoProduccionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
