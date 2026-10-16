using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("comprob_prod")]
	public class ComprobanteProducto : BaseEntity
	{
		public override int EntityId => ComprobanteProductoId;
		[Key]
		[Column("comprob_prod_id", Order = 1)]
		public int ComprobanteProductoId { get; set; }

		[Column("comprobante_id")]
		public int ComprobanteId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

		[Column("descripcion")]
		public string Descripcion { get; set; }

		[Column("cantidad")]
		public decimal Cantidad { get; set; }

		[Column("unidad_medida_id")]
		public int? UnidadMedidaId { get; set; }

		[Column("cant_prese")]
		public decimal CantidadPresentacion { get; set; }

		[Column("cant_equiv")]
		public decimal CantidadEquivalente { get; set; }

		[Column("valor_u_lista")]
		public decimal ValorUnitarioLista { get; set; }

		[Column("precio_u_lista")]
		public decimal PrecioUnitarioLista { get; set; }

		[Column("valor_unitario")]
		public decimal ValorUnitario { get; set; }

		[Column("precio_unitario")]
		public decimal PrecioUnitario { get; set; }

		[Column("cant_bloqueada")]
		public string CantidadBloqueada { get; set; }

		[Column("inc_dcto_lp")]
		public string IncluyeDescuentoListaPrecio { get; set; }

		[Column("permite_dcto")]
		public string PermiteDescuento { get; set; }

		[Column("soli_auto_dcto")]
		public string SolicitaAutorizacionDescuento { get; set; }

		[Column("precio_auto")]
		public decimal PrecioAutorizado { get; set; }

		[Column("reg_refe_id")]
		public int? RegistroReferencialId { get; set; }

		[Column("monto_bruto")]
		public decimal MontoBruto { get; set; }

		[Column("tipo_dcto_id")]
		public int TipoDescuentoId { get; set; }

		[Column("dcto_valor")]
		public decimal DescuentoValor { get; set; }

		[Column("porc_dcto")]
		public decimal PorcentajeDescuento { get; set; }

		[Column("monto_dcto1")]
		public decimal MontoDescuento1 { get; set; }

		[Column("monto_dcto2")]
		public decimal MontoDescuento2 { get; set; }

		[Column("monto_neto")]
		public decimal MontoNeto { get; set; }

		[Column("monto_igv")]
		public decimal MontoIgv { get; set; }

		[Column("monto_isc")]
		public decimal MontoIsc { get; set; }

		[Column("monto_total")]
		public decimal MontoTotal { get; set; }

		[Column("tipo_detrac_id")]
		public int TipoDetraccionId { get; set; }

		[Column("porc_detrac")]
		public decimal PorcentajeDetraccion { get; set; }

		[Column("monto_detrac")]
		public decimal MontoDetraccion { get; set; }

		[Column("tipo_afecta_igv_id")]
		public int TipoAfectacionIgvId { get; set; }

		[Column("tipo_isc_id")]
		public int? TipoIscId { get; set; }

		[Column("cant_nota_c")]
		public decimal CantidadNotaCredito { get; set; }

		[Column("reg_canje_id")]
		public int? RegistroCanjeId { get; set; }

		[Column("reg_combo_id")]
		public int? RegistroComboId { get; set; }

		[Column("cant_entrega")]
		public decimal CantidadEntregada{ get; set; }
		[Column("fecha_desp")]
		public DateTime? FechaDespacho { get; set; }
		[Column("observa_desp")]
		public string ObservacionDespacho { get; set; }
		[Column("entrega_desp")]
		public string EntregaDespacho { get; set; }

		[Column("personal_id")]
		public int? PersonalId { get; set; }

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

		public Comprobante Comprobante { get; set; }
		public Producto Producto { get; set; }
		public UnidadMedida UnidadMedida { get; set; }
		public TipoDetraccion TipoDetraccion { get; set; }
		public TipoAfectacionIgv TipoAfectacionIgv { get; set; }
		public TipoIsc TipoIsc { get; set; }
		public TipoDescuento TipoDescuento { get; set; }
		public Personal Personal { get; set; }
		public string NotNullRegistroReferencialId { get; set; }
		public string ConSaldoNotaCredito { get; set; }
		public string NullRegistroCanjeId { get; set; }
		public List<int> ComprobanteProductoIdList { get; set; }
		public string NullRegistroComboId { get; set; }
	}
}
