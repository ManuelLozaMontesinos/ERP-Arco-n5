using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class MovimientoAlmacenCierreProxy : WebApiProxyClient, IMovimientoAlmacenCierreProxy
	{
		private const string API_CONTROLLER = "MovimientoAlmacenCierre";
		public MovimientoAlmacenCierreDTO GetById(MovimientoAlmacenCierreDTO dto)
		{
			var result = RestClient.PostApi<MovimientoAlmacenCierreDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<MovimientoAlmacenCierreDTO> Search(MovimientoAlmacenCierreDTO dto)
		{
			var result = RestClient.PostApi<List<MovimientoAlmacenCierreDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public MovimientoAlmacenCierreDTO SingleSearch(MovimientoAlmacenCierreDTO dto)
		{
			var result = RestClient.PostApi<MovimientoAlmacenCierreDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(MovimientoAlmacenCierreDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(MovimientoAlmacenCierreDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(MovimientoAlmacenCierreDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public void Aperturar(MovimientoAlmacenCierreDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Aperturar"), dto);
		}
		public void Cerrar(MovimientoAlmacenCierreDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Cerrar"), dto);
		}
		public void Reprocesar(MovimientoAlmacenCierreDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Reprocesar"), dto);
		}
		public void ReprocesoAnual(MovimientoAlmacenCierreDTO dto)
		{
            RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ReprocesoAnual"), dto);  
        }

    }
}
