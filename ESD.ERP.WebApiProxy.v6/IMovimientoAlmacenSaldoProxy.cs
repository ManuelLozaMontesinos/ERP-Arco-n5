using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMovimientoAlmacenSaldoProxy
	{
		MovimientoAlmacenSaldoDTO GetById(MovimientoAlmacenSaldoDTO dto);
		List<MovimientoAlmacenSaldoDTO> Search(MovimientoAlmacenSaldoDTO dto);
		MovimientoAlmacenSaldoDTO SingleSearch(MovimientoAlmacenSaldoDTO dto);
		int Insert(MovimientoAlmacenSaldoDTO dto);
		int Update(MovimientoAlmacenSaldoDTO dto);
		void Delete(MovimientoAlmacenSaldoDTO dto);
		decimal ObtenerSaldoVenta(MovimientoAlmacenSaldoDTO dto);
		void ValidarSaldoVenta(MovimientoAlmacenSaldoDTO dto);
		void ValidarSaldoVentaCombo(List<MovimientoAlmacenSaldoDTO> list);
		void ActualizarCantidadReserva(MovimientoAlmacenSaldoDTO dto);
		string ListarSaldoMultiAlmacenGrid(MovimientoAlmacenSaldoDTO dto);
	}
}
