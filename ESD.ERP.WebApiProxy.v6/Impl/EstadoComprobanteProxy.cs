using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoComprobanteProxy : WebApiProxyClient, IEstadoComprobanteProxy
	{
		private const string API_CONTROLLER = "EstadoComprobante";
		public EstadoComprobanteDTO GetById(EstadoComprobanteDTO dto)
		{
			var result = RestClient.PostApi<EstadoComprobanteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoComprobanteDTO> Search(EstadoComprobanteDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoComprobanteDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoComprobanteDTO SingleSearch(EstadoComprobanteDTO dto)
		{
			var result = RestClient.PostApi<EstadoComprobanteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoComprobanteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoComprobanteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoComprobanteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public EstadoComprobanteDTO ObtenerPorCodigo(EstadoComprobanteDTO dto)
		{
			var result = RestClient.PostApi<EstadoComprobanteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
