using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IComprobanteProductoRepository : IGenericRepository<ComprobanteProducto>
	{
		void ActualizarMonto(ComprobanteProducto entity);
		void EliminarPorComprobanteId(ComprobanteProducto entity);
		void ActualizarSolicitaAutorizacionDescuento(ComprobanteProducto entity);
		void ActualizarPrecioAutorizado(ComprobanteProducto entity);
		void ActualizarPrecioMontoLote(List<ComprobanteProducto> list);
		void ActualizarCantidadNotaCredito(ComprobanteProducto entity);
		List<ComprobanteProducto> ListarPendienteCanje(ComprobanteProducto entity);
		List<ComprobanteProducto> ListarPorIdList(ComprobanteProducto entity);
		void ActualizarRegistroCanjeId(ComprobanteProducto entity);
		void ActualizarNuloRegistroCanjeId(ComprobanteProducto entity);
		void ActualizarCantidadEntregada(ComprobanteProducto entity);
	}
}
