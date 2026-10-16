using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("pedido_prod")]
	public class PedidoProducto : BaseEntity
	{
		public override int EntityId => PedidoProdId;
		[Key]
		[Column("pedido_prod_id", Order = 1)]
		public int PedidoProdId { get; set; }

		[Column("pedido_id")]
		public int PedidoId { get; set; }

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

		[Column("inc_dcto_lp")]
		public string IncDctoLp { get; set; }

		[Column("soli_auto_dcto")]
		public string SoliAutoDcto { get; set; }

		[Column("tiene_saldo")]
		public string TieneSaldo { get; set; }

		[Column("monto_bruto")]
		public decimal MontoBruto { get; set; }

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

		[Column("tipo_afecta_igv_id")]
		public int TipoAfectacionIgvId { get; set; }

		[Column("tipo_isc_id")]
		public int? TipoIscId { get; set; }

		[Column("almacen_id")]
		public int? AlmacenId { get; set; }

		[Column("reclasifica")]
		public string Reclasifica { get; set; }

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

		public Pedido Pedido { get; set; }
		public Producto Producto { get; set; }
		public UnidadMedida UnidadMedida { get; set; }
		public TipoAfectacionIgv TipoAfectacionIgv { get; set; }
		public TipoIsc TipoIsc { get; set; }
	}
}
