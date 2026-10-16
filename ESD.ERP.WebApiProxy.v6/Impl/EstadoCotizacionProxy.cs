using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoCotizacionProxy : WebApiProxyClient, IEstadoCotizacionProxy
	{
		private const string API_CONTROLLER = "EstadoCotizacion";
		public EstadoCotizacionDTO GetById(EstadoCotizacionDTO dto)
		{
			var result = RestClient.PostApi<EstadoCotizacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoCotizacionDTO> Search(EstadoCotizacionDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoCotizacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoCotizacionDTO SingleSearch(EstadoCotizacionDTO dto)
		{
			var result = RestClient.PostApi<EstadoCotizacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoCotizacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoCotizacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoCotizacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public EstadoCotizacionDTO ObtenerPorCodigo(EstadoCotizacionDTO dto)
		{
			var result = RestClient.PostApi<EstadoCotizacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
