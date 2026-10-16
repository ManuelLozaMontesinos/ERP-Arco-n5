using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IComprobanteRepository : IGenericRepository<Comprobante>
	{
		void ActualizarMonto(Comprobante entity);
		void ActualizarMedioPago(Comprobante entity);
		void ActualizarEstadoComprobante(Comprobante entity);
		void ActualizarEmision(Comprobante entity);
		void ActualizarEstadoSunat(Comprobante entity);
		void ActualizarNumeroGuiaRemision(Comprobante entity);
      void ActualizarEmisionElectronica(Comprobante entity);
		List<Comprobante> ListarDocumentoEmitido(Comprobante entity);
		List<Comprobante> ListarResumenEstadoSunat(Comprobante entity);
		void ActualizarPorComprobanteElectronicoIdList(Comprobante entity);
		void ActualizarDetraccion(Comprobante entity);
		void ActualizarBancarizado(Comprobante entity);
		void ActualizarTipoComprobante(Comprobante entity);
		List<dynamic> ListarCobranza(Comprobante entity);
		void ActualizarMontoAnticipo(Comprobante entity);
		void ActualizarMontoAnticipoDescuento(Comprobante entity);
		List<Comprobante> BuscarDespacho(Comprobante entity);
		void ActualizarEstadoDespacho(Comprobante entity);
		List<Comprobante> BuscarPorEmpresaSede(Comprobante entity);
		void ActualizarEstadoDespachoPorCantidadEntregada(Comprobante entity);
	}
}
