using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IMovimientoAlmacenSaldoRepository : IGenericRepository<MovimientoAlmacenSaldo>
	{
		MovimientoAlmacenSaldo ObtenerPorEmpresaSede(MovimientoAlmacenSaldo entity);
		void ActualizarCantidadReserva(MovimientoAlmacenSaldo entity);
		void ReprocesarCantidadReserva(MovimientoAlmacenSaldo entity);
		List<dynamic> ListarSaldoMultiAlmacen(MovimientoAlmacenSaldo entity);
	}
}