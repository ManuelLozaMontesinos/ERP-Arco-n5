using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class RutaProduccionProxy : WebApiProxyClient, IRutaProduccionProxy
	{
		private const string API_CONTROLLER = "RutaProduccion";
		public RutaProduccionDTO GetById(RutaProduccionDTO dto)
		{
			var result = RestClient.PostApi<RutaProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<RutaProduccionDTO> Search(RutaProduccionDTO dto)
		{
			var result = RestClient.PostApi<List<RutaProduccionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public RutaProduccionDTO SingleSearch(RutaProduccionDTO dto)
		{
			var result = RestClient.PostApi<RutaProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(RutaProduccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(RutaProduccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(RutaProduccionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
