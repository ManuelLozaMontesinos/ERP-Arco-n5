using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IAnticipoClienteRepository : IGenericRepository<AnticipoCliente>
	{
		void ActualizarExtornado(AnticipoCliente entity);
		void ActualizarEstadoComprobante(AnticipoCliente entity);
		void ActualizarEmision(AnticipoCliente entity);
      void ActualizarMontoAplicado(AnticipoCliente entity);
		int ObtenerSecuencia(AnticipoCliente entity);

	}
}
