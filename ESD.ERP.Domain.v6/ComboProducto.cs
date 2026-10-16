using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("combo_producto")]
	public class ComboProducto : BaseEntity
	{
		public override int EntityId => ComboProductoId;
		[Key]
		[Column("combo_producto_id", Order = 1)]
		public int ComboProductoId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

		[Column("prod_inclu_id")]
		public int ProductoIncluidoId { get; set; }

		[Column("cantidad")]
		public decimal Cantidad { get; set; }

		[Column("porc_precio")]
		public decimal PorcentajePrecio { get; set; }

		[Column("num_dec_redo")]
		public int NumeroDecimalesRedondeo { get; set; }

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

		public Producto Producto { get; set; }
		public Producto ProductoIncluido { get; set; }
	}
}
