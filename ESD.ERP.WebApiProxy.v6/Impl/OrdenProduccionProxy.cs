using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class OrdenProduccionProxy : WebApiProxyClient, IOrdenProduccionProxy
	{
		private const string API_CONTROLLER = "OrdenProduccion";
		public OrdenProduccionDTO GetById(OrdenProduccionDTO dto)
		{
			var result = RestClient.PostApi<OrdenProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<OrdenProduccionDTO> Search(OrdenProduccionDTO dto)
		{
			var result = RestClient.PostApi<List<OrdenProduccionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public OrdenProduccionDTO SingleSearch(OrdenProduccionDTO dto)
		{
			var result = RestClient.PostApi<OrdenProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(OrdenProduccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(OrdenProduccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(OrdenProduccionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public void CerrarDespacho(OrdenProduccionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "CerrarDespacho"), dto);
		}
		public void CerrarInsumo(OrdenProduccionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "CerrarInsumo"), dto);
		}
		public void CambiarEstado(OrdenProduccionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "CambiarEstado"), dto);
		}
		public void AsignarMaquina(OrdenProduccionDTO dto)
		{
            RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "AsignarMaquina"), dto);
        }

    }
}
