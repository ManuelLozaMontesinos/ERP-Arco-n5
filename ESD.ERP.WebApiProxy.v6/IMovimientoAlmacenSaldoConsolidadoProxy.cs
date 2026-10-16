using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMovimientoAlmacenSaldoConsolidadoProxy
	{
		MovimientoAlmacenSaldoConsolidadoDTO GetById(MovimientoAlmacenSaldoConsolidadoDTO dto);
		List<MovimientoAlmacenSaldoConsolidadoDTO> Search(MovimientoAlmacenSaldoConsolidadoDTO dto);
		MovimientoAlmacenSaldoConsolidadoDTO SingleSearch(MovimientoAlmacenSaldoConsolidadoDTO dto);
		int Insert(MovimientoAlmacenSaldoConsolidadoDTO dto);
		int Update(MovimientoAlmacenSaldoConsolidadoDTO dto);
		void Delete(MovimientoAlmacenSaldoConsolidadoDTO dto);
		void Valorizar(MovimientoAlmacenSaldoConsolidadoDTO dto);
	}
}
