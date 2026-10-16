using System;
using System.Collections.Generic;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
   public class ComprobanteDTO : DTOBase
	{
		public override int Id => ComprobanteId;
		public int ComprobanteId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public int TipoDocumentoComercialId { get; set; }
		public string NumeroOperacion { get; set; }
		public string SerieNumero { 
			get {
				if (string.IsNullOrEmpty(Numero))
					return NumeroOperacion;

				if (string.IsNullOrEmpty(Numero.Trim()))
					return NumeroOperacion;

				return string.Concat(Serie, SeparadorConstante.GUION, Numero);
			} 
		}
		public string TipoDocumentoSerieNumero
		{
			get
			{
				if (string.IsNullOrEmpty(Numero))
					return NumeroOperacion;

				if (string.IsNullOrEmpty(Numero.Trim()))
					return NumeroOperacion;

				if (TipoDocumentoComercialDTO == null)
					return NumeroOperacion;

				return string.Concat(TipoDocumentoComercialDTO.Abreviatura, SeparadorConstante.DOS_PUNTOS,
					Serie, SeparadorConstante.GUION, Numero);
			}
		}
		public string CotizacionPedido
		{
			get
			{
				if (PedidoId.HasValue)
					return PedidoDTO != null ? PedidoDTO.Codigo : string.Empty;

				if (CotizacionId.HasValue)
					return CotizacionDTO != null ? CotizacionDTO.Codigo : string.Empty;

				return string.Empty;
			}
		}
		public string Serie { get; set; }
		public string Numero { get; set; }
		public DateTime Fecha { get; set; }
		public int? CotizacionId { get; set; }
		public int? PedidoId { get; set; }
		public int ProveedorId { get; set; }
		public string NombreAlterno { get; set; }
		public int VendedorId { get; set; }
		public int MonedaId { get; set; }
		public decimal MontoTipoCambio { get; set; }
		public int ListaPrecioId { get; set; }
		public int TipoCondicionPagoId { get; set; }
		public int? AlmacenId { get; set; }
		public string Observacion { get; set; }
		public int MedioPagoId { get; set; }
		public decimal PorcentajeTarjeta { get; set; }
		public int ImpuestoTasaId { get; set; }
		public string NumeroGuiaRemision { get; set; }
		public decimal MontoBrutoDocumento { get; set; }
		public decimal MontoNetoDocumento { get; set; }
		public decimal MontoIgvDocumento { get; set; }
		public decimal MontoTotalDocumento { get; set; }
		public decimal MontoBruto { get; set; }
		public decimal PorcentajeDescuento { get; set; }
		public decimal MontoDescuento { get; set; }
		public decimal MontoDescuentoItem { get; set; }
		public decimal MontoNeto { get; set; }
		public decimal MontoIgv { get; set; }
		public decimal MontoIsc { get; set; }
		public decimal MontoAnticipo { get; set; }
		public decimal MontoTotal { get; set; }
		public decimal MontoAnticipoDescuento { get; set; }
		public string Canjeado { get; set; }
		public int TipoComprobanteId { get; set; }
		public int? ComprobanteCanjeId { get; set; }
		public int? EstadoSunatId { get; set; }
		public int TipoDetraccionId { get; set; }
		public decimal PorcentajeDetraccion { get; set; }
		public decimal MontoDetraccion { get; set; }
		public string NumeroOperacionPago { get; set; }
		public decimal MontoPago { get; set; }
		public DateTime? FechaPago { get; set; }
		public decimal MontoCargo { get; set; }
		public string EsContingencia { get; set; }
		public string Emitido { get; set; }
		public int? ComprobanteElectronicoId { get; set; }
		public string CodigoHash { get; set; }
		public int EstadoComprobanteId { get; set; }
		public int? CajaId { get; set; }
		public string Bancarizado { get; set; }
		public int? EstadoDespachoId { get; set; }
		public int? PersonalId { get; set; }
		public DateTime? FechaDespacho { get; set; }
		public string ObservacionDespacho { get; set; }
		public string EntregaDespacho { get; set; }
		public string ColorLista { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public TipoDocumentoComercialDTO TipoDocumentoComercialDTO { get; set; }
		public CotizacionDTO CotizacionDTO { get; set; }
		public PedidoDTO PedidoDTO { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
		public VendedorDTO VendedorDTO { get; set; }
		public MonedaDTO MonedaDTO { get; set; }
		public ListaPrecioDTO ListaPrecioDTO { get; set; }
		public TipoCondicionPagoDTO TipoCondicionPagoDTO { get; set; }
		public AlmacenDTO AlmacenDTO { get; set; }
		public MedioPagoDTO MedioPagoDTO { get; set; }
		public ImpuestoTasaDTO ImpuestoTasaDTO { get; set; }
		public TipoComprobanteDTO TipoComprobanteDTO { get; set; }
		public ComprobanteDTO ComprobanteCanjeDTO { get; set; }
		public EstadoSunatDTO EstadoSunatDTO { get; set; }
		public TipoDetraccionDTO TipoDetraccionDTO { get; set; }
		public EstadoComprobanteDTO EstadoComprobanteDTO { get; set; }
		public CajaDTO CajaDTO { get; set; }
		public EstadoDespachoDTO EstadoDespachoDTO { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
		public List<string> EstadoComprobanteCodigoList { get; set; }
		public List<string> TipoDocumentoComercialList { get; set; }
		public List<int> ComprobanteElectronicoIdList { get; set; }
		public List<string> EstadoSunatList { get; set; }
		public List<string> TipoComprobanteList { get; set; }
		public string NotTipoDetraccion { get; set; }
		public decimal MontoTotalMayorIgual { get; set; }
		public List<ComprobanteProductoDTO> ComprobanteProductoDTOList { get; set; }
	}
}
