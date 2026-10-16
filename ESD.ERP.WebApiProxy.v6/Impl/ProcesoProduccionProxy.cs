using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ProcesoProduccionProxy : WebApiProxyClient, IProcesoProduccionProxy
	{
		private const string API_CONTROLLER = "ProcesoProduccion";
		public ProcesoProduccionDTO GetById(ProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<ProcesoProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ProcesoProduccionDTO> Search(ProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<List<ProcesoProduccionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ProcesoProduccionDTO SingleSearch(ProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<ProcesoProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ProcesoProduccionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
