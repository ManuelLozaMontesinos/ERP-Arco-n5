using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("produ_detalle")]
	public class ProductoDetalle : BaseEntity
	{
		public override int EntityId => ProductoDetalleId;
		[Key]
		[Column("produ_detalle_id", Order = 1)]
		public int ProductoDetalleId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

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
	}
}
