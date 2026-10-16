using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class MovimientoAlmacenSaldoProxy : WebApiProxyClient, IMovimientoAlmacenSaldoProxy
	{
		private const string API_CONTROLLER = "MovimientoAlmacenSaldo";
		public MovimientoAlmacenSaldoDTO GetById(MovimientoAlmacenSaldoDTO dto)
		{
			var result = RestClient.PostApi<MovimientoAlmacenSaldoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<MovimientoAlmacenSaldoDTO> Search(MovimientoAlmacenSaldoDTO dto)
		{
			var result = RestClient.PostApi<List<MovimientoAlmacenSaldoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public MovimientoAlmacenSaldoDTO SingleSearch(MovimientoAlmacenSaldoDTO dto)
		{
			var result = RestClient.PostApi<MovimientoAlmacenSaldoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(MovimientoAlmacenSaldoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(MovimientoAlmacenSaldoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(MovimientoAlmacenSaldoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public decimal ObtenerSaldoVenta(MovimientoAlmacenSaldoDTO dto)
      {
			var result = RestClient.PostApi<decimal>(WebApiMethod.Create(API_CONTROLLER, "ObtenerSaldoVenta"), dto);
			return result;
		}
      public void ValidarSaldoVenta(MovimientoAlmacenSaldoDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ValidarSaldoVenta"), dto);
		}
      public void ActualizarCantidadReserva(MovimientoAlmacenSaldoDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarCantidadReserva"), dto);
		}
      public void ValidarSaldoVentaCombo(List<MovimientoAlmacenSaldoDTO> list)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ValidarSaldoVentaCombo"), list);
		}
		public string ListarSaldoMultiAlmacenGrid(MovimientoAlmacenSaldoDTO dto)
		{
			var result = RestClient.PostApi<string>(WebApiMethod.Create(API_CONTROLLER, "ListarSaldoMultiAlmacenGrid"), dto);
			return result;
		}
	}
}
