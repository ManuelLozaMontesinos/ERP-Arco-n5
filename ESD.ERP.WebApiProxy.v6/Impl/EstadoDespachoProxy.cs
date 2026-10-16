using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoDespachoProxy : WebApiProxyClient, IEstadoDespachoProxy
	{
		private const string API_CONTROLLER = "EstadoDespacho";
		public EstadoDespachoDTO GetById(EstadoDespachoDTO dto)
		{
			var result = RestClient.PostApi<EstadoDespachoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoDespachoDTO> Search(EstadoDespachoDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoDespachoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoDespachoDTO SingleSearch(EstadoDespachoDTO dto)
		{
			var result = RestClient.PostApi<EstadoDespachoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoDespachoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoDespachoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoDespachoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public EstadoDespachoDTO ObtenerPorCodigo(EstadoDespachoDTO dto)
		{
			var result = RestClient.PostApi<EstadoDespachoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
