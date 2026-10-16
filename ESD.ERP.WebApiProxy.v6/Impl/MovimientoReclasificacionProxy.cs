using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class MovimientoReclasificacionProxy : WebApiProxyClient, IMovimientoReclasificacionProxy
	{
		private const string API_CONTROLLER = "MovimientoReclasificacion";
		public MovimientoReclasificacionDTO GetById(MovimientoReclasificacionDTO dto)
		{
			var result = RestClient.PostApi<MovimientoReclasificacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<MovimientoReclasificacionDTO> Search(MovimientoReclasificacionDTO dto)
		{
			var result = RestClient.PostApi<List<MovimientoReclasificacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public MovimientoReclasificacionDTO SingleSearch(MovimientoReclasificacionDTO dto)
		{
			var result = RestClient.PostApi<MovimientoReclasificacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(MovimientoReclasificacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(MovimientoReclasificacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(MovimientoReclasificacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public void Solicitar(MovimientoReclasificacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Solicitar"), dto);
		}
        public void Aprobar(MovimientoReclasificacionDTO dto)
        {
            RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Aprobar"), dto);
        }
        public void Desaprobar(MovimientoReclasificacionDTO dto)
        {
            RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Desaprobar"), dto);
        }
    }
}
