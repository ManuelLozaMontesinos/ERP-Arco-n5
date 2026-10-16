using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IMovimientoAlmacenRepository : IGenericRepository<MovimientoAlmacen>
	{
		void ActualizarEstadoMovimiento(MovimientoAlmacen entity);
		List<MovimientoAlmacen> BuscarPorEmpresa(MovimientoAlmacen entity);
		void ActualizarGuiaRemision(MovimientoAlmacen entity);
		void ActualizarTieneGuiaRemision(MovimientoAlmacen entity);
		void ActualizarEstadoAprobacion(MovimientoAlmacen entity);
		void ActualizarMesContable(MovimientoAlmacen entity);

    }
}
