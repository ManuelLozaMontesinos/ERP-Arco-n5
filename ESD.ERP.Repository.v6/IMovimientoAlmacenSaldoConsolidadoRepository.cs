using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IMovimientoAlmacenSaldoConsolidadoRepository : IGenericRepository<MovimientoAlmacenSaldoConsolidado>
	{
      void Valorizar(MovimientoAlmacenSaldoConsolidado movimientoAlmacenSaldoConsolidado);
   }
}
