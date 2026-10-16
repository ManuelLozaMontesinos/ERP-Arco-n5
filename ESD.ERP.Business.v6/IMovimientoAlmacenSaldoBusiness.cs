using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMovimientoAlmacenSaldoBusiness
	{
		MovimientoAlmacenSaldoDTO GetById(int id);
		List<MovimientoAlmacenSaldoDTO> Search(MovimientoAlmacenSaldoDTO dto);
		MovimientoAlmacenSaldoDTO SingleSearch(MovimientoAlmacenSaldoDTO dto);
		int Insert(MovimientoAlmacenSaldoDTO dto);
		int Update(MovimientoAlmacenSaldoDTO dto);
		void Delete(MovimientoAlmacenSaldoDTO dto);
		decimal ObtenerSaldoDisponible(MovimientoAlmacenProductoDTO dto);
		decimal ObtenerSaldoVenta(MovimientoAlmacenSaldoDTO dto);
		void ValidarSaldoVenta(MovimientoAlmacenSaldoDTO dto);
		void ValidarSaldoVentaCombo(List<MovimientoAlmacenSaldoDTO> list);
		void ActualizarCantidadReserva(MovimientoAlmacenSaldoDTO dto);
		void ReprocesarCantidadReserva(MovimientoAlmacenSaldoDTO dto);
		List<dynamic> ListarSaldoMultiAlmacen(MovimientoAlmacenSaldoDTO dto);
	}
}
