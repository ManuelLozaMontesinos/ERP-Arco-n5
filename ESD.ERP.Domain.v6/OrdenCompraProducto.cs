using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("orden_c_prod")]
	public class OrdenCompraProducto : BaseEntity
	{
		public override int EntityId => OrdenCompraProductoId;
		[Key]
		[Column("orden_c_prod_id", Order = 1)]
		public int OrdenCompraProductoId { get; set; }

		[Column("orden_co_id")]
		public int OrdenCompraId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

		[Column("descripcion")]
		public string Descripcion { get; set; }

		[Column("cantidad")]
		public decimal Cantidad { get; set; }

		[Column("cant_ing")]
		public decimal CantidadIngresada { get; set; }

		[Column("valor_unitario")]
		public decimal ValorUnitario { get; set; }

		[Column("precio_unitario")]
		public decimal PrecioUnitario { get; set; }

		[Column("monto_bruto")]
		public decimal MontoBruto { get; set; }

		[Column("monto_impuesto")]
		public decimal MontoImpuesto { get; set; }

		[Column("monto_total")]
		public decimal MontoTotal { get; set; }

		[Column("es_adicional")]
		public string EsAdicional { get; set; }

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

		public OrdenCompra OrdenCompra { get; set; }
		public Producto Producto { get; set; }
		public TipoAfectacionIgv TipoAfectacionIgv { get; set; }
		public string ConCantidadPendiente { get; set; }
		public List<int> OrdenCompraProductoIdList { get; set; }
	}
}
