using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class OrdenProduccionSubProcesoProxy : WebApiProxyClient, IOrdenProduccionSubProcesoProxy
	{
		private const string API_CONTROLLER = "OrdenProduccionSubProceso";
		public OrdenProduccionSubProcesoDTO GetById(OrdenProduccionSubProcesoDTO dto)
		{
			var result = RestClient.PostApi<OrdenProduccionSubProcesoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<OrdenProduccionSubProcesoDTO> Search(OrdenProduccionSubProcesoDTO dto)
		{
			var result = RestClient.PostApi<List<OrdenProduccionSubProcesoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public OrdenProduccionSubProcesoDTO SingleSearch(OrdenProduccionSubProcesoDTO dto)
		{
			var result = RestClient.PostApi<OrdenProduccionSubProcesoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(OrdenProduccionSubProcesoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(OrdenProduccionSubProcesoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(OrdenProduccionSubProcesoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public void ActualizarInicio(OrdenProduccionSubProcesoDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarInicio"), dto);
		}
      public void ActualizarFin(OrdenProduccionSubProcesoDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarFin"), dto);
	  }
   }
}