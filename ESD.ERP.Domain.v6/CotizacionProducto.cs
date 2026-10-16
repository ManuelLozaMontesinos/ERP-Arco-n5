using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("cotiza_prod")]
	public class CotizacionProducto : BaseEntity
	{
		public override int EntityId => CotizacionProductoId;
		[Key]
		[Column("cotiza_prod_id", Order = 1)]
		public int CotizacionProductoId { get; set; }

		[Column("cotiza_id")]
		public int CotizacionId { get; set; }

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

		[Column("cant_compro")]
		public decimal CantidadComprobante { get; set; }

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

		[Column("sin_saldo")]
		public string SinSaldo { get; set; }

		[Column("soli_auto_dcto")]
		public string SolicitaAutorizacionDescuento { get; set; }

		[Column("precio_auto")]
		public decimal PrecioAutorizado { get; set; }

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

		[Column("peso_neto")]
		public decimal PesoNeto { get; set; }

		[Column("tipo_afecta_igv_id")]
		public int TipoAfectacionIgvId { get; set; }

		[Column("tipo_isc_id")]
		public int? TipoIscId { get; set; }

		[Column("reg_combo_id")]
		public int? RegistroComboId { get; set; }

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

		[Column("cantidad_parcial")]
		public decimal CantidadParcial { get; set; }

        public Cotizacion Cotizacion { get; set; }
		public Producto Producto { get; set; }
		public FamiliaProducto FamiliaProducto { get; set; }
        public UnidadMedida UnidadMedida { get; set; }
		public TipoAfectacionIgv TipoAfectacionIgv { get; set; }
		public TipoIsc TipoIsc { get; set; }
		public TipoDescuento TipoDescuento { get; set; }
		public string ConCantidadPendiente { get; set; }
		public List<int> CotizacionProductoIdList { get; set; }
		public string NullRegistroComboId { get; set; }
	}
}
