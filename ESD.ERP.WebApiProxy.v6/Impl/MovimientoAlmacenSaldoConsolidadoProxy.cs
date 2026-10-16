using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class MovimientoAlmacenSaldoConsolidadoProxy : WebApiProxyClient, IMovimientoAlmacenSaldoConsolidadoProxy
	{
		private const string API_CONTROLLER = "MovimientoAlmacenSaldoConsolidado";
		public MovimientoAlmacenSaldoConsolidadoDTO GetById(MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			var result = RestClient.PostApi<MovimientoAlmacenSaldoConsolidadoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<MovimientoAlmacenSaldoConsolidadoDTO> Search(MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			var result = RestClient.PostApi<List<MovimientoAlmacenSaldoConsolidadoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public MovimientoAlmacenSaldoConsolidadoDTO SingleSearch(MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			var result = RestClient.PostApi<MovimientoAlmacenSaldoConsolidadoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public void Valorizar(MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Valorizar"), dto);
		}
	}
}