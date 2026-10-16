using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface ICotizacionProductoRepository : IGenericRepository<CotizacionProducto>
	{
		List<CotizacionProducto> EnvioProduccion(CotizacionProducto entity);
        void EliminarPorComprobanteId(CotizacionProducto entity);
		void ActualizarSolicitaAutorizacionDescuento(CotizacionProducto entity);
		void ActualizarPrecioAutorizado(CotizacionProducto entity);
		void ActualizarPrecioMontoLote(List<CotizacionProducto> list);
		void ActualizarCantidadComprobante(CotizacionProducto entity);
		void ActualizarSinSaldo(CotizacionProducto entity);
		void ActualizarSaldoProduccion(CotizacionProducto entity);

    }
}