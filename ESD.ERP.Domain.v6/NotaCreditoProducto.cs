using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("nota_c_prod")]
	public class NotaCreditoProducto : BaseEntity
	{
		public override int EntityId => NotaCreditoProductoId;
		[Key]
		[Column("nota_c_prod_id", Order = 1)]
		public int NotaCreditoProductoId { get; set; }

		[Column("nota_credito_id")]
		public int NotaCreditoId { get; set; }

		[Column("comprob_prod_id")]
		public int? ComprobanteProductoId { get; set; }

		[Column("producto_id")]
		public int? ProductoId { get; set; }

		[Column("descripcion")]
		public string Descripcion { get; set; }

		[Column("cantidad")]
		public decimal Cantidad { get; set; }

		[Column("precio_unitario")]
		public decimal PrecioUnitario { get; set; }

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

		public NotaCredito NotaCredito { get; set; }
		public ComprobanteProducto ComprobanteProducto { get; set; }
		public Producto Producto { get; set; }
		public TipoAfectacionIgv TipoAfectacionIgv { get; set; }
		public TipoIsc TipoIsc { get; set; }
	}
}
