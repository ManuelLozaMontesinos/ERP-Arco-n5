using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoOrdenProduccionProxy : WebApiProxyClient, IEstadoOrdenProduccionProxy
	{
		private const string API_CONTROLLER = "EstadoOrdenProduccion";
		public EstadoOrdenProduccionDTO GetById(EstadoOrdenProduccionDTO dto)
		{
			var result = RestClient.PostApi<EstadoOrdenProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoOrdenProduccionDTO> Search(EstadoOrdenProduccionDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoOrdenProduccionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoOrdenProduccionDTO SingleSearch(EstadoOrdenProduccionDTO dto)
		{
			var result = RestClient.PostApi<EstadoOrdenProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoOrdenProduccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoOrdenProduccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoOrdenProduccionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public EstadoOrdenProduccionDTO ObtenerPorCodigo(EstadoOrdenProduccionDTO dto)
		{
			var result = RestClient.PostApi<EstadoOrdenProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
