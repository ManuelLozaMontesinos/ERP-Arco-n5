using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ICotizacionRepository : IGenericRepository<Cotizacion>
	{
        void ActualizarMonto(Cotizacion entity);
		void ActualizarEstado(Cotizacion entity);
		void ActualizarMontoAnticipo(Cotizacion entity);
		void ActualizarEnviadoProduccion(Cotizacion entity);
	}
}
