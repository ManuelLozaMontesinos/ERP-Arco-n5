using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoMovimientoReclasificacionProxy : WebApiProxyClient, IEstadoMovimientoReclasificacionProxy
	{
		private const string API_CONTROLLER = "EstadoMovimientoReclasificacion";
		public EstadoMovimientoReclasificacionDTO GetById(EstadoMovimientoReclasificacionDTO dto)
		{
			var result = RestClient.PostApi<EstadoMovimientoReclasificacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoMovimientoReclasificacionDTO> Search(EstadoMovimientoReclasificacionDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoMovimientoReclasificacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoMovimientoReclasificacionDTO SingleSearch(EstadoMovimientoReclasificacionDTO dto)
		{
			var result = RestClient.PostApi<EstadoMovimientoReclasificacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoMovimientoReclasificacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoMovimientoReclasificacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoMovimientoReclasificacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public EstadoMovimientoReclasificacionDTO ObtenerPorCodigo(EstadoMovimientoReclasificacionDTO dto)
		{
			var result = RestClient.PostApi<EstadoMovimientoReclasificacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
