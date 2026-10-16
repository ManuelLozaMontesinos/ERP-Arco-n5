using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface INotaCreditoRepository : IGenericRepository<NotaCredito>
	{
		void ActualizarMonto(NotaCredito entity);
		void ActualizarEmision(NotaCredito entity);
		void ActualizarEmisionElectronica(NotaCredito entity);
		void ActualizarEstadoSunat(NotaCredito entity);
		void ActualizarPorComprobanteElectronicoIdList(NotaCredito entity);
		void ActualizarEstadoComprobante(NotaCredito entity);
	}
}
