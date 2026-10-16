using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("comprobante")]
	public class Comprobante : BaseEntity
	{
		public override int EntityId => ComprobanteId;
		[Key]
		[Column("comprobante_id", Order = 1)]
		public int ComprobanteId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("tipo_doc_com_id")]
		public int TipoDocumentoComercialId { get; set; }

		[Column("numero_opera")]
		public string NumeroOperacion { get; set; }

		[Column("serie")]
		public string Serie { get; set; }

		[Column("numero")]
		public string Numero { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("cotiza_id")]
		public int? CotizacionId { get; set; }

		[Column("pedido_id")]
		public int? PedidoId { get; set; }

		[Column("proveedor_id")]
		public int ProveedorId { get; set; }

		[Column("nombre_alte")]
		public string NombreAlterno { get; set; }

		[Column("vendedor_id")]
		public int VendedorId { get; set; }

		[Column("moneda_id")]
		public int MonedaId { get; set; }

		[Column("monto_tc")]
		public decimal MontoTipoCambio { get; set; }

		[Column("lista_precio_id")]
		public int ListaPrecioId { get; set; }

		[Column("tipo_c_pago_id")]
		public int TipoCondicionPagoId { get; set; }

		[Column("almacen_id")]
		public int? AlmacenId { get; set; }

		[Column("observa")]
		public string Observacion { get; set; }

		[Column("medio_pago_id")]
		public int MedioPagoId { get; set; }

		[Column("porcen_tarj")]
		public decimal PorcentajeTarjeta { get; set; }

		[Column("impuesto_tasa_id")]
		public int ImpuestoTasaId { get; set; }

		[Column("nro_guia_rem")]
		public string NumeroGuiaRemision { get; set; }

		[Column("monto_bru_d")]
		public decimal MontoBrutoDocumento { get; set; }
		[Column("monto_net_d")]
		public decimal MontoNetoDocumento { get; set; }
		[Column("monto_igv_d")]
		public decimal MontoIgvDocumento { get; set; }
		[Column("monto_tot_d")]
		public decimal MontoTotalDocumento { get; set; }

		[Column("monto_bruto")]
		public decimal MontoBruto { get; set; }

		[Column("porc_dcto")]
		public decimal PorcentajeDescuento { get; set; }

		[Column("monto_dcto")]
		public decimal MontoDescuento { get; set; }

		[Column("monto_dcto_item")]
		public decimal MontoDescuentoItem { get; set; }

		[Column("monto_neto")]
		public decimal MontoNeto { get; set; }

		[Column("monto_igv")]
		public decimal MontoIgv { get; set; }

		[Column("monto_isc")]
		public decimal MontoIsc { get; set; }

		[Column("monto_anticipo")]
		public decimal MontoAnticipo { get; set; }
		[Column("monto_total")]
		public decimal MontoTotal { get; set; }
		[Column("monto_ant_dcto")]
		public decimal MontoAnticipoDescuento { get; set; }
		[Column("canjeado")]
		public string Canjeado { get; set; }

		[Column("tipo_comprob_id")]
		public int TipoComprobanteId { get; set; }

		[Column("comp_canje_id")]
		public int? ComprobanteCanjeId { get; set; }

		[Column("estado_sunat_id")]
		public int? EstadoSunatId { get; set; }

		[Column("tipo_detrac_id")]
		public int TipoDetraccionId { get; set; }

		[Column("porc_detrac")]
		public decimal PorcentajeDetraccion { get; set; }

		[Column("monto_detrac")]
		public decimal MontoDetraccion { get; set; }

		[Column("num_opera_pago")]
		public string NumeroOperacionPago { get; set; }

		[Column("monto_pago")]
		public decimal MontoPago { get; set; }

		[Column("fecha_pago")]
		public DateTime? FechaPago { get; set; }

		[Column("monto_cargo")]
		public decimal MontoCargo { get; set; }

		[Column("es_contingencia")]
		public string EsContingencia { get; set; }

		[Column("emitido")]
		public string Emitido { get; set; }

		[Column("compro_elec_id")]
		public int? ComprobanteElectronicoId { get; set; }

		[Column("codigo_hash")]
		public string CodigoHash { get; set; }

		[Column("estado_compro_id")]
		public int EstadoComprobanteId { get; set; }

		[Column("caja_id")]
		public int? CajaId { get; set; }

		[Column("bancariza")]
		public string Bancarizado { get; set; }

		[Column("estado_despacho_id")]
		public int? EstadoDespachoId { get; set; }

		[Column("personal_id")]
		public int? PersonalId { get; set; }
		[Column("fecha_desp")]
		public DateTime? FechaDespacho { get; set; }
		[Column("observa_desp")]
		public string ObservacionDespacho { get; set; }
		[Column("entrega_desp")]
		public string EntregaDespacho { get; set; }

		[Column("color_lista")]
		public string ColorLista { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }
		public PeriodoEmpresaSede PeriodoEmpresaSede { get; set; }
		public TipoDocumentoComercial TipoDocumentoComercial { get; set; }
		public Cotizacion Cotizacion { get; set; }
		public Pedido Pedido { get; set; }
		public Proveedor Proveedor { get; set; }
		public Vendedor Vendedor { get; set; }
		public Moneda Moneda { get; set; }
		public ListaPrecio ListaPrecio { get; set; }
		public TipoCondicionPago TipoCondicionPago { get; set; }
		public Almacen Almacen { get; set; }
		public MedioPago MedioPago { get; set; }
		public ImpuestoTasa ImpuestoTasa { get; set; }
		public TipoComprobante TipoComprobante { get; set; }
		public EstadoSunat EstadoSunat { get; set; }
		public TipoDetraccion TipoDetraccion { get; set; }
		public EstadoComprobante EstadoComprobante { get; set; }
		public Caja Caja { get; set; }
		public EstadoDespacho EstadoDespacho { get; set; }
		public Personal Personal { get; set; }
		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
		public List<string> EstadoComprobanteCodigoList { get; set; }
		public List<string> TipoDocumentoComercialList { get; set; }
		public List<int> ComprobanteElectronicoIdList { get; set; }
		public List<string> EstadoSunatList { get; set; }
		public List<string> TipoComprobanteList { get; set; }
		public string NotTipoDetraccion { get; set; }
		public decimal MontoTotalMayorIgual { get; set; }
	}
}
