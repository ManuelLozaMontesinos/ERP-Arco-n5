using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMovimientoAlmacenSaldoConsolidadoBusiness
	{
		MovimientoAlmacenSaldoConsolidadoDTO GetById(int id);
		List<MovimientoAlmacenSaldoConsolidadoDTO> Search(MovimientoAlmacenSaldoConsolidadoDTO dto);
		MovimientoAlmacenSaldoConsolidadoDTO SingleSearch(MovimientoAlmacenSaldoConsolidadoDTO dto);
		int Insert(MovimientoAlmacenSaldoConsolidadoDTO dto);
		int Update(MovimientoAlmacenSaldoConsolidadoDTO dto);
		void Delete(MovimientoAlmacenSaldoConsolidadoDTO dto);
		void Valorizar(MovimientoAlmacenSaldoConsolidadoDTO dto);
	}
}
